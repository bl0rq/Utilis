using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
    public static class DictionaryExtensions
    {
        public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey )
        {
            return oDictionary.SafeGet ( sKey, ( ) => default ( T_Value ), false );
        }

        //public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey, System.Threading.ReaderWriterLockSlim rwlLock )
        //{
        //	T_Value oReturn = default ( T_Value );

        //	Runner.RunReadLocked (
        //		( ) =>
        //		{
        //			oReturn = oDictionary.SafeGet ( sKey, ( ) => default ( T_Value ), false );
        //		}, rwlLock );

        //	return oReturn;
        //}

        public static T_Value SafeGet<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> oDictionary, T_Key sKey, Func<T_Value> fnGetDefault, bool bAddDefault )
        {
            if ( oDictionary == null ) return fnGetDefault ( );

            T_Value value;
            if ( oDictionary.TryGetValue ( sKey, out value ) )
                return value;
            else
            {
                T_Value oDefault = fnGetDefault ( );

                if ( bAddDefault )
                    oDictionary [ sKey ] = oDefault;

                return oDefault;
            }
        }

        //public static string SafeGetValue ( this System.Web.HttpCookieCollection htCookies, string sKey )
        //{
        //    System.Web.HttpCookie oCookie = htCookies[ sKey ];
        //    if ( oCookie != null )
        //        return oCookie.Value;
        //    else
        //        return null;
        //}

        //public static T_Value GetOrCreate<T_Key, T_Value> ( this IDictionary<T_Key, T_Value> ht, T_Key key, Func<T_Key, T_Value> fnCreate, System.Threading.ReaderWriterLockSlim rwlLock ) where T_Value : class
        //{
        //	T_Value value = null;
        //	Runner.RunReadLocked ( ( ) =>
        //	{
        //		value = ht.SafeGet ( key );
        //	}, rwlLock );

        //	if ( value == null )
        //	{
        //		Runner.RunWriteLocked ( ( ) =>
        //		{
        //			value =
        //				ht.SafeGet ( key )
        //				?? fnCreate ( key );
        //		}, rwlLock );
        //	}

        //	return value;
        //}

        public static void AddRange<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, IEnumerable<T_Value> aValues )
        {
            foreach ( T_Value value in aValues )
            {
                ht.Add ( key, value );
            }
        }

        public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, IList<T_Value>> ht, T_Key key, T_Value value )
        {
            Add (
                ht,
                key,
                ( ) => new List<T_Value> ( ),
                ( list, v ) => list.Add ( v ),
                ( list, v ) => list.Contains ( v ),
                value );
        }

        public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, T_Value value )
        {
            Add (
                ht,
                key,
                ( list, v ) => list.Add ( v ),
                ( list, v ) => list.Contains ( v ),
                value );
        }

        public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, T_Value value )
        {
            Remove (
                ht,
                key,
                value,
                ( list, v ) => list.Remove ( v ),
                list => list.Count );
        }

        public static void Add<T_Key, T_Value, T_Collection> ( this Dictionary<T_Key, T_Collection> ht, T_Key key, T_Value value ) where T_Collection : IList<T_Value>, new ( )
        {
            Add (
                ht,
                key,
                ( list, v ) => list.Add ( v ),
                ( list, v ) => list.Contains ( v ),
                value );
        }

        public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, System.Collections.IList> ht, T_Key key, T_Value value )
        {
            Remove (
                ht,
                key,
                value,
                remove: ( list, v ) => list.Remove ( v ),
                count: list => list.Count );
        }

        internal static void Add<T_Key, T_Value, T_Collection> (
            this Dictionary<T_Key, T_Collection> ht,
            T_Key key,
            Action<T_Collection, T_Value> add,
            Func<T_Collection, T_Value, bool> contains,
            T_Value value ) where T_Collection : new ( )
        {
            Add (
                ht,
                key,
                ( ) => new T_Collection ( ),
                add,
                contains,
                value );
        }

        internal static void Add<T_Key, T_Value, T_Collection> (
            this Dictionary<T_Key, T_Collection> ht,
            T_Key key,
            Func<T_Collection> createCollection,
            Action<T_Collection, T_Value> add,
            Func<T_Collection, T_Value, bool> contains,
            T_Value value )
        {
            T_Collection list = default ( T_Collection );
            if ( !ht.ContainsKey ( key ) )
            {
                list = createCollection ( );
                ht [ key ] = list;

                add ( list, value );
            }
            else
            {
                list = ht [ key ];
                if ( !contains ( list, value ) )
                    add ( list, value );
            }
        }

        internal static void Remove<T_Key, T_Value, T_Collection> (
            this Dictionary<T_Key, T_Collection> ht,
            T_Key key,
            T_Value value,
            Action<T_Collection, T_Value> remove,
            Func<T_Collection, int> count )
        {
            if ( ht.ContainsKey ( key ) )
            {
                var list = ht [ key ];
                remove ( list, value );

                if ( count ( list ) == 0 )
                    ht.Remove ( key );
            }
        }

        public static bool Contains<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, T_Value value ) where T_Value : class //, IEquatable<T_Value>
        {
            if ( ht.ContainsKey ( key ) )
                return ht [ key ].Any ( itm => itm == value );
            else
                return false;
        }
    }
}
