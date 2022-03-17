using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;

namespace FocuSpider
{
    class SonyWindowClicker
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int VK_PHOTO = 0x61;

        public static bool DoPhoto()
        {
            IntPtr hWnd = FindWindow(null, "Remote");
            if (hWnd != IntPtr.Zero)
            {
                PostMessage(hWnd, WM_KEYDOWN, VK_PHOTO, 0);
                Thread.Sleep(10);
                PostMessage(hWnd, WM_KEYUP, VK_PHOTO, 0);
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
