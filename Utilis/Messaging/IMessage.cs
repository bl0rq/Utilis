using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Messaging
{
	public interface IMessage { bool IsAsync { get; } }

	public abstract class BaseDataMessage<T> : IMessage
	{
		public T Data { get; set; }

		protected BaseDataMessage ( )
		{
		}

		protected BaseDataMessage ( T data )
		{
			Data = data;
		}

		public virtual bool IsAsync
		{
			get { return true; }
		}
	}

	//public interface IAsyncMessage : IMessage { }
}