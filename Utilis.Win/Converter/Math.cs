using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public class Multiply : ObjectModel.Singleton<Multiply>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if ( value == null )
				return null;

			decimal param = decimal.Parse ( (string)parameter );
			if ( value is int )
				return ( (int)value ) * param;
			else if ( value is double )
				return ( (double)value ) * (double)param;
			else
				throw new NotSupportedException ( "Type " + value.GetType ( ) + " is not supported." );
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ( );
		}
	}
}
