using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PLANT_BCS.Controllers
{
    public class BacklogApprovalController : Controller
    {
        // GET: BacklogApproval
        public ActionResult ListApproval()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        
        public ActionResult DetailBacklog(string noBacklog)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.noBacklog = noBacklog;
            return View();
        }
    }
}