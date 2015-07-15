using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Win
{
    public class BaseUserControl<T> : System.Windows.Controls.UserControl, IView<T> where T : ViewModel.Base
    {
        // Shared between Control and Page
        public BaseUserControl ( )
        {
            DataContextChanged += BasePage_DataContextChanged;
        }

        void BasePage_DataContextChanged ( object sender, System.Windows.DependencyPropertyChangedEventArgs e )
        {
            if ( m_inViewModelSet )
                return;

            if ( e.NewValue != null )
                ViewModel = Contract.AssertIsType<T> ( ( ) => e.NewValue, e.NewValue );
            else
                ViewModel = null;
        }

        protected virtual void OnViewModelSet ( T oldValue, T newValue )
        {
        }

        private bool m_inViewModelSet = false;
        private T m_viewModel;
        public T ViewModel
        {
            get { return m_viewModel; }
            set
            {
                var oldValue = m_viewModel;

                m_viewModel = value;

                m_inViewModelSet = true;
                DataContext = value;
                m_inViewModelSet = false;

                OnViewModelSet ( oldValue, value );
            }
        }

        public ViewModel.Base ViewModelObject
        {
            get { return ViewModel; }
            set
            {
                if ( value == null )
                    ViewModel = null;
                else
                {
                    ViewModel = Contract.AssertIsType<T> ( ( ) => value, value );
                }
            }
        }
    }
}
