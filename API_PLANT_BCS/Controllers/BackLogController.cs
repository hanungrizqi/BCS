using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls.WebParts;
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
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.STATUS != "PROGRESS").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListBacklogPlanner/{dstrct}")]
        public IHttpActionResult Get_ListBacklogPlanner(string dstrct)
        {
            try
            {
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "Planner1").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListBacklogReview/{dstrct}")]
        public IHttpActionResult Get_ListBacklogReview(string dstrct)
        {
            try
            {
                //var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "ADM1" && !(a.STATUS.Contains("CANCEL"))).ToList();
                var data = db.VW_T_BACKLOGs.ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListBacklogWOWR/{dstrct}")]
        public IHttpActionResult Get_ListBacklogWOWR(string dstrct)
        {
            try
            {
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "ADM1" && a.STATUS=="OPEN").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListLogistic/{dstrct}")]
        public IHttpActionResult Get_ListLogistic(string dstrct)
        {
            try
            {
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "Logistic" && a.STATUS != "PLANNER CANCEL").ToList();

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
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "Planner").ToList();

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
                var data = db.VW_T_PART_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).ToList();

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
                var data = db.VW_R_PART_MSF170s.Where(a => a.PART_NO == partNO  && a.DSTRCT_CODE == site).FirstOrDefault();
                return Ok(new { Data = data, Remarks = true });
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
                var data = db.VW_T_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).FirstOrDefault();

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
                if (param.STATUS == "PLANNER CANCEL")
                {
                    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                }
                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();
                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Rescheduled_Backlog")]
        public IHttpActionResult Rescheduled_Backlog(TBL_T_BACKLOG param)
        {
            try
            {
                if (param.STATUS == "PLANNER CANCEL")
                {
                    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                }

                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();
                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.PLAN_REPAIR_DATE_2 = param.PLAN_REPAIR_DATE_2;
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
                if (param[0].PART_NO == null)
                {
                    return Ok(new { Remarks = true });
                }
                List<TBL_T_RECOMMENDED_PART> tbl = new List<TBL_T_RECOMMENDED_PART>();

                foreach (var item in param)
                {
                    var cek = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.PART_ID == item.PART_ID).FirstOrDefault();
                    if (cek == null)
                    {
                        tbl.Add(item);
                    }
                    else
                    {
                        cek.QTY = item.QTY;
                        cek.FIG_NO = item.FIG_NO;
                        cek.INDEX_NO = item.INDEX_NO;
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
                var dataPart = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.NO_BACKLOG == noBacklog).ToList();
                var dataRepair = db.TBL_T_RECOMMENDED_REPAIRs.Where(a => a.NO_BACKLOG == noBacklog).ToList();

                db.TBL_T_BACKLOGs.DeleteOnSubmit(data);
                db.TBL_T_RECOMMENDED_PARTs.DeleteAllOnSubmit(dataPart);
                db.TBL_T_RECOMMENDED_REPAIRs.DeleteAllOnSubmit(dataRepair);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Delete_BacklogPart/{partID}")]
        public IHttpActionResult Delete_BacklogPart(string partID)
        {
            try
            {
                var data = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.PART_ID == partID).FirstOrDefault();

                db.TBL_T_RECOMMENDED_PARTs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpGet]
        [Route("Get_BacklogRepair/{noBacklog}")]
        public IHttpActionResult Get_BacklogRepair(string noBacklog)
        {
            try
            {
                var data = db.TBL_T_RECOMMENDED_REPAIRs.Where(a => a.NO_BACKLOG == noBacklog).ToList();

                return Ok(new { Data = data, Total = data.Count() });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        //Post Registrasi Backlog
        [HttpPost]
        [Route("Create_BacklogRepair")]
        public IHttpActionResult Create_BacklogRepair(IList<TBL_T_RECOMMENDED_REPAIR> listRepair)
        {
            try
            {
                if (listRepair[0].PROBLEM_DESC == null) {
                    return Ok(new { Remarks = true });
                }

                List<TBL_T_RECOMMENDED_REPAIR> tblRepair = new List<TBL_T_RECOMMENDED_REPAIR>();

                foreach (var repair in listRepair)
                {
                    tblRepair.Add(new TBL_T_RECOMMENDED_REPAIR
                    {
                        NO_BACKLOG = repair.NO_BACKLOG,
                        PROBLEM_DESC = repair.PROBLEM_DESC,
                        ACTIVITY_REPAIR = repair.ACTIVITY_REPAIR
                    });
                }

                db.TBL_T_RECOMMENDED_REPAIRs.InsertAllOnSubmit(tblRepair);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        
        [HttpPost]
        [Route("Update_BacklogRepair")]
        public IHttpActionResult Update_BacklogRepair(TBL_T_RECOMMENDED_REPAIR listRepair)
        {
            try
            {
                var cek = db.TBL_T_RECOMMENDED_REPAIRs.Where(a => a.REPAIR_ID == listRepair.REPAIR_ID).FirstOrDefault();
                cek.ACTIVITY_REPAIR = listRepair.ACTIVITY_REPAIR;
                cek.PROBLEM_DESC = listRepair.PROBLEM_DESC;

                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Delete_BacklogReparir/{repairID}")]
        public IHttpActionResult Delete_BacklogReparir(int repairID)
        {
            try
            {
                var data = db.TBL_T_RECOMMENDED_REPAIRs.Where(a => a.REPAIR_ID == repairID).FirstOrDefault();

                db.TBL_T_RECOMMENDED_REPAIRs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpGet]
        [Route("Update_Closing_Backlog")]
        public IHttpActionResult Update_Closing_Backlog()
        {
            try
            {
                var dataCek = db.VW_T_BACKLOGs.Where(a => a.STATUS == "PROGRESS").ToList();
                foreach (var item in dataCek)
                {
                    var cekCompleteItem = db.VW_R_COMPLETE_CODEs.Where(a => a.DSTRCT_CODE == item.DSTRCT_CODE && a.WORK_ORDER == item.WO_NO).FirstOrDefault();
                    if (cekCompleteItem != null)
                    {
                        var updateTbl = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == item.NO_BACKLOG).FirstOrDefault();
                        updateTbl.STATUS = "CLOSE";
                        updateTbl.INSTALL_DATE = cekCompleteItem.JOB_DUR_DATE;
                    }
                }
                db.SubmitChanges();
                return Ok(new { Remarks = true, Message = "Update Backlog Status Berhasil" });
            }
            catch (Exception ex)
            {
                return Ok(new { Remarks = false, Message = ex.Message });
            }
        }


    }
}
