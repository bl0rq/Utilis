using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class PropertyChangedEventArgsExtensions
	{
		public static bool IsProperty<T, TR> ( this System.ComponentModel.PropertyChangedEventArgs e, System.Linq.Expressions.Expression<Func<T, TR>> exp )
		{
			return e.PropertyName == exp.PropertyName ( );
		}
	}
}
