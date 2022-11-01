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
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace PLANT_BCS.Controllers
{
    public class LogisticController : Controller
    {
        DB_PLANT_BCSDataContext db = new DB_PLANT_BCSDataContext();

        // GET: Logistic
        public ActionResult Index()
        {
            if (Session["nrp"] == null)
            {
                return RedirectToAction("index", "login");
            }
            return View();
        }
        
        public ActionResult ListPartRequest()
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

            List<VW_T_PART_BACKLOG> tbl = new List<VW_T_PART_BACKLOG>();
            List<VW_R_LOCATION_ON_STOCK> listWH = new List<VW_R_LOCATION_ON_STOCK>();

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
                    tbl = (List<VW_T_PART_BACKLOG>)data.tbl;
                    ViewBag.dataPart = tbl;
                    ViewBag.noBacklog = noBacklog;

                }
                
                if (dataWH.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var ApiResponse = dataWH.Content.ReadAsStringAsync().Result;
                    Cls_LocationOnStock data = new Cls_LocationOnStock();
                    data = JsonConvert.DeserializeObject<Cls_LocationOnStock>(ApiResponse);
                    listWH = (List<VW_R_LOCATION_ON_STOCK>)data.tbl;
                    ViewBag.dataWH = listWH;
                }
            }

            return View();
        }

        public JsonResult Upload2(HttpPostedFileBase file)
        {
            List<TBL_T_RECOMMENDED_PART> output = new List<TBL_T_RECOMMENDED_PART>();

            try
            {
                List<TBL_T_RECOMMENDED_PART> cls = new List<TBL_T_RECOMMENDED_PART>();

                System.Data.DataSet ds = new System.Data.DataSet();
                if (Request.Files["file"].ContentLength > 0)
                {
                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        string fileLocation = Server.MapPath("~/Content/Document/");
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }

                        string filename = DateTime.UtcNow.ToLocalTime().ToString("yyyy-MM-dd-hh-mm-ss") + Path.GetFileName(file.FileName);
                        string pathToExcelFile = Path.Combine(fileLocation, filename);
                        Request.Files["file"].SaveAs(Path.Combine(fileLocation, filename));
                        string excelConnectionString = string.Empty;
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";

                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(fileLocation, filename) + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            excelConnectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path.Combine(fileLocation, filename) + ";Extended Properties = 'Excel 12.0 Xml;HDR=YES;IMEX=2'; ");

                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);

                        string query = string.Format("Select * from [{0}]", excelSheets[0]);
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                        {
                            dataAdapter.Fill(ds);
                        }
                    }

                    for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i][0].ToString() == null || ds.Tables[0].Rows[i][0].ToString() == "")
                        {
                            break;
                        }
                        else
                        {
                            cls.Add(new TBL_T_RECOMMENDED_PART
                            {
                                NO_BACKLOG = ds.Tables[0].Rows[i][0].ToString(),
                                PART_NO = ds.Tables[0].Rows[i][7].ToString(),
                                DSTRCT_CODE = ds.Tables[0].Rows[i][1].ToString(),
                                ETA_SUPPLY = DateTime.Parse(ds.Tables[0].Rows[i][14].ToString()),
                                LOCATION_ON_STOCK = ds.Tables[0].Rows[i][13].ToString(),
                                //AVAILABLE_STOCK = Convert.ToInt32(ds.Tables[0].Rows[i][13])
                            });
                        }

                    }
                }


                foreach (var data in cls)
                {
                    try
                    {
                        string partID = data.NO_BACKLOG + data.PART_NO;
                        var cek = db.TBL_T_RECOMMENDED_PARTs.Where(a => a.PART_ID == partID).FirstOrDefault();
                        cek.ETA_SUPPLY = data.ETA_SUPPLY;
                        cek.LOCATION_ON_STOCK = data.LOCATION_ON_STOCK;
                        cek.AVAILABLE_STOCK = data.AVAILABLE_STOCK;
                    }
                    catch (Exception)
                    {
                        output.Add(new TBL_T_RECOMMENDED_PART
                        {
                            PART_NO = data.PART_NO,
                            NO_BACKLOG = data.NO_BACKLOG,
                            ETA_SUPPLY = data.ETA_SUPPLY,
                            LOCATION_ON_STOCK = data.LOCATION_ON_STOCK,
                            AVAILABLE_STOCK = data.AVAILABLE_STOCK,
                        });

                    }
                }

                db.SubmitChanges();
                return Json(new { Data = output, Remarks = true/*, Url = path*/, Message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { Remarks = false, Message = "Error : " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}