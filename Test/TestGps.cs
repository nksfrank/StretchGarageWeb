using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer.Utilities;

namespace Test
{
    [TestClass]
    public class TestGps
    {
        [TestMethod]
        public void TestDistanceBetweenPlacesM()
        {
            var expect = 2105.91;
            var actual = Gps.DistanceBetweenPlacesM(55.60296650, 13.02299800, 55.60284900, 12.99749900);
            Assert.AreEqual(expect, actual, 0.01);

            expect = 0.00;
            actual = Gps.DistanceBetweenPlacesM(55.60284900, 12.99749900, 55.60284900, 12.99749900);
            Assert.AreEqual(expect, actual, 0.01);
        }
        [TestMethod]
        public void TestDistanceBetweenPlacesKm()
        {
            var expect = 2.10591;
            var actual = Gps.DistanceBetweenPlacesKm(55.60296650, 13.02299800, 55.60284900, 12.99749900);
            Assert.AreEqual(expect, actual, 0.00001);

            expect = 0.00;
            actual = Gps.DistanceBetweenPlacesKm(55.60284900, 12.99749900, 55.60284900, 12.99749900);
            Assert.AreEqual(expect, actual, 0.1);
        }
    }
}
