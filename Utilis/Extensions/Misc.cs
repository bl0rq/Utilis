using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
    public static class MiscExtensions
    {
        /// <summary>
        /// Disposes an object (if its not null) inside a try/catch w/ an empty catch.
        /// WARNING: suppresses (but logs) the error!
        /// </summary>
        public static void DisposeWithCare ( this IDisposable disposable )
        {
            if ( disposable != null )
            {
                try
                {
                    disposable.Dispose ( );
                }
                catch ( Exception e )
                {
                    Logger.Log ( e, disposable.GetType ( ).Name + " Dispose" );
                }
            }
        }

        public static void DisposeItems ( this IEnumerable<IDisposable> disposables )
        {
            foreach ( IDisposable oDisposable in disposables )
            {
                oDisposable.DisposeWithCare ( );
            }
        }

        public static string ToFullString ( this Exception exception )
        {
            StringBuilder sb = new StringBuilder ( );
            sb.AppendLine ( exception.Message );
            var inner = exception;
            int tabs = 0;
            while ( inner.InnerException != null )
            {
                sb.Append ( '\t', ++tabs );
                sb.Append ( "Inner: " );
                sb.AppendLine ( inner.InnerException.Message );
                inner = exception.InnerException;
            }
            sb.AppendLine ( exception.StackTrace );
            return sb.ToString ( );
        }
    }
}
