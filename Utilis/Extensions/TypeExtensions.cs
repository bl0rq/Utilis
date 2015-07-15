using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Extensions
{
    public static class TypeExtensions
    {
        private static readonly ObjectModel.ConcurrentCache<object, bool> ms_cache = new ObjectModel.ConcurrentCache<object, bool> ( );
        public static bool HasAttribute<T> ( this Type t ) where T : Attribute
        {
            return ms_cache.GetOrAdd (
                t,
                tt =>
                    typeof ( T )
                    .GetTypeInfo ( )
                    .GetCustomAttributes ( true )
                    .OfType<T> ( )
                    .Any ( ) );
        }

        public static bool HasAttribute<T> ( this FieldInfo field ) where T : Attribute
        {
            return ms_cache.GetOrAdd (
                field,
                tt =>
                    field
                    .GetCustomAttributes ( true )
                    .OfType<T> ( )
                    .Any ( ) );
        }

        public static bool HasAttribute<T> ( this PropertyInfo field ) where T : Attribute
        {
            return ms_cache.GetOrAdd (
                field,
                tt =>
                    field
                    .GetCustomAttributes ( true )
                    .OfType<T> ( )
                    .Any ( ) );
        }
    }
}
