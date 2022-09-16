using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PLANT_BCS.Models;
using API_PLANT_BCS.ViewModel;

namespace API_PLANT_BCS.Controllers
{
    [RoutePrefix("api/Login")]
    public class LoginController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        [HttpPost]
        [Route("Get_Login")]
        public IHttpActionResult Get_Login(ClsLogin param)
        {
            bool remarks = false;
            try
            {

                bool status = param.Login();

                remarks = status;

                return Ok(new { Remarks = remarks });
            }
            catch (Exception)
            {

                return Ok(remarks);
            }

        }
    }
}
