using System;

namespace Utilis.Messaging
{
    public class StatusMessage : BaseDataMessage<StatusMessage.Status>
    {
        public enum Types
        {
            Fatal = 1, 
            Error = 2, 
            Warn = 3, 
            Information = 4, 
            Debug = 5
        }

        public class Status
        {
            public Types Type { get; set; }
            public string Context { get; set; }
            public string Message { get; set; }
            public string TechnicalDetail { get; set; }
            public DateTimeOffset Timestamp { get; set; }
        }

        public StatusMessage ( Types type, string context, string message, string technicalDetail )
        {
            Data = new Status ( )
                {
                    Type = type,
                    Context = context,
                    Message = message,
                    TechnicalDetail = technicalDetail,
                    Timestamp = DateTimeOffset.Now
                };
        }

        public StatusMessage ( Status status )
        {
            Data = status;
        }

        public StatusMessage (
            Types type,
            string message,
            string technicalDetail,
            [System.Runtime.CompilerServices.CallerMemberName] string method = "",
            [System.Runtime.CompilerServices.CallerFilePath] string file = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int callingLineNumber = 0 )
        {
            Data = new Status ( )
            {
                Type = type,
                Context = System.IO.Path.GetFileNameWithoutExtension ( file ) + "." + method + ", " + file + ":" + callingLineNumber,
                Message = message,
                TechnicalDetail = technicalDetail,
                Timestamp = DateTimeOffset.Now
            };
        }
    }
}