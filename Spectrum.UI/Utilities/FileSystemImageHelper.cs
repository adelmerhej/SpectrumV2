using DevExpress.Utils.Drawing.Helpers;
using Spectrum.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Spectrum.Utilities
{
    internal class FileSystemImageHelper
    {
        private static int DeltaSize => 50;

        [ComImport]
        [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IImageList
        {
            [PreserveSig]
            int Add(IntPtr hbmImage, IntPtr hbmMask, ref int pi);

            [PreserveSig]
            int ReplaceIcon(int i, IntPtr hicon, ref int pi);

            [PreserveSig]
            int SetOverlayImage(int iImage, int iOverlay);

            [PreserveSig]
            int Replace(int i, IntPtr hbmImage, IntPtr hbmMask);

            [PreserveSig]
            int AddMasked(IntPtr hbmImage, int crMask, ref int pi);

            [PreserveSig]
            int Draw(ref IMAGELISTDRAWPARAMS pimldp);

            [PreserveSig]
            int Remove(int i);

            [PreserveSig]
            int GetIcon(int i, int flags, ref IntPtr picon);

            [PreserveSig]
            int GetImageInfo(int i, ref IMAGEINFO pImageInfo);

            [PreserveSig]
            int Copy(int iDst, IImageList punkSrc, int iSrc, int uFlags);

            [PreserveSig]
            int Merge(int i1, IImageList punk2, int i2, int dx, int dy, ref Guid riid, ref IntPtr ppv);

            [PreserveSig]
            int Clone(ref Guid riid, ref IntPtr ppv);

            [PreserveSig]
            int GetImageRect(int i, ref NativeMethods.RECT prc);

            [PreserveSig]
            int GetIconSize(ref int cx, ref int cy);

            [PreserveSig]
            int SetIconSize(int cx, int cy);

            [PreserveSig]
            int GetImageCount(ref int pi);

            [PreserveSig]
            int SetImageCount(int uNewCount);

            [PreserveSig]
            int SetBkColor(int clrBk, ref int pclr);

            [PreserveSig]
            int GetBkColor(ref int pclr);

            [PreserveSig]
            int BeginDrag(int iTrack, int dxHotspot, int dyHotspot);

            [PreserveSig]
            int EndDrag();

            [PreserveSig]
            int DragEnter(IntPtr hwndLock, int x, int y);

            [PreserveSig]
            int DragLeave(IntPtr hwndLock);

            [PreserveSig]
            int DragMove(int x, int y);

            [PreserveSig]
            int SetDragCursorImage(ref IImageList punk, int iDrag, int dxHotspot, int dyHotspot);

            [PreserveSig]
            int DragShowNolock(int fShow);

            [PreserveSig]
            int GetDragImage(ref NativeMethods.POINT ppt, ref NativeMethods.POINT pptHotspot, ref Guid riid, ref IntPtr ppv);

            [PreserveSig]
            int GetItemFlags(int i, ref int dwFlags);

            [PreserveSig]
            int GetOverlayImage(int iOverlay, ref int piIndex);
        }

        public static Image GetFileExtensionImage(string ext, IconSizeType sizeType, Size itemSize)
        {
            if (string.IsNullOrEmpty(ext))
            {
                return null;
            }

            return IconToBitmap(GetFileExtensionIcon(ext, sizeType, itemSize), sizeType, itemSize);
        }

        public static Image IconToBitmap(Icon ico, IconSizeType sizeType, Size itemSize)
        {
            Image image = null;
            if (ico == null)
            {
                return new Bitmap(itemSize.Width, itemSize.Height);
            }

            Bitmap bitmap = null;
            try
            {
                bitmap = ico.ToBitmap();
            }
            catch
            {
                ico.Dispose();
                return new Bitmap(itemSize.Width, itemSize.Height);
            }

            ico.Dispose();
            int num = CalcRealSize(bitmap);
            int desiredSize = GetDesiredSize(sizeType);
            if (num > desiredSize)
            {
                image = ResizeBitmap(bitmap, itemSize);
                bitmap.Dispose();
                return image;
            }

            if (Math.Abs(num - desiredSize) < DeltaSize)
            {
                return bitmap;
            }

            image = MakeBorder(itemSize, bitmap, num);
            bitmap.Dispose();
            return image;
        }

        public static Icon GetFileExtensionIcon(string path, IconSizeType sizeType, Size itemSize)
        {
            return GetIconCore(path, sizeType, itemSize, 16656u);
        }

        private static Icon GetIconCore(string path, IconSizeType sizeType, Size itemSize, uint flags)
        {
            SHFILEINFO psfi = default(SHFILEINFO);
            IntPtr num = SHGetFileInfo(path, 0u, ref psfi, (uint)Marshal.SizeOf(psfi), flags);
            int iIcon = psfi.iIcon;
            IImageList systemImageListHandle = GetSystemImageListHandle(sizeType);
            IntPtr picon = IntPtr.Zero;
            if (systemImageListHandle != null)
            {
                systemImageListHandle.GetIcon(iIcon, 33, ref picon);
                Marshal.ReleaseComObject(systemImageListHandle);
            }

            Icon result = null;
            if (picon != IntPtr.Zero)
            {
                Icon icon = Icon.FromHandle(picon);
                result = (Icon)icon.Clone();
                DestroyIcon(icon.Handle);
                DestroyIcon(psfi.hIcon);
            }

            return result;
        }

        private static IImageList GetSystemImageListHandle(IconSizeType sizeType)
        {
            IImageList ppv = null;
            Guid riid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            int num = SHGetImageList((int)sizeType, ref riid, ref ppv);
            return ppv;
        }

        private static int CalcRealSize(Bitmap bmp)
        {
            for (int num = bmp.Height - 1; num >= 0; num--)
            {
                for (int i = 0; i < bmp.Width; i++)
                {
                    if (bmp.GetPixel(num, i).A != 0 || bmp.GetPixel(i, num).A != 0)
                    {
                        return num;
                    }
                }
            }

            return Math.Max(bmp.Width, bmp.Height);
        }

        private static int GetDesiredSize(IconSizeType sizeType)
        {
            return GetDesiredSizeCore(sizeType);
        }

        private static int GetDesiredSizeCore(IconSizeType sizeType)
        {
            switch (sizeType)
            {
                case IconSizeType.Medium:
                    return 32;
                case IconSizeType.Small:
                    return 16;
                case IconSizeType.Large:
                    return 48;
                case IconSizeType.ExtraLarge:
                    return 254;
                default:
                    return 0;
            }
        }

        public static Bitmap ResizeBitmap(Bitmap bmpSource, Size newSize)
        {
            Bitmap bitmap = new Bitmap(newSize.Width, newSize.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(bmpSource, 0, 0, newSize.Width, newSize.Height);
            }
            return bitmap;
        }

        private static Image MakeBorder(Size itemSize, Bitmap bmpSource, int sizeReal)
        {
            using (Bitmap bitmap = bmpSource.Clone(new Rectangle(0, 0, sizeReal, sizeReal), bmpSource.PixelFormat))
            {
                Bitmap bitmap2 = new Bitmap(itemSize.Width, itemSize.Height);
                using (Graphics graphics = Graphics.FromImage(bitmap2))
                {
                    graphics.DrawRectangle(Pens.LightGray, 0, 0, itemSize.Width - 1, itemSize.Height - 1);
                    Point point = new Point(bitmap2.Width / 2 - bitmap.Width / 2, bitmap2.Width / 2 - bitmap.Width / 2);
                    graphics.DrawImage(bitmap, point);
                }

                return bitmap2;
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("shell32.dll", EntryPoint = "#727")]
        private static extern int SHGetImageList(int iImageList, ref Guid riid, ref IImageList ppv);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct SHFILEINFO
        {
            public IntPtr hIcon;

            public int iIcon;

            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
    }

    internal struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;

        public IntPtr himl;

        public int i;

        public IntPtr hdcDst;

        public int x;

        public int y;

        public int cx;

        public int cy;

        public int xBitmap;

        public int yBitmap;

        public int rgbBk;

        public int rgbFg;

        public int fStyle;

        public int dwRop;

        public int fState;

        public int Frame;

        public int crEffect;
    }

    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;

        public IntPtr hbmMask;

        public int Unused1;

        public int Unused2;

        public NativeMethods.RECT rcImage;
    }
}
