using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class ExpressionExtensions
	{
		public static string GetName<T> ( this System.Linq.Expressions.Expression<Func<T>> exp )
		{
			System.Linq.Expressions.MemberExpression memberExp = exp.Body as System.Linq.Expressions.MemberExpression;
			if ( memberExp == null )
				throw new Exception ( "Body of lamba must be a member access." );

			return memberExp.Member.Name;
		}

		public static string PropertyName<T, TR> ( this System.Linq.Expressions.Expression<Func<T, TR>> exp )
		{
			if ( exp.NodeType != System.Linq.Expressions.ExpressionType.Lambda )
				throw new Exception ( "Expression must be a lambda" );

			System.Linq.Expressions.MemberExpression memberExp = exp.Body as System.Linq.Expressions.MemberExpression;
			if ( memberExp == null )
				throw new Exception ( "Body of lamba must be a property access." );

			return memberExp.Member.Name;
		}
	}
}
