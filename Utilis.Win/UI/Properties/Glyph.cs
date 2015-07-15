using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Win.Properties
{
    public static class Glyph
    {
        public static System.Windows.Media.Geometry GetData ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Media.Geometry)obj.GetValue ( DataProperty );
        }
        public static void SetData ( System.Windows.DependencyObject obj, System.Windows.Media.Geometry value )
        {
            obj.SetValue ( DataProperty, value );
        }
        public static readonly System.Windows.DependencyProperty DataProperty =
            System.Windows.DependencyProperty.RegisterAttached (
                "Data",
                typeof ( System.Windows.Media.Geometry ),
                typeof ( Glyph ),
                new System.Windows.PropertyMetadata ( null ) );
    }
}
