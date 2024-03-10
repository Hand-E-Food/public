using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MarkRichardson.SkillTreeTemplate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ShowAndSave(Animal.Player);
        }

        /// <summary>
        /// Displays the specified skill tree in a form, and saves the graphics to PNG and EMF files.
        /// </summary>
        /// <param name="animal">The animal to show.</param>
        private static void ShowAndSave(Animal animal)
        {
            SkillTreePainter painter;
            if (animal == Animal.Player)
                painter = new PlayerSkillTreePainter(true);
            else
                painter = new AnimalSkillTreePainter(animal);

            try
            {
                RunForm(painter);
                GeneratePng(painter, string.Format("{0}.png", animal));
                GenerateEmf(painter, string.Format("{0}.emf", animal));
            }
            finally
            {
                if (painter != null)
                    painter.Dispose();
            }
        }

        /// <summary>
        /// Displays the skill tree in a Windows form.
        /// </summary>
        /// <param name="painter">The painter that generates the output.</param>
        private static void RunForm(SkillTreePainter painter)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm { Painter = painter });
        }

        /// <summary>
        /// Saves the skill tree as a Protable Network Graphics file.
        /// </summary>
        /// <param name="painter">The painter that generates the output.</param>
        /// <param name="path">The path to save the graphics to.</param>
        private static void GeneratePng(SkillTreePainter painter, string path)
        {
            var bounds = new Rectangle(0, 0, 1900, 1900); // 100 px/cm
            using (var image = new Bitmap(bounds.Width, bounds.Height))
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                painter.Paint(graphics, bounds);
                image.Save(path, ImageFormat.Png);
            }
        }

        /// <summary>
        /// Saves the skill tree as a Enhanced Metafile.
        /// </summary>
        /// <param name="painter">The painter that generates the output.</param>
        /// <param name="path">The path to save the graphics to.</param>
        private static void GenerateEmf(SkillTreePainter painter, string path)
        {
            var bounds = new Rectangle(0, 0, 1900, 1900); // 100 px/cm
            using (var image = NewEmf(bounds))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.PageUnit = GraphicsUnit.Pixel;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    painter.Paint(graphics, bounds);
                }
                SaveEmf(image, path);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Metafile"/> with the specified dimensions.
        /// </summary>
        /// <param name="bounds">The Metafile's dimensions.</param>
        /// <param name="unit">The units in which the bounds are specified.</param>
        /// <returns>The initialised Metafile.</returns>
        private static Metafile NewEmf(Rectangle bounds, MetafileFrameUnit unit = MetafileFrameUnit.Pixel)
        {
            using (var stream = new MemoryStream())
            using (var offScreenBufferGraphics = Graphics.FromHwndInternal(IntPtr.Zero))
            {
                IntPtr handle = offScreenBufferGraphics.GetHdc();
                var image = new Metafile(stream, handle, bounds, unit, EmfType.EmfPlusOnly);
                offScreenBufferGraphics.ReleaseHdc();
                return image;
            }
        }

        /// <summary>
        /// Saves the specified <see cref="Metafile"/> to the specified path.
        /// </summary>
        /// <param name="image">The Metafile to save.</param>
        /// <param name="path">The path to same the metafile to.</param>
        private static void SaveEmf(Metafile image, string path)
        {
            IntPtr iptrMetafileHandle = image.GetHenhmetafile();
            try
            {
                CopyEnhMetaFile(iptrMetafileHandle, path);
            }
            finally
            {
                DeleteEnhMetaFile(iptrMetafileHandle);
            }
        }

        /// <summary>
        /// Saves the specified Enhanced Metafile to a file.
        /// </summary>
        /// <param name="hemfSrc">The <see cref="Metafile"/> handle.</param>
        /// <param name="lpszFile">The path of the file to save to.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern IntPtr CopyEnhMetaFile(IntPtr hemfSrc, String lpszFile);

        /// <summary>
        /// Closes the Metafile handle.
        /// </summary>
        /// <param name="hemf">The handle to close.</param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        static extern int DeleteEnhMetaFile(IntPtr hemf);
    }
}
