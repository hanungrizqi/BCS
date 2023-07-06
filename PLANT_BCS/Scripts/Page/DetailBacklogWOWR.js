$("document").ready(function () {
    Codebase.helpersOnLoad(['jq-select2']);

    var idProfile = $("#hd_idProfile").val();
    if (idProfile == 1) {
        $("#tomboladmin").show();
    } else {
        $("#tomboladmin").hide();
    }

    $("#PartError").hide();
    getEqNumber();
    getCompCode();
    getSource();
    getOriID();
    getSTDJob();
    //console.log("#txt_eqNumberTemp");

    document.getElementById("txt_dInspecton").setAttribute("max", new Date().toISOString().split("T")[0]);
    document.getElementById("txt_planRD1").setAttribute("min", new Date().toISOString().split("T")[0]);
    document.getElementById("txt_planRD2").setAttribute("min", new Date().toISOString().split("T")[0]);
})

$("#txt_eqNumber").on("change", function () {
    let egi = $(this).find(':selected').attr('data-egi');
    $("#txt_egi").val(egi);
})

$("#txt_standJob").on("change", function () {
    let wg = $(this).find(':selected').attr('data-wg');
    $("#txt_wg").val(wg);
})

var table = $("#table_part").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogPart/" + $("#txt_noBl").val(),
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
        { data: 'LOCATION_ON_STOCK' },
        {
            data: 'ETA_SUPPLY',
            render: function (data, type, row) {
                let text = "";
                if (data != null) {
                    text = moment(data).format("DD/MM/YYYY");
                }
                return text;
            }
        },
        { data: 'PART_CLASS' },
        { data: 'ACT_ONSITE_DATE' },
        { data: 'ACCTUAL_SUPPLY_DATE' }
    ],
    

});
console.log(table.ETA_SUPPLY);

var tableRepair = $("#table_repair").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogRepair/" + $("#txt_noBl").val(),
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

function getEqNumber() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_EqNumber/" + $("#hd_site").val() + "/" + $("#hd_nrp").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_eqNumber').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                /*if (val.EQUIP_NO == $("#txt_eqNumberTemp").val()) {*/
                //edit 13.03.2023 (trim() untuk menghilangkan whitespace)
                if ((val.EQUIP_NO).trim() == $("#txt_eqNumberTemp").val().trim()) {
                    text += '<option selected="selected" value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '" >' + val.EQUIP_NO + '</option>';
                } else {
                    text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '">' + val.EQUIP_NO + '</option>';
                }
            });
            $("#txt_eqNumber").append(text);
            console.log(result.Data);
        }
    });
}

function getCompCode() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_CompCode", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_compCode').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.COMP_CODE == $("#txt_compCodeTemp").val()) {
                    text += '<option selected value="' + val.COMP_CODE + '">' + val.COMP_CODE + ' - ' + val.COMP_DESC + '</option>';
                } else {
                    text += '<option value="' + val.COMP_CODE + '">' + val.COMP_CODE + ' - ' + val.COMP_DESC + '</option>';
                }
            });
            $("#txt_compCode").append(text);
        }
    });
}

function getSource() {
    $.ajax({
        url: $("#web_link").val() + "/api/Setting/Get_Source", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_source').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.SOURCE == $("#txt_sourceTemp").val()) {
                    text += '<option selected value="' + val.SOURCE + '">' + val.SOURCE + '</option>';
                } else {
                    text += '<option value="' + val.SOURCE + '">' + val.SOURCE + '</option>';
                }
            });
            $("#txt_source").append(text);
        }
    });
}

//function getOriID() {
//    $.ajax({
//        url: $("#web_link").val() + "/api/Master/Get_OriginatorID/" + $("#txt_dstrct").val(),
//        type: "GET",
//        cache: false,
//        success: function (result) {
//            $('#txt_nrpGl').empty();
//            $('#txt_oriID').empty();
//            text = '<option></option>';
//            $.each(result.Data, function (key, val) {
//                if (val.EMPLOYEE_ID == $("#txt_oriIDTemp").val()) {
//                    text += '<option selected value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
//                } else {
//                    text += '<option value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
//                }
//            });
//            $("#txt_oriID").append(text);
//            $("#txt_nrpGl").append(text);
//        }
//    });
//}

