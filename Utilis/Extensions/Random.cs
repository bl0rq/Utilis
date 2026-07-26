using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
    public static class RandomExtensions
    {
        public static bool NextBool ( this System.Random oRandom )
        {
            if ( oRandom == null )
                throw new ArgumentNullException ( nameof ( oRandom ) );

            return oRandom.NextDouble ( ) >= .5;
        }

        /// <summary>
        /// Returns a bool w/ the dTrueOdds of being true where dTrueOdds is between 0 and 1
        /// </summary>
        public static bool NextBool ( this System.Random oRandom, double dTrueOdds )
        {
            if ( oRandom == null )
                throw new ArgumentNullException ( nameof ( oRandom ) );

            if ( dTrueOdds < 0 || dTrueOdds > 1 )
                throw new ArgumentOutOfRangeException ( nameof ( dTrueOdds ), "Must be between 0 and 1 inclusive." );

            return oRandom.NextDouble ( ) >= ( 1 - dTrueOdds );
        }

        public static T NextEnum<T> ( this System.Random oRandom ) where T : struct, Enum
        {
            if ( oRandom == null )
                throw new ArgumentNullException ( nameof ( oRandom ) );

            T [] aValues = Enum.GetValues<T> ( );
            return aValues [ oRandom.Next ( 0, aValues.Length ) ];
        }

        public static T NextItem<T> ( this System.Random oRandom, IList<T> arr )
        {
            if ( oRandom == null )
                throw new ArgumentNullException ( nameof ( oRandom ) );
            if ( arr == null )
                throw new ArgumentNullException ( nameof ( arr ) );
            if ( arr.Count == 0 )
                throw new ArgumentException ( "Collection must contain at least one item.", nameof ( arr ) );

            return arr [ oRandom.Next ( 0, arr.Count ) ];
        }

        public static T NextItem<T> ( this System.Random oRandom, T [] arr )
        {
            if ( oRandom == null )
                throw new ArgumentNullException ( nameof ( oRandom ) );
            if ( arr == null )
                throw new ArgumentNullException ( nameof ( arr ) );
            if ( arr.Length == 0 )
                throw new ArgumentException ( "Collection must contain at least one item.", nameof ( arr ) );

            return arr [ oRandom.Next ( 0, arr.Length ) ];
        }

        private static readonly char [] ms_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgehijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]\\{}|;':\",./<>?`~".ToCharArray ( );
        public static string NextString ( this Random random, int count )
        {
            if ( random == null )
                throw new ArgumentNullException ( nameof ( random ) );

            if ( count <= 0 )
                return "";
            else
            {
                StringBuilder sb = new StringBuilder ( count );
                for ( int i = 0 ; i < count ; i++ )
                {
                    sb.Append ( random.NextItem ( ms_chars ) );
                }
                return sb.ToString ( );
            }
        }
    }
}
