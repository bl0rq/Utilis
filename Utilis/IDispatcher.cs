using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis
{
    public interface IDispatcher
    {
        bool CheckAccess ( );
        Task RunAsync ( Action act );
    }
}

namespace Utilis.Test
{
    public class FakeDispatcher : IDispatcher
    {
        private readonly bool m_checkAccessResult;

        public FakeDispatcher ( bool checkAccessResult = true )
        {
            m_checkAccessResult = checkAccessResult;
        }


        public bool CheckAccess ( )
        {
            return m_checkAccessResult;
        }

        public Task RunAsync ( Action act )
        {
            act ( );
            return Task.FromResult<object> ( null );
        }
    }
}
