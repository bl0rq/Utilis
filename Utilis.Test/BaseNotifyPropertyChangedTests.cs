using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using NUnit.Framework;
using Utilis.ObjectModel;

namespace Utilis.Test
{
    [TestFixture]
    public class BaseNotifyPropertyChangedTests
    {
        [Test]
        public void SetProperty_FiresPropertyChanged_WhenValueChanges()
        {
            var vm = new TestNotifyObject();
            var changed = new List<string?>();
            vm.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

            vm.Name = "first";

            Assert.AreEqual(1, changed.Count);
            Assert.AreEqual(nameof(TestNotifyObject.Name), changed[0]);
        }

        [Test]
        public void SetProperty_DoesNotFire_WhenValueIsSame()
        {
            var vm = new TestNotifyObject();
            var changed = 0;
            vm.PropertyChanged += (_, _) => changed++;

            vm.Name = "same";
            vm.Name = "same";

            Assert.AreEqual(1, changed);
        }

        [Test]
        public void DisableEnableOnChange_BatchesPropertiesChanged()
        {
            var vm = new TestNotifyObject();
            var propertyChangedCount = 0;
            var propertiesChangedPayload = new List<IEnumerable<Pair<string?, TunneledPropertyChangedEventArgs>>>();

            vm.PropertyChanged += (_, _) => propertyChangedCount++;
            vm.PropertiesChanged += (_, items) => propertiesChangedPayload.Add(items.ToArray());

            vm.DisableOnChange();
            vm.Name = "a";
            vm.Name = "b";
            vm.Age = 2;
            vm.EnableOnChange();

            Assert.AreEqual(2, propertyChangedCount);
            Assert.AreEqual(1, propertiesChangedPayload.Count);

            var names = propertiesChangedPayload[0].Select(i => i.A).ToArray();
            Assert.That(names, Is.EquivalentTo(new[] { nameof(TestNotifyObject.Name), nameof(TestNotifyObject.Age) }));
        }

        [Test]
        public void OnPropertyChanged_WithInner_ProvidesTunneledInfo()
        {
            var vm = new TestNotifyObject();
            TunneledPropertyChangedEventArgs? tunneled = null;
            vm.PropertyChanged += (_, e) => tunneled = e as TunneledPropertyChangedEventArgs;

            vm.RaiseWrapped(nameof(TestNotifyObject.Name), new PropertyChangedEventArgs("InnerProp"));

            Assert.IsNotNull(tunneled);
            Assert.That(tunneled, Is.Not.Null);
            Assert.AreEqual(nameof(TestNotifyObject.Name), tunneled.PropertyName);
            Assert.IsTrue(tunneled.ContainsProperty(nameof(TestNotifyObject.Name)));
            Assert.IsTrue(tunneled.ContainsProperty("InnerProp"));
        }

        private sealed class TestNotifyObject : BaseNotifyPropertyChanged
        {
            private string? _name;
            private int _age;

            public string? Name
            {
                get => _name;
                set => SetProperty(ref _name, value);
            }

            public int Age
            {
                get => _age;
                set => SetProperty(ref _age, value);
            }

            public void RaiseWrapped(string propertyName, PropertyChangedEventArgs inner)
            {
                OnPropertyChanged(propertyName, inner);
            }
        }
    }
}
