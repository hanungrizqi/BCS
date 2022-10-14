Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    getEqNumber();
    getCompCode();
    getSource();
    /*getNRPGL();*/
    getOriID();
    getSTDJob();

    document.getElementById("txt_dInspecton").setAttribute("max", new Date().toISOString().split("T")[0]);
    document.getElementById("txt_planRD1").setAttribute("min", new Date().toISOString().split("T")[0]);
})

$('#modal_repair').on('hidden.bs.modal', function () {
    $("#text_problemDesc").val("");
    $("#txt_activityRepair").val("");
});

$('#modal').on('hidden.bs.modal', function () {
    $("#txt_partNo").val("");
    $("#txt_stckCode").val("");
    $("#txt_fiqNo").val("");
    $("#txt_indexNo").val("");
    $("#txt_qty").val("");
});

var table = $("#table_part").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogPart/" + $("#txt_noBl").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 4, 5, 6, 7, 8, 9, 10] }
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
        { data: 'UOM' },
        { data: 'PART_CLASS' },
        { data: 'STATUS' },
        {
            data: 'PART_ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${data}" data-value="${row.PART_NO}" data-stck="${row.STOCK_CODE}" data-fig="${row.FIG_NO}" data-index="${row.INDEX_NO}" data-qty="${row.QTY}"
                                onclick="editPart(this)" class="btn btn-sm btn-info" title="Edit" data-bs-toggle="modal" data-bs-target="#modal_update">Edit
                           </button>`
                action += `<button type="button" value="${data}" onclick="deletePart(this.value)" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],

});

var tableRepair = $("#table_repair").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogRepair/" + $("#txt_noBl").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 3] }
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
        { data: 'ACTIVITY_REPAIR' },
        {
            data: 'REPAIR_ID',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<button type="button" value="${data}" data-prob="${row.PROBLEM_DESC}" data-act="${row.ACTIVITY_REPAIR}"
                                onclick="editRepair(this.value,this.getAttribute('data-prob'),this.getAttribute('data-act'))" 
                                class="btn btn-sm btn-info" title="Edit" data-bs-toggle="modal" data-bs-target="#modal_repairUpdate">Edit
                           </button>`
                action += `<button type="button" value="${data}" onclick="deleteRepair(this.value)" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],

});

$("#txt_eqNumber").on("change", function () {
    let egi = $(this).find(':selected').attr('data-egi');
    $("#txt_egi").val(egi);
})

$("#txt_standJob").on("change", function () {
    let wg = $(this).find(':selected').attr('data-wg');
    $("#txt_wg").val(wg);
})

function searchPartNo() {
    let stckCode = $("#txt_stckCode").val();
    if (stckCode == "" || stckCode == null) {
        Swal.fire(
            'Warning!',
            'Mohon input stock code',
            'warning'
        );
        return false;
    }
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_PartNo/" + stckCode, //URI,
        type: "GET",
        cache: false,
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (result) {
            if (result.Data != null) {
                $("#txt_partNo").val(result.Data.PART_NO);
            } else {
                Swal.fire(
                    'Warning!',
                    'Stock Code Tidak Terdaftar',
                    'warning'
                );
            }
            $("#overlay").hide();
        }
    });
}

function getEqNumber() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_EqNumber/" + $("#hd_site").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_eqNumber').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.EQUIP_NO == $("#txt_eqNumberTemp").val()) {
                    text += '<option selected="selected" value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '" >' + val.EQUIP_NO + '</option>';
                } else {
                    text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '">' + val.EQUIP_NO + '</option>';
                }
            });
            $("#txt_eqNumber").append(text);
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

//function getNRPGL() {
//    $.ajax({
//        url: $("#web_link").val() + "/api/Master/Get_NRPGL/" + $("#txt_dstrct").val(),
//        type: "GET",
//        cache: false,
//        success: function (result) {
//            $('#txt_nrpGl').empty();
//            text = '<option></option>';
//            $.each(result.Data, function (key, val) {
//                if (val.EMPLOYEE_ID == $("#txt_nrpGlTemp").val()) {
//                    text += '<option selected value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
//                } else {
//                    text += '<option value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
//                }
//            });
//            $("#txt_nrpGl").append(text);
//        }
//    });
//}

