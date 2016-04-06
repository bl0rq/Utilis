using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation.Win
{
    public class Service : IService
    {
        private readonly System.Windows.Controls.Frame m_frame;
        private readonly IViewMapper m_viewMapper;
        private readonly bool m_isSecondaryWindow;
        private bool m_removeBackStackEntryOnNavigate = false;

        public Service ( System.Windows.Controls.Frame frame, IViewMapper viewMapper, bool isSecondaryWindow = false )
        {
            m_frame = frame;
            m_frame.Navigated += m_frame_Navigated;
            m_viewMapper = viewMapper;
            m_isSecondaryWindow = isSecondaryWindow;
        }

        void m_frame_Navigated ( object sender, System.Windows.Navigation.NavigationEventArgs e )
        {
            DoNavigated ( );
        }

        public bool CanGoBack ( )
        {
            return m_frame.CanGoBack;
        }

        public void GoBack ( )
        {
            m_frame.GoBack ( );
        }

        public void RemoveBackEntry ( )
        {
            m_frame.RemoveBackEntry ( );
        }

        public void GoForward ( )
        {
            m_frame.GoForward ( );
        }

        public async Task<bool> NavigateAsync<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : ViewModel.Base
        {
            var viewType = GetViewType<T_VM> ( );

            bool navResult = false;

            await Runner.RunOnDispatcherThreadAsync ( ( ) =>
            {
                navResult = NavigateCore ( parameter, viewType );
            } );
            return navResult;
        }

        public bool Navigate<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : ViewModel.Base
        {
            var viewType = GetViewType<T_VM> ( );

            bool navResult = false;

            Runner.RunOnDispatcherThreadBlocking ( ( ) =>
            {
                navResult = NavigateCore ( parameter, viewType );
            } );
            return navResult;
        }

        public bool NavigateAndRemoveCurrentFromBackStack<T_VM> ( T_VM parameter = default ( T_VM ) ) where T_VM : ViewModel.Base
        {
            var viewType = GetViewType<T_VM> ( );

            bool result = false;
            Runner.RunOnDispatcherThreadBlocking ( ( ) =>
            {
                m_removeBackStackEntryOnNavigate = true;
                result = NavigateCore ( parameter, viewType );
            } );
            return result;
        }

        private bool NavigateCore<T_VM> ( T_VM parameter, Type viewType ) where T_VM : ViewModel.Base
        {
            bool navResult;
            var viewObject = Activator.CreateInstance ( viewType );
            var view = Contract.AssertIsType<IView> ( ( ) => viewObject, viewObject );

            DoPreNavigate ( parameter, typeof ( T_VM ), view, viewType );

            view.ViewModelObject = parameter;
            CurrentViewModel = parameter;

            navResult = m_frame.Navigate ( view );
            return navResult;
        }

        private Type GetViewType<T_VM> ( ) where T_VM : ViewModel.Base
        {
            var viewType = m_viewMapper.GetView<T_VM> ( );
            if ( viewType == null )
                throw new Exception ( "Unable to find view for ViewModel type '" + typeof ( T_VM ).Name + "'." );
            return viewType;
        }

        public event Action Navigated;

        private void DoNavigated ( )
        {
            var navigated = Navigated;
            navigated?.Invoke ( );

            if ( m_removeBackStackEntryOnNavigate )
            {
                m_removeBackStackEntryOnNavigate = false;
                RemoveBackEntry ( );
            }
        }

        public ViewModel.Base CurrentViewModel { get; private set; }

        public delegate void PreNavigateHandler ( object vm, Type vmType, object view, Type viewType );
        public event PreNavigateHandler PreNavigate;
        private void DoPreNavigate ( object vm, Type vmType, object view, Type viewType )
        {
            var preNavigate = PreNavigate;
            PreNavigate?.Invoke ( vm, vmType, view, viewType );
        }
    }
}
