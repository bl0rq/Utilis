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
            return oRandom.NextDouble ( ) >= .5;
        }

        /// <summary>
        /// Returns a bool w/ the dTrueOdds of being true where dTrueOdds is between 0 and 1
        /// </summary>
        public static bool NextBool ( this System.Random oRandom, double dTrueOdds )
        {
            return oRandom.NextDouble ( ) >= ( 1 - dTrueOdds );
        }

        public static T NextEnum<T> ( this System.Random oRandom )
        {
            // another winrt fail
            //if ( !typeof ( T ).IsEnum )
            //	throw new Exception ( "Type of T must be an enum" );

            Array aValues = Enum.GetValues ( typeof ( T ) );

            return (T)aValues.GetValue ( oRandom.Next ( 0, aValues.Length ) );
        }

        public static T NextItem<T> ( this System.Random oRandom, IList<T> arr )
        {
            return arr [ oRandom.Next ( 0, arr.Count ) ];
        }

        public static T NextItem<T> ( this System.Random oRandom, T [] arr )
        {
            return arr [ oRandom.Next ( 0, arr.Length ) ];
        }

        private static char [] ms_chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefgehijklmnopqrstuvwxyz1234567890!@#$%^&*()_+-=[]\\{}|;':\",./<>?`~".ToCharArray ( );
        public static string NextString ( this Random random, int count )
        {
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
