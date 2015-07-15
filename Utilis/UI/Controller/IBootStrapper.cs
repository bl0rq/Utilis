using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Controller
{
    public interface IBootStrapper : IDisposable
    {
        Task StartAsync ( );
    }
}