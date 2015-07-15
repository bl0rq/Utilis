using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Navigation
{
	public interface ITypeToUriMapper
	{
		Uri Map ( Type t );
	}

	public class TypeToUriMapper : ITypeToUriMapper
	{
		private readonly string m_root;

		public TypeToUriMapper ( string rootNamespace )
		{
			m_root = rootNamespace + ".";
		}

		public Uri Map ( Type t )
		{
			return new Uri ( string.Format ( "/{0}.xaml", string.Join ( "/", t.FullName.Replace ( m_root, "" ).Split ( '.' ) ) ), UriKind.Relative );
		}
	}
}
