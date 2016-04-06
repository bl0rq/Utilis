using System;
using System.Collections.Generic;
using System.Text;

namespace Utilis.Messaging
{
    public interface IFabric
    {
        void Send<T>(T msg) where T : IMessage;
        void AddListener<T>() where T : IMessage;
        void RemoveListener<T> ( ) where T : IMessage;
    }

    public class Fabric : IFabric
    {
        public void Send<T>(T msg) where T : IMessage
        {
            
        }

        public void AddListener<T>() where T : IMessage
        {

        }

        public void RemoveListener<T> ( ) where T : IMessage
        {

        }
    }
}