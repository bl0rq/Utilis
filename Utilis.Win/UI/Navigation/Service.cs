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
        private bool m_isFirstNavigation = true;
        private bool m_didIDoThat = false;

        public Service ( System.Windows.Controls.Frame frame, IViewMapper viewMapper, bool isSecondaryWindow = false )
        {
            m_frame = frame;
            m_frame.Navigated += m_frame_Navigated;
            m_frame.Navigating += m_frame_Navigating;
            m_viewMapper = viewMapper;
            m_isSecondaryWindow = isSecondaryWindow;
        }

        private void m_frame_Navigating ( object sender, System.Windows.Navigation.NavigatingCancelEventArgs e )
        {
            if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back || e.NavigationMode == System.Windows.Navigation.NavigationMode.Forward )
                CurrentViewModel?.OnNavigatingAway ( e );
        }

        void m_frame_Navigated ( object sender, System.Windows.Navigation.NavigationEventArgs e )
        {
            //Logger.Log ( Messaging.StatusMessage.Types.Debug, $"m_frame_Navigated.  e.Content: {e.Content}, e.Uri: {e.Uri}, m_didIDoThat: {m_didIDoThat}, m_isFirstNavigation: {m_isFirstNavigation}, Back Stack: {this.BackStackToString ( )}" );
            if ( !m_didIDoThat )
            {
                var view = m_frame.Content as IView;
                if ( view != null )
                {
                    CurrentViewModel = view.ViewModelObject;
                    view.ViewModelObject?.OnSecondaryNavigation ( );
                }
                else
                {
                    CurrentViewModel = null;
                    if ( m_frame.Content != null )
                        Logger.Log ( Messaging.StatusMessage.Types.Warn, "Unable to cast Navigation Frame's Content to a IView.  Type is '" + m_frame.Content.GetType ( ).FullName + "'" );
                }
            }
            else
                m_didIDoThat = false;

            DoNavigated ( );

            if ( m_isFirstNavigation && CurrentViewModel != null )
            {
                //m_removeBackStackEntryOnNavigate = true; // this will cause the first time we get navigated to to remove the previous page which should be a splash/loading page
                Logger.Log ( Messaging.StatusMessage.Types.Debug, $"Nuking back stack on first run... \r\n{BackStackToString ( )}" );

                m_isFirstNavigation = false;
                while ( m_frame.CanGoBack )
                    RemoveBackEntry ( );
            }

            NavigationCommands.Navigated ( );
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

            m_didIDoThat = true;
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
                Logger.Log ( Messaging.StatusMessage.Types.Debug, "Removing back entry on navigate.  Back Stack: " + BackStackToString ( ) );
                m_removeBackStackEntryOnNavigate = false;
                RemoveBackEntry ( );
            }
        }

        private string BackStackToString ( string separator = "\r\n" )
        {
            if ( m_frame == null )
                return "{null frame}";
            else if ( m_frame.BackStack == null )
                return "{null}";

            StringBuilder sb = new StringBuilder ( );
            int count = 0;
            foreach ( var o in m_frame.BackStack )
            {
                sb.Append ( count ).Append ( " : " ).Append ( o ).Append ( separator );
                count++;
            }
            return sb.ToString ( );
        }

        public ViewModel.Base CurrentViewModel { get; private set; }

        public delegate void PreNavigateHandler ( object vm, Type vmType, object view, Type viewType );
        public event PreNavigateHandler PreNavigate;
        private void DoPreNavigate ( object vm, Type vmType, object view, Type viewType )
        {
            var preNavigate = PreNavigate;
            preNavigate?.Invoke ( vm, vmType, view, viewType );
        }
    }
}
