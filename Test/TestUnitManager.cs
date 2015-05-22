using System;
using System.Transactions;
using BusinessLayer.UnitMgr;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Objects;
using Objects.WebApiResponse;

namespace Test
{
    [TestClass]
    public class TestUnitManager
    {
        [TestMethod]
        public void TestCreateUnit()
        {
            using (TransactionScope trans = new TransactionScope())
            {
                var unitMgr = new UnitManager();
                var expected = new UnitResponse() {Name = "Albert", Type = UnitType.MOBILE };
                var actual = unitMgr.CreateUnit(expected);
                Assert.IsTrue(actual.Success);
                var id = (int)((ApiResponse)actual).Content;
                Assert.AreNotEqual(0, id);

                var unit = unitMgr.GetUnitById(id);
                UnitResponse unitResponse = (UnitResponse)((ApiResponse)unit).Content;
                Assert.AreEqual(expected, unitResponse.Name);
                Assert.AreEqual(UnitType.MOBILE, unitResponse.Type);
            }
        }

        [TestMethod]
        public void TestGetUnitById()
        {
            using (TransactionScope trans = new TransactionScope())
            {
                var unitMgr = new UnitManager();
                var id = 1;
                var unit = unitMgr.GetUnitById(id);
                UnitResponse unitResponse = (UnitResponse)((ApiResponse)unit).Content;
                Assert.AreEqual<int>(id, unitResponse.Id);
            }
        }
    }
}
