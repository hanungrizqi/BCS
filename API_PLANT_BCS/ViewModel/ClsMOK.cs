using API_PLANT_BCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PLANT_BCS.ViewModel
{
    public class ClsMOK
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        public List<VW_R_COMP_CODE> GetListDistrict()
        {
            var data = db.VW_R_COMP_CODEs
                .Select(c => new VW_R_COMP_CODE
                {
                    COMP_CODE = c.COMP_CODE,
                    COMP_DESC = c.COMP_DESC
                })
                .ToList();
            return data;
        }
    }
}