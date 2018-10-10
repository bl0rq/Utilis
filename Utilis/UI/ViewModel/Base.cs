using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.ViewModel
{
    [System.Runtime.Serialization.DataContract]
    public abstract class Base : ObjectModel.BaseNotifyPropertyChanged
    {
        protected internal virtual void OnSecondaryNavigation ( )
        {
        }

        protected internal virtual void OnNavigatingAway ( System.ComponentModel.CancelEventArgs e )
        {
        }
    }
}
