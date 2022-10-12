using Newtonsoft.Json;
using PLANT_BCS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PLANT_BCS.Models;

namespace PLANT_BCS.Controllers
{
    public class PlannerController : Controller
    {
        // GET: Planner
        public ActionResult ListBacklog()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public async Task<ActionResult> RescheduledBacklog(string noBacklog)
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
                ViewBag.noBacklog = noBacklog;
            }

            return View();
        }

    }
}