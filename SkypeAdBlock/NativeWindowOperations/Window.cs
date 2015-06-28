using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SkypeAdBlock.NativeWindowOperations
{
    /// <summary>
    /// Операции с окнами (через user32.dll).
    /// </summary>
    public static class Window
    {
        [StructLayout(LayoutKind.Sequential)]
        [DebuggerDisplay("{Left}, {Top}, {Right}, {Bottom}")]
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        [DebuggerDisplay("{X}, {Y}")]
        public struct Point
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll")]
        static extern IntPtr GetParent(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);
        [DllImport("user32.dll")]
        static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        public static extern int ShowWindow(IntPtr hwnd, int command);
        [DllImport("user32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Закрыть.
        /// </summary>
        public static IntPtr GetConsoleHandle()
        {
            return GetConsoleWindow();
        }


        /// <summary>
        /// Найти окно по имени класса и заголовку.
        /// </summary>
        public static IntPtr Find(string className, string windowName)
        {
            return FindWindow(className, windowName);
        }


        /// <summary>
        /// Найти дочернее окно по имени класса и заголовку.
        /// </summary>
        public static IntPtr Find(IntPtr parentHandle, IntPtr childAfter, string className, string windowName)
        {
            return FindWindowEx(parentHandle, childAfter, className, windowName);
        }



        /// <summary>
        /// Закрыть.
        /// </summary>
        public static void Close(IntPtr handle)
        {
            const UInt32 WM_CLOSE = 0x0010;
            SendMessage(handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }


        /// <summary>
        /// Границы окна относительно родительского.
        /// </summary>
        public static Rect GetRelativeRect(IntPtr handle)
        {
            IntPtr parentHandle = GetParent(handle);

            Rect clientRect;
            GetClientRect(parentHandle, out clientRect);
            var p = new Point { X = clientRect.Left, Y = clientRect.Top };
            ClientToScreen(parentHandle, ref p);

            var rect = new Rect();
            GetWindowRect(handle, ref rect);

            rect.Left -= p.X;
            rect.Top -= p.Y;
            rect.Right -= p.X;
            rect.Bottom -= p.Y;

            return rect;
        }


        /// <summary>
        /// Размер родительского окна.
        /// </summary>
        public static Point GetParentWindowSize(IntPtr handle)
        {
            IntPtr parentHandle = GetParent(handle);

            Rect clientRect;
            GetClientRect(parentHandle, out clientRect);
            return new Point { X = clientRect.Right - clientRect.Left, Y = clientRect.Bottom - clientRect.Top };
        }


        /// <summary>
        /// Переместить окно.
        /// </summary>
        public static void Move(IntPtr handle, int x, int y, int width, int height, bool repaint = true)
        {
            MoveWindow(handle, x, y, width, height, repaint);
        }


        /// <summary>
        /// Скрыть.
        /// </summary>
        public static void Hide(IntPtr handle)
        {
            const int SW_HIDE = 0;
            ShowWindow(handle, SW_HIDE);
        }

        /// <summary>
        /// Показать.
        /// </summary>
        public static void Show(IntPtr handle)
        {
            const int SW_SHOW = 5;
            ShowWindow(handle, SW_SHOW);
        }

        /// <summary>
        /// Поднять поверх других окон.
        /// </summary>
        public static void SetForeground(IntPtr handle)
        {
            SetForegroundWindow(handle);
        }
    }
}
