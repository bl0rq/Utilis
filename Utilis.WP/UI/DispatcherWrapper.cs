using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.WP.UI
{
	public class DispatcherWrapper : IDispatcher
	{
		private readonly System.Windows.Threading.Dispatcher m_dispatcher;

		public DispatcherWrapper ( System.Windows.Threading.Dispatcher dispatcher )
		{
			Contract.AssertNotNull ( ( ) => dispatcher, dispatcher );

			m_dispatcher = dispatcher;
		}

		public bool CheckAccess ( )
		{
			return m_dispatcher.CheckAccess ( );
		}

		public async Task RunAsync ( Action act )
		{
			await Task.Run ( ( ) =>
			{
				System.Threading.ManualResetEvent evt = new System.Threading.ManualResetEvent ( false );
				m_dispatcher.BeginInvoke ( ( ) =>
				{
					act ( );
					evt.Set ( );
				} );
				evt.WaitOne ( );
			} );
		}

		public void BeginInvoke ( Action act )
		{
			m_dispatcher.BeginInvoke ( act );
		}
	}
}