function getOriID() {
    NRP_GL = $("#txt_nrpGl").val("");
    ORIGINATOR_ID = $("#txt_oriID").val("");
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_OriginatorID2/" + $("#txt_dstrct").val() + "/" + $("#hd_nrp").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_nrpGl').empty();
            $('#txt_oriID').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.EMPLOYEE_ID == $("#txt_oriIDTemp").val()) {
                    text += '<option selected value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
                } else {
                    text += '<option value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
                }
            });
            $("#txt_oriID").append(text);
            $("#txt_nrpGl").append(text);
        }
    });
}

function getSTDJob() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_STDJob/" + $("#hd_site").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_standJob').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.STD_JOB_NO == $("#txt_standJobTemp").val()) {
                    text += '<option selected value="' + val.STD_JOB_NO + '" data-wg="' + val.WORK_GROUP + '">' + val.STD_JOB_NO + ' - ' + val.STD_JOB_DESC + '</option>';
                } else {
                    text += '<option value="' + val.STD_JOB_NO + '" data-wg="' + val.WORK_GROUP + '">' + val.STD_JOB_NO + ' - ' + val.STD_JOB_DESC + '</option>';
                }
            });
            $("#txt_standJob").append(text);
        }
    });
}

function SendBackBacklog(status) {

    NO_BACKLOG = $("#txt_noBl").val();
    DSTRCT_CODE = $("#txt_dstrct").val();
    EQP_NUMBER = $("#txt_eqNumber").val();
    COMP_CODE = $("#txt_compCode").val();
    EGI = $("#txt_egi").val();
    HM = $("#txt_hm").val();
    BACKLOG_DESC = $("#txt_blDesc").val();
    INSPECTON_DATE = $("#txt_dInspecton").val();
    INSPECTOR = $("#txt_inspector").val();
    SOURCE = $("#txt_source").val();
    WORK_GROUP = $("#txt_wg").val();
    STD_JOB = $("#txt_standJob").val();
    NRP_GL = $("#txt_nrpGl").val();
    ORIGINATOR_ID = $("#txt_oriID").val();
    PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    MANPOWER = $("#txt_mp").val();
    HOUR_EST = $("#txt_hEst").val();
    POSISI_BACKLOG = $("#txt_posBL").val();
    CREATED_BY = $("#txt_inspector").val();
    REMARKS = $("#txt_note").val();
    PLANSTR_TIME = $("#txt_planStrTime").val();
    UPDATED_BY = $("#hd_idNrp").val();

    if (COMP_CODE == "") {
        window.location.hash = "txt_compCode";
        return false;
    } else if (BACKLOG_DESC == "") {
        window.location.hash = "txt_blDesc";
        return false;
    } else if (EQP_NUMBER == "") {
        window.location.hash = "txt_eqNumber";
        return false;
    } else if (HM == "") {
        window.location.hash = "txt_hm";
        return false;
    } else if (HOUR_EST == "") {
        window.location.hash = "txt_hEst";
        return false;
    } else if (INSPECTON_DATE == "") {
        window.location.hash = "txt_dInspecton";
        return false;
    } else if (MANPOWER == "") {
        window.location.hash = "txt_mp";
        return false;
    } else if (NRP_GL == "") {
        window.location.hash = "txt_nrpGl";
        return false;
    } else if (ORIGINATOR_ID == "") {
        window.location.hash = "txt_oriID";
        return false;
    } else if (PLAN_REPAIR_DATE_1 == "") {
        window.location.hash = "txt_planRD1";
        return false;
    } else if (SOURCE == "") {
        window.location.hash = "txt_source";
        return false;
    } else if (STD_JOB == "") {
        window.location.hash = "txt_standJob";
        return false;
    }

    if (status == "LOGISTIC APPROVED") {
        if (REMARKS == "") {
            Swal.fire(
                'Warning!',
                'Message : Delivery Instruction Back belum diisi',
                'warning'
            );
            window.location.hash = "txt_note";
            return false;
        }
    }

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBl").val();
    dataBacklog.DSTRCT_CODE = $("#txt_dstrct").val();
    dataBacklog.EQP_NUMBER = $("#txt_eqNumber").val();
    dataBacklog.COMP_CODE = $("#txt_compCode").val();
    dataBacklog.EGI = $("#txt_egi").val();
    dataBacklog.HM = $("#txt_hm").val();
    dataBacklog.BACKLOG_DESC = $("#txt_blDesc").val();
    dataBacklog.INSPECTON_DATE = $("#txt_dInspecton").val();
    dataBacklog.INSPECTOR = $("#txt_inspector").val();
    dataBacklog.SOURCE = $("#txt_source").val();
    dataBacklog.WORK_GROUP = $("#txt_wg").val();
    dataBacklog.STD_JOB = $("#txt_standJob").val();
    dataBacklog.NRP_GL = $("#txt_nrpGl").val();
    dataBacklog.ORIGINATOR_ID = $("#txt_oriID").val();
    dataBacklog.PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    dataBacklog.MANPOWER = $("#txt_mp").val();
    dataBacklog.HOUR_EST = $("#txt_hEst").val();
    dataBacklog.CREATED_BY = $("#txt_inspector").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.POSISI_BACKLOG = $("#txt_posBL").val();
    dataBacklog.UPDATED_BY = $("#hd_idNrp").val();
    /*dataBacklog.STATUS = "OPEN";*/

    if (status == "LOGISTIC APPROVED") {
        dataBacklog.POSISI_BACKLOG = "Planner1";
    } else {
        dataBacklog.POSISI_BACKLOG = "ADM1";
    }
    dataBacklog.STATUS = status;

    if (dataBacklog) {

        FnSubmit(dataBacklog);
    }
}

