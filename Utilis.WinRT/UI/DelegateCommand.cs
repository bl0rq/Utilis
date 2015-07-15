using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI
{
    public interface IDelegateCommand : System.Windows.Input.ICommand
    {
        void FireCanExecuteChanged ( );
    }

    public class DelegateCommand : IDelegateCommand
    {
        public bool Async { get; set; }

        private bool m_bIsExecuting = false;
        private bool IsExecuting
        {
            get
            {
                return m_bIsExecuting;
            }
            set
            {
                if ( m_bIsExecuting != value )
                {
                    m_bIsExecuting = value;
                    FireCanExecuteChanged ( );
                }
            }
        }

        public DelegateCommand ( Action executeMethod )
            : this ( executeMethod, null )
        {
        }

        public DelegateCommand ( Action executeMethod, Func<bool> canExecuteMethod )
        {
            Async = false;

            if ( executeMethod == null )
            {
                throw new ArgumentNullException ( "executeMethod" );
            }

            m_actExecuteMethod = executeMethod;
            m_actCanExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute ( object parameter )
        {
            return !IsExecuting && CanExecute ( );
        }

        public bool CanExecute ( )
        {
            if ( IsExecuting )
                return false;

            if ( m_actCanExecuteMethod != null )
                return m_actCanExecuteMethod ( );
            return true;
        }

        public void Execute ( object parameter )
        {
            Execute ( );
        }

        public void Execute ( )
        {
            if ( m_actExecuteMethod != null )
            {
                IsExecuting = true;

                if ( Async )
                    Runner.RunAsync ( m_actExecuteMethod, B => IsExecuting = false );
                else
                {
                    m_actExecuteMethod ( );
                    IsExecuting = false;
                }
            }
        }

        private readonly Action m_actExecuteMethod = null;
        private readonly Func<bool> m_actCanExecuteMethod = null;

        public event EventHandler CanExecuteChanged;
        public void FireCanExecuteChanged ( )
        {
            EventHandler action = CanExecuteChanged;
            if ( action != null )
                //action ( this, EventArgs.Empty );
                Runner.RunOnDispatcherThread ( ( ) => action ( this, EventArgs.Empty ) );
        }
    }

    public class ArgumentDelegateCommand : ArgumentDelegateCommand<object>
    {
        public ArgumentDelegateCommand ( Action<object> executeMethod )
            : base ( executeMethod )
        {
        }

        public ArgumentDelegateCommand ( Action<object> executeMethod, Func<object, bool> canExecuteMethod )
            : base ( executeMethod, canExecuteMethod )
        {
        }
    }

    public class ArgumentDelegateCommand<T> : IDelegateCommand
    {
        public bool Async { get; set; }

        private bool m_bIsExecuting = false;
        private bool IsExecuting
        {
            get
            {
                return m_bIsExecuting;
            }
            set
            {
                if ( m_bIsExecuting != value )
                {
                    m_bIsExecuting = value;
                    FireCanExecuteChanged ( );
                }
            }
        }

        public ArgumentDelegateCommand ( Action<T> executeMethod )
            : this ( executeMethod, null )
        {
            Async = true;
        }

        public ArgumentDelegateCommand ( Action<T> executeMethod, Func<T, bool> canExecuteMethod )
        {
            if ( executeMethod == null )
                throw new ArgumentNullException ( "executeMethod" );

            m_actExecuteMethod = executeMethod;
            m_actCanExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute ( object parameter )
        {
            if ( IsExecuting )
                return false;

            if ( m_actCanExecuteMethod != null )
                return m_actCanExecuteMethod ( (T)parameter );

            return true;
        }

        public void Execute ( object parameter )
        {
            //T o = Contract.AssertIsType<T> ( ( ) => parameter, parameter );

            if ( m_actExecuteMethod != null )
            {
                IsExecuting = true;

                if ( Async )
                    Runner.RunAsync ( ( ) => m_actExecuteMethod ( (T)parameter ), B => IsExecuting = false );
                else
                {
                    m_actExecuteMethod ( (T)parameter );
                    IsExecuting = false;
                }
            }
        }

        private readonly Action<T> m_actExecuteMethod = null;
        private readonly Func<T, bool> m_actCanExecuteMethod = null;

        public event EventHandler CanExecuteChanged;
        public void FireCanExecuteChanged ( )
        {
            EventHandler action = CanExecuteChanged;
            if ( action != null )
                action ( this, EventArgs.Empty );
            //Runner.RunOnDispatcherThread ( ( ) => action ( this, EventArgs.Empty ), System.Windows.Application.Current.Dispatcher );
        }
    }
}
