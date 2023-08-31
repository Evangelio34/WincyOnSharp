using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WindowOpacityChanger
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool SetLayeredWindowAttributes(IntPtr hWnd, uint crKey, byte bAlpha, uint dwFlags);

        private const int GWL_EXSTYLE = -20;
        private const uint WS_EX_LAYERED = 0x80000;
        private const uint LWA_ALPHA = 0x2;

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        struct WindowInfo
        {
            public IntPtr Handle;
            public string Title;
        }

        static List<WindowInfo> windowList = new List<WindowInfo>();
        static string ownWindowTitle = "Wincy";

        static bool EnumWindowsCallback(IntPtr hWnd, IntPtr lParam)
        {
            StringBuilder windowTitle = new StringBuilder(256);
            GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

            if (IsWindowVisible(hWnd) && windowTitle.Length > 0)
            {
                if (ownWindowTitle != windowTitle.ToString() && hWnd != IntPtr.Zero)
                {
                    WindowInfo windowInfo = new WindowInfo
                    {
                        Handle = hWnd,
                        Title = windowTitle.ToString()
                    };
                    windowList.Add(windowInfo);
                }
            }

            return true;
        }

        static void SetWindowOpacity(IntPtr hWnd, byte opacity)
        {
            SetWindowLong(hWnd, GWL_EXSTYLE, (int)(GetWindowLong(hWnd, GWL_EXSTYLE) | WS_EX_LAYERED));
            SetLayeredWindowAttributes(hWnd, 0, opacity, LWA_ALPHA);
        }

        static void Main(string[] args)
        {
            bool continuar = false;
            EnumWindows(EnumWindowsCallback, IntPtr.Zero);
            Console.Clear();
            Console.WriteLine("__          ___                   ");
            Console.WriteLine(" . .        / (_)                 ");
            Console.WriteLine("  . .  /.  / / _ _ __   ___ _   _ ");
            Console.WriteLine("   . ./  ./ / | | '_ . / __| | | |");
            Console.WriteLine("    .  /.  /  | | | | | (__| |_| |");
            Console.WriteLine("     ./  ./   |_|_| |_|.___|.__, |");
            Console.WriteLine("                             __/ |");
            Console.WriteLine("                            |___/ ");
            Console.WriteLine("                              On C#  ");
            int openedWindowsCount = windowList.Count;
Console.WriteLine($"Ventanas Abiertas: {openedWindowsCount}");


            do
            {
                for (int i = 0; i < windowList.Count; i++)
                {
                    Console.WriteLine("\n[" + (i + 1) + "] " + windowList[i].Title);
                }

                int selectedWindowNumber;
                Console.WriteLine("\nIngrese el numero de la ventana a la que desea ajustar la transparencia: ");
                selectedWindowNumber = Convert.ToInt32(Console.ReadLine());

                if (selectedWindowNumber >= 1 && selectedWindowNumber <= windowList.Count)
                {
                    IntPtr selectedWindowHandle = windowList[selectedWindowNumber - 1].Handle;

                    Console.WriteLine("Ingrese el nivel de opacidad (0-255): ");
                    int opacity = Convert.ToInt32(Console.ReadLine());

                    SetWindowOpacity(selectedWindowHandle, (byte)opacity);
                    Console.WriteLine("Transparencia de la ventana ajustada.");
                }
                else
                {
                    Console.WriteLine("Numero de ventana inválido.");
                }

                Console.WriteLine("¿Desea continuar? (S/N): ");
                char respuesta = Console.ReadKey().KeyChar;
                continuar = (respuesta == 'S' || respuesta == 's');

            } while (continuar);

            Console.Clear();
            Console.WriteLine("\u3053\u306E\u7D20\u4E00\u3064\u306E\u4E16\u754C\u306B\u795D\u798F\u3092\uFF01 is the Best Anime"); // "この素晴らしい世界に祝福を！"
            Thread.Sleep(3000);
            Environment.Exit(0);
        }
    }
}
