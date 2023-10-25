using System;

namespace LibrarySystem.CoreDependencies.SymLinker
{
    public class LinuxSymLinkCreator : ISymLinkCreator
    {
        public bool CreateSymLink(string linkPath, string targetPath, bool file)
        {
            throw new NotImplementedException();
        }
    }
}
