using System;

namespace Tubumu.Core.Extensions
{
    public static class NumericExtensions
    {
        private const double FileSizeMod = 1024.0;
        private static string[] FileSizeUnits = new[] { "", "K", "M", "G", "T", "P" };

        public static string ToFloatFileSize(this int size)
        {
            return ToFloatFileSize((long)size);
        }

        public static string ToFloatFileSize(this long size)
        {
            double fSize = size;
            var i = 0;
            while (fSize >= FileSizeMod)
            {
                fSize /= FileSizeMod;
                i++;
            }

            return fSize.ToString("f2") + FileSizeUnits[i];
        }

        public static string ToIntFileSize(this int size)
        {
            return ToIntFileSize((long)size);
        }

        public static string ToIntFileSize(this long size)
        {
            double fSize = size;
            var i = 0;
            while (fSize >= FileSizeMod)
            {
                fSize /= FileSizeMod;
                i++;
            }

            return Math.Round(fSize) + FileSizeUnits[i];
        }
    }
}
