using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis
{
    [AttributeUsage ( AttributeTargets.Class )]
    public class RegisterServiceAttribute : Attribute
    {
    }

    [AttributeUsage ( AttributeTargets.Class )]
    public class RegisterSingletonServiceAttribute : Attribute
    {
    }
}
