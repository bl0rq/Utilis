using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This was causing stupid issues.  No clue how or why.
//namespace System.Collections.Specialized
//{
//    // why did this damn thing not exist?!?
//    public interface INotifyCollectionChanged<T> : ICollection<T>, INotifyCollectionChanged
//    {

//    }
//}
namespace Utilis.ObjectModel
{
    public interface INotifyCollectionChanged<T> : ICollection<T>, System.Collections.Specialized.INotifyCollectionChanged
    {

    }

    public class ObservableCollection<T> : System.Collections.ObjectModel.ObservableCollection<T>, INotifyCollectionChanged<T>
    {
        public ObservableCollection ( )
        {
        }

        public ObservableCollection ( List<T> list )
            : base ( list )
        {
        }

        public ObservableCollection ( IEnumerable<T> collection )
            : base ( collection )
        {
        }

        private event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChangedNormal;
        private event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChangedDispatcher;

        public override event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if ( Runner.Dispatcher != null && Runner.Dispatcher.CheckAccess ( ) )
                    CollectionChangedDispatcher += value;
                else
                    CollectionChangedNormal += value;
            }
            remove
            {
                if ( Runner.Dispatcher != null && Runner.Dispatcher.CheckAccess ( ) )
                    CollectionChangedDispatcher -= value;
                else
                    CollectionChangedNormal -= value;
            }
        }

        private readonly object m_collectionChangedLock = new object ( );
        protected override void OnCollectionChanged ( System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
        {
            // one at a time or things get confusing.  
            //TODO: Could possibly queue them to keep from blocking?
            lock ( m_collectionChangedLock )
            {
                var dlgCollectionChangedDispatcher = CollectionChangedDispatcher;
                if ( dlgCollectionChangedDispatcher != null )
                    Runner.RunOnDispatcherThreadBlocking ( ( ) => dlgCollectionChangedDispatcher ( this, e ) ); // go sync here to ensure we do not continue until things have been processed

                var dlgCollectionChangedNormal = CollectionChangedNormal;
                if ( dlgCollectionChangedNormal != null )
                    dlgCollectionChangedNormal ( this, e );
            }
        }
    }
}
