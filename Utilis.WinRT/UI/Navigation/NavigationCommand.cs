using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    public class NavigationCommand<T> : System.Windows.Input.ICommand where T : ViewModel.Base
    {
        private readonly Func<bool> m_canExecute;
        private readonly Func<T> m_createViewModel;

        public NavigationCommand ( Func<T> createViewModel )
        {
            Contract.AssertNotNull ( ( ) => createViewModel, createViewModel );
            m_createViewModel = createViewModel;
        }

        public NavigationCommand ( Func<T> createViewModel, Func<bool> canExecute )
            : this ( createViewModel )
        {
            m_canExecute = canExecute;
        }

        public bool CanExecute ( object parameter )
        {
            return m_canExecute == null || m_canExecute ( );
        }

        public void Execute ( object parameter )
        {
            ServiceLocator.Instance.GetInstance<Navigation.IService> ( ).Navigate ( m_createViewModel ( ) );
        }

        public void FireCanExecuteChanged ( )
        {
            DoCanExecuteChanged ( );
        }
        public event EventHandler CanExecuteChanged;

        private void DoCanExecuteChanged ( )
        {
            var canExecuteChanged = CanExecuteChanged;
            if ( canExecuteChanged != null )
                canExecuteChanged ( this, new EventArgs ( ) );
        }
    }

    public class NavigationCommandAutoCreate<T> : NavigationCommand<T> where T : ViewModel.Base, new ( )
    {
        public NavigationCommandAutoCreate ( )
            : base ( ( ) => new T ( ) )
        {
        }

        public NavigationCommandAutoCreate ( Func<bool> canExecute )
            : base ( ( ) => new T ( ), canExecute )
        {
        }
    }

    public class NavigationCommandResolve<T> : NavigationCommand<T> where T : ViewModel.Base
    {
        public NavigationCommandResolve ( )
            : base ( ResolveInstance )
        {
        }

        public NavigationCommandResolve ( Func<bool> canExecute )
            : base ( ResolveInstance, canExecute )
        {
        }

        private static T ResolveInstance ( )
        {
            T instance = ServiceLocator.Instance.GetInstance<T> ( );
            if ( instance == null )
                throw new InvalidOperationException ( "Unable to resolve type '" + typeof ( T ) + "'." );
            else
                return instance;
        }
    }
}
