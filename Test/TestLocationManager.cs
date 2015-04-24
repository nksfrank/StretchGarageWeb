using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer.LocationManager;

namespace Test
{
    [TestClass]
    public class TestLocationManager
    {
        [TestMethod]
        public void TestCalculateInterval()
        {
            var expect = 3920;
            var actual = LocationManager.CalculateUpdateInterval(2200);
            Assert.AreEqual(expect, actual);

            expect = 3564;
            actual = LocationManager.CalculateUpdateInterval(2000);
            Assert.AreEqual(expect, actual);

            expect = 3564;
            actual = LocationManager.CalculateUpdateInterval(2);
            Assert.AreNotEqual(expect, actual);
        }
    }
}
