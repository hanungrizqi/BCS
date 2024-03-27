using API_PLANT_BCS.Models;
using API_PLANT_BCS.ViewModel;
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
        ClsMOK cls = new ClsMOK();

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
        [Route("Get_ListBacklogRegister/{dstrct}")]
        public IHttpActionResult Get_ListBacklogRegister(string dstrct = "")
        {
            try
            {
                db.CommandTimeout = 120;
                var data = db.TBL_T_REGISTERs.Where(a => a.DSTRCT_CODE == dstrct && a.STATUS == "CREATED").ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_LastRegister/{noregister}")]
        public IHttpActionResult Get_LastRegister(string noregister)
        {
            try
            {
                var data = db.TBL_T_REGISTERs.Where(a => a.NO_REGISTER == noregister).OrderByDescending(a => a.CREATED_DATE).FirstOrDefault();

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
                var data = cls.GetListDistrict();

                return Ok(new { Data = data, Status = true, Message = "Data berhasil diambil!" });
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

        //Get Detail Part
        [HttpGet]
        [Route("Get_PartDetail")]
        public IHttpActionResult Get_PartDetail(string partNO, string site)
        {
            try
            {
                var data = db.VW_R_PART_MSF170s.Where(a => a.PART_NO == partNO && a.DSTRCT_CODE == site).FirstOrDefault();
                return Ok(new { Data = data, Remarks = true });
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

                    if (cek.POSISI_BACKLOG != null && cek.POSISI_BACKLOG != "")
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
        [Route("Create_BacklogPartTemp")]
        public IHttpActionResult Create_BacklogPartTemp(IList<TBL_T_TEMPORARY_PART> param)
        {
            try
            {
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

        [HttpPost]
        [Route("Create_BacklogRepair")]
        public IHttpActionResult Create_BacklogRepair(IList<TBL_T_RECOMMENDED_REPAIR> listRepair)
        {
            try
            {
                if (listRepair[0].PROBLEM_DESC == null)
                {
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
    }
}
