using LibrarySystem.Debugging;

namespace LibrarySystem
{
    public class LibrarySystemConsts
    {
        public const string LocalizationSourceName = "LibrarySystem";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "f60e6a99a9d2423f94807f4e8eb39b5c";
    }
}