function getOriID() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_OriginatorID/" + $("#txt_dstrct").val(),
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

function saveBacklog(postStatus) {
    if (postStatus == "SUBMITTED") {

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
        STATUS = postStatus;

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

        var dataPart = table.data().count();
        var dataRepair = tableRepair.data().count();

        if (dataPart == "0" && dataRepair == "0") {
            Swal.fire(
                'Submit error!',
                'Mohon untuk menginput list Recommended Part atau Recommended Repair',
                'warning'
            );
            return false;
        }

        let tRow = $("#table_part >tbody >tr").length;
        $.each($("#table_part tbody tr"), function (index) {
            partStatus = $(this).find('td:eq(9)').html();
            if (partStatus == "NONE") {
                Swal.fire(
                    'Submit error!',
                    'Ada Part number yang belum aktif',
                    'warning'
                );
                return false;
            } else {
                let getin = index + 1;
                if (getin == tRow) {
                    submitBacklog(postStatus)
                }
            }
        });
    } else {
        submitBacklog(postStatus)
    }
}

function submitBacklog(postStatus) {

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
    dataBacklog.STATUS = postStatus;
    if (postStatus == "SUBMITTED") {
        dataBacklog.POSISI_BACKLOG = "Planner";
    } else {
        dataBacklog.POSISI_BACKLOG = $("#txt_posBL").val();
    }

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_Backlog", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            $("#overlay").show();
        },
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
                        window.location.href = "/Backlog/ListBacklog";
                    }
                })
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

