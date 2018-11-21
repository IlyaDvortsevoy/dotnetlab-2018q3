using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestService.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("get")]        
        public IHttpActionResult Get()
        {
            return Ok("OK!");
        }
    }
}
