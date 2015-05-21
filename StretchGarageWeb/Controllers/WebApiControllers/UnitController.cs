using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
    }
}