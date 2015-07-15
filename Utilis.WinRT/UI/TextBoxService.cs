using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI
{
	public class TextBoxService
	{
		public static string GetRealTimeText ( Windows.UI.Xaml.Controls.TextBox obj )
		{
			return (string)obj.GetValue ( RealTimeTextProperty );
		}

		public static void SetRealTimeText ( Windows.UI.Xaml.Controls.TextBox obj, string value )
		{
			obj.SetValue ( RealTimeTextProperty, value );
		}

		public static readonly Windows.UI.Xaml.DependencyProperty RealTimeTextProperty =
			Windows.UI.Xaml.DependencyProperty.RegisterAttached ( 
				"RealTimeText", 
				typeof ( string ), 
				typeof ( TextBoxService ), 
				null );

		public static bool GetIsAutoUpdate ( Windows.UI.Xaml.Controls.TextBox obj )
		{
			return (bool)obj.GetValue ( IsAutoUpdateProperty );
		}

		public static void SetIsAutoUpdate ( Windows.UI.Xaml.Controls.TextBox obj, bool value )
		{
			obj.SetValue ( IsAutoUpdateProperty, value );
		}

		public static readonly Windows.UI.Xaml.DependencyProperty IsAutoUpdateProperty =
			Windows.UI.Xaml.DependencyProperty.RegisterAttached ( 
				"IsAutoUpdate", 
				typeof ( bool ), 
				typeof ( TextBoxService ), 
				new Windows.UI.Xaml.PropertyMetadata ( false, OnIsAutoUpdateChanged ) );

		private static void OnIsAutoUpdateChanged ( Windows.UI.Xaml.DependencyObject sender, Windows.UI.Xaml.DependencyPropertyChangedEventArgs e )
		{
			var value = (bool)e.NewValue;
			var textbox = (Windows.UI.Xaml.Controls.TextBox)sender;

			if ( value )
			{
				Observable.FromEventPattern<Windows.UI.Xaml.Controls.TextChangedEventHandler, Windows.UI.Xaml.Controls.TextChangedEventArgs> (
							  o => textbox.TextChanged += o,
							  o => textbox.TextChanged -= o )
						  .Do ( _ => textbox.SetValue ( RealTimeTextProperty, textbox.Text ) )
						  .Subscribe ( );
			}
		}
	}
}
