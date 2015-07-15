using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Utilis
{
	public static class Pair
	{
		public static Pair<T_1, T_2> Create<T_1, T_2> ( T_1 a, T_2 b )
		{
			return new Pair<T_1, T_2> ( a, b );
		}
	}

	public class Pair<T_1, T_2> : System.ComponentModel.INotifyPropertyChanged
	{
		private T_1 m_a;
		public T_1 A
		{
			get { return m_a; }
			set
			{
				if ( !Compare ( m_a, value ) )
				{
					m_a = value;
					OnPropertyChanged ( "A" );
				}
			}
		}

		private T_2 m_b;
		public T_2 B
		{
			get { return m_b; }
			set
			{
				if ( !Compare ( m_b, value ) )
				{
					m_b = value;
					OnPropertyChanged ( "B" );
				}
			}
		}

		public Pair ( )
		{
		}

		public Pair ( T_1 objA, T_2 objB )
		{
			m_a = objA;
			m_b = objB;
		}

		public override bool Equals ( object obj )
		{
			Pair<T_1, T_2> oOtherPair = obj as Pair<T_1, T_2>;
			if ( oOtherPair == null )
				return false;
			else
				return Compare<T_1> ( this.A, oOtherPair.A ) && Compare<T_2> ( this.B, oOtherPair.B );
		}

		public override int GetHashCode ( )
		{
			int nHashCode = 0;
			if ( A != null )
				nHashCode = A.GetHashCode ( );
			try
			{
				if ( B != null )
					nHashCode += B.GetHashCode ( );
			}
			catch ( OverflowException )
			{
				// this should work (i think).
				nHashCode = int.MinValue + B.GetHashCode ( );
			}
			if ( nHashCode == 0 )
				return base.GetHashCode ( );
			else
				return nHashCode;
		}

		private bool Compare<T> ( T o1, T o2 )
		{
			if ( o1 == null && o2 == null )
				return true;
			else if ( o1 == null || o2 == null )
				return false;
			else
			{
				IComparable ic_o1 = o1 as IComparable;
				if ( ic_o1 != null )
				{
					IComparable ic_o2 = o2 as IComparable;
					if ( ic_o2 != null )
					{
						return ic_o1.CompareTo ( ic_o2 ) == 0;
					}
				}
				return o1.Equals ( o2 );
			}
		}

		#region INotifyPropertyChanged Members

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged ( string sName )
		{
			System.ComponentModel.PropertyChangedEventHandler oHandler = PropertyChanged;
			if ( oHandler != null )
				oHandler ( this, new System.ComponentModel.PropertyChangedEventArgs ( sName ) );
		}

		#endregion

		public override string ToString ( )
		{
			return ( m_a == null ? "{null}" : m_a.ToString ( ) ) + " : " + ( m_b == null ? "{null}" : m_b.ToString ( ) );
		}
	}

	public class PairAOnlyComparer<T_1, T_2> : IEqualityComparer<Pair<T_1, T_2>>
	{
		#region IEqualityComparer<Pair<T_1,T_2>> Members

		public bool Equals ( Pair<T_1, T_2> x, Pair<T_1, T_2> y )
		{
			if ( x == null && y == null )
				return true;
			else if ( x == null || y == null )
				return false;
			else if ( Equals ( x.A, default ( T_2 ) ) )
				return Equals ( y.A, default ( T_2 ) );
			else
				return x.A.Equals ( y.A );
		}

		public int GetHashCode ( Pair<T_1, T_2> obj )
		{
			if ( obj == null )
				return 0;
			else
				return obj.A.GetHashCode ( );
		}

		#endregion
	}

	public class PairBOnlyComparer<T_1, T_2> : IEqualityComparer<Pair<T_1, T_2>>
	{
		#region IEqualityComparer<Pair<T_1,T_2>> Members

		public bool Equals ( Pair<T_1, T_2> x, Pair<T_1, T_2> y )
		{
			if ( x == null && y == null )
				return true;
			else if ( x == null || y == null )
				return false;
			else if ( Equals ( x.B, default ( T_2 ) ) )
				return Equals ( y.B, default ( T_2 ) );
			else
				return x.B.Equals ( y.B );
		}

		public int GetHashCode ( Pair<T_1, T_2> obj )
		{
			if ( obj == null )
				return 0;
			else
				return obj.B.GetHashCode ( );
		}

		#endregion
	}

	public class ItemBySelectorComparer<Item_T, Parameter_T> : IEqualityComparer<Item_T>
	{
		private readonly Func<Item_T, Parameter_T> m_oFnItemSelector;

		public ItemBySelectorComparer ( Func<Item_T, Parameter_T> fnItemSelector )
		{
			m_oFnItemSelector = fnItemSelector;
		}

		public bool Equals ( Item_T x, Item_T y )
		{
			if ( Object.Equals ( x, default ( Item_T ) ) && Object.Equals ( y, default ( Item_T ) ) )
				return true;
			else if ( Object.Equals ( x, default ( Item_T ) ) || Object.Equals ( y, default ( Item_T ) ) )
				return false;
			else
			{
				Parameter_T oParamX = m_oFnItemSelector ( x );
				Parameter_T oParamY = m_oFnItemSelector ( y );

				if ( Object.Equals ( oParamX, default ( Item_T ) ) && Object.Equals ( oParamY, default ( Item_T ) ) )
					return true;
				else if ( Object.Equals ( oParamX, default ( Item_T ) ) || Object.Equals ( oParamY, default ( Item_T ) ) )
					return false;
				else
					return oParamX.Equals ( oParamY );
			}
		}

		public int GetHashCode ( Item_T obj )
		{
			if ( Object.Equals ( obj, default ( Item_T ) ) )
				return 0;
			else
			{
				Parameter_T oParam = m_oFnItemSelector ( obj );
				if ( Equals ( oParam, default ( Parameter_T ) ) )
					return 0;
				else
					return oParam.GetHashCode ( );
			}
		}
	}
}
