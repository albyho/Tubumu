namespace Tubumu.Modules.Admin.Settings
{
    /// <summary>
    /// AvatarSettings
    /// </summary>
    public class AvatarSettings
    {
        /// <summary>
        /// ImageExtensions
        /// </summary>
        public string ImageExtensions { get; set; }

        /// <summary>
        /// ImageSizeMax，单位字节
        /// </summary>
        public int ImageSizeMax { get; set; }

        /// <summary>
        /// FileExtensions
        /// </summary>
        public string FileExtensions { get; set; }

        /// <summary>
        /// FileSizeMax，单位字节
        /// </summary>
        public int FileSizeMax { get; set; }

        /// <summary>
        /// Consturctor
        /// </summary>
        public AvatarSettings()
        {
            ImageExtensions = ".jpg;png";
            ImageSizeMax = 1024 * 1024;
            FileExtensions = ".zip";
            FileSizeMax = 1024 * 1024;
        }
    }
}
