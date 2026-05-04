using Spectrum.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Spectrum.Utilities
{
    public class FileSystemImageCache
    {
        private readonly Dictionary<string, Image> iconForExtensions = new Dictionary<string, Image>();

        private static FileSystemImageCache cacheCore;

        public static FileSystemImageCache Cache
        {
            get
            {
                if (cacheCore == null)
                {
                    cacheCore = new FileSystemImageCache();
                }

                return cacheCore;
            }
        }

        public bool EnableFileIconCaching { get; set; }

        public string[] ForbidFileIconCachingForCertainExtensions { get; set; }

        protected FileSystemImageCache()
        {
            EnableFileIconCaching = true;
            ForbidFileIconCachingForCertainExtensions = new string[4] { ".lnk", ".exe", ".dll", ".ico" };
        }

        public Image GetFileExtensionImage(string ext, IconSizeType sizeType, Size itemSize)
        {
            if (iconForExtensions.TryGetValue(ext, out var value))
            {
                return value;
            }

            Image fileExtensionImage = FileSystemImageHelper.GetFileExtensionImage(ext, sizeType, itemSize);
            iconForExtensions[ext] = fileExtensionImage;
            return fileExtensionImage;
        }

        protected virtual string GetKey(string path)
        {
            if (string.IsNullOrEmpty(path) || !EnableFileIconCaching)
            {
                return null;
            }

            string extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
            {
                return null;
            }

            if (ForbidFileIconCachingForCertainExtensions != null)
            {
                for (int i = 0; i < ForbidFileIconCachingForCertainExtensions.Length; i++)
                {
                    if (string.Equals(extension, ForbidFileIconCachingForCertainExtensions[i], StringComparison.OrdinalIgnoreCase))
                    {
                        return null;
                    }
                }
            }

            return extension;
        }

        public void ClearCache()
        {
            iconForExtensions.Clear();
        }
    }
}
