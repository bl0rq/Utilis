using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public class BoolVisibility : ObjectModel.Singleton<BoolVisibility>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null )
				return System.Windows.Visibility.Collapsed;

			bool bVisible = (bool)value;

			string sParameter = parameter as string;
			if ( sParameter != null && sParameter.ToLower ( ) != "true" )
				bVisible = !bVisible;

			return bVisible
				? System.Windows.Visibility.Visible
				: System.Windows.Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class BoolHiddenVisibility : ObjectModel.Singleton<BoolHiddenVisibility>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null )
				return System.Windows.Visibility.Collapsed;

			bool bVisible = (bool)value;

			string sParameter = parameter as string;
			if ( sParameter != null && sParameter.ToLower ( ) != "true" )
				bVisible = !bVisible;

			return bVisible
				? System.Windows.Visibility.Visible
				: System.Windows.Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class NullVisibility : ObjectModel.Singleton<NullVisibility>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			bool bVisible = value != null;

			string sParameter = parameter as string;
			if ( sParameter != null && sParameter.ToLower ( ) != "true" )
				bVisible = !bVisible;

			return bVisible
				? System.Windows.Visibility.Visible
				: System.Windows.Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class CountVisibility : ObjectModel.Singleton<CountVisibility>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			if ( value == null )
				return System.Windows.Visibility.Collapsed;

			int iValue;
			if ( value is System.Collections.ICollection )
				iValue = ( (System.Collections.ICollection)value ).Count;
			else
				iValue = System.Convert.ToInt32 ( value );

			bool bVisible = iValue > 0;

			string sParameter = parameter as string;
			if ( sParameter != null && sParameter.ToLower ( ) != "true" )
				bVisible = !bVisible;

			return bVisible
				? System.Windows.Visibility.Visible
				: System.Windows.Visibility.Collapsed;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}