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
            var expect = 65346;
            var actual = LocationManager.CalculateUpdateInterval(2200);
            Assert.AreEqual(expect, actual);

            expect = 59405;
            actual = LocationManager.CalculateUpdateInterval(2000);
            Assert.AreEqual(expect, actual);

            expect = 59405;
            actual = LocationManager.CalculateUpdateInterval(2);
            Assert.AreNotEqual(expect, actual);
        }
    }
}
