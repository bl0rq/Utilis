using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilis.Extensions;

namespace Utilis.ObjectModel
{
    [System.Runtime.Serialization.DataContract]
    public abstract class BaseNotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly object m_propertiesListSync = new object ( );

        public event Action<BaseNotifyPropertyChanged, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>>> PropertiesChanged;
        protected void DoPropertiesChanged ( BaseNotifyPropertyChanged sender, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>> e )
        {
            if ( !m_isPropertiesChangedDisabled )
            {
                Action<BaseNotifyPropertyChanged, IEnumerable<Pair<string, TunneledPropertyChangedEventArgs>>> act = PropertiesChanged;
                if ( act != null )
                    act ( sender, e );
            }
        }

        protected bool SetProperty<T> ( ref T oldValue, T newValue, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = "" )
        {
            bool areDifferent = !object.Equals ( oldValue, newValue );
            oldValue = newValue;

            if ( areDifferent )
            {
                OnPropertyChanged ( propertyName );
            }

            return areDifferent;
        }

        private bool m_isPropertiesChangedDisabled = false;
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void DoPropertyChanged ( System.ComponentModel.PropertyChangedEventArgs e )
        {
            System.ComponentModel.PropertyChangedEventHandler oHandler = PropertyChanged;
            if ( oHandler != null )
                oHandler ( this, e );
        }

        protected virtual void OnPropertyChanged ( System.ComponentModel.PropertyChangedEventArgs e )
        {


        }

        protected void OnPropertyChanged<T> ( System.Linq.Expressions.Expression<Func<T>> exp )
        {
            OnPropertyChanged ( exp.GetName ( ) );
        }

        protected void OnPropertyChanged ( string name )
        {
            TunneledPropertyChangedEventArgs e = new TunneledPropertyChangedEventArgs ( name, this, null );

            OnPropertyChanged ( e );

            if ( !m_isDisableOnChange )
            {
                DoPropertyChanged ( e );

                DoPropertiesChanged ( this, new [] { new Pair<string, TunneledPropertyChangedEventArgs> ( e.PropertyName, e ) } );
            }
            else if ( m_isTrackChanges )
            {
                lock ( m_propertiesListSync )
                    if ( !m_propertiesChanged.Any ( item => item.A == name ) )
                        m_propertiesChanged.Add ( new Pair<string, TunneledPropertyChangedEventArgs> ( name, e ) );

                m_needsChangeFired = true;
            }
        }

        protected void OnPropertyChanged ( string name, System.ComponentModel.PropertyChangedEventArgs inner )
        {
            TunneledPropertyChangedEventArgs e = new TunneledPropertyChangedEventArgs ( name, this, inner );

            OnPropertyChanged ( e );

            //TunneledPropertyChangedEventArgs innerTunned = inner as TunneledPropertyChangedEventArgs;
            //if ( inner != null && innerTunned == null )
            //    innerTunned = new TunneledPropertyChangedEventArgs ( inner.PropertyName, null, null );

            if ( !m_isDisableOnChange )
            {
                DoPropertyChanged ( e );

                DoPropertiesChanged ( this, new [] { new Pair<string, TunneledPropertyChangedEventArgs> ( e.PropertyName, e ) } );
            }
            else if ( m_isTrackChanges )
            {
                lock ( m_propertiesListSync )
                    if ( !m_propertiesChanged.Any ( item => item.A == name && item.B == inner ) )
                        m_propertiesChanged.Add ( new Pair<string, TunneledPropertyChangedEventArgs> ( name, e ) );

                m_needsChangeFired = true;
            }
        }
        
        private int m_disabledCount = 0;
        protected bool m_isDisableOnChange = false;
        protected bool m_needsChangeFired = true;
        protected bool m_isTrackChanges = true;
        protected List<Pair<string, TunneledPropertyChangedEventArgs>> m_propertiesChanged = new List<Pair<string, TunneledPropertyChangedEventArgs>> ( );

        public void DisableOnChange ( )
        {
            DisableOnChange ( true );
        }

        public void DisableOnChange ( bool bTrackChanges )
        {
            m_isTrackChanges = bTrackChanges;

            m_isDisableOnChange = true;
            m_disabledCount++;
        }

        public void EnableOnChange ( )
        {
            m_disabledCount--;
            if ( m_disabledCount == 0 )
            {
                m_isDisableOnChange = false;
                if ( m_needsChangeFired )
                {
                    Pair<string, TunneledPropertyChangedEventArgs> [] propertiesChanged;
                    lock ( m_propertiesListSync )
                    {
                        propertiesChanged = m_propertiesChanged.ToArray ( );
                        m_propertiesChanged.Clear ( );
                    }

                    m_isPropertiesChangedDisabled = true;
                    foreach ( Pair<string, TunneledPropertyChangedEventArgs> oProperty in propertiesChanged )
                    {
                        OnPropertyChanged ( oProperty.A, oProperty.B );
                    }
                    m_isPropertiesChangedDisabled = false;

                    DoPropertiesChanged ( this, propertiesChanged );
                }
            }
        }
    }


    public class TunneledPropertyChangedEventArgs : System.ComponentModel.PropertyChangedEventArgs
    {
        private static Pair<object, string> [] EmptyList = new Pair<object, string> [] { };

        public System.ComponentModel.PropertyChangedEventArgs Inner { get; set; }

        public Type CurrentType { get; set; }

        public object Sender { get; set; }

        public TunneledPropertyChangedEventArgs ( string sPropertyName, object oSender, System.ComponentModel.PropertyChangedEventArgs oInner )
            : base ( sPropertyName )
        {
            Sender = oSender;
            CurrentType = oSender.GetType ( );
            Inner = oInner;
        }

        public bool ContainsProperty ( string sProperty )
        {
            if ( string.IsNullOrEmpty ( sProperty ) )
                return false;

            return GetAllPropertiesChanged ( ).Any ( item => item.B == sProperty );
        }

        public virtual IEnumerable<Pair<object, string>> GetAllPropertiesChanged ( )
        {
            return new Pair<object, string> [] { new Pair<object, string> ( Sender, PropertyName ) }.Concat ( GetAllInnerPropertiesChanged ( ) );
        }

        protected IEnumerable<Pair<object, string>> GetAllInnerPropertiesChanged ( )
        {
            if ( Inner != null )
            {
                TunneledPropertyChangedEventArgs oInner = Inner as TunneledPropertyChangedEventArgs;
                if ( oInner != null )
                    return oInner.GetAllPropertiesChanged ( );
                else
                    return new Pair<object, string> [] { new Pair<object, string> ( null, Inner.PropertyName ) };
            }
            return EmptyList;
        }
    }

    public class TunneledMultiPropertyChangedEventArgs : TunneledPropertyChangedEventArgs
    {
        public IEnumerable<string> PropertyNames { get; set; }

        public TunneledMultiPropertyChangedEventArgs ( IEnumerable<string> aPropertyNames, object oSender, System.ComponentModel.PropertyChangedEventArgs oInner )
            : base ( aPropertyNames.First ( ), oSender, oInner )
        {
            PropertyNames = aPropertyNames;
        }

        public override IEnumerable<Pair<object, string>> GetAllPropertiesChanged ( )
        {
            return PropertyNames
                .Select ( sPropertyName => new Pair<object, string> ( Sender, sPropertyName ) )
                .Concat ( GetAllInnerPropertiesChanged ( ) );
        }
    }
}
