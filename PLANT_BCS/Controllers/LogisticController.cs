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
    public class LogisticController : Controller
    {
        // GET: Logistic
        public ActionResult Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }

        public async Task<ActionResult> ETASupply(string noBacklog)
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }

            List<VW_PART_BACKLOG> tbl = new List<VW_PART_BACKLOG>();
            List<VW_LOCATION_ON_STOCK> listWH = new List<VW_LOCATION_ON_STOCK>();

            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri((string)Session["Web_Link"]);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/Backlog/Get_BacklogPart/" + noBacklog);
                HttpResponseMessage dataWH = await client.GetAsync("api/Master/Get_LocationOnStock/" + Session["Site"].ToString());

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = Res.Content.ReadAsStringAsync().Result;
                    Cls_PartBacklog data = new Cls_PartBacklog();
                    data = JsonConvert.DeserializeObject<Cls_PartBacklog>(ApiResponse);
                    tbl = (List<VW_PART_BACKLOG>)data.tbl;
                    ViewBag.dataPart = tbl;
                    ViewBag.noBacklog = noBacklog;

                }
                
                if (dataWH.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = dataWH.Content.ReadAsStringAsync().Result;
                    Cls_LocationOnStock data = new Cls_LocationOnStock();
                    data = JsonConvert.DeserializeObject<Cls_LocationOnStock>(ApiResponse);
                    listWH = (List<VW_LOCATION_ON_STOCK>)data.tbl;
                    ViewBag.dataWH = listWH;
                }
            }

            return View();
        }
    }
}