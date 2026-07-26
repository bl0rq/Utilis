using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class CollectionExtensions
	{
		[Obsolete ( "Prefer string.Join(delimiter, source.Select(selector)) or string.Join(delimiter, source)." )]
		public static string Join<T> ( this IEnumerable<T> Collection, char sDelimiter, Func<T, string> JoinFunction )
		{
			if ( JoinFunction != null )
				return string.Join ( sDelimiter, Collection.Select ( JoinFunction ) );

			return string.Join ( sDelimiter, Collection );
		}

		[Obsolete ( "Prefer string.Join(delimiter, source.Select(selector)) or string.Join(delimiter, source)." )]
		public static string Join<T> ( this IEnumerable<T> Collection, string sDelimiter, Func<T, string> JoinFunction )
		{
			if ( JoinFunction != null )
				return string.Join ( sDelimiter, Collection.Select ( JoinFunction ) );

			return string.Join ( sDelimiter, Collection );
		}

		public static T FirstOrDefaultNullList<T> ( this IEnumerable<T> arr )
		{
			return arr == null ? default ( T ) : arr.FirstOrDefault ( );
		}

		public static int IndexOf<T> ( this IList<T> arr, Func<T, bool> fnWhere )
		{
			for ( int i = 0 ; i < arr.Count ; i++ )
			{
				if ( fnWhere ( arr[ i ] ) )
					return i;
			}
			return -1;
		}

		public static int IndexOf<T> ( this IList<T> arr, T o ) where T : class
		{
			for ( int i = 0 ; i < arr.Count ; i++ )
			{
				if ( arr[ i ] == o )
					return i;
			}
			return -1;
		}

		public static void Remove<T> ( this ICollection<T> arr, Func<T, bool> fnWhere )
		{
			T[ ] aToRemove = arr.Where ( fnWhere ).ToArray ( );
			foreach ( T oToRemove in aToRemove )
			{
				arr.Remove ( oToRemove );
			}
		}
        public static T[] Remove<T> ( this T[] arr, T item ) where T : class
        {
			int foundAt = -1;
			for ( int i = 0 ; i < arr.Length ; i++ )
			{
                if ( arr [ i ] == item )
				{
					foundAt = i;
					break;
				}
			}

			if ( foundAt == -1 )
				return arr;
			else
			{
				T [] newArr = new T [ arr.Length - 1 ];
				for ( int i = 0 ; i < foundAt ; i++ )
				{
					newArr [ i ] = arr [ i ];
				}

				for ( int i = foundAt + 1 ; i < arr.Length ; i++ )
                {
                    newArr [ i - 1 ] = arr [ i ];
                }

				return newArr;
            }
        }
        public static IEnumerable<T> Insert<T> ( this IEnumerable<T> aOriginal, int nInsertIndex, IEnumerable<T> aNew )
		{
			var aOriginalEnumerator = aOriginal.GetEnumerator ( );

			for ( int i = 0 ; i < nInsertIndex && aOriginalEnumerator.MoveNext ( ) ; i++ )
				yield return aOriginalEnumerator.Current;

			foreach ( T o in aNew )
				yield return o;

			while ( aOriginalEnumerator.MoveNext ( ) )
				yield return aOriginalEnumerator.Current;

		}

		/// <summary>
		/// Allows the conversion of a List of any TEntry that supports a conversion to a type T
		/// and returns a List<T> of the converted item
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TEntry"></typeparam>
		/// <param name="aList"></param>
		/// <param name="fnItemSelector"></param>
		/// <returns></returns>
		public static List<T> ToList<T, TEntry> ( this IEnumerable<TEntry> aList, Func<TEntry, T> fnItemSelector )
		{
			return aList.Select ( fnItemSelector ).ToList ( );
		}

		public static void AddRange<T> ( this ICollection<T> aTarget, IEnumerable<T> aSource )
		{
			List<T> asList = aTarget as List<T>;
			if ( asList != null )
			{
				asList.AddRange ( aSource );
				return;
			}

			foreach ( T o in aSource )
			{
				aTarget.Add ( o );
			}
		}

		public static void AddRange ( this System.Collections.IList aTarget, System.Collections.IEnumerable aSource )
		{
			foreach ( object o in aSource )
			{
				aTarget.Add ( o );
			}
		}

		public static IEnumerable<T> Repeat<T> ( this IEnumerable<T> a, int nTimes )
		{
			return Enumerable.Range ( 0, nTimes ).SelectMany ( _ => a );
		}

		public static void Push<T> ( this Stack<T> st, IEnumerable<T> arr )
		{
			foreach ( T t in arr )
			{
				st.Push ( t );
			}
		}

		public static void EnqueueRange<T> ( this Queue<T> q, IEnumerable<T> arr )
		{
			foreach ( T t in arr )
			{
				q.Enqueue ( t );
			}
		}

#if !SILVERLIGHT
		public static Dictionary<TKey, List<TValue>> ToListDictionary<TKey, TValue> ( this IEnumerable<TValue> aList, Func<TValue, TKey> fnKeySelector )
		{
			Dictionary<TKey, List<TValue>> ht = new Dictionary<TKey, List<TValue>> ( );

			foreach ( TValue item in aList )
			{
				ht.Add ( fnKeySelector ( item ), item );
			}

			return ht;
		}

		public static Dictionary<TKey, List<TValue>> ToListDictionary<TKey, TSource, TValue> (
			this IEnumerable<TSource> aList,
			Func<TSource, TKey> fnKeySelector,
			Func<TSource, TValue> fnValueSelector )
		{
			Dictionary<TKey, List<TValue>> ht = new Dictionary<TKey, List<TValue>> ( );

			foreach ( TSource item in aList )
			{
				ht.Add ( fnKeySelector ( item ), fnValueSelector ( item ) );
			}

			return ht;
		}

		//public static void AddRange<T> ( this HashSet<T> hs, IEnumerable<T> arr )
		//{
		//	if ( arr == null )
		//		return;

		//	foreach ( T o in arr )
		//	{
		//		hs.Add ( o );
		//	}
		//}

		//public static bool ContainsAny<T> ( this IEnumerable<T> a, IEnumerable<T> aItems )
		//{
		//	HashSet<T> hsItems =
		//		aItems as HashSet<T>
		//		?? new HashSet<T> ( aItems );

		//	return a.Any ( hsItems.Contains );
		//}
#endif

		public static bool ArrayEquals<T> ( this T[ ] a, T[ ] b )
		{
			if ( a == null )
				return b == null;
			else if ( b == null )
				return false;
			else
				return a.SequenceEqual ( b );
		}

		public static IEnumerable<T> Concat<T> ( this IEnumerable<T> a, T item )
		{
			foreach ( T o in a )
			{
				yield return o;
			}

			yield return item;
		}

		public static bool IsNullOrEmpty<T> ( this T[ ] a )
		{
			return a == null || a.Length == 0;
		}

		public static TSource Second<TSource> ( this IEnumerable<TSource> source )
		{
			return source.ItemByNumber ( 1 );
		}

		public static TSource Third<TSource> ( this IEnumerable<TSource> source )
		{
			return source.ItemByNumber ( 2 );
		}

		public static TSource SecondToLast<TSource> ( this IEnumerable<TSource> source )
		{
			if ( source == null )
				return default ( TSource );

			return source.Reverse ( ).Skip ( 1 ).FirstOrDefault ( );
		}

		private static TSource ItemByNumber<TSource> ( this IEnumerable<TSource> source, uint n )
		{
			if ( source == null )
				return default ( TSource );

			IList<TSource> list = source as IList<TSource>;
			if ( list != null )
			{
				if ( list.Count > n )
					return list[ (int)n ];
			}
			else
			{
				int nCount = 0;
				using ( IEnumerator<TSource> enumerator = source.GetEnumerator ( ) )
				{
					bool bMoveNext;
					while ( ( bMoveNext = enumerator.MoveNext ( ) ) && nCount < n )
					{
						nCount++;
					}

					if ( bMoveNext )
						return enumerator.Current;
				}
			}

			throw new ArgumentException ( "source does not contain " + n + " or more elements.", "source" );
		}
	}
}