function CreateWoWr(mode) {


    table = $("#table_part").val(table.ETA_SUPPLY);
    

    NO_BACKLOG = $("#txt_noBl").val();
    DSTRCT_CODE = $("#txt_dstrct").val();
    EQP_NUMBER = $("#txt_eqNumber").val();
    COMP_CODE = $("#txt_compCode").val();
    EGI = $("#txt_egi").val();
    HM = $("#txt_hm").val();
    BACKLOG_DESC = $("#txt_blDesc").val();
    INSPECTON_DATE = $("#txt_dInspecton").val();
    INSPECTOR = $("#txt_inspector").val();
    SOURCE = $("#txt_source").val();
    WORK_GROUP = $("#txt_wg").val();
    STD_JOB = $("#txt_standJob").val();
    NRP_GL = $("#txt_nrpGl").val();
    ORIGINATOR_ID = $("#txt_oriID").val();
    PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    MANPOWER = $("#txt_mp").val();
    HOUR_EST = $("#txt_hEst").val();
    POSISI_BACKLOG = $("#txt_posBL").val();
    CREATED_BY = $("#txt_inspector").val();
    REMARKS = $("#txt_note").val();
    PLANSTR_TIME = $("#txt_planStrTime").val();
    UPDATED_BY = $("#hd_idNrp").val();

    if (COMP_CODE == "") {
        window.location.hash = "txt_compCode";
        return false;
    } else if (BACKLOG_DESC == "") {
        window.location.hash = "txt_blDesc";
        return false;
    } else if (EQP_NUMBER == "") {
        window.location.hash = "txt_eqNumber";
        return false;
    } else if (HM == "") {
        window.location.hash = "txt_hm";
        return false;
    } else if (HOUR_EST == "") {
        window.location.hash = "txt_hEst";
        return false;
    } else if (INSPECTON_DATE == "") {
        window.location.hash = "txt_dInspecton";
        return false;
    } else if (MANPOWER == "") {
        window.location.hash = "txt_mp";
        return false;
    } else if (NRP_GL == "") {
        window.location.hash = "txt_nrpGl";
        return false;
    }
    else if (ORIGINATOR_ID == "") {
        window.location.hash = "txt_oriID";
        return false;
    }
    else if (PLAN_REPAIR_DATE_1 == "") {
        window.location.hash = "txt_planRD1";
        return false;
    } else if (SOURCE == "") {
        window.location.hash = "txt_source";
        return false;
    } else if (STD_JOB == "") {
        window.location.hash = "txt_standJob";
        return false;
    }

    if (mode == "create") {
        if (REMARKS == "") {
            Swal.fire(
                'Warning!',
                'Message : Delivery Instruction belum diisi',
                'warning'
            );
            window.location.hash = "txt_note";
            return false;
        }
        if (table == "") {
            Swal.fire(
                'Warning!',
                'Message : Table Recomended part ada yang kosong',
                'warning'
            );
            window.location.hash = "table_part";
            return false;
        }
        if (PLANSTR_TIME == "") {
            Swal.fire(
                'Warning!',
                'Message : Plan start time belum diisi',
                'warning'
            );
            window.location.hash = "txt_planStrTime";
            return false;
        }
        
    }

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBl").val();
    dataBacklog.DSTRCT_CODE = $("#txt_dstrct").val();
    dataBacklog.EQP_NUMBER = $("#txt_eqNumber").val();
    dataBacklog.COMP_CODE = $("#txt_compCode").val();
    dataBacklog.EGI = $("#txt_egi").val();
    dataBacklog.HM = $("#txt_hm").val();
    dataBacklog.BACKLOG_DESC = $("#txt_blDesc").val();
    dataBacklog.INSPECTON_DATE = $("#txt_dInspecton").val();
    dataBacklog.INSPECTOR = $("#txt_inspector").val();
    dataBacklog.SOURCE = $("#txt_source").val();
    dataBacklog.WORK_GROUP = $("#txt_wg").val();
    dataBacklog.STD_JOB = $("#txt_standJob").val();
    dataBacklog.NRP_GL = $("#txt_nrpGl").val();
    dataBacklog.ORIGINATOR_ID = $("#txt_oriID").val();
    dataBacklog.PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    dataBacklog.MANPOWER = $("#txt_mp").val();
    dataBacklog.HOUR_EST = $("#txt_hEst").val();
    dataBacklog.CREATED_BY = $("#txt_inspector").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.POSISI_BACKLOG = $("#txt_posBL").val();
    dataBacklog.UPDATED_BY = $("#hd_idNrp").val();
    dataBacklog.STATUS = "OPEN";


    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_Backlog_WOWR", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                if (mode == "create") {
                    createWOWR();
                } else if (mode == "save") {
                    Swal.fire(
                        'Saved!',
                        'Header Backlog Created!',
                        'success'
                    );
                    $("#overlay").hide();
                }

            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );

                $("#overlay").hide();
            }


        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function Save(mode) {

    NO_BACKLOG = $("#txt_noBl").val();
    DSTRCT_CODE = $("#txt_dstrct").val();
    EQP_NUMBER = $("#txt_eqNumber").val();
    COMP_CODE = $("#txt_compCode").val();
    EGI = $("#txt_egi").val();
    HM = $("#txt_hm").val();
    BACKLOG_DESC = $("#txt_blDesc").val();
    INSPECTON_DATE = $("#txt_dInspecton").val();
    INSPECTOR = $("#txt_inspector").val();
    SOURCE = $("#txt_source").val();
    WORK_GROUP = $("#txt_wg").val();
    STD_JOB = $("#txt_standJob").val();
    NRP_GL = $("#txt_nrpGl").val();
    ORIGINATOR_ID = $("#txt_oriID").val();
    PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    MANPOWER = $("#txt_mp").val();
    HOUR_EST = $("#txt_hEst").val();
    POSISI_BACKLOG = $("#txt_posBL").val();
    CREATED_BY = $("#txt_inspector").val();
    REMARKS = $("#txt_note").val();
    PLANSTR_TIME = $("#txt_planStrTime").val();
    UPDATED_BY = $("#hd_idNrp").val();

    if (COMP_CODE == "") {
        window.location.hash = "txt_compCode";
        return false;
    } else if (BACKLOG_DESC == "") {
        window.location.hash = "txt_blDesc";
        return false;
    } else if (EQP_NUMBER == "") {
        window.location.hash = "txt_eqNumber";
        return false;
    } else if (HM == "") {
        window.location.hash = "txt_hm";
        return false;
    } else if (HOUR_EST == "") {
        window.location.hash = "txt_hEst";
        return false;
    } else if (INSPECTON_DATE == "") {
        window.location.hash = "txt_dInspecton";
        return false;
    } else if (MANPOWER == "") {
        window.location.hash = "txt_mp";
        return false;
    } else if (NRP_GL == "") {
        window.location.hash = "txt_nrpGl";
        return false;
    }
    else if (ORIGINATOR_ID == "") {
        window.location.hash = "txt_oriID";
        return false;
    }
    else if (PLAN_REPAIR_DATE_1 == "") {
        window.location.hash = "txt_planRD1";
        return false;
    } else if (SOURCE == "") {
        window.location.hash = "txt_source";
        return false;
    } else if (STD_JOB == "") {
        window.location.hash = "txt_standJob";
        return false;
    }

    if (mode == "save") {
        //if (REMARKS == "") {
        //    Swal.fire(
        //        'Warning!',
        //        'Message : Delivery Instruction belum diisi',
        //        'warning'
        //    );
        //    window.location.hash = "txt_note";
        //    return false;
        //}
        if (PLANSTR_TIME == "") {
            Swal.fire(
                'Warning!',
                'Message : Plan start time belum diisi',
                'warning'
            );
            window.location.hash = "txt_planStrTime";
            return false;
        }
    }

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBl").val();
    dataBacklog.DSTRCT_CODE = $("#txt_dstrct").val();
    dataBacklog.EQP_NUMBER = $("#txt_eqNumber").val();
    dataBacklog.COMP_CODE = $("#txt_compCode").val();
    dataBacklog.EGI = $("#txt_egi").val();
    dataBacklog.HM = $("#txt_hm").val();
    dataBacklog.BACKLOG_DESC = $("#txt_blDesc").val();
    dataBacklog.INSPECTON_DATE = $("#txt_dInspecton").val();
    dataBacklog.INSPECTOR = $("#txt_inspector").val();
    dataBacklog.SOURCE = $("#txt_source").val();
    dataBacklog.WORK_GROUP = $("#txt_wg").val();
    dataBacklog.STD_JOB = $("#txt_standJob").val();
    dataBacklog.NRP_GL = $("#txt_nrpGl").val();
    dataBacklog.ORIGINATOR_ID = $("#txt_oriID").val();
    dataBacklog.PLAN_REPAIR_DATE_1 = $("#txt_planRD1").val();
    dataBacklog.MANPOWER = $("#txt_mp").val();
    dataBacklog.HOUR_EST = $("#txt_hEst").val();
    dataBacklog.CREATED_BY = $("#txt_inspector").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.POSISI_BACKLOG = $("#txt_posBL").val();
    dataBacklog.UPDATED_BY = $("#hd_idNrp").val();
    dataBacklog.STATUS = "OPEN";


    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_Backlog_WOWR", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                if (mode == "create") {
                    createWOWR();
                } else if (mode == "save") {
                    Swal.fire(
                        'Saved!',
                        'Header Backlog saved!',
                        'success'
                    );
                    $("#overlay").hide();
                }

            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    'Message : ' + data.Message,
                    'error'
                );

                $("#overlay").hide();
            }


        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function FnSubmit(dataBacklog) {
    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Rescheduled_WOWR", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been back to Planner Logistic!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/Backlog/CreateWOWR";
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

function createWOWR() {
    //edit 2023/07/05
    debugger
    var idPosition = document.getElementById("hd_idPosition").value;
    $.ajax({
        url: $("#web_link").val() + "/api/EllWOWR/Create_WO?noBacklog=" + $("#txt_noBl").val() + "&StrTime=" + $("#txt_planStrTime").val(), //URI
        //url: $("#web_link").val() + "/api/EllWOWR/Create_WO?noBacklog=" + $("#txt_noBl").val() + "&StrTime=" + $("#txt_planStrTime").val() + "&idPosition=" + idPosition,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Create WO & WR Berhasil!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
                }).then((result) => {
                    if (result.isConfirmed) {
                        window.location.href = "/Backlog/CreateWOWR";
                    }
                })
            } if (data.Remarks == false) {
                Swal.fire(
                    'Error!',
                    data.Message,
                    'error'
                );
                saveBacklog("error");
                $("#overlay").hide();
                if (data.Pos == "PART") {
                    showPartError(data);
                }
                $("#overlay").hide();
            }

        },
        error: function (xhr) {
            alert(xhr.responseText);
            $("#overlay").hide();
        }
    });
}

function showPartError(data) {
    $("#PartError").show();
    $.each(data.Data, function (index, item) {
        let STOCK_CODE = item.stockCode,
            SUB_STOCK_CODE = item.Rel_stockCode,
            STATUS = item.Status,
            NO = index + 1;

        DetailTableBody = $("#table_error tBody");
        DetailTableBody.empty();
        let ListPart = '<tr>' +
            '<td>' + NO + '</td>' +
            '<td>' + STOCK_CODE + '</td>' +
            '<td>' + STATUS + '</td>' +
            '</tr>';

        DetailTableBody.append(ListPart);
    });
}