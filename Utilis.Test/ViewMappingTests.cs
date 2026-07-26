using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Utilis.UI;

namespace Utilis.Test
{
    [TestFixture]
    public class ViewMappingTests
    {
        [Test]
        public void ViewFinder_FindsGenericViews()
        {
            var finder = new ViewFinder();
            var matches = finder.Find(typeof(DashboardView).Assembly).ToArray();

            Assert.IsTrue(matches.Any(p => p.ViewType.AsType() == typeof(DashboardView) && p.ViewModelType.AsType() == typeof(DashboardViewModel)));
        }

        [Test]
        public void ViewMapper_ReturnsMappedView_ForExactViewModelType()
        {
            var mapper = new ViewMapper(new ViewFinder(), typeof(DashboardView).Assembly);

            var view = mapper.GetView(typeof(DashboardViewModel));

            Assert.AreEqual(typeof(DashboardView), view);
        }

        [Test]
        public void ViewMapper_FallsBackToBaseViewModelType()
        {
            var mapper = new ViewMapper(new ViewFinder(), typeof(DashboardView).Assembly);

            var view = mapper.GetView(typeof(DashboardChildViewModel));

            Assert.AreEqual(typeof(DashboardView), view);
        }

        [Test]
        public void ViewMapper_ReturnsNull_WhenNoMappingExists()
        {
            var mapper = new ViewMapper(new ViewFinder(), typeof(DashboardView).Assembly);

            var view = mapper.GetView(typeof(UnmappedViewModel));

            Assert.IsNull(view);
        }

        private class DashboardViewModel : Utilis.UI.ViewModel.Base
        {
        }

        private class DashboardChildViewModel : DashboardViewModel
        {
        }

        private class UnmappedViewModel : Utilis.UI.ViewModel.Base
        {
        }

        private class DashboardView : IView<DashboardViewModel>
        {
            public DashboardViewModel ViewModel { get; set; } = null!;

            public Utilis.UI.ViewModel.Base ViewModelObject
            {
                get => ViewModel;
                set => ViewModel = (DashboardViewModel)value;
            }
        }
    }
}
