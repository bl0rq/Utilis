using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis
{
	public static class Runner
	{
		public static bool AsyncEnabled { get; set; }
		public static IDispatcher Dispatcher { get; set; }
		public static Action<Action, AsyncCallback> AsyncCall { get; set; }

		public static event Action<Exception, string> Error;
		private static bool DoError ( Exception ex, string sContextName )
		{
			Logger.Log ( ex, sContextName );

			Action<Exception, string> handler = Error;
			if ( handler != null )
			{
				handler ( ex, sContextName );
				return true;
			}
			else
				return false;
		}

		static Runner ( )
		{
			AsyncEnabled = true;
			AsyncCall = ( act, callback ) => act.BeginInvoke ( callback, null );
		}

		public static void RunAsync ( Action act, AsyncCallback callback = null )
		{
			RunAsync (
				act,
				callback,
				ex => DoError ( ex, "Runner.RunAsync" ) );
		}

		public static void RunAsync ( Action act, AsyncCallback callback, Action<Exception> fnErrorHandler )
		{
			RunAsync ( act, callback, fnErrorHandler == null ? (Func<Exception, bool>)null : ex => { fnErrorHandler ( ex ); return true; } );
		}

		public static void RunAsync ( Action act, AsyncCallback callback, Func<Exception, bool> fnErrorHandler )
		{
			try
			{
				Action wrapper = ( ) => RunWrapped ( act );
				if ( AsyncEnabled )
					AsyncCall ( wrapper, callback );
				else
				{
					wrapper ( );
					if ( callback != null )
						callback ( null );
				}
			}
			catch ( Exception ex )
			{
				if ( !fnErrorHandler ( ex ) )
					throw;
			}
		}

		public static async Task RunOnDispatcherThread ( Action act )
		{
			if ( Dispatcher.CheckAccess ( ) )
				RunWrapped ( act );
			else
				await Dispatcher.RunAsync ( ( ) => RunWrapped ( act ) );
		}

		public static void RunWrapped ( Action act )
		{
			try
			{
				act ( );
			}
			catch ( Exception ex )
			{
				if ( !DoError ( ex, "Runner.RunWrapped" ) )
					throw;
			}
		}

		public static void RunReadLocked ( Action act, System.Threading.ReaderWriterLockSlim oLock )
		{
			oLock.EnterReadLock ( );
			try
			{
				act ( );
			}
			finally
			{
				oLock.ExitReadLock ( );
			}
		}

		public static void RunWriteLocked ( Action act, System.Threading.ReaderWriterLockSlim oLock )
		{
			oLock.EnterWriteLock ( );
			try
			{
				act ( );
			}
			finally
			{
				oLock.ExitWriteLock ( );
			}
		}

		public static T RunReadLocked<T> ( Func<T> act, System.Threading.ReaderWriterLockSlim oLock )
		{
			oLock.EnterReadLock ( );
			try
			{
				return act ( );
			}
			finally
			{
				oLock.ExitReadLock ( );
			}
		}

		public static T RunWriteLocked<T> ( Func<T> act, System.Threading.ReaderWriterLockSlim oLock )
		{
			oLock.EnterWriteLock ( );
			try
			{
				return act ( );
			}
			finally
			{
				oLock.ExitWriteLock ( );
			}
		}
	}
}