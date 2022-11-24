using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using API_PLANT_BCS.RequisitionService;
using API_PLANT_BCS.WorkOrderService;
using API_PLANT_BCS.Models;
using EllipseWebServicesClient;
using Oracle.ManagedDataAccess.Client;

namespace API_PLANT_BCS.ViewModel
{
    public class Cls_CreateWO
    {
        DB_Plant_BCSDataContext db = new DB_Plant_BCSDataContext();

        public string p_string_conn = "User Id = ellipse; Password=n4ngg4l4; DATA SOURCE = kphodbel601:1521/ellkpd";

        public string SCSUB_ID { get; set; }
        public string Rel_Stock_ID { get; set; }
        public string Issue_rule { get; set; }

        public int GetAvailableStock(string dstrct, string stcCode, string wh)
        {
            int result = 0;

            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = p_string_conn;
            _connection.Open();

            string sql_stat_type = "select AVAILABLE from (SELECT b.DSTRCT_CODE, b.WHOUSE_ID, a.STOCK_CODE,sum(a.SOH) as SOH,c.DUES_OUT,c.LOANS_DUE_OUT,c.WHOUSE_PICKED, (sum(a.SOH) - c.WHOUSE_PICKED - c.DUES_OUT - c.LOANS_DUE_OUT) AS AVAILABLE FROM MSF1HD a right join MSF1CS b on a.CUSTODIAN_ID = b.CUSTODIAN_ID left join MSF180 c on b.DSTRCT_CODE = c.DSTRCT_CODE AND b.WHOUSE_ID = c.WHOUSE_ID AND a.STOCK_CODE = c.STOCK_CODE where a.STK_OWNERSHP_IND = 'O' AND a.HOLDING_TYPE = 'F' GROUP BY b.DSTRCT_CODE, b.WHOUSE_ID, a.STOCK_CODE, c.DUES_OUT ,c.LOANS_DUE_OUT ,c.WHOUSE_PICKED) available where DSTRCT_CODE = '" + dstrct + "' and STOCK_CODE = '" + stcCode + "' and WHOUSE_ID = '" + wh + "'";

            OracleCommand command = new OracleCommand(sql_stat_type, _connection);
            OracleDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    result = Convert.ToInt32(dr["AVAILABLE"].ToString());
                }
            }
            _connection.Close();
            return result;
        }
        
        public Cls_CreateWO GetSubstitusiStock(string stcCode)
        {
            Cls_CreateWO cls = new Cls_CreateWO();

            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = p_string_conn;
            _connection.Open();

            string sql_stat_type = "select scsub_id, rel_stock_code, issue_rule from msf17t WHERE stock_code = '" + stcCode + "' and scsub_status = 'A'";

            OracleCommand command = new OracleCommand(sql_stat_type, _connection);
            OracleDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    cls.SCSUB_ID = dr["scsub_id"].ToString();
                    cls.Rel_Stock_ID = dr["rel_stock_code"].ToString();
                    cls.Issue_rule = dr["issue_rule"].ToString();
                }
            }
            _connection.Close();
            return cls;
        }
        
        public bool CekAvailableStockInDistrict(string dstrct, string stcCode)
        {
            bool result = false;

            OracleConnection _connection = new OracleConnection();
            _connection.ConnectionString = p_string_conn;
            _connection.Open();

            string sql_stat_type = "select a.scsub_id, a.rel_stock_code, a.issue_rule from msf17t a left join msf17u b on a.scsub_id = b.scsub_id WHERE a.stock_code = '" + stcCode + "' and a.scsub_status = 'A' and dstrct_code = '" + dstrct + "'";

            OracleCommand command = new OracleCommand(sql_stat_type, _connection);
            OracleDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                result = true;
            }
            _connection.Close();
            return result;
        }

        public Cls_CreateWOWRResult CraeteWO(VW_T_BACKLOG dataBacklog, TimeSpan planStrTime)
        {
            Cls_CreateWOWRResult cls = new Cls_CreateWOWRResult();

            try
            {
                string eqpnumber = dataBacklog.EQP_NUMBER;
                string stndrdJob = dataBacklog.STD_JOB;
                string backlogdesc = dataBacklog.BACKLOG_DESC.Replace(" ", "_"); 
                string elluser = dataBacklog.ORIGINATOR_ID;
                string compCode = dataBacklog.COMP_CODE;
                TimeSpan hourEst = TimeSpan.FromHours((double)dataBacklog.HOUR_EST);
                TimeSpan planFinTime = planStrTime.Add(hourEst);

                DateTime rep = (DateTime)dataBacklog.PLAN_REPAIR_DATE_1;
                var rep2 = dataBacklog.PLAN_REPAIR_DATE_2;

                WorkOrderService.WorkOrderService i_obj_service = new WorkOrderService.WorkOrderService();
                WorkOrderService.WorkOrderServiceCreateReplyDTO i_obj_create_rslt1 = new WorkOrderService.WorkOrderServiceCreateReplyDTO();
                WorkOrderService.OperationContext i_obj_context = new WorkOrderService.OperationContext();
                WorkOrderService.WorkOrderServiceCreateRequestDTO i_obj_requisition_dto1 = new WorkOrderServiceCreateRequestDTO();
                WorkOrderService.WorkOrderServiceCreateReplyDTO i_obj_create = new WorkOrderService.WorkOrderServiceCreateReplyDTO();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                i_obj_context.district = dataBacklog.DSTRCT_CODE;
                i_obj_context.position = str_posisi;
                ClientConversation.authenticate(str_username, str_password);

                i_obj_requisition_dto1.equipmentRef = eqpnumber;
                i_obj_requisition_dto1.stdJobNo = stndrdJob;
                i_obj_requisition_dto1.workOrderDesc = backlogdesc.ToUpper();
                i_obj_requisition_dto1.originatorId = elluser;
                i_obj_requisition_dto1.workOrderType = "MO";
                i_obj_requisition_dto1.maintenanceType = "BL";
                i_obj_requisition_dto1.compCode = compCode;
                i_obj_requisition_dto1.planStrTime = planStrTime.ToString("hhmmss");
                i_obj_requisition_dto1.planFinTime = planFinTime.ToString("hhmmss");

                if (rep2 == null)
                {
                    i_obj_requisition_dto1.planStrDate = rep.ToString("yyyyMMdd");
                }
                else
                {
                    DateTime repDate2 = (DateTime)rep2;
                    i_obj_requisition_dto1.planStrDate = repDate2.ToString("yyyyMMdd");
                }

                i_obj_create_rslt1 = i_obj_service.create(i_obj_context, i_obj_requisition_dto1);
                string workOrder;
                string workOrderPre;
                workOrderPre = i_obj_create_rslt1.workOrder.prefix.ToString();
                workOrder = i_obj_create_rslt1.workOrder.no.ToString();
                string WO = workOrderPre + workOrder;

                cls.Remarks = true;
                cls.Message = "Create WR Berhasil";
                cls.WoNo = WO;

                TBL_H_BACKLOG_WO_WR tbl = new TBL_H_BACKLOG_WO_WR
                {
                    NO_BACKLOG = dataBacklog.NO_BACKLOG,
                    WO_NO = WO
                };

                db.TBL_H_BACKLOG_WO_WRs.InsertOnSubmit(tbl);
                db.SubmitChanges();

                return (cls);
            }
            catch (Exception ex)
            {
                cls.Remarks = false;
                cls.Message = ex.Message;
                return (cls);
            }
        }

        public Cls_CreateWOWRResult CreateWR(string workOrder, IList<VW_T_PART_BACKLOG> dataStock, VW_T_BACKLOG dataBacklog)
        {
            Cls_CreateWOWRResult cls = new Cls_CreateWOWRResult();

            try
            {

                RequisitionService.RequisitionService i_obj_services = new RequisitionService.RequisitionService();
                RequisitionService.OperationContext i_obj_context = new RequisitionService.OperationContext();
                RequisitionServiceCreateItemRequestDTO i_obj_screenDTO = new RequisitionServiceCreateItemRequestDTO();

                RequisitionServiceCreateHeaderRequestDTO i_obj_requisition_dto1 = new RequisitionServiceCreateHeaderRequestDTO();
                RequisitionServiceCreateHeaderReplyDTO i_obj_create_rslt1 = new RequisitionServiceCreateHeaderReplyDTO();
                RequisitionService.WorkOrderDTO i_obj_wo1 = new RequisitionService.WorkOrderDTO();

                //inisialisasi item
                RequisitionServiceCreateItemRequestDTO i_obj_screenDTODetail = new RequisitionServiceCreateItemRequestDTO();
                RequisitionServiceCreateItemReplyDTO i_obj_create_resultDetail = new RequisitionServiceCreateItemReplyDTO();
                RequisitionItemDTO i_obj_reqDTOItem = new RequisitionItemDTO();
                RequisitionServiceCreateItemReplyCollectionDTO i_obj_create_resultDetail2 = new RequisitionServiceCreateItemReplyCollectionDTO();
                string equipmentRefA;
                string costCentreA;

                //item field
                List<RequisitionItemDTO> i_obj_listfield = new List<RequisitionItemDTO>();

                string str_username = ConfigurationManager.AppSettings["username"].ToString();
                string str_password = ConfigurationManager.AppSettings["password"].ToString();
                string str_posisi = ConfigurationManager.AppSettings["pos_id"].ToString();

                ClientConversation.authenticate(str_username, str_password);

                string wono, elldistrict, elluser1, wahid, delinstrb, descbacklog, nobacklog1, DelivInstrA_Combine;

                elluser1 = dataBacklog.ORIGINATOR_ID;
                elldistrict = dataBacklog.DSTRCT_CODE;
                wono = workOrder;
                wahid = dataStock[0].LOCATION_ON_STOCK;
                delinstrb = dataBacklog.REMARKS;
                nobacklog1 = dataBacklog.NO_BACKLOG;
                descbacklog = dataBacklog.BACKLOG_DESC.Replace(" ", "_").ToUpper();
                DelivInstrA_Combine = nobacklog1 + descbacklog;

                i_obj_context.district = dataBacklog.DSTRCT_CODE;
                i_obj_context.position = str_posisi;

                i_obj_requisition_dto1.ireqType = "NI";
                i_obj_requisition_dto1.districtCode = elldistrict;
                i_obj_requisition_dto1.requiredByDate = DateTime.Now.ToString("yyyyMMdd");
                string date_wr = i_obj_requisition_dto1.requiredByDate;
                i_obj_requisition_dto1.requestedBy = elluser1;
                i_obj_requisition_dto1.authsdBy = " ";
                i_obj_requisition_dto1.delivInstrA = DelivInstrA_Combine;
                i_obj_requisition_dto1.delivInstrB = delinstrb;
                i_obj_requisition_dto1.answerA = "05";
                i_obj_requisition_dto1.issTranType = "NI";
                i_obj_requisition_dto1.origWhouseId = wahid;
                i_obj_requisition_dto1.costDistrictA = elldistrict;
                i_obj_wo1.prefix = wono.Substring(0, 2);
                i_obj_wo1.no = wono.Substring(2, 6);
                i_obj_requisition_dto1.workOrderA = i_obj_wo1;
                i_obj_requisition_dto1.allocPcA = 100.ToString();
                i_obj_requisition_dto1.costDistrictA = elldistrict;


                //'submit header
                i_obj_create_rslt1 = i_obj_services.createHeader(i_obj_context, i_obj_requisition_dto1);

                string ireqNo;
                ireqNo = i_obj_create_rslt1.ireqNo.ToString();
                string WR_Num = ireqNo;

                //'nampiilin equipment & costcentre
                equipmentRefA = i_obj_create_rslt1.equipmentRefA.ToString();
                costCentreA = i_obj_create_rslt1.costCentreA.ToString();

                try
                {
                    string part_UOM, stockcode;
                    decimal qty;
                    List<RequisitionItemDTO> obj_listfield = new List<RequisitionItemDTO>();
                    List<RequisitionServiceCreateItemRequestDTO> i_obj_listfield2 = new List<RequisitionServiceCreateItemRequestDTO>();

                    //'item
                    i_obj_screenDTODetail.districtCode = elldistrict;
                    i_obj_screenDTODetail.ireqNo = ireqNo;
                    i_obj_screenDTODetail.ireqType = "NI";

                    foreach (var item in dataStock)
                    {
                        stockcode = item.STOCK_CODE;
                        part_UOM = item.UOM;
                        qty = (decimal)item.QTY;

                        i_obj_listfield.Add(new RequisitionItemDTO
                        {
                            itemType = "S",
                            stockCode = stockcode,
                            unitOfMeasure = part_UOM,
                            quantityRequiredSpecified = true,
                            quantityRequired = qty
                        });
                    }

                    i_obj_screenDTODetail.requisitionItems = i_obj_listfield.ToArray();
                    i_obj_listfield2.Add(i_obj_screenDTODetail);

                    i_obj_create_resultDetail2 = i_obj_services.multipleCreateItem(i_obj_context, i_obj_listfield2.ToArray());

                    //'finalize
                    RequisitionServiceFinaliseRequestDTO reqDTOFinal = new RequisitionServiceFinaliseRequestDTO();
                    RequisitionServiceFinaliseReplyCollectionDTO i_obj_resultFinal = new RequisitionServiceFinaliseReplyCollectionDTO();

                    //'finalize field
                    List<RequisitionService.RequisitionServiceFinaliseRequestDTO> i_obj_listfield3 = new List<RequisitionService.RequisitionServiceFinaliseRequestDTO>();
                    reqDTOFinal.ireqNo = i_obj_create_rslt1.ireqNo;
                    reqDTOFinal.ireqType = "NI";
                    i_obj_listfield3.Add(reqDTOFinal);
                    i_obj_resultFinal = i_obj_services.multipleFinalise(i_obj_context, i_obj_listfield3.ToArray());

                    cls.Remarks = true;
                    cls.Message = "Create WR Berhasil";
                    cls.IReqNo = reqDTOFinal.ireqNo;


                    var data = db.TBL_H_BACKLOG_WO_WRs.Where(a => a.NO_BACKLOG == dataBacklog.NO_BACKLOG).FirstOrDefault();
                    data.IREQ_NO = reqDTOFinal.ireqNo;
                    db.SubmitChanges();

                    return cls;
                }
                catch (Exception ex)
                {
                    string pesanError = "Loop WR Error " + ex.Message.ToString();
                    cls.Message = pesanError;
                    cls.Remarks = false;
                    return cls;
                }
            }
            catch (Exception ex)
            {
                string pesanError = "Error Header WR " + ex.Message.ToString();
                cls.Message = pesanError;
                cls.Remarks = false;
                return cls;
            }
        }

    }
}