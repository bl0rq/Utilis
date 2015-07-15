using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Win
{
    public class SingletonChecker : IDisposable
    {
        private readonly string m_applicationDataFolder;
        private readonly string m_applicationName;
        private bool m_suppressPidDelete = false;

        public SingletonChecker ( string applicationDataFolder, Type applicationType )
        {
            m_applicationDataFolder = applicationDataFolder;
            m_applicationName = applicationType.Assembly.GetName ( ).Name;
        }

        public void Dispose ( )
        {
            if ( !System.IO.Directory.Exists ( m_applicationDataFolder ) )
                System.IO.Directory.CreateDirectory ( m_applicationDataFolder );

            if ( m_suppressPidDelete )
                return;

            System.IO.FileInfo fiPIDLock = GetPIDLock ( );
            if ( fiPIDLock.Exists )
                fiPIDLock.Delete ( );
        }

        public bool IsFirstInstance ( )
        {
            // for debug, let them roll
            if ( System.Diagnostics.Process.GetCurrentProcess ( ).ProcessName.ToLower ( ).Contains ( "vshost" ) )
                return true;

            System.Diagnostics.Process oProcess = GetProcess ( );
            if ( oProcess == null )
                StorePID ( GetPIDLock ( ) );
            else
            {
                try
                {
                    if ( oProcess.HasExited
                         || oProcess.ProcessName != System.Diagnostics.Process.GetCurrentProcess ( ).ProcessName )
                        // || oProcess.ProcessName != sApplicationName + ".vshost" )
                        StorePID ( GetPIDLock ( ) );
                    else
                    {
                        m_suppressPidDelete = true;
                        return false;
                    }
                }
                catch ( Exception )
                {
                    StorePID ( GetPIDLock ( ) );
                }
            }

            return true;
        }

        public System.Diagnostics.Process GetProcess ( )
        {
            System.IO.FileInfo fiPIDLock = GetPIDLock ( );
            if ( !fiPIDLock.Exists )
                return null;

            string sPID = System.IO.File.ReadAllText ( fiPIDLock.FullName );
            int nPID;
            if ( !int.TryParse ( sPID, out nPID ) )
                return null;

            try
            {
                return System.Diagnostics.Process.GetProcessById ( nPID );
            }
            catch
            {
                return null;
            }
        }

        private System.IO.FileInfo GetPIDLock ( )
        {
            return new System.IO.FileInfo ( System.IO.Path.Combine ( m_applicationDataFolder, m_applicationName + ".pid" ) );
        }

        private static void StorePID ( System.IO.FileInfo fiPIDLock )
        {
            System.IO.File.WriteAllText ( fiPIDLock.FullName, System.Diagnostics.Process.GetCurrentProcess ( ).Id.ToString ( ) );
        }
    }
}

