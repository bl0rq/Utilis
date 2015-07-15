using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Win.Converter
{
    public class DrawingColorToBrush : ObjectModel.Singleton<DrawingColorToBrush>, ObjectModel.ICanBeASingleton, System.Windows.Data.IValueConverter
    {
        private static readonly Dictionary<System.Drawing.Color, System.Windows.Media.SolidColorBrush> ms_brushesByColor = new Dictionary<System.Drawing.Color, System.Windows.Media.SolidColorBrush> ( );

        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            System.Drawing.Color color = (System.Drawing.Color)value;

            System.Windows.Media.SolidColorBrush brush;
            if ( !ms_brushesByColor.TryGetValue ( color, out brush ) )
            {
                brush = new System.Windows.Media.SolidColorBrush ( System.Windows.Media.Color.FromArgb ( color.A, color.R, color.G, color.B ) );
                ms_brushesByColor [ color ] = brush;
            }

            return brush;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotSupportedException ( );
        }
    }
}
