using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Messaging
{
    /// <summary>
    /// Used to attribute a message class to keep it from being logged when sent
    /// </summary>
    public class TrivialAttribute : Attribute
    {
    }

    public abstract class BaseAsyncMessage : IMessage
    {
        public bool IsAsync
        {
            get { return true; }
        }
    }

    public abstract class BaseSyncMessage : IMessage
    {
        public bool IsAsync
        {
            get { return false; }
        }
    }

    public class AppStartedMessage : BaseAsyncMessage
    {
    }

    public class AppShutdownMessage : BaseSyncMessage
    {
    }

    public class AppKillRequestedMessage : BaseAsyncMessage
    {

    }

    public class UserInfoMessage : BaseAsyncMessage
    {
        public string Message { get; private set; }

        public UserInfoMessage ( string sMessage )
        {
            Message = sMessage;
        }
    }

    public class StartupCompleteMessage : BaseSyncMessage
    {

    }

    public class FirstWindowLoaded : BaseSyncMessage
    {
    }

    public class LoadingProgress : BaseAsyncMessage
    {
        public string Message { get; private set; }

        public LoadingProgress ( string message )
        {
            Message = message;
        }
    }
}
