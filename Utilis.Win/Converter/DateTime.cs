using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Utilis.Win.Converters
{
	public class MonthConverter : System.Windows.Data.IValueConverter
	{
		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;

			int nMonthID = (int)value;
			return DateTimeFormatInfo.CurrentInfo.MonthNames[ ( nMonthID - 1 ) ];

		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}

	public class DayConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter Members

		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;

			return value;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;

			return value;
		}

		#endregion
	}

	public class StandardTo24HourTimeConverter : System.Windows.Data.IValueConverter
	{
		#region IValueConverter Members

		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;

			DateTime dtLocal = ( (DateTime)value );

			string sFormattedDate = string.Format ( "{0:MM/dd/yyyy HH:mm}", dtLocal );

			return sFormattedDate;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;

			string sDateTime = (string)value;
			try
			{
				return DateTime.Parse ( sDateTime );
			}
			catch ( Exception )
			{
				return null;
			}
		}

		#endregion
	}

	public class TimeSpanSeconds : ObjectModel.Singleton<TimeSpanSeconds>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null || !( value is TimeSpan ) )
				return null;
			else
			{
				return ( (TimeSpan)value ).TotalSeconds;
			}

		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;
			else
			{
				if ( value is int )
					return TimeSpan.FromSeconds ( (int)value );
				else if ( value is double )
					return TimeSpan.FromSeconds ( (double)value );
				else
					throw new NotSupportedException ( "Unsupported type: " + value.GetType ( ).FullName );
			}
		}
	}

	public class TimeSpanProperConverter : ObjectModel.Singleton<TimeSpanProperConverter>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		#region IValueConverter Members

		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			if ( value == null )
				return null;
			else if ( value is TimeSpan )
				return FormatTimeSpan ( (TimeSpan)value );
			else if ( value is int )
				return FormatTimeSpan ( TimeSpan.FromSeconds ( (int)value ) );
			else
				return null;
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}

		#endregion

		private string FormatTimeSpan ( TimeSpan ts )
		{
			if ( ts.TotalDays >= 1 )
				return string.Format ( "{0} days, {1}:{2} hrs", ts.Days, ts.Hours, ( ts.Minutes + ( ts.Seconds > 30 ? 1 : 0 ) ).ToString ( "00" ) );
			else if ( ts.Hours >= 1 )
				return string.Format ( "{0}:{1}:{2}", ts.Hours, ts.Minutes.ToString ( "00" ), ( ts.Seconds + ( ts.Milliseconds > 500 ? 1 : 0 ) ).ToString ( "00" ) );
			else
				return string.Format ( "{0}:{1}", ts.Minutes.ToString ( ), ( ts.Seconds + ( ts.Milliseconds > 500 ? 1 : 0 ) ).ToString ( "00" ) );
		}
	}

	public class TimeSpanToFrequency : ObjectModel.Singleton<TimeSpanToFrequency>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		#region IValueConverter Members

		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			TimeSpan? ts = value is TimeSpan ? (TimeSpan)value : (TimeSpan?)null;
			if ( ts == null )
				return 0;

			int nCount;
			if ( parameter == null )
				nCount = 10;
			else
				nCount = System.Convert.ToInt32 ( parameter );

			return Math.Ceiling ( ts.Value.TotalSeconds / nCount );
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}

		#endregion
	}

	public class Frequency : ObjectModel.Singleton<Frequency>, System.Windows.Data.IValueConverter, ObjectModel.ICanBeASingleton
	{
		#region IValueConverter Members

		public object Convert ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			double dValue;
			if ( value is double )
				dValue = (double)value;
			else
				return value;

			int nCount;
			if ( parameter == null )
				nCount = 10;
			else
				nCount = System.Convert.ToInt32 ( parameter );

			return Math.Ceiling ( dValue / nCount );
		}

		public object ConvertBack ( object value, Type targetType, object parameter, CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}

		#endregion
	}
}
