using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Win.Properties
{
    public class Item
    {
        public static System.Windows.Input.ICommand GetDoubleClickCommand ( System.Windows.DependencyObject obj )
        {
            return (System.Windows.Input.ICommand)obj.GetValue ( DoubleClickCommandProperty );
        }

        public static void SetDoubleClickCommand ( System.Windows.DependencyObject obj, System.Windows.Input.ICommand value )
        {
            obj.SetValue ( DoubleClickCommandProperty, value );
        }

        // Using a DependencyProperty as the backing store for DoubleClickCommand.  This enables animation, styling, binding, etc...
        public static readonly System.Windows.DependencyProperty DoubleClickCommandProperty =
            System.Windows.DependencyProperty.RegisterAttached ( 
                "DoubleClickCommand", 
                typeof ( System.Windows.Input.ICommand ), 
                typeof ( Item ), 
                new System.Windows.PropertyMetadata ( null, DoubleClickCommandChanged ) );

        private static void DoubleClickCommandChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            
        }
    }
}
