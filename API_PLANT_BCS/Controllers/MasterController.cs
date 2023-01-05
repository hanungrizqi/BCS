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
                var data = db.VW_M_JOBSITEs.ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }



        [HttpGet]
        [Route("Get_JobsiteByUsername")]
        public IHttpActionResult Get_JobsiteByUsername(string username = "")
        {
            try
            {
                var data = db.VW_MSF020s.Where(x => x.ENTITY == username).ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_LocationOnStock/{dstrct}")]
        public IHttpActionResult Get_LocationOnStock(string dstrct)
        {
            try
            {
                var data = db.VW_WH_STOCK_CODEs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

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
        [Route("Get_PartNo")]
        public IHttpActionResult Get_PartNo(string stckCode, string dsctrct)
        {
            try
            {
                string partNo = "";
                if (stckCode == null)
                {
                    stckCode = "";
                    dsctrct = "";
                }
                else
                {
                    partNo = stckCode;
                    stckCode = stckCode.PadLeft(9, '0');
                }
                var data = db.VW_R_STOCK_CODEs.Where(b => b.DSTRCT_CODE == dsctrct && (b.STOCK_CODE == stckCode || b.PART_NO == partNo)).ToList();

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
        
        [HttpGet]
        [Route("Get_EqNumber/{site}/{nrp}")]
        public IHttpActionResult Get_EqNumber(string site = "", string nrp = "")
        {
            try
            {
                if(site == "INDE")
                {
                    //valisasi wheel
                    var karyawan_wheel = db.VW_KARYAWAN_PLANT_WHEELs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
                    var karyawan_hauling = db.VW_KARYAWAN_PLANT_HAULINGs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
                    var karyawan_track = db.VW_KARYAWAN_PLANT_TRACKs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();

                    if(karyawan_hauling != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_HAULINGs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else if(karyawan_track != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_TRACK_WHEELs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else if(karyawan_wheel != null)
                    {
                        var data = db.VW_R_EQ_NUMBER_TRACK_WHEELs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                    else
                    {
                        var data = db.VW_R_EQ_NUMBERs.Where(a => a.DSTRCT_CODE == site).ToList();

                        return Ok(new { Data = data, Total = data.Count() });
                    }
                }
                else
                {
                    var data = db.VW_R_EQ_NUMBERs.Where(a => a.DSTRCT_CODE == site).ToList();

                    return Ok(new { Data = data, Total = data.Count() });
                }
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_EGI/{eqNumber}")]
        public IHttpActionResult Get_EGI(string eqNumber)
        {
            try
            {
                var data = db.VW_R_EQ_NUMBERs.Where(a => a.EQUIP_NO == eqNumber).FirstOrDefault();

                return Ok(new { Data = data });
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
        [Route("Get_NRPGL/{site}")]
        public IHttpActionResult Get_NRPGL(string site)
        {
            try
            {
                var data = db.VW_KARYAWAN_PLANTs.Where(a => a.DSTRCT_CODE == site).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_OriginatorID/{site}")]
        public IHttpActionResult Get_OriginatorID(string site)
        {
            try
            {
                var data = db.VW_KARYAWAN_PLANT_N_MCHes.Where(a => a.DSTRCT_CODE == site).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_STDJob/{site}")]
        public IHttpActionResult Get_STDJob(string site)
        {
            try
            {
                var data = db.VW_R_STD_JOBs.Where(a => a.DSTRCT_CODE == site).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
    }
}
