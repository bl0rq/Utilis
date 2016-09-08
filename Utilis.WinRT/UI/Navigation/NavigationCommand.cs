using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
    public static class NavigationCommands
    {
        static NavigationCommands ( )
        {
            try
            {
                Back = new DelegateCommand (
                    ( ) => ServiceLocator.Instance?.GetInstance<IService> ( )?.GoBack ( ),
                    ( ) => ServiceLocator.Instance?.GetInstance<IService> ( )?.CanGoBack ( ) ?? true );
            }
            catch ( NullReferenceException e )
            {
                // this should never actually happen...
                System.Diagnostics.Debug.WriteLine ( e );
            }
        }
        public static System.Windows.Input.ICommand Back { get; }

        internal static void Navigated ( )
        {
            ( (IDelegateCommand)Back ).FireCanExecuteChanged ( );
        }
    }

    public class NavigationCommand<T> : System.Windows.Input.ICommand where T : ViewModel.Base
    {
        private readonly Func<bool> m_canExecute;
        private readonly Func<T> m_createViewModel;
        private readonly IService m_service;

        public NavigationCommand ( Func<T> createViewModel, Navigation.IService service = null )
        {
            Contract.AssertNotNull ( ( ) => createViewModel, createViewModel );
            m_createViewModel = createViewModel;
            m_service = service;
        }

        public NavigationCommand ( Func<T> createViewModel, Func<bool> canExecute, Navigation.IService service = null )
            : this ( createViewModel, service )
        {
            m_canExecute = canExecute;
        }

        public bool CanExecute ( object parameter )
        {
            return m_canExecute == null || m_canExecute ( );
        }

        public void Execute ( object parameter )
        {
            var vm = m_createViewModel ( );
            if ( vm != null )
                ( m_service ?? ServiceLocator.Instance.GetInstance<Navigation.IService> ( ) ).Navigate ( vm );
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
                Runner.RunOnDispatcherThread ( ( ) => canExecuteChanged ( this, EventArgs.Empty ) );
        }
    }

    public class NavigationCommandAutoCreate<T> : NavigationCommand<T> where T : ViewModel.Base, new()
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
