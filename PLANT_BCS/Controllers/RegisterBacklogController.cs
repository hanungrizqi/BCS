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

namespace PLANT_BCS.Controllers
{
    public class RegisterBacklogController : Controller
    {
        
        // GET: RegisterBacklog
        public async Task<ActionResult> index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            
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

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    tbl = JsonConvert.DeserializeObject<TBL_T_BACKLOG>(ApiResponse);
                    //vW_REGISTERs = JsonConvert.DeserializeObject<VW_REGISTER>(ApiResponse);
                    if (tbl.NO_BACKLOG == null)
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

                return View();
            }
        }
    }
}