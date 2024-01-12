using API_PLANT_BCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_PLANT_BCS.Controllers
{
    [RoutePrefix("api/MOK")]
    public class MOKController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        //Get Last No backlog
        [HttpGet]
        [Route("Get_LastNoBacklog_Mok")]
        public IHttpActionResult Get_LastNoBacklog_Mok(string dstrct)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct).OrderByDescending(a => a.CREATED_DATE).FirstOrDefault();
                string NoBacklog = "";
                if (data == null)
                {
                    NoBacklog = "0001" + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy") + dstrct;
                }
                else
                {
                    string month = data.NO_BACKLOG.Substring(4, 2);
                    string year = data.NO_BACKLOG.Substring(6, 4);
                    string thisMonth = DateTime.Now.ToString("MM");
                    string thisYear = DateTime.Now.ToString("yyyy");
                    if (month == thisMonth && year == thisYear)
                    {
                        int setNo = Convert.ToInt32(data.NO_BACKLOG.Substring(0, 4)) + 1;
                        NoBacklog = setNo.ToString().PadLeft(4, '0') + thisMonth + thisYear + dstrct;
                    }
                    else
                    {
                        NoBacklog = "0001" + thisMonth + thisYear + dstrct;
                    }
                }

                return Ok(new { Data = data, NoBacklog = NoBacklog});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
