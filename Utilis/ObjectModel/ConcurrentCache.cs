using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.ObjectModel
{
    public class ConcurrentCache<TKey, TValue>
    {
        private IImmutableDictionary<TKey, TValue> m_cache = ImmutableDictionary.Create<TKey, TValue> ( );

        public TValue GetOrAdd ( TKey key, Func<TKey, TValue> valueFactory )
        {
            Contract.AssertNotNull ( ( ) => key, key );
            Contract.AssertNotNull ( ( ) => valueFactory, valueFactory );

            TValue newValue = default ( TValue );

            bool newValueCreated = false;

            while ( true )
            {
                var oldCache = m_cache;

                TValue value;

                if ( oldCache.TryGetValue ( key, out value ) )
                    return value;

                if ( !newValueCreated )
                {
                    newValue = valueFactory ( key );

                    newValueCreated = true;
                }

                var newCache = oldCache.Add ( key, newValue );

                if ( System.Threading.Interlocked.CompareExchange ( ref m_cache, newCache, oldCache ) == oldCache )
                {
                    return newValue;
                }
                // Failed to write the new cache, try again
            }
        }

        public void Clear ( )
        {
            m_cache = m_cache.Clear ( );
        }
    }
}
