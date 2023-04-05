using API_PLANT_BCS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PLANT_BCS.ViewModel
{
    public class ClsTemp
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();
        public string PART_ID { get; set; }
        public string NO_BACKLOG { get; set; }
        public string PART_NO { get; set; }
        public string FIG_NO { get; set; }
        public string INDEX_NO { get; set; }
        public int QTY { get; set; }
        public string DSTRCT_CODE { get; set; }
        public int STOCK_CODE { get; set; }
        public string STK_DESC { get; set; }
        public string UOM { get; set; }
        public string PART_CLASS { get; set; }

        public void Create_BacklogPartTemp()
        {
            TBL_T_TEMPORARY_PART tbl = new TBL_T_TEMPORARY_PART();
            tbl.PART_NO = PART_NO;
            tbl.FIG_NO = FIG_NO;
            tbl.INDEX_NO = INDEX_NO;
            tbl.QTY = QTY;
            tbl.DSTRCT_CODE = DSTRCT_CODE;
            tbl.STOCK_CODE = STOCK_CODE;
            tbl.STK_DESC = STK_DESC;
            tbl.UOM = UOM;
            tbl.PART_CLASS = PART_CLASS;
            tbl.NO_BACKLOG = NO_BACKLOG;
            tbl.PART_ID = PART_ID;
            

            db.TBL_T_TEMPORARY_PARTs.InsertOnSubmit(tbl);
            db.SubmitChanges();
        }
        public void deletePartTemp()
        {
            var query = db.TBL_T_TEMPORARY_PARTs.Where(t => t.PART_ID == PART_ID).FirstOrDefault();
            db.TBL_T_TEMPORARY_PARTs.DeleteOnSubmit(query);
            db.SubmitChanges();
        }
    }
}