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

        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        internal static extern uint SendInput(uint nInputs, [MarshalAs(UnmanagedType.LPArray), In] INPUT[] pInputs, int cbSize);

        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        private static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hWnd);

        const int WM_LBUTTONDOWN = 0x201;
        const int WM_LBUTTONUP = 0x202;

#pragma warning disable 649
        internal struct INPUT
        {
            public UInt32 Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        internal struct MOUSEINPUT
        {
            public Int32 X;
            public Int32 Y;
            public UInt32 MouseData;
            public UInt32 Flags;
            public UInt32 Time;
            public IntPtr ExtraInfo;
        }

#pragma warning restore 649
        public static int _makeParam(int p, int p_2)
        {
            return ((p_2 << 16) | (p & 0xFFFF));
        }

        public static void DoClick()
        {
            IntPtr hWnd = FindWindow(null, "Számológép");
            Console.WriteLine(hWnd);
            SetForegroundWindow(hWnd);

            PostMessage(hWnd, WM_LBUTTONDOWN, 0, _makeParam(400, 400));
            Thread.Sleep(10);
            PostMessage(hWnd, WM_LBUTTONUP, 0, _makeParam(300, 300));
        }

        //public static void ClickOnPoint(IntPtr wndHandle, Point clientPoint)
        //{
        //    var oldPos = System.Windows.Input.Cursor.Position;

        //    /// get screen coordinates
        //    ClientToScreen(wndHandle, ref clientPoint);

        //    /// set cursor on coords, and press mouse
        //    Cursor.Position = new Point(clientPoint.X, clientPoint.Y);

        //    var inputMouseDown = new INPUT();
        //    inputMouseDown.Type = 0; /// input type mouse
        //    inputMouseDown.Data.Mouse.Flags = 0x0002; /// left button down

        //    var inputMouseUp = new INPUT();
        //    inputMouseUp.Type = 0; /// input type mouse
        //    inputMouseUp.Data.Mouse.Flags = 0x0004; /// left button up

        //    var inputs = new INPUT[] { inputMouseDown, inputMouseUp };
        //    SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));

        //    /// return mouse 
        //    Cursor.Position = oldPos;
        //}
    }
}
