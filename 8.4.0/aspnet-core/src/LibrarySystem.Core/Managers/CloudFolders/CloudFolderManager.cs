using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.UI;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LibrarySystem.Common;
using LibrarySystem.CoreDependencies.IOC;
using LibrarySystem.Entities;
using LibrarySystem.Managers.CloudFolders.Dtos;
using LibrarySystem.Managers.Common;
using LibrarySystem.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LibrarySystem.Managers.CloudFolders
{
    public class CloudFolderManager : ApplicationService
    {
        private readonly IWorkScope _workScope;
        private readonly CommonManager _commonManager;
        private readonly Cloudinary _cloudinary;

        public const string CLOUD_NAME = "dduv8pom4";
        public const string API_KEY = "952444439587681";
        public const string API_SECRET = "ubB0ir_v5YXR4KxmnZnuQHORoew";

        public CloudFolderManager(
            IWorkScope workScope,
            CommonManager commonManager
            )
        {
            _workScope = workScope;
            _commonManager = commonManager;

            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRET);
            _cloudinary = new Cloudinary(account);
        }

        #region LocalDB CloudFolder

        public async Task<TreeFolderDto> GetAll(InputToGetFolderDto input)
        {
            var listCFs = await _workScope.GetAll<CloudFolder>().ToListAsync();
            var listData = listCFs
                .Select(x => new CloudFolderDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsActive = x.IsActive,
                    Level = x.Level,
                    IsLeaf = x.IsLeaf,
                    ParentId = x.ParentId,
                    Code = x.Code,
                    CombineName = x.CombineName,
                })
                .OrderBy(x => CommonUtil.GetNaturalSortKey(x.Code))
                .ToList();

            if (input.IsGetAll())
            {
                var treeFolderDto = new TreeFolderDto
                {
                    Childrens = listData.GenerateTree(c => c.Id, c => c.ParentId)
                };
                return treeFolderDto;
            }

            var listFolderIds = listData
                    .WhereIf(input.IsLeaf.HasValue, x => x.IsLeaf == input.IsLeaf)
                    .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive)
                    .WhereIf(!string.IsNullOrEmpty(input.SearchText),
                    (x => x.Name.ToLower().Contains(input.SearchText.Trim().ToLower()) ||
                    (x.Code.ToLower().Contains(input.SearchText.Trim().ToLower()))))
                    .Select(x => x.Id)
                    .ToList();

            var resultIds = new List<long>();
            foreach (var id in listFolderIds)
            {
                resultIds.AddRange(_commonManager.GetAllNodeAndLeafIdById(id, listCFs, true));
            }
            resultIds = resultIds.Distinct().ToList();

            var filteredTreeFolderDto = new TreeFolderDto
            {
                Childrens = listData
                        .Where(x => resultIds.Contains(x.Id))
                        .ToList()
                        .GenerateTree(c => c.Id, c => c.ParentId),
            };

            return filteredTreeFolderDto;
        }

        #endregion LocalDB CloudFolder

        #region LocalDB-Cloud CloudFolder

        public async Task<CloudFolderDto> GetFolderById(long folderId)
        {
            var existedFolder = await _workScope.GetAll<CloudFolder>()
                                        .Where(x => x.Id == folderId)
                                        .FirstOrDefaultAsync();

            if(existedFolder == null)
                throw new UserFriendlyException($"Folder with Id {folderId} does not exist!");

            var folderDto = new CloudFolderDto()
            {
                Id = existedFolder.Id,
                Name = existedFolder.Name,
                IsActive = existedFolder.IsActive,
                Level = existedFolder.Level,
                IsLeaf = existedFolder.IsLeaf,
                ParentId = existedFolder.ParentId,
                Code = existedFolder.Code,
                CombineName = existedFolder.CombineName,
            };

            return folderDto;
        }

        public async Task<CreateCloudFolderDto> CreateFolder(CreateCloudFolderDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name) || string.IsNullOrWhiteSpace(input.Code))
                throw new UserFriendlyException($"Name or Code can't be Null or Empty!");

            var isExisted = await _workScope.GetAll<CloudFolder>()
                            .AnyAsync(x => x.Name == input.Name || x.Code == input.Code);

            if(isExisted)
                throw new UserFriendlyException($"Folder with Name '{input.Name}' or Code '{input.Code}' already exists!");

            var folderMap = ObjectMapper.Map<CloudFolder>(input);
            folderMap.IsActive = true;
            folderMap.IsLeaf = true;

            if (input.ParentId.HasValue)
            {
                var parentFolder = await _workScope.GetAll<CloudFolder>()
                                    .Where(x => x.Id == input.ParentId.Value)
                                    .FirstOrDefaultAsync();
                if (parentFolder != null)
                {
                    parentFolder.IsLeaf = false;
                    folderMap.Level = parentFolder.Level + 1;
                    folderMap.CombineName = parentFolder.CombineName + "/" + folderMap.Name;
                    // Save changes after modifying entities
                    await _workScope.UpdateAsync(parentFolder);
                }
                else
                    throw new UserFriendlyException("Parent folder not found!");

            }
            else
            {
                folderMap.Level = 1;
                folderMap.CombineName = folderMap.Name;
            }

            var isCloudExisted = await CheckExistedFolder(folderMap.CombineName);
            if (isCloudExisted)
                throw new UserFriendlyException($"A folder with that name already exists in Cloudinary!");
            else
                await _cloudinary.CreateFolderAsync(folderMap.CombineName);

            var idCreateFolder = await _workScope.InsertAndGetIdAsync<CloudFolder>(folderMap);

            if(idCreateFolder == 0)
                throw new UserFriendlyException($"There is something wrong with creating Folder!");

            return input;
        }

        public async Task<UpdateCloudFolderDto> UpdateFolder(UpdateCloudFolderDto input)
        {
            var cloudFolder = await _workScope.GetAll<CloudFolder>()
                                .Where(x => x.Id == input.Id) 
                                .FirstOrDefaultAsync();
            if (cloudFolder == null)
                throw new UserFriendlyException($"Can not found folder with Id = {input.Id}");

            var duplicatedFolder = await _workScope.GetAll<CloudFolder>()
                                    .AnyAsync(x => (x.Name == input.Name || x.Code == input.Code) && x.Id != input.Id);
            if(duplicatedFolder)
                throw new UserFriendlyException(String.Format("Name or Code already exist in Cloud Folder!"));

            if (input.Code != cloudFolder.Code && !cloudFolder.IsLeaf)
                await RenameChildCode(input);

            var oldCloudName = cloudFolder.CombineName;
            var split = cloudFolder.CombineName.Split('/');
            var countSlash = split.Length - 1;
            if (input.Name != cloudFolder.Name)
            {
                var splitCombineName = cloudFolder.CombineName.Split('/');
                splitCombineName[countSlash] = input.Name;
                cloudFolder.CombineName = string.Join('/', splitCombineName);

                //await UpdateCloudinaryFolder(oldCloudName, cloudFolder.CombineName);

                await _workScope.UpdateAsync<CloudFolder>(cloudFolder);

                await RenameChildCombineName(input);
            }
            cloudFolder.Name = input.Name;
            cloudFolder.Code = input.Code;

            await _workScope.UpdateAsync<CloudFolder>(cloudFolder);

            return input;
        }

        private async Task RenameChildCode(UpdateCloudFolderDto input)
        {
            var listCFs = await _workScope.GetAll<CloudFolder>().ToListAsync();
            var listChildIds = _commonManager.GetAllChildId(input.Id, listCFs).Distinct();
            var listChilds = listCFs.Where(x => listChildIds.Contains(x.Id) && x.Id != input.Id).ToList();

            var split = input.Code.Split('.');
            var countDot = split.Length - 1;
            for (int i = 0; i < listChilds.Count; i++)
            {
                var splitChild = listChilds[i].Code.Split(".");
                splitChild[countDot] = split[countDot];
                listChilds[i].Code = string.Join(".", splitChild);
            }

            await _workScope.UpdateRangeAsync<CloudFolder>(listChilds);
        }

        private async Task RenameChildCombineName(UpdateCloudFolderDto input)
        {
            var cloudFolder = await _workScope.GetAll<CloudFolder>()
                                .Where(x => x.Id == input.Id)
                                .FirstOrDefaultAsync();
            var listCFs = await _workScope.GetAll<CloudFolder>().ToListAsync();
            var listChildIds = _commonManager.GetAllChildId(input.Id, listCFs).Distinct();
            var listChilds = listCFs.Where(x => listChildIds.Contains(x.Id) && x.Id != input.Id).ToList();

            var split = cloudFolder.CombineName.Split('/');
            var countSlash = split.Length - 1;
            for (int i = 0; i < listChilds.Count; i++)
            {
                var oldCloudName = listChilds[i].CombineName;
                var splitChild = listChilds[i].CombineName.Split('/');
                splitChild[countSlash] = split[countSlash].Trim();
                listChilds[i].CombineName = string.Join("/", splitChild);
                //await UpdateCloudinaryFolder(oldCloudName, listChilds[i].CombineName);
            }

            await _workScope.UpdateRangeAsync<CloudFolder>(listChilds);     
        }
     
        public async Task DeleteFolder(long id)
        {
            var folder = await _workScope.GetAsync<CloudFolder>(id);
            if (folder == null)
                throw new UserFriendlyException($"Folder {id} does not exist!");

            long? parentID = folder.ParentId;

            var allFolders = await _workScope.GetAll<CloudFolder>().ToListAsync();

            if (!folder.IsLeaf)
            {
                var listIds = _commonManager.GetAllNodeAndLeafIdById(id, allFolders).Distinct().ToList();
                var deleteTasks = listIds
                 .Select(async Id =>
                 {
                     //await ValidToDeleteSubFolder(Id);
                     var childFolder = allFolders.FirstOrDefault(f => f.Id == Id);
                     if (childFolder != null)
                     {
                         var isDeletedInCloud = await DeleteCloudinaryFolder(childFolder.CombineName);
                         if (isDeletedInCloud)
                             await _workScope.DeleteAsync<CloudFolder>(Id);
                         else
                             throw new UserFriendlyException($"Failed to delete Folder {childFolder.CombineName} in Cloudinary!");
                     }
                 });

                await Task.WhenAll(deleteTasks);
            }
            else
            {
                var isDelInCloud = await DeleteCloudinaryFolder(folder.CombineName);
                if (isDelInCloud)
                    await _workScope.DeleteAsync<CloudFolder>(id);
                else
                    throw new UserFriendlyException($"Failed to delete Folder {folder.CombineName} in Cloudinary!");
            }

            if (parentID.HasValue)
            {
                var parent = await _workScope.GetAsync<CloudFolder>(parentID.Value);
                var countRemainChild = _workScope.GetAll<CloudFolder>().Any(child => child.ParentId == parentID);
                if (!countRemainChild)
                {
                    parent.IsLeaf = true;
                    await _workScope.UpdateAsync(parent);
                }
            }
        }

        private async Task ValidToDeleteSubFolder(long id)
        {
            var folder = await _workScope.GetAll<CloudFolder>()
                .Where(x => x.CloudFiles != null && x.CloudFiles.Count > 0 && x.Id == id)
                .FirstOrDefaultAsync();
            if (folder != default)
                throw new UserFriendlyException($"Can not delete folder because it has assets");
        }

        private async Task ValidToDeleteListFolder(long id, List<long> listFolderIds = default)
        {
            var listFolders = new List<CloudFolder>();
            var listCFIds = new List<long>();
            if(listFolderIds == default)
            {
                listFolders = await _workScope.GetAll<CloudFolder>().ToListAsync();
                listCFIds = _commonManager.GetAllNodeAndLeafIdById(id, listFolders).Distinct().ToList();
            }
            var listIds = _workScope.GetAll<CloudFile>()
                .WhereIf(listFolderIds != default, x => listCFIds.Contains(x.FolderId))
                .Select(x => x.FolderId)
                .ToList();

            if(listIds != null && listIds.Count() > 0)
                throw new UserFriendlyException($"Cannot delete folder because its leaf had assets");
        }

        #endregion LocalDB-Cloud CloudFolder

        #region Cloud CloudFolder

        private async Task<bool> UpdateCloudinaryFolder(string currentFolderName, string newFolderName)
        {
            var isExisted = await CheckExistedFolder(currentFolderName);
            if (!isExisted)
                throw new Exception($"Folder with name {currentFolderName} does not exist in Cloudinary!");

            var deleteFolder = await _cloudinary.DeleteFolderAsync($"{currentFolderName}");

            if (deleteFolder.StatusCode == HttpStatusCode.OK)
            {
                await _cloudinary.CreateFolderAsync(newFolderName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteCloudinaryFolder(string folderName)
        {
            var isExisted = await CheckExistedFolder(folderName);
            if (isExisted)
            {
                var deleteFolder = await _cloudinary.DeleteFolderAsync($"{folderName}");

                if (deleteFolder.StatusCode == HttpStatusCode.OK)
                    return true;
                else
                    throw new UserFriendlyException($"Folder {folderName} might still contains assets inside it in Cloudinary!");
            }
            else
                throw new UserFriendlyException($"Folder {folderName} does not exist in Cloudinary!");
        }

        private async Task<bool> CheckExistedFolder(string name)
        {
            var subFoldersResult = await _cloudinary.SubFoldersAsync(name);

            if (subFoldersResult != null && subFoldersResult.Folders != null)
                return true;
            else
                return false;
        }

        #endregion Cloud CloudFolder
    }
}
