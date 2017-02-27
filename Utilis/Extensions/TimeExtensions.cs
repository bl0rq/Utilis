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

        public static string ToTimestampString(this DateTimeOffset dtm)
        {
            var delta = DateTimeOffset.UtcNow - dtm;
            if ( delta.TotalSeconds <= 0 )
                return "now";
            else if ( delta.TotalSeconds <= 120 )
                return "1 minute ago";
            else if ( delta.TotalMinutes < 60 )
                return Math.Round ( delta.TotalMinutes, 0 ) + " minutes ago";
            else if ( delta.TotalHours < 24 )
                return Math.Round ( delta.TotalHours, 0 ) + " hours ago";
            else if ( delta.TotalDays < 60 )
                return Math.Round ( delta.TotalDays, 0 ) + " days ago";
            else
                return (int)( delta.TotalDays / 30 ) + " months ago";
        }
    }
}