using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Utilis.Extensions;

namespace Utilis.Messaging
{
    public interface IListener<T> where T : IMessage
    {
        void Receive ( T message );
    }

    public class Bus
    {
        private static readonly Bus ms_instance = new Bus ( );
        public static Bus Instance
        {
            get
            {
                return ms_instance;
            }
        }

        private readonly Dictionary<Type, List<object>> m_listenersByMessageType = new Dictionary<Type, List<object>> ( );
        private readonly System.Threading.ReaderWriterLockSlim m_rwlListeners = new System.Threading.ReaderWriterLockSlim ( );

        private Bus ( )
        {

        }


        public void Send<T> ( T message ) where T : IMessage
        {
            Contract.AssertNotNull ( ( ) => message, message );

            Array listeners = null;
            m_rwlListeners.ReadLocked ( ( ) =>
            {
                System.Collections.IList listenersRaw = m_listenersByMessageType.SafeGet ( message.GetType ( ) );
                if ( listenersRaw != null )
                {
                    lock ( listenersRaw )
                    {
                        if ( listenersRaw.Count > 0 )
                        {
                            listeners = new object [ listenersRaw.Count ];
                            listenersRaw.CopyTo ( listeners, 0 );
                        }
                    }
                }
            } );

            if ( listeners != null )
            {
                LogMessage ( message, listeners );

                foreach ( IListener<T> listener in listeners )
                {
                    // clousure binding fixup
                    IListener<T> listenerCopy = listener;

                    if ( message.IsAsync )
                        Runner.RunAsync ( ( ) => listenerCopy.Receive ( message ) );
                    else
                        Runner.RunWrapped ( ( ) => listenerCopy.Receive ( message ) );
                }
            }
            else if ( typeof ( T ) != typeof ( StatusMessage ) )
                Logger.Log (
                    StatusMessage.Types.Debug,
                    string.Format ( "Noone Listening for Message : Type = '{0}', Message = '{1}'.",
                        typeof ( T ).FullName,
                        message ),
                    null,
                    "BandSox.Utility.Messaging.Bus.Send<T>()" );

            Fabric?.Send ( message );
        }

        private void LogMessage<T> ( T message, Array listeners )
        {
            if ( IsMessageLoggingEnabled
                && typeof ( T ) != typeof ( StatusMessage ) // this stops the loop thing from looping (status sends status sends...)
                && !typeof ( T ).HasAttribute<TrivialAttribute> ( ) )
                Logger.Log (
                    StatusMessage.Types.Debug,
                    string.Format (
                        "Sending Message : Type = '{0}', Message = '{1}' to {2} Listners.",
                        typeof ( T ).FullName,
                        message,
                        listeners.Length ),
                    null,
                    "BandSox.Utility.Messaging.Bus.Send<T>()" );
        }

        public IDisposable ListenFor<T> ( IListener<T> listener ) where T : IMessage
        {
            Logger.Log (
                StatusMessage.Types.Debug,
                string.Format (
                    "Adding listener '{0}' for Message Type '{1}'",
                    listener,
                    typeof ( T ).FullName ),
                null,
                "BandSox.Utility.Messaging.Bus.ListenFor<T>()" );

            m_rwlListeners.WriteLocked ( ( ) => m_listenersByMessageType.Add ( typeof ( T ), (object)listener ) );
            Fabric?.AddListener<T> ( );
            return new ListnerDisposer<T> ( listener );
        }

        public void UnListenFor<T> ( IListener<T> listener ) where T : IMessage
        {
            Logger.Log (
                StatusMessage.Types.Debug,
                string.Format (
                    "Removing listener '{0}' for Message Type '{1}'",
                    listener,
                    typeof ( T ).FullName ),
                null,
                "BandSox.Utility.Messaging.Bus.ListenFor<T>()" );

            m_rwlListeners.WriteLocked ( ( ) => m_listenersByMessageType.Remove ( typeof ( T ), listener ) );
            Fabric?.RemoveListener<T> ( );
        }

        private class ListnerDisposer<T> : IDisposable where T : IMessage
        {
            private readonly IListener<T> m_listener;

            public ListnerDisposer ( IListener<T> listener )
            {
                Contract.AssertNotNull ( ( ) => listener, listener );
                m_listener = listener;
            }

            public void Dispose ( )
            {
                Bus.Instance.UnListenFor<T> ( m_listener );
            }
        }

        public bool IsMessageLoggingEnabled { get; set; }
        public IFabric Fabric { get; set; }
    }

    [AttributeUsage ( AttributeTargets.Class, Inherited = false, AllowMultiple = false )]
    public sealed class AutoListenAttribute : Attribute
    {
    }

    public class ListenerFinder : IListener<AppShutdownMessage>
    {
        private readonly List<IDisposable> m_aListeners = new List<IDisposable> ( );

        public ListenerFinder ( )
        {
            Bus.Instance.ListenFor ( this );
        }

        public void Find ( System.Reflection.Assembly oAssembly )
        {
            // the assholes that did the winrt shit decided we were not good enough for this crap (maybe there is a work around?)
            //Type[ ] aTypes = oAssembly
            //	.DefinedTypes
            //	.Where (
            //		oType =>
            //			oType.AsType()..GetInterfaces ( )
            //				.Where ( oInterfaceType => oInterfaceType == typeof ( IListener<> ) )
            //				.Any ( )
            //			&& oType.GetCustomAttributes ( typeof ( AutoListenAttribute ), true ).Any ( )
            //			&& oType.GetConstructor ( new Type[ ] { } ) != null )
            //	.ToArray ( );

            //lock ( this )
            //{
            //	foreach ( Type oType in aTypes )
            //	{
            //		// dirtly little hackary...
            //		System.Reflection.MethodInfo oMethod = typeof ( Bus ).GetMethod ( "ListenFor" );
            //		m_aListeners.Add (
            //			(IDisposable)oMethod
            //				.MakeGenericMethod ( oType )
            //				.Invoke (
            //					Bus.Instance,
            //					new object[ ] { Activator.CreateInstance ( oType ) } ) );
            //	}
            //}
        }

        public void Receive ( AppShutdownMessage message )
        {
            m_aListeners.DisposeItems ( );
        }
    }
}
