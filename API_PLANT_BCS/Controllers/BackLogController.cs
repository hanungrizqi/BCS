using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using API_PLANT_BCS.Models;
using API_PLANT_BCS.ViewModel;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web.SessionState;
using System.Web;

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
                db.CommandTimeout = 120;
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
                db.CommandTimeout = 120;
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
        public async Task<IHttpActionResult> Get_ListBacklogReview(string dstrct)
        {
            try
            {
                db.CommandTimeout = 120;
                //var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "ADM1" && !(a.STATUS.Contains("CANCEL"))).ToList();
                //var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct)
                //                            .Select(a => new { a.NO_BACKLOG, a.DSTRCT_CODE, a.EQP_NUMBER, a.COMP_CODE, a.COMP_DESC, a.EGI, a.HM, a.BACKLOG_DESC, a.INSPECTON_DATE, a.INSPECTOR, a.INSPECTOR_NAME, a.SOURCE, a.WORK_GROUP, a.STD_JOB, a.NRP_GL, a.NRP_GL_NAME, a.ORIGINATOR_ID, a.ORIGINATOR_ID_NAME, a.PLAN_REPAIR_DATE_1, a.PLAN_REPAIR_DATE_2, a.MANPOWER, a.HOUR_EST, a.POSISI_BACKLOG, a.CREATED_DATE, a.CREATED_BY, a.UPDATED_DATE, a.UPDATED_BY, a.REMARKS, a.WO_NO, a.IREQ_NO, a.INSTALL_DATE })
                //                            .ToList();
                //var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct).ToList();
                var data = await Task.Run(() =>
                {
                    return db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct).ToList();
                });

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        
        [HttpGet]
        [Route("Get_ListBacklogWOWR/{dstrct}")]
        public IHttpActionResult Get_ListBacklogWOWR(/*string dstrct, */string dstrct = "", string nrp = "")
        {
            //try
            //{
            //    if (dstrct == "INDE")
            //    {
            //        //valisasi wheel
            //        var karyawan_wheel = db.VW_KARYAWAN_PLANT_WHEELs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
            //        var karyawan_hauling = db.VW_KARYAWAN_PLANT_HAULINGs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();
            //        var karyawan_track = db.VW_KARYAWAN_PLANT_TRACKs.Where(x => x.EMPLOYEE_ID == nrp).FirstOrDefault();

            //        if (karyawan_hauling != null)
            //        {
            //            var data = db.VW_R_EQ_NUMBER_HAULINGs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

            //            return Ok(new { Data = data, Total = data.Count() });
            //        }
            //        else if (karyawan_track != null)
            //        {
            //            var data = db.VW_R_EQ_NUMBER_TRACKs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

            //            return Ok(new { Data = data, Total = data.Count() });
            //        }
            //        else if (karyawan_wheel != null)
            //        {
            //            var data = db.VW_R_EQ_NUMBER_WHEELs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

            //            return Ok(new { Data = data, Total = data.Count() });
            //        }
            //        else
            //        {
            //            var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "ADM1" && a.STATUS == "OPEN").ToList();

            //            return Ok(new { Data = data });
            //        }
            //    }
            //    else
            //    {
            //        var data = db.VW_R_EQ_NUMBERs.Where(a => a.DSTRCT_CODE == dstrct).ToList();

            //        return Ok(new { Data = data, Total = data.Count() });
            //    }

            //}
            //catch (Exception)
            //{
            //    return BadRequest();
            //}
            //lama
            try
            {
                db.CommandTimeout = 120;
                var data = db.VW_T_BACKLOGs.Where(a => a.DSTRCT_CODE == dstrct && a.POSISI_BACKLOG == "ADM1" && a.STATUS == "OPEN").ToList();

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
                db.CommandTimeout = 120;
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
                db.CommandTimeout = 120;
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
                db.CommandTimeout = 120;
                var data = db.VW_T_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Cek_History_Part")]
        public IHttpActionResult Cek_History_Part(VW_H_PART_BACKLOG param)
        {
            try
            {
                var cek = db.VW_H_PART_BACKLOGs.Where(a => a.DSTRCT_CODE == param.DSTRCT_CODE && a.EQP_NUMBER == param.EQP_NUMBER && a.STOCK_CODE == param.STOCK_CODE).FirstOrDefault();
                if (cek != null)
                {
                    //gk kepake, langsung cek if dibawah lagi
                    //cek.NO_BACKLOG = param.NO_BACKLOG;
                    //cek.DSTRCT_CODE = param.DSTRCT_CODE;
                    //cek.EQP_NUMBER = param.EQP_NUMBER;
                    //cek.STOCK_CODE = param.STOCK_CODE;
                    //cek.PART_NO = param.PART_NO;
                    //cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                    //cek.STATUS = param.STATUS;
                    //cek.CREATED_DATE = param.CREATED_DATE;
                    //cek.REMARKS = param.REMARKS;

                    if (cek.DSTRCT_CODE == param.DSTRCT_CODE && cek.EQP_NUMBER == param.EQP_NUMBER && cek.STOCK_CODE == param.STOCK_CODE)
                    {
                        var nobl = cek.NO_BACKLOG;
                        var eqpno = cek.EQP_NUMBER;
                        var stckd = cek.STOCK_CODE;
                        //var partn = param.PART_NO;
                        var posb = cek.POSISI_BACKLOG;
                        var stts = cek.STATUS;
                        //edit 13.05.2023
                        var message = string.Format("Stock code {0}, sudah ada di backlog {1}, dengan status {2}", stckd, nobl, stts);
                        return Ok(new { Remarkss = true, Datas = cek, bl = nobl, eq = eqpno, sc = stckd, /*pn = partn,*/ pb = posb, st = stts, Messages = message });
                    }
                    return Ok(new { Remarks = true });
                }
                else
                {
                    
                    return Ok(new { Remarks = true });

                }
                
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

                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

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

                    //history

                    if(cek.POSISI_BACKLOG != null && cek.POSISI_BACKLOG != "")
                    {
                        string oldl_posisi = cek.POSISI_BACKLOG;
                        his.No_Backlog = param.NO_BACKLOG;
                        his.Posisi_Backlog = oldl_posisi;
                        his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                        db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);

                        //edit 13.05.2023
                        return Ok(new { Remarkss = true, Message = "NO_BACKLOG already exists. Please Refresh Page" });
                    }
                    
                }
                else
                {
                    //edit 13.05.2023
                    var tbls = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();
                    if (tbls != null)
                    {
                        return Ok(new { Remarkss = true, Message = "NO_BACKLOG already exists. Please Refresh Page" });
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
                    //TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();
                    //tbl.NO_BACKLOG = param.NO_BACKLOG;
                    //tbl.DSTRCT_CODE = param.DSTRCT_CODE;
                    //tbl.EQP_NUMBER = param.EQP_NUMBER;
                    //tbl.COMP_CODE = param.COMP_CODE;
                    //tbl.EGI = param.EGI;
                    //tbl.HM = param.HM;
                    //tbl.BACKLOG_DESC = param.BACKLOG_DESC;
                    //tbl.INSPECTON_DATE = param.INSPECTON_DATE;
                    //tbl.INSPECTOR = param.INSPECTOR;
                    //tbl.SOURCE = param.SOURCE;
                    //tbl.WORK_GROUP = param.WORK_GROUP;
                    //tbl.STD_JOB = param.STD_JOB;
                    //tbl.NRP_GL = param.NRP_GL;
                    //tbl.ORIGINATOR_ID = param.ORIGINATOR_ID;
                    //tbl.PLAN_REPAIR_DATE_1 = param.PLAN_REPAIR_DATE_1;
                    //tbl.MANPOWER = param.MANPOWER;
                    //tbl.HOUR_EST = param.HOUR_EST;
                    //tbl.POSISI_BACKLOG = param.POSISI_BACKLOG;
                    //tbl.STATUS = param.STATUS;
                    //tbl.REMARKS = param.REMARKS;
                    //tbl.CREATED_BY = param.CREATED_BY;
                    //tbl.CREATED_DATE = DateTime.UtcNow.ToLocalTime();

                    //db.TBL_T_BACKLOGs.InsertOnSubmit(tbl);
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
        [Route("Create_Backlog_WOWR")]
        public IHttpActionResult Create_Backlog_WOWR(TBL_T_BACKLOG param)
        {
            try
            {
                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();

                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

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

                    //history

                    if (cek.POSISI_BACKLOG != null && cek.POSISI_BACKLOG != "")
                    {
                        string oldl_posisi = cek.POSISI_BACKLOG;
                        his.No_Backlog = param.NO_BACKLOG;
                        his.Posisi_Backlog = oldl_posisi;
                        his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                        db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);
                    }

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
        [Route("Rescheduled_WOWR")]
        public IHttpActionResult Rescheduled_WOWR(TBL_T_BACKLOG param)
        {
            try
            {
                if (param.STATUS == "PLANNER CANCEL")
                {
                    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                }
                string old_posisi = "";

                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();

                old_posisi = cek.POSISI_BACKLOG;

                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.PLAN_REPAIR_DATE_2 = param.PLAN_REPAIR_DATE_2;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                //history backlog
                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

                his.No_Backlog = param.NO_BACKLOG;
                his.Posisi_Backlog = old_posisi;
                his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);

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
                string old_posisi = "";
                if (param.STATUS == "PLANNER CANCEL")
                {
                    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                }

                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();

                old_posisi = cek.POSISI_BACKLOG;

                //update
                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                //history backlog
                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

                his.No_Backlog = param.NO_BACKLOG;
                his.Posisi_Backlog = old_posisi;
                his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);

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
                string old_posisi = "";

                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();

                old_posisi = cek.POSISI_BACKLOG;

                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.PLAN_REPAIR_DATE_2 = param.PLAN_REPAIR_DATE_2;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                //history backlog
                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

                his.No_Backlog = param.NO_BACKLOG;
                his.Posisi_Backlog = old_posisi;
                his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);

                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Rescheduled_Logistic")]
        public IHttpActionResult Rescheduled_Logistic(TBL_T_BACKLOG param)
        {
            try
            {
                if (param.STATUS == "PLANNER CANCEL")
                {
                    db.cusp_NotifBacklogCancel(param.NO_BACKLOG);
                }
                string old_posisi = "";

                var cek = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == param.NO_BACKLOG).FirstOrDefault();

                old_posisi = cek.POSISI_BACKLOG;

                cek.STATUS = param.STATUS;
                cek.REMARKS = param.REMARKS;
                cek.POSISI_BACKLOG = param.POSISI_BACKLOG;
                cek.PLAN_REPAIR_DATE_2 = param.PLAN_REPAIR_DATE_2;
                cek.UPDATED_BY = param.UPDATED_BY;
                cek.UPDATED_DATE = DateTime.UtcNow.ToLocalTime();

                //history backlog
                TBL_H_APPROVAL_BACKLOG his = new TBL_H_APPROVAL_BACKLOG();

                his.No_Backlog = param.NO_BACKLOG;
                his.Posisi_Backlog = old_posisi;
                his.Approved_Date = DateTime.UtcNow.ToLocalTime();

                db.TBL_H_APPROVAL_BACKLOGs.InsertOnSubmit(his);

                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("Create_BacklogPartTemp")]
        public IHttpActionResult Create_BacklogPartTemp(IList<TBL_T_TEMPORARY_PART> param)
        {
            try
            {
                //if (param[0].PART_NO == null)
                //{
                //    return Ok(new { Remarks = true });
                //}
                List<TBL_T_TEMPORARY_PART> tbl = new List<TBL_T_TEMPORARY_PART>();

                foreach (var item in param)
                {
                    var cek = db.TBL_T_TEMPORARY_PARTs.Where(a => a.PART_ID == item.PART_ID).FirstOrDefault();
                    if (cek != null)
                    {
                        cek.PART_ID = item.PART_ID;
                        cek.NO_BACKLOG = item.NO_BACKLOG;
                        cek.PART_NO = item.PART_NO;
                        cek.FIG_NO = item.FIG_NO;
                        cek.INDEX_NO = item.INDEX_NO;
                        cek.QTY = item.QTY;
                        cek.DSTRCT_CODE = item.DSTRCT_CODE;
                        cek.STOCK_CODE = item.STOCK_CODE;
                        //cek.STK_DESC = item.STK_DESC;
                        //cek.UOM = item.UOM;
                        //cek.PART_CLASS = item.PART_CLASS;
                        db.TBL_T_TEMPORARY_PARTs.InsertOnSubmit(cek);
                        db.SubmitChanges();
                        return Ok(new { Remarks = true });
                    }
                    else
                    {
                        TBL_T_TEMPORARY_PART tblt = new TBL_T_TEMPORARY_PART();
                        tblt.PART_ID = item.PART_ID;
                        tblt.NO_BACKLOG = item.NO_BACKLOG;
                        tblt.PART_NO = item.PART_NO;
                        tblt.FIG_NO = item.FIG_NO;
                        tblt.INDEX_NO = item.INDEX_NO;
                        tblt.QTY = item.QTY;
                        tblt.DSTRCT_CODE = item.DSTRCT_CODE;
                        tblt.STOCK_CODE = item.STOCK_CODE;
                        //cek.STK_DESC = item.STK_DESC;
                        //cek.UOM = item.UOM;
                        //cek.PART_CLASS = item.PART_CLASS;
                        db.TBL_T_TEMPORARY_PARTs.InsertOnSubmit(tblt);
                        db.SubmitChanges();
                        return Ok(new { Remarks = true });
                    }
                }

                db.TBL_T_TEMPORARY_PARTs.InsertAllOnSubmit(tbl);
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
        [Route("deletePartTemp")]
        public IHttpActionResult deletePartTemp(string partId)
        {
            try
            {
                var dataPart = db.TBL_T_TEMPORARY_PARTs.Where(a => a.PART_ID == partId).FirstOrDefault();

                db.TBL_T_TEMPORARY_PARTs.DeleteOnSubmit(dataPart);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }

        [HttpPost]
        [Route("deleteRepairTemp")]
        public IHttpActionResult deleteRepairTemp(string partId)
        {
            try
            {
                var dataPart = db.TBL_T_TEMPORARY_REPAIRs.Where(a => a.NO_BACKLOG == partId).ToList();

                db.TBL_T_TEMPORARY_REPAIRs.DeleteAllOnSubmit(dataPart);
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

        //Post Repair Temp
        [HttpPost]
        [Route("Create_RepairTemp")]
        public IHttpActionResult Create_RepairTemp(IList<TBL_T_TEMPORARY_REPAIR> listRepair)
        {
            try
            {
                //if (listRepair[0].PROBLEM_DESC == null)
                //{
                //    return Ok(new { Remarks = true });
                //}

                List<TBL_T_TEMPORARY_REPAIR> tblRepair = new List<TBL_T_TEMPORARY_REPAIR>();

                foreach (var repair in listRepair)
                {
                    tblRepair.Add(new TBL_T_TEMPORARY_REPAIR
                    {
                        REPAIR_ID = repair.REPAIR_ID,
                        NO_BACKLOG = repair.NO_BACKLOG,
                        PROBLEM_DESC = repair.PROBLEM_DESC,
                        ACTIVITY_REPAIR = repair.ACTIVITY_REPAIR
                    });
                }

                db.TBL_T_TEMPORARY_REPAIRs.InsertAllOnSubmit(tblRepair);
                db.SubmitChanges();
                return Ok(new { Remarks = true });

                //List<TBL_T_TEMPORARY_REPAIR> tblRepair = new List<TBL_T_TEMPORARY_REPAIR>();

                //foreach (var repair in listRepair)
                //{
                //    var cek = db.TBL_T_TEMPORARY_REPAIRs.Where(a => a.REPAIR_ID == repair.REPAIR_ID).FirstOrDefault();
                //    if (cek != null)
                //    {
                //        cek.REPAIR_ID = repair.REPAIR_ID;
                //        cek.ACTIVITY_REPAIR = repair.ACTIVITY_REPAIR;
                //        cek.PROBLEM_DESC = repair.PROBLEM_DESC;
                //        cek.NO_BACKLOG = repair.NO_BACKLOG;

                //        db.TBL_T_TEMPORARY_REPAIRs.InsertOnSubmit(cek);
                //    }
                //    else
                //    {
                //        TBL_T_TEMPORARY_REPAIR tblt = new TBL_T_TEMPORARY_REPAIR();

                //        tblt.REPAIR_ID = repair.REPAIR_ID;
                //        tblt.ACTIVITY_REPAIR = repair.ACTIVITY_REPAIR;
                //        tblt.PROBLEM_DESC = repair.PROBLEM_DESC;
                //        tblt.NO_BACKLOG = repair.NO_BACKLOG;

                //        db.TBL_T_TEMPORARY_REPAIRs.InsertOnSubmit(tblt);
                //        db.SubmitChanges();
                //        return Ok(new { Remarks = true });
                //    }
                //}

                //db.TBL_T_TEMPORARY_REPAIRs.InsertAllOnSubmit(tblRepair);
                //db.SubmitChanges();
                //return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
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
                db.CommandTimeout = 120;
                var dataCek = db.VW_T_BACKLOGs.Where(a => a.STATUS == "PROGRESS").ToList();
                foreach (var item in dataCek)
                {
                    var cekCompleteItem = db.VW_R_COMPLETE_CODEs.Where(a => a.DSTRCT_CODE == item.DSTRCT_CODE && a.WORK_ORDER == item.WO_NO).FirstOrDefault();
                    if (cekCompleteItem != null)
                    {
                        var updateTbl = db.TBL_T_BACKLOGs.Where(a => a.NO_BACKLOG == item.NO_BACKLOG).FirstOrDefault();
                        updateTbl.STATUS = "CLOSE";
                        updateTbl.POSISI_BACKLOG = "CLOSED";
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
