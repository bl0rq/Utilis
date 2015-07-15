using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.ObjectModel
{
	public class Singleton<T> where T : class, ICanBeASingleton, new ( )
	{
		protected Singleton ( ) { }

		internal static Lazy<T> m_instance = new Lazy<T>(CreateInstance);

		public static T Instance
		{
			get
			{
				return m_instance.Value;
			}
		}

		protected static T CreateInstance ( )
		{
			try
			{
				return new T ( );
			}
			catch ( Exception ex )
			{
				throw new Exception (
					string.Format (
						"Unable to create instance of type {0} : {1}",
							typeof ( T ).FullName,
							ex.InnerException != null
								? ex.InnerException.Message
								: ex.Message ),
					ex );
			}
		}
	}

	public interface ICanBeASingleton
	{
	}
}
