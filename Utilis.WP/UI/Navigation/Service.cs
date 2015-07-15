using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    public class Service : IService
    {
        private readonly System.Windows.Controls.Frame m_frame;
        private readonly IViewMapper m_viewMapper;
        private readonly ITypeToUriMapper m_typeToUriMapper;

        public Service ( System.Windows.Controls.Frame frame, IViewMapper viewMapper, ITypeToUriMapper typeToUriMapper )
        {
            Contract.AssertNotNull ( ( ) => frame, frame );
            Contract.AssertNotNull ( ( ) => viewMapper, viewMapper );
            Contract.AssertNotNull ( ( ) => typeToUriMapper, typeToUriMapper );

            m_frame = frame;
            m_frame.Navigated += m_frame_Navigated;
            m_viewMapper = viewMapper;
            m_typeToUriMapper = typeToUriMapper;
        }

        void m_frame_Navigated ( object sender, System.Windows.Navigation.NavigationEventArgs e )
        {
            if ( e.NavigationMode != System.Windows.Navigation.NavigationMode.New )
                return;

            if ( m_frame.CurrentSource == m_lastUri )
            {
                var page = m_frame.Content as IView;
                if ( page == null )
                    throw new Exception (
                        string.Format ( "Invalid view type.  Was {0} for ViewModel type: {1}", ( m_frame == null ? "{null}" : m_frame.Content.GetType ( ).FullName ), m_viewModel.GetType ( ).FullName ) );
                else
                    page.ViewModelObject = m_viewModel;
            }
            //else if ( e.NavigationMode == System.Windows.Navigation.NavigationMode.Back && m_frame.CurrentSource.ToString ( ).Contains ( "Bootstrap" ) )
            //	m_frame.GoBack ( );
            //else // if ( m_frame.CurrentSource.ToString ( ).Contains ( "Bootstrap" ) )
            //	Logger.Log ( "Wrong content in frame!", m_frame.CurrentSource + " vs " + m_lastUri );
        }

        public bool CanGoBack ( )
        {
            return m_frame.CanGoBack;
        }

        public void GoBack ( )
        {
            m_frame.GoBack ( );
        }

        public bool CanGoForward ( )
        {
            return m_frame.CanGoForward;
        }

        public void GoForward ( )
        {
            m_frame.GoForward ( );
        }

        private Uri m_lastUri;
        private ViewModel.Base m_viewModel;
        private bool m_isFirst = true;

        public async Task<bool> Navigate<T_VM> ( T_VM vm = null ) where T_VM : ViewModel.Base
        {
            if ( m_isFirst )
                m_isFirst = false;

            bool navigated = false;
            await Runner.RunOnDispatcherThread (
                ( ) =>
                {
                    Type viewType = m_viewMapper.GetView<T_VM> ( );
                    var viewUri = m_typeToUriMapper.Map ( viewType );

                    if ( m_frame.Navigate ( viewUri ) )
                    {
                        m_lastUri = viewUri;
                        m_viewModel = vm;
                        CurrentViewModel = vm;
                        navigated = true;
                        DoNavigated ( );
                    }
                } );

            return navigated;
        }

        public event Action Navigated;

        private void DoNavigated ( )
        {
            var navigated = Navigated;
            if ( navigated != null )
                navigated ( );
        }

        public ViewModel.Base CurrentViewModel
        {
            get;
            private set;
        }
    }
}
