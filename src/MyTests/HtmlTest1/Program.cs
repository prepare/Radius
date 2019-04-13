using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HtmlTest1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            YourImplementation.FrameworkInitWinGDI.SetupDefaultValues();
            YourImplementation.LocalFileStorageProvider file_storageProvider = new YourImplementation.LocalFileStorageProvider("");
            PixelFarm.Platforms.StorageService.RegisterProvider(file_storageProvider);
            YourImplementation.FrameworkInitGLES.SetupDefaultValues();

            //2.2 Icu Text Break info
            //test Typography's custom text break,
            //check if we have that data?            
            //-------------------------------------------  
            string icu_datadir = YourImplementation.RelativePathBuilder.SearchBackAndBuildFolderPath(System.IO.Directory.GetCurrentDirectory(), "Radius", @"..\Typography\Typography.TextBreak\icu62\brkitr");
            if (!System.IO.Directory.Exists(icu_datadir))
            {
                throw new System.NotSupportedException("dic");
            }
            var dicProvider = new Typography.TextBreak.IcuSimpleTextFileDictionaryProvider() { DataDir = icu_datadir };
            Typography.TextBreak.CustomBreakerBuilder.Setup(dicProvider);
            PixelFarm.CpuBlit.MemBitmapExtensions.DefaultMemBitmapIO = new PixelFarm.Drawing.WinGdi.GdiBitmapIO();

#if DEBUG
            PixelFarm.CpuBlit.Imaging.PngImageWriter.InstallImageSaveToFileService((IntPtr imgBuffer, int stride, int width, int height, string filename) =>
            {

                using (System.Drawing.Bitmap newBmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    PixelFarm.CpuBlit.Imaging.BitmapHelper.CopyToGdiPlusBitmapSameSize(imgBuffer, newBmp);
                    //save
                    newBmp.Save(filename);
                }
            });
#endif




            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
