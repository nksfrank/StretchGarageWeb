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
            var expect = 130693;
            var actual = LocationManager.CalculateUpdateInterval(2200, 1000);
            Assert.AreEqual(expect, actual);

            expect = 118811;
            actual = LocationManager.CalculateUpdateInterval(2000, 1000);
            Assert.AreEqual(expect, actual);

            expect = 118811;
            actual = LocationManager.CalculateUpdateInterval(2, 1000);
            Assert.AreNotEqual(expect, actual);

            expect = 1800000;
            actual = LocationManager.CalculateUpdateInterval(30200, 1000);
            Assert.AreNotEqual(expect, actual);

            expect = 1800000;
            actual = LocationManager.CalculateUpdateInterval(30303, 1000);
            Assert.AreEqual(expect, actual);

            expect = 1800000;
            actual = LocationManager.CalculateUpdateInterval(30400, 1000);
            Assert.AreEqual(expect, actual);
        }
    }
}
