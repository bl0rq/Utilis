namespace Utilis.UI
{
	public interface IView
	{
		ViewModel.Base ViewModelObject { get; set; }
	}

	public interface IView<T> : IView where T : ViewModel.Base
	{
		T ViewModel { get; set; }
	}
}