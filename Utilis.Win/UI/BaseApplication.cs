using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilis.Extensions;

namespace Utilis.UI.Win
{
    public abstract class BaseApplication<TBootStrapper> : System.Windows.Application, Messaging.IListener<Messaging.AppKillRequestedMessage>
         where TBootStrapper : Controller.IBootStrapper
    {
        protected TBootStrapper BootStrapper { get; private set; }

        protected abstract string GetAppContainerName ( );

        private string m_localAppData = null;
        protected virtual string LocalAppData
        {
            get
            {
                if ( m_localAppData == null )
                {
                    m_localAppData = System.IO.Path.Combine ( Environment.GetFolderPath ( Environment.SpecialFolder.LocalApplicationData ), GetAppContainerName ( ) );
                    if ( !System.IO.Directory.Exists ( m_localAppData ) )
                        System.IO.Directory.CreateDirectory ( m_localAppData );
                }

                return m_localAppData;
            }
        }

        protected BaseApplication ( )
        {
            HasSplash = true;

            AnsureAppDataIsCreated ( );

            Runner.Dispatcher = new Utilis.Win.DispatcherWrapper ( Dispatcher );

            Exit += App_Exit;

            Startup += App_Startup;
            DispatcherUnhandledException += BaseApplication_DispatcherUnhandledException;

            Messaging.Bus.Instance.ListenFor ( this );

            BootStrapper = CreateMaster ( );
        }

        void BaseApplication_DispatcherUnhandledException ( object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e )
        {
            //TODO: handle this correctly
            System.Windows.MessageBox.Show ( "Fatal Unhandled Exception: " + e.Exception.Message );
        }

        private SingletonChecker m_oSingletonChecker;
        private bool CheckForSingleton ( )
        {
            if ( IsSingleton )
            {
                m_oSingletonChecker = new SingletonChecker ( LocalAppData, this.GetType ( ) );

                var bIsFirst = m_oSingletonChecker.IsFirstInstance ( );

                if ( !bIsFirst )
                    Utilis.Win.NativeMethod.User32.SetForegroundWindow ( m_oSingletonChecker.GetProcess ( ).MainWindowHandle.ToInt32 ( ) );

                return bIsFirst;
            }
            else
                return true;
        }

        protected abstract TBootStrapper CreateMaster ( );
        protected abstract bool IsSingleton { get; }
        protected virtual void HandleArguments ( bool bIsFirstInstance, string [] aArgs )
        {

        }

        async void App_Startup ( object sender, System.Windows.StartupEventArgs e )
        {
            // this is a hack to let the any window w/ the word Blend in the name not go thru the startup nonsense.
            // this lets that different window be used as startup uri on app.xaml for quick "f5" runs out of blend.
            if ( StartupUri != null && StartupUri.ToString ( ).Contains ( "Blend" ) )
                return;

            bool bIsFirst = CheckForSingleton ( );
            HandleArguments ( bIsFirst, e.Args );

            if ( !bIsFirst )
            {
                this.Shutdown ( );
            }
            else
            {
                Messaging.Bus.Instance.Send ( new Messaging.AppStartedMessage ( ) );

                Runner.RunAsync ( ( ) => Start ( ) );
            }
        }

        private void AnsureAppDataIsCreated ( )
        {
            if ( !System.IO.Directory.Exists ( LocalAppData ) )
                System.IO.Directory.CreateDirectory ( LocalAppData );
        }

        private async Task Start ( )
        {
            try
            {
                await BootStrapper.StartAsync ( );
            }
            catch ( Exception e )
            {
                Logger.Log ( e, "Mjorq.Utility.WPF.BaseApplication.Start ( )" );
                Runner.RunOnDispatcherThread (
                    ( ) =>
                    {
                        System.Windows.MessageBox.Show ( "Error in Startup: " + e.Message );
                        Shutdown ( );
                    } );
            }

            // MainWindow at this point is the Splash screen and BootStrapper has done its thing to make a new one get shown
            if ( HasSplash )
                await Runner.RunOnDispatcherThreadAsync ( ( ) => { if ( MainWindow != null ) MainWindow.Close ( ); } );

            Messaging.Bus.Instance.Send ( new Messaging.StartupCompleteMessage ( ) );

            Logger.Log ( Messaging.StatusMessage.Types.Information, GetAppContainerName ( ) + " startup complete." );
        }

        void App_Exit ( object sender, System.Windows.ExitEventArgs e )
        {
            if ( StartupUri != null && StartupUri.ToString ( ).Contains ( "Blend" ) )
                return;

            Messaging.Bus.Instance.Send ( new Messaging.AppShutdownMessage ( ) );

            BootStrapper.DisposeWithCare ( );
            m_oSingletonChecker.DisposeWithCare ( );
        }

        public void Receive ( Messaging.AppKillRequestedMessage message )
        {
            Runner.RunOnDispatcherThread ( this.Shutdown );
        }

        public bool HasSplash { get; set; }
    }
}
