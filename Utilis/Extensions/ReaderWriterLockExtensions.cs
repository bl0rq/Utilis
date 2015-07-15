using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.Extensions
{
    public static class ReaderWriterLockExtensions
    {
        public static void ReadLocked ( this System.Threading.ReaderWriterLockSlim rwl, Action act )
        {
            try
            {
                rwl.EnterReadLock ( );
                act ( );
            }
            finally
            {
                rwl.ExitReadLock ( );
            }
        }

        public static void WriteLocked ( this System.Threading.ReaderWriterLockSlim rwl, Action act )
        {
            try
            {
                rwl.EnterWriteLock ( );
                act ( );
            }
            finally
            {
                rwl.ExitWriteLock ( );
            }
        }
    }
}