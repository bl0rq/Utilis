using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    /// <summary>
    /// This is a fake navigation service to be used for testing, design time, and whatnot.
    /// </summary>
    public class FakeNavigationService : Utilis.UI.Navigation.IService
    {
        public bool CanGoBack ( )
        {
            return true;
        }

        public void GoBack ( )
        {

        }

        public void RemoveBackEntry ( )
        {

        }

        public void GoForward ( )
        {

        }

        public Task<bool> NavigateAsync<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : Utilis.UI.ViewModel.Base
        {
            CurrentViewModel = parameter;
            return Task.FromResult ( true );
        }

        public bool Navigate<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : Utilis.UI.ViewModel.Base
        {
            CurrentViewModel = parameter;
            return true;
        }

        public bool NavigateAndRemoveCurrentFromBackStack<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : Utilis.UI.ViewModel.Base
        {
            return Navigate ( parameter );
        }

        public Utilis.UI.ViewModel.Base CurrentViewModel { get; private set; }
        public event Action Navigated;
    }
}
