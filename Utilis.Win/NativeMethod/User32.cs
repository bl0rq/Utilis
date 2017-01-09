using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Win.NativeMethod
{
    public static class User32
    {
        [System.Runtime.InteropServices.DllImport ( "user32.dll" )]
        public static extern IntPtr GetForegroundWindow ( );

        [System.Runtime.InteropServices.DllImport ( "User32.dll" )]
        public static extern Int32 SetForegroundWindow ( int hWnd );

        [System.Runtime.InteropServices.DllImport ( "user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, ExactSpelling = true )]
        private static extern int GetSystemMetrics ( int nIndex );
        public static bool IsTerminalServerSession
        {
            get
            {
                return ( ( GetSystemMetrics ( 0x1000 ) & 1 ) != 0 );
            }
        }

        [System.Runtime.InteropServices.DllImport ( "user32.dll" )]
        public static extern bool SetWindowPlacement ( IntPtr hWnd, [System.Runtime.InteropServices.In] ref WINDOWPLACEMENT lpwndpl );

        [System.Runtime.InteropServices.DllImport ( "user32.dll" )]
        public static extern bool GetWindowPlacement ( IntPtr hWnd, out WINDOWPLACEMENT lpwndpl );
    }


    [Serializable]
    [System.Runtime.InteropServices.StructLayout ( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public RECT ( int left, int top, int right, int bottom )
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }
    }

    // POINT structure required by WINDOWPLACEMENT structure
    [Serializable]
    [System.Runtime.InteropServices.StructLayout ( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT ( int x, int y )
        {
            this.X = x;
            this.Y = y;
        }
    }

    // WINDOWPLACEMENT stores the position, size, and state of a window
    [Serializable]
    [System.Runtime.InteropServices.StructLayout ( System.Runtime.InteropServices.LayoutKind.Sequential )]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public POINT minPosition;
        public POINT maxPosition;
        public RECT normalPosition;

        public const int SW_HIDE = 0;
        public const int SW_SHOWNORMAL = 1;
        public const int SW_NORMAL = 1;
        public const int SW_SHOWMINIMIZED = 2;
        public const int SW_SHOWMAXIMIZED = 3;
        public const int SW_MAXIMIZE = 3;
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_SHOWMINNOACTIVE = 7;
        public const int SW_SHOWNA = 8;
        public const int SW_RESTORE = 9;
    }
}
