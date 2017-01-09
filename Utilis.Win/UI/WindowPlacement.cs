using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Utilis.Win.UI
{
    public static class WindowPlacement
    {
        private static Encoding encoding = new UTF8Encoding ( );
        private static XmlSerializer serializer = new XmlSerializer ( typeof ( NativeMethod.WINDOWPLACEMENT ) );

        public static void SetPlacement ( IntPtr windowHandle, string placementXml )
        {
            if ( string.IsNullOrEmpty ( placementXml ) )
            {
                return;
            }

            byte [] xmlBytes = encoding.GetBytes ( placementXml );

            try
            {
                NativeMethod.WINDOWPLACEMENT placement;
                using ( MemoryStream memoryStream = new MemoryStream ( xmlBytes ) )
                {
                    placement = (NativeMethod.WINDOWPLACEMENT)serializer.Deserialize ( memoryStream );
                }

                placement.length = Marshal.SizeOf ( typeof ( NativeMethod.WINDOWPLACEMENT ) );
                placement.flags = 0;
                placement.showCmd = ( placement.showCmd == NativeMethod.WINDOWPLACEMENT.SW_SHOWMINIMIZED ? NativeMethod.WINDOWPLACEMENT.SW_SHOWNORMAL : placement.showCmd );
                NativeMethod.User32.SetWindowPlacement ( windowHandle, ref placement );
            }
            catch ( InvalidOperationException )
            {
                // Parsing placement XML failed. Fail silently.
            }
        }

        public static string GetPlacement ( IntPtr windowHandle )
        {
            NativeMethod.WINDOWPLACEMENT placement;
            NativeMethod.User32.GetWindowPlacement ( windowHandle, out placement );

            using ( MemoryStream memoryStream = new MemoryStream ( ) )
            {
                using ( XmlTextWriter xmlTextWriter = new XmlTextWriter ( memoryStream, Encoding.UTF8 ) )
                {
                    serializer.Serialize ( xmlTextWriter, placement );
                    byte [] xmlBytes = memoryStream.ToArray ( );
                    return encoding.GetString ( xmlBytes );
                }
            }
        }
    }
}
