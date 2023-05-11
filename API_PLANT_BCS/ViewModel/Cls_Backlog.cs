using API_PLANT_BCS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PLANT_BCS.ViewModel
{
    public class Cls_Backlog
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();
        [JsonProperty("Data")]
        public TBL_T_BACKLOG tbl { get; set; }
    }
}