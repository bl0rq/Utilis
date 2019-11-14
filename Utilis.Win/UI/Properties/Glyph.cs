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

        public static System.Windows.Media.Brush GetFill ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Media.Brush)obj.GetValue ( FillProperty );
        }

        public static void SetFill ( System.Windows.DependencyObject obj, System.Windows.Media.Brush value )
        {
            obj.SetValue ( FillProperty, value );
        }
        public static readonly System.Windows.DependencyProperty FillProperty =
            System.Windows.DependencyProperty.RegisterAttached (
                "Fill",
                typeof ( System.Windows.Media.Brush ),
                typeof ( Glyph ),
                new System.Windows.PropertyMetadata ( null ) );

        public static System.Windows.Media.Brush GetStroke ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Media.Brush)obj.GetValue ( StrokeProperty );
        }

        public static void SetStroke ( System.Windows.DependencyObject obj, System.Windows.Media.Brush value )
        {
            obj.SetValue ( StrokeProperty, value );
        }
        public static readonly System.Windows.DependencyProperty StrokeProperty =
            System.Windows.DependencyProperty.RegisterAttached (
                "Stroke",
                typeof ( System.Windows.Media.Brush ),
                typeof ( Glyph ),
                new System.Windows.PropertyMetadata ( null ) );


        public static System.Windows.Media.Color GetFillColor ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Media.Color)obj.GetValue ( FillColorProperty );
        }

        public static void SetFillColor ( System.Windows.DependencyObject obj, System.Windows.Media.Color value )
        {
            obj.SetValue ( FillColorProperty, value );
        }
        public static readonly System.Windows.DependencyProperty FillColorProperty =
            System.Windows.DependencyProperty.RegisterAttached (
                "FillColor",
                typeof ( System.Windows.Media.Color ),
                typeof ( Glyph ),
                new System.Windows.PropertyMetadata ( null ) );

        public static System.Windows.Media.Color GetStrokeColor ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Media.Color)obj.GetValue ( StrokeColorProperty );
        }

        public static void SetStrokeColor ( System.Windows.DependencyObject obj, System.Windows.Media.Color value )
        {
            obj.SetValue ( StrokeColorProperty, value );
        }
        public static readonly System.Windows.DependencyProperty StrokeColorProperty =
            System.Windows.DependencyProperty.RegisterAttached (
                "StrokeColor",
                typeof ( System.Windows.Media.Color ),
                typeof ( Glyph ),
                new System.Windows.PropertyMetadata ( null ) );
    }
}
