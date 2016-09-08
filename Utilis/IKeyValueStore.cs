using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis
{
    public interface IKeyValueStore
    {
        object this [ string propertyName ] { get; set; }
        void Save ( );
    }
}
