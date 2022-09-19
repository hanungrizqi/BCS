using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PLANT_BCS.Models;

namespace API_PLANT_BCS.Controllers
{
    [RoutePrefix("api/Master")]
    public class MasterController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        [HttpGet]
        [Route("Get_Jobsite")]
        public IHttpActionResult Get_Jobsite()
        {
            try
            {
                var data = db.VW_JOBSITEs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Employee")]
        public IHttpActionResult Get_Employee()
        {
            try
            {
                var data = db.VW_KARYAWAN_ALLs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_Employee/{id}")]
        public IHttpActionResult Get_Employee(string id)
        {
            try
            {
                var data = db.VW_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID ==id ).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_Group")]
        public IHttpActionResult Get_Group()
        {
            try
            {
                var data = db.TBL_M_ROLEs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

    }
}
