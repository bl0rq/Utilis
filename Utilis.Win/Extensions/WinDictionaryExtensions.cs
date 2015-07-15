using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Extensions
{
    public static class WinDictionaryExtensions
    {
        public static void Add<T_Key, T_Value> ( this Dictionary<T_Key, ObjectModel.ObservableCollection<T_Value>> ht, T_Key key, T_Value value )
        {
            DictionaryExtensions.Add<T_Key, T_Value, ObjectModel.ObservableCollection<T_Value>> (
                ht,
                key,
                ( list, v ) => list.Add ( v ),
                ( list, v ) => list.Contains ( v ),
                value );
        }

        public static void Remove<T_Key, T_Value> ( this Dictionary<T_Key, List<T_Value>> ht, T_Key key, T_Value value )
        {
            DictionaryExtensions.Remove (
                ht,
                key,
                value,
                ( list, v ) => list.Remove ( v ),
                list => list.Count );
        }
    }
}
