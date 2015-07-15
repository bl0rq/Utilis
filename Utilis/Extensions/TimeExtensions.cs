using System;
using System.Collections.Generic;
using System.Text;

namespace Utilis.Extensions
{
    public static class TimeExtensions
    {
        public static string ToPrettyString ( this TimeSpan ts )
        {
            if ( ts == TimeSpan.Zero )
                return "00:00";
            else if ( ts.TotalHours >= 1 )
                return string.Format (
                    "{0}:{1}:{2}",
                    Math.Floor ( ts.TotalHours ),
                    Math.Floor ( ts.TotalMinutes - ( Math.Floor ( ts.TotalHours ) * 60 ) ).ToString ( "00" ),
                    Math.Round ( ts.TotalSeconds - ( Math.Floor ( ts.TotalMinutes ) * 60 ), 1 ).ToString ( "00.#" ) );
            else if ( ts.TotalMinutes >= 1 )
                return string.Format (
                    "{0}:{1}",
                    Math.Floor ( ts.TotalMinutes ).ToString ( "00" ),
                    Math.Round ( ts.TotalSeconds - ( Math.Floor ( ts.TotalMinutes ) * 60 ), 1 ).ToString ( "00.#" ) );
            else if ( ts.TotalSeconds >= 1 )
                return "00:" + Math.Round ( ts.TotalSeconds, 1 ).ToString ( "00.#" );
            else
                return Math.Round ( ts.TotalMilliseconds, 1 ) + " ms";
        }
    }
}