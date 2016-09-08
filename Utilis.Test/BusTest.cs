using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Utilis.Test
{
    [TestClass]
    public class BusTest
    {
        [TestMethod]
        public void TestBasicSend ( )
        {
            var busCounter = new BusCountListener<JunkMessage> ( );
            using ( Messaging.Bus.Instance.ListenFor ( busCounter ) )
            {
                Messaging.Bus.Instance.Send ( new JunkMessage ( ) );
            }

            Assert.AreEqual ( 1, busCounter.Count );
        }
    }

    public class BusCountListener<T> : Messaging.IListener<T> where T : Messaging.IMessage
    {
        public int Count { get; private set; }

        public BusCountListener ( )
        {
            Count = 0;
        }

        public void Receive ( T message )
        {
            Count++;
        }
    }

    public class JunkMessage : Messaging.BaseSyncMessage
    {
    }
}
