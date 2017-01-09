using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Win.Converter
{
    public class BoolToBrush : System.Windows.Data.IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            bool bValue = false;
            if ( value is bool )
            {
                bValue = (bool)value;
            }
            else
                return DefaultBrush;

            return ( bValue ? TrueBrush : FalseBrush ) ?? DefaultBrush;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var b = value as System.Windows.Media.Brush;
            if ( b == TrueBrush )
                return true;
            else
                return false;
        }

        public System.Windows.Media.Brush DefaultBrush { get; set; }
        public System.Windows.Media.Brush TrueBrush { get; set; }
        public System.Windows.Media.Brush FalseBrush { get; set; }
    }
}
