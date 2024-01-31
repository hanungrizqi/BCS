using API_PLANT_BCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_PLANT_BCS.Controllers
{
    [RoutePrefix("api/MOK")]
    public class MOKController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        //Get Last No backlog
        [HttpGet]
        [Route("submitNoRegister")]
        public IHttpActionResult submitNoRegister(string eqpnumber)
        {
            try
            {
                var data = db.TBL_T_REGISTERs.OrderByDescending(a => a.CREATED_DATE).FirstOrDefault();
                string NoRegister = "";
                if (data == null)
                {
                    NoRegister = "0001-PLANET-BCS-" + eqpnumber;
                }
                else
                {
                    int setNo = Convert.ToInt32(data.NO_REGISTER.Substring(0, 4)) + 1;
                    NoRegister = setNo.ToString().PadLeft(4, '0') + "-PLANET-BCS-" + eqpnumber;
                }

                return Ok(new { Data = data, NoRegister = NoRegister });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_CompCode")]
        public IHttpActionResult Get_CompCode()
        {
            try
            {
                var data = db.VW_R_COMP_CODEs.ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Source")]
        public IHttpActionResult Get_Source()
        {
            try
            {
                var data = db.TBL_M_SOURCEs.ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_ListBacklog")]
        public async Task<IHttpActionResult> Get_ListBacklog()
        {
            try
            {
                db.CommandTimeout = 120;
                var data = await Task.Run(() =>
                {
                    return db.TBL_T_BACKLOGs.Where(a => a.POSISI_BACKLOG != "CLOSED" && a.STATUS != "CLOSE").ToList();
                });

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
