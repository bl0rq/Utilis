using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Win.Converters
{
	public enum ConverterEngineResult { Positive, Negative, Default }

	public class ConverterEngine<TIn, TOut, TParam>
	{
		private readonly Func<TIn, ConverterEngineResult> m_fnCondition;
		private readonly Func<TIn, TParam, ConverterEngineResult> m_fnConditionParam;

		private readonly TOut m_oPositive;
		private readonly TOut m_oNegative;
		private readonly TOut m_oDefault;

		private ConverterEngine ( TOut oPositive = default(TOut), TOut oNegative = default(TOut), TOut oDefault = default(TOut) )
		{
			m_oDefault = Equals ( oDefault, default ( TOut ) ) ? oNegative : oDefault;
			m_oNegative = oNegative;
			m_oPositive = oPositive;
		}

		public ConverterEngine ( Func<TIn, TParam, ConverterEngineResult> fnConditionParam, TOut oPositive = default(TOut), TOut oNegative = default(TOut), TOut oDefault = default(TOut) )
			: this ( oPositive, oNegative, oDefault )
		{
			Contract.AssertNotNull ( ( ) => fnConditionParam, fnConditionParam );
			m_fnConditionParam = fnConditionParam;
		}

		public ConverterEngine ( Func<TIn, ConverterEngineResult> fnCondition, TOut oPositive = default(TOut), TOut oNegative = default(TOut), TOut oDefault = default(TOut) )
			: this ( oPositive, oNegative, oDefault )
		{
			Contract.AssertNotNull ( ( ) => fnCondition, fnCondition );
			m_fnCondition = fnCondition;
		}

		public TOut Convert ( object value, object parameter )
		{
			if ( value == null || !( value is TIn ) )
				return m_oDefault;
			else
			{
				TIn oIn = (TIn)value;
				if ( m_fnCondition != null )
				{
					return ConvertResultToOut ( m_fnCondition ( oIn ) );
				}
				else
				{
					if ( parameter == null || !( parameter is TParam ) )
						return m_oDefault;
					else
						return ConvertResultToOut ( m_fnConditionParam ( oIn, (TParam)parameter ) );
				}
			}
		}

		private TOut ConvertResultToOut ( ConverterEngineResult eResult )
		{
			switch ( eResult )
			{
				case ConverterEngineResult.Positive:
					return m_oPositive;
				case ConverterEngineResult.Negative:
					return m_oNegative;
				case ConverterEngineResult.Default:
					return m_oDefault;
				default:
					throw new ArgumentOutOfRangeException ( "eResult" );
			}
		}
	}

	// IsType and variants
	public abstract class IsType<TOut> : System.Windows.Data.IValueConverter
	{
		private readonly ConverterEngine<object, TOut, string> m_oEngine;

		protected IsType (
			TOut oPositive = default(TOut),
			TOut oNegative = default(TOut),
			TOut oDefault = default(TOut) )
		{
			m_oEngine = new ConverterEngine<object, TOut, string> (
				DoConvert,
				oPositive,
				oNegative,
				oDefault );
		}

		protected virtual ConverterEngineResult DoConvert ( object oValue, string sParameter )
		{
			System.Collections.IEnumerable aValue = oValue as System.Collections.IEnumerable;

			Type oValueType = null;
			if ( aValue != null && !( oValue is string ) )
			{
				object oListItem = aValue.Cast<object> ( ).FirstOrDefault ( );
				if ( oListItem == null )
					return ConverterEngineResult.Default;
				else
					oValueType = oListItem.GetType ( );
			}
			else
				oValueType = oValue.GetType ( );

			string[ ] aTypes = sParameter.Split ( '|' );

			return aTypes.Contains ( oValueType.FullName ) ? ConverterEngineResult.Positive : ConverterEngineResult.Negative;
		}

		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			return m_oEngine.Convert ( value, parameter );
		}
		public virtual object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}

	}
	public class IsTypeToVisibility : IsType<System.Windows.Visibility>
	{
		public IsTypeToVisibility ( )
			: base (
				oPositive: System.Windows.Visibility.Visible,
				oNegative: System.Windows.Visibility.Collapsed,
				oDefault: System.Windows.Visibility.Collapsed )
		{
		}
	}

	public class IsTypeToBool : IsType<bool>
	{
		public IsTypeToBool ( )
			: base (
				oPositive: true,
				oNegative: false,
				oDefault: false )
		{
		}
	}

	public class IsNotTypeToVisibility : IsType<System.Windows.Visibility>
	{
		public IsNotTypeToVisibility ( )
			: base (
				oPositive: System.Windows.Visibility.Collapsed,
				oNegative: System.Windows.Visibility.Visible,
				oDefault: System.Windows.Visibility.Visible )
		{
		}
	}

	// visibility variants
	public class NullVisibility2 : System.Windows.Data.IValueConverter
	{
		private readonly ConverterEngine<object, System.Windows.Visibility, string> m_oEngine;

		public NullVisibility2 ( )
		{
			m_oEngine = new ConverterEngine<object, System.Windows.Visibility, string> (
				DoConvert,
				oPositive: System.Windows.Visibility.Collapsed );
		}

		protected virtual ConverterEngineResult DoConvert ( object oValue, string sParameter )
		{
			bool bIsNull = oValue == null;
			bool bParameter;
			bool.TryParse ( sParameter, out bParameter );

			if ( bParameter )
				bIsNull = !bIsNull;

			return bIsNull
				? ConverterEngineResult.Positive
				: ConverterEngineResult.Negative;
		}

		public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			return m_oEngine.Convert ( value, parameter );
		}

		public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
		{
			throw new NotImplementedException ( );
		}
	}
}
