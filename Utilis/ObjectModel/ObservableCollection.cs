using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilis.Extensions;

namespace Utilis.ObjectModel
{
    public interface INotifyCollectionChanged<T> : ICollection<T>, System.Collections.Specialized.INotifyCollectionChanged, System.ComponentModel.INotifyPropertyChanged
    {
        void AddRange ( IList<T> items );
        void AddRange ( T [] items );
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

        bool m_isInAddRange = false;
        public void AddRange ( IList<T> items )
        {
            if ( m_isInAddRange )
                throw new NotSupportedException ( "Another add-range is already in progress!" );

            m_isInAddRange = true;

            foreach ( var item in items )
            {
                Add ( item );
            }

            var dlgCollectionChangedDispatcher = CollectionChangedDispatcher;
            if ( dlgCollectionChangedDispatcher != null )
                Runner.RunOnDispatcherThreadBlocking (
                    ( ) =>
                    {
                        foreach ( var item in items )
                        {
                            dlgCollectionChangedDispatcher (
                                this,
                                new System.Collections.Specialized.NotifyCollectionChangedEventArgs ( System.Collections.Specialized.NotifyCollectionChangedAction.Add, item ) );
                        }
                    } );

            var dlgCollectionChangedNormal = CollectionChangedNormal;
            if ( dlgCollectionChangedNormal != null )
            {
                foreach ( var item in items )
                {
                    dlgCollectionChangedNormal (
                        this,
                        new System.Collections.Specialized.NotifyCollectionChangedEventArgs ( System.Collections.Specialized.NotifyCollectionChangedAction.Add, item ) );
                }
            }

            m_isInAddRange = false;
        }

        public void AddRange ( T [] items )
        {
            AddRange ( (IList<T>)items );
        }

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
            if ( m_isInAddRange )
                return;

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

        protected bool SetProperty<T_P> ( ref T_P oldValue, T_P newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
        {
            bool areDifferent = !object.Equals ( oldValue, newValue );
            oldValue = newValue;

            if ( areDifferent )
            {
                OnPropertyChanged ( propertyName );
            }

            return areDifferent;
        }
        protected void OnPropertyChanged<T_P> ( System.Linq.Expressions.Expression<Func<T_P>> exp )
        {
            OnPropertyChanged ( exp.GetName ( ) );
        }

        protected void OnPropertyChanged ( string name )
        {
            var e = new System.ComponentModel.PropertyChangedEventArgs ( name );

            OnPropertyChanged ( e );
        }
    }
}
