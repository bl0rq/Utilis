using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    public class NavigationException : Exception
    {
        public NavigationException ( string s )
            : base ( s )
        {
        }
    }
}
