using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilis.UI
{
	public class BasePage<T> : Microsoft.Phone.Controls.PhoneApplicationPage, IView<T> where T : ViewModel.Base
	{
		//protected override void OnNavigatedTo ( System.Windows.Navigation.NavigationEventArgs e )
		//{
		//	base.OnNavigatedTo ( e );

		//	ViewModel = e.Content as T;

		//	this.DataContext = ViewModel;
		//}

		protected virtual void OnViewModelSet ( )
		{
		}

		private T m_viewModel;
		public T ViewModel
		{
			get { return m_viewModel; }
			set { m_viewModel = value; DataContext = value; OnViewModelSet ( ); }
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
