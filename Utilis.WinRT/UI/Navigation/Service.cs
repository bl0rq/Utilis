using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    public class Service : IService
    {
        private readonly Windows.UI.Xaml.Controls.Frame m_frame;
        private readonly IViewMapper m_vm;
        private readonly Func<ViewModel.Base, bool> m_isFirstPage;

        public event Action Navigated;
        public ViewModel.Base CurrentViewModel { get; private set; }

        private void DoNavigated ( )
        {
            Action act = Navigated;
            if ( act != null )
                act ( );
        }

        public Service ( Windows.UI.Xaml.Controls.Frame frame, IViewMapper vm, Func<ViewModel.Base, bool> isFirstPage )
        {
            Contract.AssertNotNull ( ( ) => frame, frame );
            Contract.AssertNotNull ( ( ) => vm, vm );
            Contract.AssertNotNull ( ( ) => isFirstPage, isFirstPage );

            m_frame = frame;
            m_vm = vm;
            m_isFirstPage = isFirstPage;

            m_frame.Navigated += m_frame_Navigated;
        }

        void m_frame_Navigated ( object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e )
        {
            DoNavigated ( );
        }

        public bool CanGoBack ( )
        {
            return !m_isFirstPage ( CurrentViewModel ) && m_frame.CanGoBack;
        }

        public void GoBack ( )
        {
            m_frame.GoBack ( );

            var content = m_frame.Content as IView;
            if ( content != null )
                CurrentViewModel = content.ViewModelObject;
        }

        public void GoForward ( )
        {
            m_frame.GoForward ( );
        }

        public async Task<bool> Navigate<T_VM> ( T_VM parameter = null ) where T_VM : ViewModel.Base
        {
            bool result = false;
            //TODO: abstract this
            await m_frame.Dispatcher.RunAsync ( Windows.UI.Core.CoreDispatcherPriority.Normal, ( ) =>
            {
                CurrentViewModel = parameter;

                var viewModelType = typeof ( T_VM );

                var viewType = m_vm.GetView<T_VM> ( );
                if ( viewType == null )
                    throw new NavigationException ( "Unable to find view for type '" + viewModelType.FullName + "'." );

                result = m_frame.Navigate ( viewType, parameter );
            } );
            return result;
        }
    }
}
