using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PLANT_BCS.Models;

namespace API_PLANT_BCS.Controllers
{

    [RoutePrefix("api/Logistic")]
    public class LogisticController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        [HttpPost]
        [Route("Update_ETASupply")]
        public IHttpActionResult Update_ETASupply(IList<TBL_T_RECOMMENDED_PART> param)
        {
            try
            {
                List<TBL_T_RECOMMENDED_PART> tbl = new List<TBL_T_RECOMMENDED_PART>();

                foreach (var item in param)
                {
                    var cek = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.PART_ID == item.PART_ID).FirstOrDefault();
                    cek.ETA_SUPPLY = item.ETA_SUPPLY;
                    cek.LOCATION_ON_STOCK = item.LOCATION_ON_STOCK;
                    cek.AVAILABLE_STOCK = item.AVAILABLE_STOCK;
                }

                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

    }
}
