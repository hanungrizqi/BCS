﻿using API_PLANT_BCS.Models;
using API_PLANT_BCS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_PLANT_BCS.Controllers
{

    [RoutePrefix("api/EllWOWR")]
    public class EllWOWRController : ApiController
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        [HttpPost]
        [Route("Create_WO")]
        public IHttpActionResult Create_WO(string noBacklog)
        {
            try
            {
                Cls_CreateWO cls = new Cls_CreateWO();
                List<Cls_StockCode> stck = new List<Cls_StockCode>();

                var dataStock = db.VW_PART_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).ToList();
                var dataBacklog = db.VW_BACKLOGs.Where(a => a.NO_BACKLOG == noBacklog).FirstOrDefault();
                foreach (var item in dataStock)
                {
                    int available = cls.GetAvailableStock(item.DSTRCT_CODE, item.STOCK_CODE, item.LOCATION_ON_STOCK);
                    if (available <= 0)
                    {
                        Cls_CreateWO data = cls.GetSubstitusiStock(item.STOCK_CODE);
                        if (data.Rel_Stock_ID != null)
                        {
                            bool status = cls.CekAvailableStockInDistrict(item.DSTRCT_CODE, data.SCSUB_ID);

                            stck.Add(new Cls_StockCode
                            {
                                stockCode = item.STOCK_CODE,
                                Rel_stockCode = data.Rel_Stock_ID,
                                Status = status,
                            });
                        }
                        
                    }
                }

                var cek = stck.Where(a => a.Status == false).ToList();
                if (cek.Count > 0)
                {
                    return Ok(new { Data = cek, Remarks = false, Pos = "PART", Message = "Error : Substitusi stock code belum terdaftar di district!" });
                }
                else
                {
                    Cls_CreateWOWRResult results = cls.CraeteWO(dataBacklog);
                    if (results.Remarks == true)
                    {
                        Cls_CreateWOWRResult result1 = cls.CreateWR(results.WoNo, dataStock, dataBacklog);
                        if (result1.Remarks == true)
                        {
                            return Ok(new { Data = result1, Remarks = true, Message = "Create WO & WR Berhasil !" });
                        }
                        else
                        {
                            return Ok(new { Data = result1, Pos = "WR", Remarks = false, Message = result1.Message });
                        }
                    }
                    else
                    {
                        return Ok(new { Data = results, Pos = "WO", Remarks = false, Message = results.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new {Remarks = false, Message = ex.Message });
            }
        }
    }
}
