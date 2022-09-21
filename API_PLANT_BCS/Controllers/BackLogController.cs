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
    [RoutePrefix("api/BackLog")]
    public class BackLogController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        //Get Last No backlog
        [HttpGet]
        [Route("Get_LastNoBacklog/{dstrct}")]
        public IHttpActionResult Get_LastNoBacklog(string dstrct)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct).OrderByDescending(a => a.CREATED_DATE).FirstOrDefault();

                return Ok(new {Data = data});
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListBacklog/{dstrct}")]
        public IHttpActionResult Get_ListBacklog(string dstrct)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListApprovalBacklog/{dstrct}")]
        public IHttpActionResult Get_ListApprovalBacklog(string dstrct)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.STATUS == "SUBMITTED").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Get Recommended Part
        [HttpGet]
        [Route("Get_BacklogPart/{noBacklog}")]
        public IHttpActionResult Get_BacklogPart(string noBacklog)
        {
            try
            {
                var data = db.VW_PART_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Get Detail Part
        [HttpGet]
        [Route("Get_PartDetail")]
        public IHttpActionResult Get_PartDetail(string partNO, string site)
        {
            try
            {
                var data = db.VW_PART_MSF100s.Where(a => a.PART_NO == partNO && a.DSTRCT_CODE == site).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        //Get Detail Backlog
        [HttpGet]
        [Route("Get_BacklogDetail/{noBacklog}")]
        public IHttpActionResult Get_BacklogDetail(string noBacklog)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        //Post Registrasi Backlog
        [HttpPost]
        [Route("Create_Backlog")]
        public IHttpActionResult Create_Backlog(TBL_T_BACKLOG param)
        {
            try
            {
                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();
                if (cek != null)
                {
                    cek.NO_BACKLOG = param.NO_BACKLOG;
                    cek.DSTRCT_CODE = param.DSTRCT_CODE;
                    cek.EQP_NUMBER = param.EQP_NUMBER;
                    cek.COMP_CODE = param.COMP_CODE;
                    cek.EGI = param.EGI;
                    cek.HM = param.HM;
                    cek.BACKLOG_DESC = param.BACKLOG_DESC;
                    cek.INSPECTON_DATE = param.INSPECTON_DATE;
                    cek.INSPECTOR = param.INSPECTOR;
                    cek.SOURCE = param.SOURCE;
                    cek.WORK_GROUP = param.WORK_GROUP;
                    cek.STD_JOB = param.STD_JOB;
                    cek.NRP_GL = param.NRP_GL;
                    cek.ORIGINATOR_ID = param.ORIGINATOR_ID;
                    cek.PLAN_REPAIR_DATE_1 = param.PLAN_REPAIR_DATE_1;
                    cek.MANPOWER = param.MANPOWER;
                    cek.HOUR_EST = param.HOUR_EST;
                    cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                    cek.STATUS = param.STATUS;
                    cek.REMARKS = param.REMARKS;
                    cek.UPDATED_BY = param.UPDATED_BY;
                    cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                }
                else
                {
                    TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();
                    tbl.NO_BACKLOG = param.NO_BACKLOG;
                    tbl.DSTRCT_CODE = param.DSTRCT_CODE;
                    tbl.EQP_NUMBER = param.EQP_NUMBER;
                    tbl.COMP_CODE = param.COMP_CODE;
                    tbl.EGI = param.EGI;
                    tbl.HM = param.HM;
                    tbl.BACKLOG_DESC = param.BACKLOG_DESC;
                    tbl.INSPECTON_DATE = param.INSPECTON_DATE;
                    tbl.INSPECTOR = param.INSPECTOR;
                    tbl.SOURCE = param.SOURCE;
                    tbl.WORK_GROUP = param.WORK_GROUP;
                    tbl.STD_JOB = param.STD_JOB;
                    tbl.NRP_GL = param.NRP_GL;
                    tbl.ORIGINATOR_ID = param.ORIGINATOR_ID;
                    tbl.PLAN_REPAIR_DATE_1 = param.PLAN_REPAIR_DATE_1;
                    tbl.MANPOWER = param.MANPOWER;
                    tbl.HOUR_EST = param.HOUR_EST;
                    tbl.POSISI_BACKLOG = param.POSISI_BACKLOG;
                    tbl.STATUS = param.STATUS;
                    tbl.REMARKS = param.REMARKS;
                    tbl.CREATED_BY = param.CREATED_BY;
                    tbl.CREATED_DATE = DateTime.UtcNow.ToLocalTime();
                    db.TBL_T_BACKLOGs.InsertOnSubmit(tbl);
                }
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Approve_Backlog")]
        public IHttpActionResult Approve_Backlog(TBL_T_BACKLOG param)
        {
            try
            {
                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();
                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        //Post Registrasi Backlog
        [HttpPost]
        [Route("Create_BacklogPart")]
        public IHttpActionResult Create_BacklogPart(IList<TBL_T_RECOMMENDED_PART> param)
        {
            try
            {
                List<TBL_T_RECOMMENDED_PART> tbl = new List<TBL_T_RECOMMENDED_PART>();

                foreach (var item in param)
                {
                    var cek = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.PART_ID == item.PART_ID).FirstOrDefault();
                    if (cek == null)
                    {
                        tbl.Add(item);
                    }
                }

                db.TBL_T_RECOMMENDED_PARTs.InsertAllOnSubmit(tbl);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Delete_Backlog")]
        public IHttpActionResult Delete_Backlog(string noBacklog)
        {
            try
            {
                var data = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).FirstOrDefault();

                db.TBL_T_BACKLOGs.DeleteOnSubmit(data);
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
