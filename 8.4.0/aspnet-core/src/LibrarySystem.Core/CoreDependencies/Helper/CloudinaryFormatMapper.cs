using LibrarySystem.Constants.Enum;
using System;

namespace LibrarySystem.CoreDependencies.Helper
{
    public static class CloudinaryFormatMapper
    {
        public static string MapToCloudinaryFormat(FileType format)
        {
            switch (format)
            {
                case FileType.JPG:
                    return "jpg";
                case FileType.JPGE:
                    return "jpge";
                case FileType.GIF:
                    return "gif";
                case FileType.PNG:
                    return "png";
                case FileType.JPE:
                    return "jpe";
                case FileType.TIF:
                    return "tif";
                default:
                    throw new ArgumentException("Unsupported format type");
            }
        }
    }
}
