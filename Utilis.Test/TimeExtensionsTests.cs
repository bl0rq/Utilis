using System;
using NUnit.Framework;
using Utilis.Extensions;

namespace Utilis.Test
{
    [TestFixture]
    public class TimeExtensionsTests
    {
        [Test]
        public void ToPrettyString_ReturnsExpectedFormatsAcrossRanges()
        {
            Assert.AreEqual("00:00", TimeSpan.Zero.ToPrettyString());
            Assert.AreEqual("500 ms", TimeSpan.FromMilliseconds(500).ToPrettyString());
            Assert.AreEqual("00:01", TimeSpan.FromSeconds(1).ToPrettyString());
            Assert.AreEqual("01:30", TimeSpan.FromSeconds(90).ToPrettyString());
            Assert.AreEqual("1:01:01", TimeSpan.FromSeconds(3661).ToPrettyString());
        }

        [Test]
        public void ToTimestampString_ReturnsNow_ForFutureTimestamps()
        {
            var future = DateTimeOffset.UtcNow.AddSeconds(1);

            Assert.AreEqual("now", future.ToTimestampString());
        }

        [Test]
        public void ToTimestampString_ReturnsMinuteText_ForRecentTimes()
        {
            var recent = DateTimeOffset.UtcNow.AddSeconds(-30);
            var oneMinuteWindow = DateTimeOffset.UtcNow.AddSeconds(-100);

            Assert.AreEqual("1 minute ago", recent.ToTimestampString());
            Assert.AreEqual("1 minute ago", oneMinuteWindow.ToTimestampString());
        }

        [Test]
        public void ToTimestampString_ReturnsExpectedBuckets_ForOlderTimes()
        {
            var minutes = DateTimeOffset.UtcNow.AddMinutes(-10);
            var hours = DateTimeOffset.UtcNow.AddHours(-5);
            var days = DateTimeOffset.UtcNow.AddDays(-12);
            var months = DateTimeOffset.UtcNow.AddDays(-90);

            Assert.AreEqual("10 minutes ago", minutes.ToTimestampString());
            Assert.AreEqual("5 hours ago", hours.ToTimestampString());
            Assert.AreEqual("12 days ago", days.ToTimestampString());
            Assert.AreEqual("3 months ago", months.ToTimestampString());
        }
    }
}
