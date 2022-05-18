
using MyApp1;
using System.Drawing;
using System.Drawing.Imaging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using MyApp1;
using System.Drawing.Imaging;
namespace MyApp1
{
    class ScreenCapture
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetDesktopWindow();

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public static System.Drawing.Image CaptureDesktop()
        {
            return CaptureWindow(GetDesktopWindow());
        }

        public static System.Drawing.Bitmap CaptureActiveWindow()
        {
            return CaptureWindow(GetForegroundWindow());
        }

        public static System.Drawing.Bitmap CaptureWindow(IntPtr handle)
        {
            var rect = new Rect();
            GetWindowRect(handle, ref rect);
            var bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            var result = new System.Drawing.Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            return result;
        }
    }
    class Program
    {
        static void CapturePictures()
        {
            Image ImageFile = ScreenCapture.CaptureDesktop();

            string imageName = "Image" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

            var folderPath = $@"C:\Users\{Environment.UserName}\Desktop\Pictures\";
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

            }
            var imagePath = Path.Combine(folderPath, imageName);
            ImageFile.Save(imagePath, ImageFormat.Png);

        }

        static void GetFiles()

        {

            DirectoryInfo di = new DirectoryInfo($@"C:\Users\{Environment.UserName}\Desktop\Pictures");
            FileInfo[] files = di.GetFiles("*.png");
            string str = "";
            foreach (FileInfo file in files)
            {
                Console.WriteLine(file.Name);
            }
        }

        static void Main(string[] args)
        {
            string choice;
            while (true)
            {
                Console.WriteLine("Enter your choice: s-for screenshot, b-for showing all image names, e-for exit");
                choice = Console.ReadLine();
                if (choice.ToLower() == "s")
                {
                    CapturePictures();
                }
                else if (choice.ToLower() == "b")
                {
                    GetFiles();
                }
                else
                {
                    break;
                }

            }
        }
    }
}