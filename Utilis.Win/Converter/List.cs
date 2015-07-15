using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public class AddSeparator : System.Windows.Data.IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			System.Collections.IEnumerable a = value as System.Collections.IEnumerable;
			if ( a == null )
				return null;

			return InsertSeparators ( a, parameter as string ?? ", " );
		}

		private IEnumerable<object> InsertSeparators ( System.Collections.IEnumerable a, string sSeparator )
		{
			if ( a.Cast<object> ( ).Any ( ) )
			{
				yield return a.Cast<object> ( ).First ( );

				foreach ( var o in a.Cast<object> ( ).Skip ( 1 ) )
				{
					yield return sSeparator;
					yield return o;
				}
			}
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class ToCSV : System.Windows.Data.IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			System.Collections.IEnumerable a = value as System.Collections.IEnumerable;
			if ( a == null || !a.Cast<object> ( ).Any ( ) )
				return "";

			return string.Join ( parameter as string ?? ", ", a.Cast<object> ( ) );
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class Take : System.Windows.Data.IValueConverter
	{
		private static Take ms_oInstance;
		public static Take Instance { get { return ms_oInstance ?? ( ms_oInstance = new Take ( ) ); } }

		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			System.Collections.IEnumerable aValues = value as System.Collections.IEnumerable;
			if ( aValues == null )
				return value;
			else
			{
				int nCount;
				if ( int.TryParse ( (string)parameter, out nCount ) )
					return aValues.Cast<object> ( ).Take ( nCount );
				else
					throw new ArgumentException ( "parameter (" + parameter + ") is not an int" );
			}
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
