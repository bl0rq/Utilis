using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilis.Extensions;
using Utilis.Win.Extensions;

namespace Utilis.Win.UI.Behavior
{
    public class PersistLocation : System.Windows.Interactivity.Behavior<System.Windows.Window>
    {
        protected override void OnAttached ( )
        {
            base.OnAttached ( );

            AssociatedObject.SourceInitialized += AssociatedObject_SourceInitialized;
            AssociatedObject.Closing += AssociatedObject_Closing;
        }

        private void AssociatedObject_Closing ( object sender, System.ComponentModel.CancelEventArgs e )
        {
            StorePlacement ( );
        }

        private void AssociatedObject_SourceInitialized ( object sender, EventArgs e )
        {
            IKeyValueStore kvStore = ServiceLocator.Instance.GetInstance<IKeyValueStore> ( );
            if ( kvStore != null )
            {
                var placementXml = kvStore [ GetName ( ) ];
                Console.WriteLine ( "Loading: Key: " + GetName ( ) + ", Value: " + ( placementXml ?? "{null}" ) );
                AssociatedObject.SetPlacement ( (string)placementXml );
            }
            else
                Logger.LogError ( "Unable to find IKeyValueStore.", "Unable to persist window location for " + GetName ( ) + " due to missing IKeyValueStore", "Utilis.Win.UI.Behavior.PersistLocation" );
        }

        private string GetName ( )
        {
            return KeyName ?? ( AssociatedObject.Name ?? AssociatedObject.GetType ( ).Name ) + "_Placement";
        }

        protected override void OnDetaching ( )
        {
            base.OnDetaching ( );

            StorePlacement ( );

            AssociatedObject.SourceInitialized -= AssociatedObject_SourceInitialized;
            AssociatedObject.Closing -= AssociatedObject_Closing;
        }

        private void StorePlacement ( )
        {
            IKeyValueStore kvStore = ServiceLocator.Instance.GetInstance<IKeyValueStore> ( );
            if ( kvStore != null )
            {
                var placementXml = AssociatedObject.GetPlacement ( );
                Console.WriteLine ( "Saving: Key: " + GetName ( ) + ", Value: " + ( placementXml ?? "{null}" ) );
                kvStore [ GetName ( ) ] = placementXml;
                kvStore.Save ( );
            }
            else
                Logger.LogError ( "Unable to find IKeyValueStore.", "Unable to persist window location for " + GetName ( ) + " due to missing IKeyValueStore", "Utilis.Win.UI.Behavior.PersistLocation" );
        }

        public string KeyName
        {
            get { return (string)GetValue ( KeyNameProperty ); }
            set { SetValue ( KeyNameProperty, value ); }
        }
        public static readonly System.Windows.DependencyProperty KeyNameProperty =
            System.Windows.DependencyProperty.Register (
                "KeyName",
                typeof ( string ),
                typeof ( PersistLocation ),
                new System.Windows.PropertyMetadata ( null ) );


    }
}
