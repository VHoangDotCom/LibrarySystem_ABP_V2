using System.Collections.Generic;

namespace LibrarySystem.Utils
{
    public class AuditLogUtils
    {
        public static Dictionary<string, string> Dict = new Dictionary<string, string>()
        {
            {"Abp.AspNetCore.Mvc.Controllers.AbpUserConfigurationController GetAll", "Trang Admin > Configurations"},
           //Config call API from other app here
        };

        public static string GetNote(string serviceName, string methodName)
        {
            if (!Dict.ContainsKey(serviceName + " " + methodName)) return "";
            return Dict[serviceName + " " + methodName];
        }
    }
}
