using System;
using NUnit.Framework;

namespace Utilis.Test
{
    [TestFixture]
    public class ContractTests
    {
        [Test]
        public void AssertNotNull_ReturnsValue_WhenNotNull()
        {
            var value = "abc";
            var result = Contract.AssertNotNull(() => value, value);
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void AssertNotNull_Throws_WhenNull()
        {
            string? value = null;
            var ex = Assert.Throws<Contract.AssertionException>(() => Contract.AssertNotNull(() => value, value));
            Assert.IsTrue(ex.Message.Contains("value must not be null."));
        }

        [Test]
        public void AssertNotEmptyString_ReturnsValue_WhenNotEmpty()
        {
            var value = "x";
            var result = Contract.AssertNotEmpty(() => value, value);
            Assert.AreEqual("x", result);
        }

        [Test]
        public void AssertNotEmptyString_Throws_WhenEmpty()
        {
            var value = "";
            var ex = Assert.Throws<Contract.AssertionException>(() => Contract.AssertNotEmpty(() => value, value));
            Assert.IsTrue(ex.Message.Contains("value must not be empty."));
        }

        [Test]
        public void AssertNotEmptyArray_Throws_WhenNullOrEmpty()
        {
            int[]? nullItems = null;
            int[] emptyItems = Array.Empty<int>();

            var exNull = Assert.Throws<Contract.AssertionException>(() => Contract.AssertNotEmpty(() => nullItems!, nullItems!));
            var exEmpty = Assert.Throws<Contract.AssertionException>(() => Contract.AssertNotEmpty(() => emptyItems, emptyItems));

            Assert.IsTrue(exNull.Message.Contains("nullItems must not be empty."));
            Assert.IsTrue(exEmpty.Message.Contains("emptyItems must not be empty."));
        }

        [Test]
        public void Ensure_Throws_WhenFalse()
        {
            var ex = Assert.Throws<Contract.AssertionException>(() => Contract.Ensure(false, "bad"));
            Assert.AreEqual("bad", ex.Message);
        }

        [Test]
        public void AssertTrue_Throws_WhenFalse()
        {
            var ex = Assert.Throws<Contract.AssertionException>(() => Contract.AssertTrue(false, "false"));
            Assert.AreEqual("false", ex.Message);
        }

        [Test]
        public void AssertIsType_ReturnsTypedValue_WhenTypeMatches()
        {
            object obj = "abc";
            var result = Contract.AssertIsType<string>(() => obj!, obj!);
            Assert.AreEqual("abc", result);
        }

        [Test]
        public void AssertIsType_ReturnsDefault_WhenNull()
        {
#pragma warning disable CS8603, CS8604
            object? obj = null;
            var result = Contract.AssertIsType<string>(() => obj, obj);
            Assert.IsNull(result);
#pragma warning restore CS8603, CS8604
        }

        [Test]
        public void AssertIsType_Throws_WhenTypeDiffers()
        {
            object obj = 3;
            var ex = Assert.Throws<Contract.AssertionException>(() => Contract.AssertIsType<string>(() => obj, obj));
            Assert.IsTrue(ex.Message.Contains("Invalid type (Int32) for obj"));
        }
    }
}
