using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilis.Extensions;

namespace Utilis.Win.Converters
{
	public class SliderValueUnderMouse : System.Windows.Data.IValueConverter 
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			System.Windows.Controls.Slider oSlider = value as System.Windows.Controls.Slider;
			if ( oSlider == null )
				return null;
			else
			{
				System.Windows.FrameworkElement PART_Track = (System.Windows.FrameworkElement)oSlider.FindName ( "PART_Track" );

				TimeSpan ts = TimeSpan.FromSeconds ( ( System.Windows.Input.Mouse.GetPosition ( PART_Track ).X / PART_Track.ActualWidth ) * oSlider.Maximum );
				return ts.ToPrettyString ( );
			}
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
