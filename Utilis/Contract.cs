using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis
{
#if SILVERLIGHT
	// oh silverlight, you love to leave out the most random crap...
	public class ApplicationException : Exception
	{
		protected ApplicationException ( string message )
			: base ( message )
		{

		}

		protected ApplicationException ( )
		{
		}

		protected ApplicationException ( string message, Exception inner )
			: base ( message, inner )
		{
		}
	}
#endif
	public static class Contract
	{
		public static void AssertNotNull ( System.Linq.Expressions.Expression<Func<object>> ex, object o )
		{
			if ( o == null )
				throw new AssertionException ( GetPropertyName ( ex ) + " must not be null." );
		}

		public static void AssertNotEmpty ( System.Linq.Expressions.Expression<Func<string>> fn, string s )
		{
			if ( string.IsNullOrEmpty ( s ) )
				throw new AssertionException ( GetPropertyName ( fn ) + " must not be empty." );
		}

		public static void Ensure ( bool assertion, string message )
		{
			if ( !assertion )
				throw new AssertionException ( message );
		}

		private static string GetPropertyName<T> ( System.Linq.Expressions.Expression<Func<T>> exp )
		{
			Ensure ( exp.NodeType == System.Linq.Expressions.ExpressionType.Lambda, "Expression must be a lamda" );

			System.Linq.Expressions.MemberExpression memberExp = exp.Body as System.Linq.Expressions.MemberExpression;
			Ensure ( memberExp != null, "Body of lamba must be a property access." );

			return memberExp.Member.Name;
		}

		public class ContractException : Exception
		{
			protected ContractException ( ) { }
			protected ContractException ( string message ) : base ( message ) { }
			protected ContractException ( string message, Exception inner ) : base ( message, inner ) { }
		}

		public class AssertionException : ContractException
		{
			public AssertionException ( ) { }
			public AssertionException ( string message ) : base ( message ) { }
			public AssertionException ( string message, Exception inner ) : base ( message, inner ) { }
		}

		public static void AssertTrue ( bool b, string sMessage )
		{
			if ( !b )
				throw new AssertionException ( sMessage );
		}

		public static T AssertIsType<T> ( System.Linq.Expressions.Expression<Func<object>> exp, object o )
		{
			if ( o == null )
				return default ( T );
			else if ( o is T )
			{
				return (T)o;
			}
			else
				throw new AssertionException ( "Invalid type (" + o.GetType ( ).Name + ") for " + GetPropertyName ( exp ) );
		}
	}
}
