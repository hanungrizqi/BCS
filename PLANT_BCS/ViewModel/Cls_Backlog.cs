using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using PLANT_BCS.Models;

namespace PLANT_BCS.ViewModel
{
    public class Cls_Backlog
    {
        [JsonProperty("Data")]
        public TBL_T_BACKLOG tbl { get; set; }
    }
}