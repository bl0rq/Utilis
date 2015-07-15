using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public class ImageSource : System.Windows.Data.IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			value = value ?? parameter;

			if ( value == null )
				return null;
			else if ( value is Uri )
			{
				var oImage = new System.Windows.Media.Imaging.BitmapImage ( );
				oImage.BeginInit ( );
				oImage.UriSource = (Uri)value;
				oImage.EndInit ( );
				return oImage;
			}
			else if ( value is string )
			{
				var oImage = new System.Windows.Media.Imaging.BitmapImage ( );

				oImage.BeginInit ( );
				oImage.UriSource = new Uri ( (string)value, UriKind.RelativeOrAbsolute );
				oImage.EndInit ( );
				return oImage;
			}
			else
				return null;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
