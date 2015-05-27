using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Antlr.Runtime.Tree;
using BusinessLayer.ParkingManager;
using BusinessLayer.UnitMgr;
using Objects.WebApiResponse;
using Objects;

namespace StretchGarageWeb.Controllers
{
    public class UnitController : ApiController
    {
        // GET api/Unit/{id}
        public HttpResponseMessage Get(int id)
        {
            var unitMgr = new UnitManager();
            var res = unitMgr.GetUnitById(id);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
        }
        // POST api/Unit/
        public HttpResponseMessage Post([FromBody]UnitResponse value)
        {
            var unitMgr = new UnitManager();
            var res = unitMgr.CreateUnit(value);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody]UnitResponse value)
        {
            var unitMgr = new UnitManager();
            var res = unitMgr.UpdateUnit(value);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);
            return Request.CreateResponse(HttpStatusCode.OK, (ApiResponse)res);
        }

        [HttpPost]
        public HttpResponseMessage Park(int id, int parkingPlaceId)
        { 
            var unitMgr = new UnitManager();
            var res = unitMgr.GetUnitById(id);
            if(res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, res.Message);
            var unit = (UnitResponse)((ApiResponse) res).Content;
            
            var parkingplaceMgr = new ParkingPlaceManager();
            res = parkingplaceMgr.GetParkingPlace(parkingPlaceId);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, res.Message);

            var parkingPlace = (ParkingPlaceResponse) ((ApiResponse) res).Content;
            
            var parkMgr = new ParkCarManager();
            if (parkMgr.IsParked(unit.Id))
                parkMgr.UnParkCar(unit.Id);

            res = parkMgr.ParkCar(unit.Id, parkingPlace.Id);
            if (res is Error && res.Success == false)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse(HttpStatusCode.OK, res.Success);
        }

        [HttpDelete]
        public HttpResponseMessage Park(int id)
        {
            var unitMgr = new UnitManager();
            var res = unitMgr.GetUnitById(id);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, res.Message);
            var unit = (UnitResponse) ((ApiResponse) res).Content;

            var parkMgr = new ParkCarManager();
            parkMgr.UnParkCar(unit.Id);

            return Request.CreateResponse(HttpStatusCode.Accepted, (ApiResponse )res);
        }
    }
}