using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Win
{
    public class DispatcherWrapper : IDispatcher
    {
        private readonly System.Windows.Threading.Dispatcher m_dispatcher;

        public DispatcherWrapper ( System.Windows.Threading.Dispatcher dispatcher )
        {
            Contract.AssertNotNull ( ( ) => dispatcher, dispatcher );
            m_dispatcher = dispatcher;
        }

        public System.Windows.Threading.Dispatcher RealDispatcher => m_dispatcher;

        public bool CheckAccess ( )
        {
            return m_dispatcher.CheckAccess ( );
        }

        public async Task RunAsync ( Action act )
        {
            await m_dispatcher.InvokeAsync ( act );
        }
    }
}
