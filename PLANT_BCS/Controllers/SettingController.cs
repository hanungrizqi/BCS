using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLANT_BCS.Models;

namespace PLANT_BCS.Controllers
{
    public class SettingController : Controller
    {
        DB_PLANT_BCSDataContext db = new DB_PLANT_BCSDataContext();
        public ActionResult Users()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.Emp = db.VW_KARYAWAN_ALLs.ToList();
            ViewBag.Group = db.TBL_M_ROLEs.ToList();
            return View();
        }
        
        public ActionResult Source()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        
        public ActionResult Menu()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }

        [HttpPost]
        public JsonResult Create_User(TBL_M_USER param)
        {
            try
            {
                TBL_M_USER tbl = new TBL_M_USER();
                tbl.ID_Role = param.ID_Role;
                tbl.Username = param.Username;

                db.TBL_M_USERs.InsertOnSubmit(tbl);
                db.SubmitChanges();
                return Json(new { Remarks = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Remarks = false, Message = ex }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult Get_UserSetting()
        {
            var data = db.VW_Users.ToList(); 
            return new JsonResult() { Data = new { Data = data }, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }

        public JsonResult Delete_User(int role, string nrp)
        {
            try
            {
                var data = db.TBL_M_USERs.Where(a => a.ID_Role == role && a.Username == nrp).FirstOrDefault();

                db.TBL_M_USERs.DeleteOnSubmit(data);
                db.SubmitChanges();
                return Json(new { Remarks = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { Remarks = false, Message = e }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}