using System;
using System.Runtime.Serialization;

namespace Utilis.Messaging
{
    //[SerializableAttribute]
    [DataContract]
    public class StatusMessage : IMessage // BaseDataMessage<StatusMessage.Status>
    {
        [DataContract]
        public enum Types
        {
            Fatal = 1,
            Error = 2,
            Warn = 3,
            Information = 4,
            Debug = 5
        }

        [DataContract]
        public class Status
        {
            [DataMember ( Order = 1 )]
            public Types Type { get; set; }

            [DataMember ( Order = 2 )]
            public string Context { get; set; }

            [DataMember ( Order = 3 )]
            public string Message { get; set; }

            [DataMember ( Order = 4 )]
            public string TechnicalDetail { get; set; }

            [DataMember ( Order = 5 )]
            public DateTimeOffset Timestamp { get; set; }

            public override string ToString ( )
            {
                return $"Type: {Type}, Context: {Context}, Message: {Message}, TechnicalDetail: {TechnicalDetail}, Timestamp: {Timestamp}";
            }
        }

        public StatusMessage ( )
        {
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

        public override string ToString ( )
        {
            return Data?.ToString ( ) ?? "{null data}";
        }

        [System.Runtime.Serialization.DataMember ( Order = 1 )]
        public StatusMessage.Status Data { get; set; }

        public bool IsAsync { get; } = true;
    }
}