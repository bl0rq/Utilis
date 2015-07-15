using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Utilis.Extensions;

namespace Utilis
{
	public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, Messaging.IListener<Messaging.AppShutdownMessage>
	{
		void RegisterInstance<T> ( T o );
		void RemoveInstance<T> ( );
	}

	public class ServiceLocator : Microsoft.Practices.ServiceLocation.ServiceLocatorImplBase, IServiceLocator
	{
		public static IServiceLocator Instance
		{
			get;
			set;
		}


		private readonly Autofac.IComponentContext m_context;
		private IDisposable m_busToken;

		private readonly Dictionary<Type, object> m_additionalTypes = new Dictionary<Type, object> ( );

		public ServiceLocator ( Autofac.IComponentContext context )
		{
		    Contract.AssertNotNull(() => context, context);

			m_context = context;

			m_busToken = Messaging.Bus.Instance.ListenFor ( this );
		}

		protected override object DoGetInstance ( Type serviceType, string key )
		{
			return
				m_additionalTypes.SafeGet ( serviceType )
				?? ( key != null
					? m_context.Resolve ( serviceType, new [] { new NamedParameter ( "key", key ) } )
					: m_context.SafeResolve ( serviceType ) );
		}

		protected override IEnumerable<object> DoGetAllInstances ( Type serviceType )
		{
			var enumerableType = typeof ( IEnumerable<> ).MakeGenericType ( serviceType );

			object instance = m_context.Resolve ( enumerableType );
			return ( (System.Collections.IEnumerable)instance ).Cast<object> ( );
		}

		public void Receive ( Messaging.AppShutdownMessage message )
		{
			m_busToken.DisposeWithCare ( );
			m_busToken = null;

			m_context.ComponentRegistry.DisposeWithCare ( );
		}

		public void RegisterInstance<T> ( T o )
		{
			lock ( m_additionalTypes )
			{
				m_additionalTypes [ typeof ( T ) ] = o;
			}
		}

		public void RemoveInstance<T> ( )
		{
			lock ( m_additionalTypes )
			{
				m_additionalTypes.Remove ( typeof ( T ) );
			}
		}
	}

	public static class ComponentContextExtensions
	{
		public static object SafeResolve ( this Autofac.IComponentContext context, Type serviceType )
		{
			object o;
			if ( context.TryResolve ( serviceType, out o ) )
				return o;
			else
				return null;
		}
	}
}
