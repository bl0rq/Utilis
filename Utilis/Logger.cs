using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilis.Extensions;

namespace Utilis
{
    public static class Logger
    {
        public static void Log ( Messaging.StatusMessage.Types type, string message )
        {
            Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( type, null, message, null ) );
        }

        public static void Log ( Messaging.StatusMessage.Types type, string message, string sDetail )
        {
            Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( type, null, message, sDetail ) );
        }

        public static void Log ( Messaging.StatusMessage.Types type, string message, string sDetail, string context )
        {
            Messaging.Bus.Instance.Send ( new Messaging.StatusMessage ( type, context, message, sDetail ) );
        }

        public static void Log ( Exception ex, string context )
        {
            Log ( ex, context, "Error in " + context + " : " + ex.Message );
        }

        public static void Log ( Exception ex, string context, string userMessage )
        {
            Messaging.Bus.Instance.Send (
                new Messaging.StatusMessage (
                    Messaging.StatusMessage.Types.Error,
                    context,
                    userMessage,
                    ex.ToString ( ) ) );
        }

        public static void Log ( TimeSpan ts, string actionName, string context )
        {
            Messaging.Bus.Instance.Send (
                new Messaging.StatusMessage (
                    Messaging.StatusMessage.Types.Debug,
                    context,
                    actionName + " took " + ts.ToPrettyString ( ),
                    null ) );
        }

        public static void Log ( TimeSpan ts, string actionName, string context, string sDetail )
        {
            Messaging.Bus.Instance.Send (
                new Messaging.StatusMessage (
                    Messaging.StatusMessage.Types.Debug,
                    context,
                    actionName + " took " + ts.ToPrettyString ( ),
                    sDetail ) );
        }

        public static void LogInfo ( string message, string context )
        {
            Log ( Messaging.StatusMessage.Types.Information, message, null, context );
        }

        public static void LogError ( string message, string sDetail, string context )
        {
            Log ( Messaging.StatusMessage.Types.Error, message, sDetail, context );
        }
    }
}
