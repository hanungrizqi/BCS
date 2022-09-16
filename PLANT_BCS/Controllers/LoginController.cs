﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PLANT_BCS.Models;

namespace PLANT_BCS.Controllers
{
    public class LoginController : Controller
    {
        DB_PLANT_BCSDataContext db = new DB_PLANT_BCSDataContext();
        // GET: Login
        public ActionResult Index()
        {
            Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
            return View();
        }

        public JsonResult MakeSession(string NRP, string Jobsite)
        {
            var dataUser = db.VW_KARYAWAN_ALLs.Where(a => a.EMPLOYEE_ID == NRP).FirstOrDefault();
            var dataRole = db.TBL_M_USERs.Where(a => a.Username == NRP).FirstOrDefault();

            if (dataRole != null)
            {

                Session["Web_Link"] = System.Configuration.ConfigurationManager.AppSettings["WebApp_Link"].ToString();
                Session["Nrp"] = NRP;
                Session["ID_Role"] = dataRole.ID_Role;
                Session["Name"] = dataUser.NAME;
                Session["Site"] = Jobsite;
                return new JsonResult() { Data = new { Remarks = true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = new { Remarks = false }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }



        public ActionResult Logout()
        {
            Session.RemoveAll();

            return RedirectToAction("Index", "Login");
        }
    }
}