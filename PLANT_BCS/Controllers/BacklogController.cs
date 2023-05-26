using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PLANT_BCS.Models;
using PLANT_BCS.ViewModel;

namespace PLANT_BCS.Controllers
{
    public class BacklogController : Controller
    {
        public async Task<JsonResult> CreateBecklog()
        {
            TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();
            string NoBacklog = "";

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/BackLog/Get_LastNoBacklog/" + Session["Site"].ToString());

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_Backlog data = new Cls_Backlog();
                    data = JsonConvert.DeserializeObject<Cls_Backlog>(ApiResponse);

                    tbl = data.tbl;
                    if (tbl == null)
                    {
                        NoBacklog = "0001" + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy") + Session["Site"].ToString();
                    }
                    else
                    {
                        string month = tbl.NO_BACKLOG.Substring(4, 2);
                        string year = tbl.NO_BACKLOG.Substring(6, 4);
                        string thisMonth = DateTime.Now.ToString("MM");
                        string thisYear = DateTime.Now.ToString("yyyy");
                        if (month == thisMonth && year == thisYear)
                        {
                            int setNo = Convert.ToInt32(tbl.NO_BACKLOG.Substring(0, 4)) + 1;
                            NoBacklog = setNo.ToString().PadLeft(4, '0') + thisMonth + thisYear + Session["Site"].ToString();
                        }
                        else
                        {
                            NoBacklog = "0001" + thisMonth + thisYear + Session["Site"].ToString();
                        }
                    }

                    ViewBag.NoBackLog = NoBacklog;

                }

                return Json(new { Remarks = true, NoBacklog = NoBacklog }, NoBacklog, JsonRequestBehavior.AllowGet);
            }
        }
        // GET: Backlog
        //public ActionResult Register()
        //{
        //    if (Session["nrp"] == null)
        //    {
        //        return RedirectToAction("index", "login");
        //    }
        //    return View();
        //}
        public async Task<ActionResult> Register()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }

            TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();
            string NoBacklog = "";
            string equip = "";

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/BackLog/Get_LastNoBacklog/" + Session["Site"].ToString());

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_Backlog data = new Cls_Backlog();
                    data = JsonConvert.DeserializeObject<Cls_Backlog>(ApiResponse);

                    tbl = data.tbl;
                    if (tbl == null)
                    {
                        NoBacklog = "0001" + DateTime.Now.ToString("MM") + DateTime.Now.ToString("yyyy") + Session["Site"].ToString();
                    }
                    else
                    {
                        string month = tbl.NO_BACKLOG.Substring(4, 2);
                        string year = tbl.NO_BACKLOG.Substring(6, 4);
                        string thisMonth = DateTime.Now.ToString("MM");
                        string thisYear = DateTime.Now.ToString("yyyy");
                        if (month == thisMonth && year == thisYear)
                        {
                            int setNo = Convert.ToInt32(tbl.NO_BACKLOG.Substring(0, 4)) + 1;
                            NoBacklog = setNo.ToString().PadLeft(4, '0') + thisMonth + thisYear + Session["Site"].ToString();
                        }
                        else
                        {
                            NoBacklog = "0001" + thisMonth + thisYear + Session["Site"].ToString();
                        }
                    }

                    //ViewBag.NoBackLog = NoBacklog;

                }
                
                //new 24.05.2023
                HttpResponseMessage Res_Equip = await client.GetAsync("api/Master/Get_EqNumber2/" + Session["Site"].ToString() + "/"  + Session["Nrp"].ToString());

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res_Equip.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res_Equip.Content.ReadAsStringAsync().Result;
                    Cls_Equip data = new Cls_Equip();
                    data = JsonConvert.DeserializeObject<Cls_Equip>(ApiResponse);

                    equip = data.Data;

                }
                if(Session["Site"].ToString() == "INDE")
                {
                    ViewBag.NoBackLog = NoBacklog + equip;
                }
                else
                {
                    ViewBag.NoBackLog = NoBacklog;
                }
                return View();
            }
        }

        public ActionResult ListBacklog()
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
        
        public ActionResult CreateWOWR(string noBacklog)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            ViewBag.noBacklog = noBacklog;
            return View();
        }

        public async Task<ActionResult> DetailWOWR(string noBacklog)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/BackLog/Get_BacklogDetail/" + noBacklog);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_Backlog data = new Cls_Backlog();
                    data = JsonConvert.DeserializeObject<Cls_Backlog>(ApiResponse);

                    tbl = data.tbl;

                }
                ViewBag.BackLog = tbl;
            }
            return View();
        }

        public async Task<ActionResult> EditBacklog(string noBacklog)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            TBL_T_BACKLOG tbl = new TBL_T_BACKLOG();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/BackLog/Get_BacklogDetail/" + noBacklog);

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_Backlog data = new Cls_Backlog();
                    data = JsonConvert.DeserializeObject<Cls_Backlog>(ApiResponse);

                    tbl = data.tbl;

                }
                ViewBag.BackLog = tbl;
            }
            return View();
        }
    }
}