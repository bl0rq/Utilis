using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Win.Extensions
{
    public static class WindowExtensions
    {
        public static void SetPlacement ( this System.Windows.Window window, string placementXml )
        {
            UI.WindowPlacement.SetPlacement ( new System.Windows.Interop.WindowInteropHelper ( window ).Handle, placementXml );
        }

        public static string GetPlacement ( this System.Windows.Window window )
        {
            return UI.WindowPlacement.GetPlacement ( new System.Windows.Interop.WindowInteropHelper ( window ).Handle );
        }
    }
}
