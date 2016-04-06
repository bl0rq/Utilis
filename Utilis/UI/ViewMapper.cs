using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Utilis.Extensions;

namespace Utilis.UI
{
    public interface IViewMapper
    {
        Type GetView<T> ( ) where T : ViewModel.Base;
        Type GetView ( Type t );
    }

    public class ViewMapper : IViewMapper
    {
        private Dictionary<Type, Type> m_htTypes;
        private readonly ViewFinder m_viewFinder;

        public ViewMapper ( ViewFinder viewFinder, params Assembly [] assemblies )
        {
            Contract.AssertNotEmpty ( ( ) => assemblies, assemblies );
            Contract.AssertNotNull ( ( ) => viewFinder, viewFinder );

            m_viewFinder = viewFinder;

            Load ( assemblies );
        }

        private void Load ( params Assembly [] assemblies )
        {
            m_htTypes = new Dictionary<Type, Type> ( );

            foreach ( var ass in assemblies )
            {
                var foundTypes = m_viewFinder.Find ( ass );
                foreach ( var foundType in foundTypes )
                {
                    var type = foundType.B.AsType ( );
                    if ( m_htTypes.ContainsKey ( type ) )
                        Logger.Log ( Messaging.StatusMessage.Types.Debug, "Overwrite view mapping for type: " + type + " from " + m_htTypes [ type ] + " to " + foundType.A.AsType ( ) );
                    m_htTypes [ type ] = foundType.A.AsType ( );
                }
            }
        }

        public Type GetView<T> ( ) where T : ViewModel.Base
        {
            return GetView ( typeof ( T ) );
        }

        public Type GetView ( Type viewModelType )
        {
            var viewTypeInfo = m_htTypes.SafeGet ( viewModelType );
            if ( viewTypeInfo == null )
            {
                var baseType = viewModelType.GetTypeInfo ( ).BaseType;
                viewTypeInfo = m_htTypes.SafeGet ( baseType );
                return viewTypeInfo;
            }
            else
                return viewTypeInfo;
        }
    }
}
