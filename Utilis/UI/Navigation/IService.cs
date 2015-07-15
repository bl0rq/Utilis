using System;

namespace Utilis.UI.Navigation
{
	public interface IService
	{
		bool CanGoBack ( );
		void GoBack ( );
		void GoForward ( );
	    System.Threading.Tasks.Task<bool> Navigate<T_VM> ( T_VM parameter = null ) where T_VM : ViewModel.Base;
		event Action Navigated;
		ViewModel.Base CurrentViewModel { get; }
	}
}