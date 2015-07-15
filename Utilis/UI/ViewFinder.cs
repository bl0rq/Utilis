using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Utilis.UI
{
    public interface IViewFinder
    {
        IEnumerable<ViewFinder.Pair> Find ( Assembly ass );
    }

    public class ViewFinder : IViewFinder
    {
        public IEnumerable<Pair> Find ( Assembly ass )
        {
            var iviewType = typeof ( IView ).GetTypeInfo ( );

            return ass.DefinedTypes
                .Where ( t => !t.IsAbstract && iviewType.IsAssignableFrom ( t ) )
                .Select (
                    type =>
                        new
                        {
                            type,
                            interfaceTypePair =
                                type.ImplementedInterfaces
                                    .Select (
                                            interfaceType => new { interfaceType, interfaceTypeInfo = interfaceType.GetTypeInfo ( ) } )
                                    .FirstOrDefault (
                                            typePair =>
                                                typePair.interfaceTypeInfo.IsGenericType
                                                && typePair.interfaceType.GetGenericTypeDefinition ( ) == typeof ( IView<> ) )
                        } )
                .Where ( p => p.interfaceTypePair != null )
                .Select (
                    p => new Pair ( p.type, p.interfaceTypePair.interfaceTypeInfo.GenericTypeArguments.First ( ).GetTypeInfo ( ) ) );
        }

        public class Pair : Pair<TypeInfo, TypeInfo>
        {
            public TypeInfo ViewType
            {
                get { return A; }
                set { A = value; }
            }

            public TypeInfo ViewModelType
            {
                get { return B; }
                private set { B = value; }
            }

            public Pair ( TypeInfo viewType, TypeInfo viewModelType )
            {
                ViewType = viewType;
                ViewModelType = viewModelType;
            }
        }
    }
}
