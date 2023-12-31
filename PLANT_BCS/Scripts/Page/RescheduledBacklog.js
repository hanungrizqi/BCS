﻿Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    getDetail();
    document.getElementById("txt_planRD2").setAttribute("min", new Date().toISOString().split("T")[0]);
})

var table = $("#table_part").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogPart/" + $("#txt_noBacklog").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [3, 4, 5, 6] }
    ],
    scrollX: true,
    columns: [
        {
            "data": null,
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        { data: 'PART_NO' },
        { data: 'STOCK_CODE' },
        { data: 'STK_DESC' },
        { data: 'FIG_NO' },
        { data: 'INDEX_NO' },
        { data: 'QTY' },
        //{ data: 'AVAILABLE_STOCK' },
        { data: 'LOCATION_ON_STOCK' },
        {
            data: 'ETA_SUPPLY',
            render: function (data, type, row) {
                let tempRepairDate1 = new Date($("#txt_planRD1Temp").val());
                let eta = new Date(data);

                //console.log("PlantRepair1 :" + tempRepairDate1);
                //console.log("ETA :" + eta);
                let text = "";

                if (eta > tempRepairDate1) {
                    eta = moment(data).format("DD/MM/YYYY");
                    text = `<span class="text-danger" data-eta="${data}">${eta}</span>`
                } else {
                    eta = moment(data).format("DD/MM/YYYY");
                    text = `<span class="text-success">${eta}</span>`
                }

                return text;
            }
        },
        { data: 'PART_CLASS' }
    ],

});

var tableRepair = $("#table_repair").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogRepair/" + $("#txt_noBacklog").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0] }
    ],
    scrollX: true,
    columns: [
        {
            "data": null,
            render: function (data, type, row, meta) {
                return meta.row + meta.settings._iDisplayStart + 1;
            }
        },
        { data: 'PROBLEM_DESC' },
        { data: 'ACTIVITY_REPAIR' }
    ],

});

function getDetail() {
    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogDetail/" + $("#txt_noBacklog").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            var dataBacklog = result.Data;
            $("#txt_noBacklog").val(dataBacklog.NO_BACKLOG);
            $("#txt_dstrct").val(dataBacklog.DSTRCT_CODE);
            $("#txt_eqNumber").val(dataBacklog.EQP_NUMBER);
            $("#txt_compCode").val(dataBacklog.COMP_CODE + " - " + dataBacklog.COMP_DESC);
            $("#txt_egi").val(dataBacklog.EGI);
            $("#txt_hm").val(dataBacklog.HM);
            $("#txt_blDesc").val(dataBacklog.BACKLOG_DESC);
            $("#txt_dInspecton").val(moment(dataBacklog.INSPECTON_DATE).format("YYYY-MM-DD"));
            $("#txt_inspector").val(dataBacklog.INSPECTOR + " - " + dataBacklog.INSPECTOR_NAME);
            $("#txt_source").val(dataBacklog.SOURCE);
            $("#txt_wg").val(dataBacklog.WORK_GROUP);
            $("#txt_standJob").val(dataBacklog.STD_JOB);
            $("#txt_nrpGl").val(dataBacklog.NRP_GL + " - " + dataBacklog.NRP_GL_NAME);
            $("#txt_oriID").val(dataBacklog.ORIGINATOR_ID + " - " + dataBacklog.ORIGINATOR_ID_NAME);
            $("#txt_planRD1").val(moment(dataBacklog.PLAN_REPAIR_DATE_1).format("YYYY-MM-DD"));
            $("#txt_planRD2").val(moment(dataBacklog.PLAN_REPAIR_DATE_2).format("YYYY-MM-DD"));
            $("#txt_mp").val(dataBacklog.MANPOWER);
            $("#txt_hEst").val(dataBacklog.HOUR_EST);
            $("#txt_posBL").val(dataBacklog.POSISI_BACKLOG);
            $("#txt_inspector").val(dataBacklog.CREATED_BY);
            $("#txt_note").val(dataBacklog.REMARKS);
        }
    });
}

function submitBacklog(status) {
    if ($("#txt_note").val() == "" || $("#txt_note").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Note!',
            'warning'
        );
        return false;
    }
    
    var validasi = false;
    var plandate2 = $("#txt_planRD2").val();
    plandate2 = plandate2.replace("-", "");
    plandate2 = plandate2.replace("-", "");
    plandate2 = parseInt(plandate2);
    /*console.log(plandate2);*/

    var etasupploy = 0

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBacklog").val();
    dataBacklog.UPDATED_BY = $("#hd_nrp").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    dataBacklog.PLAN_REPAIR_DATE_2 = $("#txt_planRD2").val();

    if (status == "PLANNER APPROVED") {
        dataBacklog.POSISI_BACKLOG = "Logistic";
    } else {
        dataBacklog.POSISI_BACKLOG = "ADM1";
    }
    dataBacklog.STATUS = status;


    let tRow = $("#table_part >tbody >tr").length;
    
    $.each($("#table_part tbody tr"), function (index) {
        
        var cekTD = $(this).find('td:eq(8) >span:first');
        if (cekTD.hasClass('text-danger')) {
            var eta = $(cekTD).attr("data-eta");
            let etaTD = new Date(eta);
            let planDate2 = new Date(dataBacklog.PLAN_REPAIR_DATE_1);
            if (dataBacklog.PLAN_REPAIR_DATE_2 != "") {
                planDate2 = new Date(dataBacklog.PLAN_REPAIR_DATE_2);
            }
            
                let getin = index + 1;
                if (getin == tRow) {
                    //FnSubmit(dataBacklog);
                    validasi = true;
            }

            eta = eta.replace("-", "");
            eta = eta.replace("-", "");
            eta = parseInt(eta);
            /*console.log(eta);*/
            if (etasupploy < eta) {
                etasupploy = eta;
                /*console.log(etasupploy);*/
            }
            //if (planDate2 < etaTD) {
                
            //    Swal.fire(
            //        'Warning',
            //        'Plan Repair date 2 belum sesusai!',
            //        'warning'
            //    );
            //    return false;
            //} else {
            //    let getin = index + 1;
            //    if (getin == tRow) {
            //        FnSubmit(dataBacklog);
            //    }
            //}
        } else {
            let getin = index + 1;
            if (getin == tRow) {
                validasi = true;
                //FnSubmit(dataBacklog);
            }
        }
    });    
    
    if (plandate2 < etasupploy) {
        
        validasi = false;
        Swal.fire(
                'Warning',
                'Plan Repair date 2 belum sesusai!',
                'warning'
        );
        return false;
    }
    else {
        validasi = true;
    }

    if ($("#txt_planRD2").val() == "") {

        validasi = false;
        Swal.fire(
            'Warning',
            'Plan Repair date 2 belum sesusai!',
            'warning'
        );
        return false;
    }

    if (validasi) {

        FnSubmit(dataBacklog);
    }
        
}

function FnSubmit(dataBacklog) {
    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Rescheduled_Backlog", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been saved!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/Planner/listBacklog";
                    }
                })
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
        }
    });
}