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
    [RoutePrefix("api/Setting")]
    public class SettingController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();


        //Setting Source
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
        [Route("Get_Source/{id}")]
        public IHttpActionResult Get_Source(int id)
        {
            try
            {
                var data = db.TBL_M_SOURCEs.Where(a => a.ID == id).FirstOrDefault();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("Get_Email")]
        public IHttpActionResult Get_Email()
        {
            var data = db.VW_Emails.ToList();
            return Ok(new { Data = data });
        }

        [HttpPost]
        [Route("Create_Email")]
        public IHttpActionResult Create_Email(TBL_M_EMAIL param)
        {
            try
            {
                TBL_M_EMAIL tbl = new TBL_M_EMAIL();
                tbl.RoleName = param.RoleName;
                tbl.Username = param.Username;

                db.TBL_M_EMAILs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpPost]
        [Route("Delete_Email")]
        public IHttpActionResult Delete_Email(int id, string nrp)
        {
            try
            {
                var data = db.TBL_M_EMAILs.Where(a => a.ID == id && a.Username == nrp).FirstOrDefault();

                db.TBL_M_EMAILs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }

        [HttpPost]
        [Route("Create_Source")]
        public IHttpActionResult Create_Source(TBL_M_SOURCE param)
        {
            try
            {
                TBL_M_SOURCE tbl = new TBL_M_SOURCE();
                tbl.SOURCE = param.SOURCE;
                db.TBL_M_SOURCEs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Update_Source")]
        public IHttpActionResult Update_Source(TBL_M_SOURCE param)
        {
            try
            {
                var data = db.TBL_M_SOURCEs.Where(a => a.ID == param.ID).FirstOrDefault();
                data.SOURCE = param.SOURCE;
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        
        [HttpPost]
        [Route("Delete_Source")]
        public IHttpActionResult Delete_Source(int id)
        {
            try
            {
                var data = db.TBL_M_SOURCEs.Where(a => a.ID == id).FirstOrDefault();
                db.TBL_M_SOURCEs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Ok(new { Remarks = true });
            }
            catch (Exception e)
            {
                return Ok(new { Remarks = false, Message = e });
            }
        }
        //END Setting Source
        //Setting User
        [HttpPost]
        [Route("Create_User")]
        public IHttpActionResult Create_User(TBL_M_USER param)
        {
            try
            {
                TBL_M_USER tbl = new TBL_M_USER();
                tbl.ID_Role = param.ID_Role;
                tbl.Username = param.Username;

                db.TBL_M_USERs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }
        }

        [HttpGet]
        [Route("Get_UserSetting")]
        public IHttpActionResult Get_UserSetting()
        {
            var data = db.VW_Users.ToList();
            return Ok(new { Data = data });
        }

        [HttpPost]
        [Route("Delete_User")]
        public IHttpActionResult Delete_User(int role, string nrp)
        {
            try
            {
                var data = db.TBL_M_USERs.Where(a => a.ID_Role == role && a.Username == nrp).FirstOrDefault();

                db.TBL_M_USERs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }
        //END Setting User
        //Setting Menu
        [HttpGet]
        [Route("Get_Menu/{group}")]
        public IHttpActionResult Get_Menu(int group)
        {
            try
            {
                var data = db.VW_MENUs.Where(a => a.ID_Role == group).ToList();

                return Ok(new { Data = data });
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("Update_Menu")]
        public IHttpActionResult Update_Menu(TBL_M_AKSE param)
        {
            try
            {
                var data = db.TBL_M_AKSEs.Where(a => a.ID_Role == param.ID_Role && a.ID_Menu == param.ID_Menu).FirstOrDefault();
                data.IS_ALLOW = param.IS_ALLOW;

                db.SubmitChanges();
                return Json(new { Remarks = true });
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex });
            }

        }
    }
}
