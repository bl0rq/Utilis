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
    }
}