function deletePart(partId) {
    Swal.fire({
        title: "Are you sure?",
        text: "You will not be able to recover this data!",
        icon: "warning",
        showCancelButton: !0,
        customClass: { confirmButton: "btn btn-alt-danger m-1", cancelButton: "btn btn-alt-secondary m-1" },
        confirmButtonText: "Yes, delete it!",
        html: !1,
        preConfirm: function (e) {
            return new Promise(function (e) {
                setTimeout(function () {
                    e();
                }, 50);
            });
        },
    }).then(function (n) {
        if (n.value == true) {
            $.ajax({
                url: $("#web_link").val() + "/api/Backlog/Delete_BacklogPart/" + partId, //URI
                type: "POST",
                success: function (data) {
                    if (data.Remarks == true) {
                        Swal.fire("Deleted!", "Your Data has been deleted.", "success");
                        table.ajax.reload();
                    } if (data.Remarks == false) {
                        Swal.fire("Cancelled", "Message : " + data.Message, "error");
                    }

                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            })
        } else {
            Swal.fire("Cancelled", "Your Data is safe", "error");
        }
    });
}

function insertPart() {
    let PART_NO = $("#txt_partNo").val(),
        FIG_NO = $("#txt_fiqNo").val(),
        INDEX_NO = $("#txt_indexNo").val(),
        QTY = $("#txt_qty").val()

    if (PART_NO == "" || FIG_NO == "" || INDEX_NO == "" || QTY == "") {
        Swal.fire(
            'Warning!',
            'Mohon input Part Number, Fiq No, Index NO & QTY',
            'warning'
        );
        return;
    }

    let tRow = $("#table_part >tbody >tr").length;

    $.each($("#table_part tbody tr"), function (index) {
        partInTable = $(this).find('td:eq(1)').html();
        if (partInTable == PART_NO) {
            Swal.fire(
                'Error!',
                'Part Number sudah ditambahkan',
                'warning'
            );
            return false;
        }
        else {
            let getin = index + 1;
            if (getin == tRow) {
                var ListPart = [];
                ListPart.length = 0;

                let PartId = $("#txt_noBl").val() + PART_NO;
                ListPart.push({
                    PART_ID: PartId,
                    NO_BACKLOG: $("#txt_noBl").val(),
                    DSTRCT_CODE: $("#txt_dstrct").val(),
                    PART_NO: PART_NO,
                    FIG_NO: FIG_NO,
                    INDEX_NO: INDEX_NO,
                    QTY: QTY,
                });

                $.ajax({
                    url: $("#web_link").val() + "/api/Backlog/Create_BacklogPart", //URI
                    data: JSON.stringify(ListPart),
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (data) {
                        if (data.Remarks == true) {
                            $('#modal').modal('hide');
                            table.ajax.reload();
                        } else {
                            Swal.fire(
                                'Error!',
                                'Message : ' + data.Message,
                                'error'
                            );
                        }
                        $("#overlay").hide();
                    },
                    error: function (xhr) {
                        alert(xhr.responseText);
                    }
                })
            }
        }
    });
}

function updatePart() {
    let PART_NO = $("#txt_partNo_update").val(),
        FIG_NO = $("#txt_fiqNo_update").val(),
        INDEX_NO = $("#txt_indexNo_update").val(),
        QTY = $("#txt_qty_update").val(),
        PART_ID = $("#txt_partId_update").val()

    var ListPart = [];
    ListPart.length = 0;

    ListPart.push({
        PART_ID: PART_ID,
        NO_BACKLOG: $("#txt_noBl").val(),
        DSTRCT_CODE: $("#txt_dstrct").val(),
        PART_NO: PART_NO,
        FIG_NO: FIG_NO,
        INDEX_NO: INDEX_NO,
        QTY: QTY,
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_BacklogPart", //URI
        data: JSON.stringify(ListPart),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                $('#modal_update').modal('hide');
                table.ajax.reload();
            } else {
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
    })
}

function editPart(PART) {
    let PART_ID = PART.value,
        PART_NO = PART.getAttribute('data-value'),
        STOCK_CODE = PART.getAttribute('data-stck'),
        FIG_NO = PART.getAttribute('data-fig'),
        INDEX_NO = PART.getAttribute('data-index'),
        QTY = PART.getAttribute('data-qty')

    $("#txt_partId_update").val(PART_ID);
    $("#txt_partNo_update").val(PART_NO);
    $("#txt_stckCode_update").val(STOCK_CODE);
    $("#txt_fiqNo_update").val(FIG_NO);
    $("#txt_indexNo_update").val(INDEX_NO);
    $("#txt_qty_update").val(QTY);
}

function editRepair(repairId, probDesc, actRepair) {
    $("#text_repairID").val(repairId);
    $("#text_problemDescUpdate").val(probDesc);
    $("#txt_activityRepairUpdate").val(actRepair);
}

function deleteRepair(repairId) {
    Swal.fire({
        title: "Are you sure?",
        text: "You will not be able to recover this data!",
        icon: "warning",
        showCancelButton: !0,
        customClass: { confirmButton: "btn btn-alt-danger m-1", cancelButton: "btn btn-alt-secondary m-1" },
        confirmButtonText: "Yes, delete it!",
        html: !1,
        preConfirm: function (e) {
            return new Promise(function (e) {
                setTimeout(function () {
                    e();
                }, 50);
            });
        },
    }).then(function (n) {
        if (n.value == true) {
            $.ajax({
                url: $("#web_link").val() + "/api/Backlog/Delete_BacklogReparir/" + repairId, //URI
                type: "POST",
                success: function (data) {
                    if (data.Remarks == true) {
                        Swal.fire("Deleted!", "Your Data has been deleted.", "success");
                        tableRepair.ajax.reload();
                    } if (data.Remarks == false) {
                        Swal.fire("Cancelled", "Message : " + data.Message, "error");
                    }

                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            })
        } else {
            Swal.fire("Cancelled", "Your Data is safe", "error");
        }
    });
}

function addRepair() {
    var ListRepair = [];
    ListRepair.length = 0;

    ListRepair.push({
        NO_BACKLOG: $("#txt_noBl").val(),
        PROBLEM_DESC: $("#text_problemDesc").val(),
        ACTIVITY_REPAIR: $("#txt_activityRepair").val()
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_BacklogRepair", //URI
        data: JSON.stringify(ListRepair),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                $('#modal_repair').modal('hide');
                tableRepair.ajax.reload();
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
    })
}

function updateRepair() {
    var obj = new Object();
    obj.REPAIR_ID = $("#text_repairID").val();
    obj.PROBLEM_DESC = $("#text_problemDescUpdate").val();
    obj.ACTIVITY_REPAIR = $("#txt_activityRepairUpdate").val();

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/update_BacklogRepair", //URI
        data: JSON.stringify(obj),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                $('#modal_repairUpdate').modal('hide');
                tableRepair.ajax.reload();
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
    })
}