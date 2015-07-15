using System;
using System.Collections.Generic;
using System.Text;

namespace Utilis.Extensions
{
    public static class IoExtensions
    {
        public static void Write ( this System.IO.Stream stream, byte[ ] bytes )
        {
            if ( stream == null )
                throw new ArgumentException ( "Stream cannot be null.", "stream" );
            if ( bytes == null )
                throw new ArgumentException ( "Byte array cannot be null.", "bytes" );

            stream.Write ( bytes, 0, bytes.Length );
        }
    }
}