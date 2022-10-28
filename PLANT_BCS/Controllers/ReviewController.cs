using Newtonsoft.Json;
using PLANT_BCS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace PLANT_BCS.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public async Task<ActionResult> Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/BackLog/Update_Closing_Backlog");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_Response data = new Cls_Response();
                    data = JsonConvert.DeserializeObject<Cls_Response>(ApiResponse);


                    ViewBag.UpdateStatus = data;
                }
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