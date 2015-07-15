using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Autofac;

namespace Utilis.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterByAttribute ( this ContainerBuilder builder, Assembly ass )
        {
            builder.RegisterAssemblyTypes ( ass )
               .Where ( t => t.GetTypeInfo ( ).CustomAttributes.Any ( o => o.AttributeType == typeof ( RegisterServiceAttribute ) ) )
               .AsSelf ( )
               .AsImplementedInterfaces ( );

            builder.RegisterAssemblyTypes ( ass )
               .Where ( t => t.GetTypeInfo ( ).CustomAttributes.Any ( o => o.AttributeType == typeof ( RegisterSingletonServiceAttribute ) ) )
               .AsSelf ( )
               .AsImplementedInterfaces ( )
               .SingleInstance ( ); ;
        }
    }
}
