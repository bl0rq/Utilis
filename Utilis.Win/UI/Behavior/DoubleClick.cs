using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI.Win.Behavior
{
    public class DoubleClick : System.Windows.Interactivity.Behavior<System.Windows.Controls.ItemsControl>
    {
        public System.Windows.Input.ICommand Command
        {
            get { return (System.Windows.Input.ICommand)GetValue ( CommandProperty ); }
            set { SetValue ( CommandProperty, value ); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly System.Windows.DependencyProperty CommandProperty =
            System.Windows.DependencyProperty.Register ( "Command", typeof ( System.Windows.Input.ICommand ), typeof ( DoubleClick ), new System.Windows.PropertyMetadata ( null ) );


        protected override void OnAttached ( )
        {
            base.OnAttached ( );

            this.AssociatedObject.MouseDoubleClick += AssociatedObject_MouseDoubleClick;
        }

        void AssociatedObject_MouseDoubleClick ( object sender, System.Windows.Input.MouseButtonEventArgs e )
        {
            if ( Command == null )
                return;

            System.Windows.Controls.ItemsControl list = sender as System.Windows.Controls.ItemsControl;
            if ( list == null )
                return;

            System.Windows.DependencyObject container =
            System.Windows.Controls.ItemsControl.ContainerFromElement ( list, (System.Windows.DependencyObject)e.OriginalSource );

            if ( container == null ||
                 Equals ( container, System.Windows.DependencyProperty.UnsetValue ) )
                return;

            object data = list.ItemContainerGenerator.ItemFromContainer ( container );
            if ( Command.CanExecute ( data ) )
                Command.Execute ( data );
        }

        protected override void OnDetaching ( )
        {
            base.OnDetaching ( );

            this.AssociatedObject.MouseDoubleClick -= AssociatedObject_MouseDoubleClick;
        }
    }
}
