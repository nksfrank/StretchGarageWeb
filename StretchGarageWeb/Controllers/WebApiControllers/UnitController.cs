using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Objects.WebApiResponse;
using Objects;

namespace StretchGarageWeb.Controllers
{
    public class UnitController : ApiController
    {
        // GET api/CreateUnit/5
        public HttpResponseMessage Get(string username, int type)
        {
            var res = BusinessLayer.UnitMgr.UnitManager.CreateUnit(username, type);
            if (res is Error)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, res.Message);

            return Request.CreateResponse<ApiResponse>(HttpStatusCode.OK, (ApiResponse)res);
        }
    }
}