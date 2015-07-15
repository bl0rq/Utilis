using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public class ByteCount : ObjectModel.Singleton<ByteCount>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			try
			{
				ulong ulValue = System.Convert.ToUInt64 ( value );

				/*
				 * 1024 = 1k
				 * 1048576 = 1mb
				 * 1073741824 == 1gb
				 */
				if ( ulValue > 1073741824 )
					return Math.Round ( ( ulValue / 1073741824d ), 2 ) + " GB";
				else if ( ulValue > 1048576 )
					return Math.Round ( ( ulValue / 1048576d ), 2 ) + " MB";
				else
					return Math.Round ( ( ulValue / 1024d ), 2 ) + " KB";
			}
			catch ( Exception )
			{
				return "0 B";
			}
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class Round : ObjectModel.Singleton<Round>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			double? d = value as double?;
			if ( !d.HasValue )
				return value;
			else
			{
				int nPlaces = 1;
				if ( parameter != null )
					int.TryParse ( (string)parameter, out nPlaces );
				return Math.Round ( d.Value, nPlaces );
			}
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
