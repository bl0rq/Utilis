using System;
using System.Collections.Generic;
using NUnit.Framework;
using Utilis.Extensions;

namespace Utilis.Test
{
    [TestFixture]
    public class PairCollectionRandomTests
    {
        [Test]
        public void Pair_EqualsAndHashCode_AreConsistent()
        {
            var a = new Pair<string, int>("x", 7);
            var b = new Pair<string, int>("x", 7);

            Assert.IsTrue(a.Equals(b));
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        [Test]
        public void PairAOnlyComparer_IgnoresBValue()
        {
            var comparer = new PairAOnlyComparer<int, string>();
            var a = new Pair<int, string>(10, "first");
            var b = new Pair<int, string>(10, "second");

            Assert.IsTrue(comparer.Equals(a, b));
        }

        [Test]
        public void RemoveArray_RemovesMatchedIndex_NotAlwaysFirst()
        {
            var one = new object();
            var two = new object();
            var three = new object();
            var arr = new[] { one, two, three };

            var result = arr.Remove(two);

            Assert.AreEqual(2, result.Length);
            Assert.AreSame(one, result[0]);
            Assert.AreSame(three, result[1]);
        }

        [Test]
        public void NextBool_WithInvalidOdds_Throws()
        {
            var random = new Random(123);

            Assert.Throws<ArgumentOutOfRangeException>(() => random.NextBool(-0.1));
            Assert.Throws<ArgumentOutOfRangeException>(() => random.NextBool(1.1));
        }

        [Test]
        public void NextItem_ThrowsForEmptyCollections()
        {
            var random = new Random(123);

            Assert.Throws<ArgumentException>(() => random.NextItem(Array.Empty<int>()));
            Assert.Throws<ArgumentException>(() => random.NextItem((IList<int>)new List<int>()));
        }

        [Test]
        public void NextEnum_ReturnsDefinedValue()
        {
            var random = new Random(123);
            var result = random.NextEnum<TestEnum>();

            Assert.IsTrue(Enum.IsDefined(typeof(TestEnum), result));
        }

        private enum TestEnum
        {
            A,
            B,
            C
        }
    }
}
