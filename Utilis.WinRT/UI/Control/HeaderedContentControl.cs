using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Control
{
	public class HeaderedContentControl : Windows.UI.Xaml.Controls.ContentControl
	{
		public static readonly Windows.UI.Xaml.DependencyProperty HeaderProperty =
			Windows.UI.Xaml.DependencyProperty.Register (
				"Header",
				typeof ( object ),
				typeof ( HeaderedContentControl ),
				new Windows.UI.Xaml.PropertyMetadata ( null ) );
		public object Header
		{
			get { return (object)GetValue ( HeaderProperty ); }
			set { SetValue ( HeaderProperty, value ); }
		}

		public static readonly Windows.UI.Xaml.DependencyProperty HeaderStyleProperty =
			Windows.UI.Xaml.DependencyProperty.Register (
				"HeaderStyle",
				typeof ( Windows.UI.Xaml.Style ),
				typeof ( HeaderedContentControl ),
				new Windows.UI.Xaml.PropertyMetadata ( null ) );
		public Windows.UI.Xaml.Style HeaderStyle
		{
			get { return (Windows.UI.Xaml.Style)GetValue ( HeaderStyleProperty ); }
			set { SetValue ( HeaderStyleProperty, value ); }
		}

		public static readonly Windows.UI.Xaml.DependencyProperty HeaderTemplateProperty =
			Windows.UI.Xaml.DependencyProperty.Register (
				"HeaderTemplate",
				typeof ( Windows.UI.Xaml.DataTemplate ),
				typeof ( HeaderedContentControl ),
				new Windows.UI.Xaml.PropertyMetadata ( null ) );
		public Windows.UI.Xaml.DataTemplate HeaderTemplate
		{
			get { return (Windows.UI.Xaml.DataTemplate)GetValue ( HeaderTemplateProperty ); }
			set { SetValue ( HeaderTemplateProperty, value ); }
		}

		public HeaderedContentControl ( )
		{
			DefaultStyleKey = typeof ( HeaderedContentControl );
		}
	}
}
