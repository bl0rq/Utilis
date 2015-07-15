using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI
{
	public class BasePage<T> : Windows.UI.Xaml.Controls.Page, IView<T> where T : ViewModel.Base
	{
		public BasePage ( )
		{
			this.Transitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection ( ) { new Windows.UI.Xaml.Media.Animation.EntranceThemeTransition ( ) };
		}
		private T m_viewModel;
		public T ViewModel
		{
			get { return m_viewModel; }
			set { m_viewModel = value; OnViewModelSet ( ); }
		}

		protected override void OnNavigatedTo ( Windows.UI.Xaml.Navigation.NavigationEventArgs e )
		{
			ViewModel = e.Parameter as T;

			this.DataContext = ViewModel;
		}

		protected virtual void OnViewModelSet ( )
		{
		}

		public ViewModel.Base ViewModelObject
		{
			get { return ViewModel; }
			set
			{
				if ( value == null )
					ViewModel = null;
				else
				{
					ViewModel = Contract.AssertIsType<T> ( ( ) => value, value );
				}
			}
		}
	}
}
