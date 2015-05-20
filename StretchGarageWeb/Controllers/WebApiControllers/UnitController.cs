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
        // GET api/CreateUnit/5
        public HttpResponseMessage Get(string username, int type)
        {
            var unitMgr = new UnitManager();
            UnitType unitType = (UnitType)type;
            var res = unitMgr.CreateUnit(username, unitType);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, (ApiResponse)res);
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