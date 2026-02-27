using DevExpress.Utils.Drawing.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectrum.Utilities
{
    internal class FileSystemImageHelper
    {
    }

    internal struct IMAGEINFO
    {
        public nint hbmImage;

        public nint hbmMask;

        public int Unused1;

        public int Unused2;

        public NativeMethods.RECT rcImage;
    }
}
