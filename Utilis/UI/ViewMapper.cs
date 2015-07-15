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

        public ViewMapper ( Assembly ass, ViewFinder viewFinder )
        {
            Contract.AssertNotNull ( ( ) => ass, ass );
            Contract.AssertNotNull ( ( ) => viewFinder, viewFinder );

            m_viewFinder = viewFinder;

            Load ( ass );
        }

        private void Load ( Assembly ass )
        {
            m_htTypes = new Dictionary<Type, Type> ( );

            var foundTypes = m_viewFinder.Find ( ass );
            foreach ( var foundType in foundTypes )
            {
                m_htTypes [ foundType.B.AsType ( ) ] = foundType.A.AsType ( );
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
