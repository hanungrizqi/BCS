Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    getEqNumber();
    getCompCode();
    getSource();
    getNRPGL();
    getOriID();
    getSTDJob();
})

var table = $("#table_part").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogPart/" + $("#txt_noBl").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 4, 5, 6, 7, 8, 9 ,10] }
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
                action += `<button type="button" onclick="editPart(${data})" class="btn btn-sm btn-info" title="Edit">Edit
                                </button>`
                action += `</div>`
                action += `<button type="button" onclick="deletePart(${data})" class="btn btn-sm btn-danger" title="Delete">Delete
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
                    text += '<option selected value="' + val.COMP_CODE + '">' + val.COMP_CODE + '</option>';
                } else {
                    text += '<option value="' + val.COMP_CODE + '">' + val.COMP_CODE + '</option>';
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

function getNRPGL() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_NRPGL", //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_nrpGl').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                if (val.EMPLOYEE_ID == $("#txt_nrpGlTemp").val()) {
                    text += '<option selected value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
                } else {
                    text += '<option value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME + '</option>';
                }
            });
            $("#txt_nrpGl").append(text);
        }
    });
}

function getOriID() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_OriginatorID" , //URI,
        type: "GET",
        cache: false,
        success: function (result) {
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

function addPartToTable() {
    let PART_NO = $("#txt_partNo").val(),
        FIG_NO = $("#txt_fiqNo").val(),
        INDEX_NO = $("#txt_indexNo").val(),
        QTY = $("#txt_qty").val()

    let tRow = $("#table_part >tbody >tr").length;

    $.each($("#table_part tbody tr"), function (index) {
        partInTable = $(this).find('td:eq(1)').html();
        if (partInTable == PART_NO) {
            Swal.fire(
                'Error!',
                'Part Number sudah terdaftar',
                'warning'
            );
            return;
        }
        else {
            let getin = index + 1;
            if (getin == tRow) {
                DetailTableBody = $("#table_part tBody");

                $.ajax({
                    url: $("#web_link").val() + "/api/Backlog/Get_PartDetail?partNO=" + PART_NO + "&site=" + $("#hd_site").val(), //URI,
                    type: "GET",
                    cache: false,
                    success: function (result) {
                        let tRow = $("#table_part >tbody >tr").length;
                        let noRow = tRow + 1;

                        if (result.Data != null) {
                            var listData = '<tr>' +
                                '<td>' + noRow + '</td>' +
                                '<td>' + PART_NO + '</td>' +
                                '<td>' + result.Data.STOCK_CODE + '</td>' +
                                '<td>' + result.Data.STK_DESC + '</td>' +
                                '<td>' + FIG_NO + '</td>' +
                                '<td>' + INDEX_NO + '</td>' +
                                '<td>' + QTY + '</td>' +
                                '<td>' + result.Data.UOM + '</td>' +
                                '<td>' + result.Data.PART_CLASS + '</td>' +
                                '<td>ACTIVE</td>' +
                                '<td><button onclick="removeList(this)" type="button" class="btn"><i class="fa fa-times text-danger"></i></button></td>' +
                                '</tr>';
                        } else {
                            var listData = '<tr>' +
                                '<td>' + noRow + '</td>' +
                                '<td>' + PART_NO + '</td>' +
                                '<td></td>' +
                                '<td></td>' +
                                '<td>' + FIG_NO + '</td>' +
                                '<td>' + INDEX_NO + '</td>' +
                                '<td>' + QTY + '</td>' +
                                '<td></td>' +
                                '<td></td>' +
                                '<td>NONE</td>' +
                                '<td><button onclick="removeList(this)" type="button" class="btn"><i class="fa fa-times text-danger"></i></button></td>' +
                                '</tr>';
                        }
                        DetailTableBody.append(listData);
                        $('#modal').modal('hide');
                    }
                });
            }
        }
    });
}

function removeList(element) {
    
}

function saveBacklog(postStatus) {
    if (postStatus == "SUBMITTED") {
        let tRow = $("#table_part >tbody >tr").length;
        $.each($("#table_part tbody tr"), function (index) {
            partStatus = $(this).find('td:eq(9)').html();
            if (partStatus == "NONE") {
                Swal.fire(
                    'Submit error!',
                    'Ada Part number yang belum aktif',
                    'warning'
                );
                return;
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
    dataBacklog.POSISI_BACKLOG = $("#txt_posBL").val();
    dataBacklog.CREATED_BY = $("#txt_inspector").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.STATUS = postStatus;

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_Backlog", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                savePart();
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

function savePart() {
    var ListPart = [];
    ListPart.length = 0;

    $.each($("#table_part tbody tr"), function () {
        let PartId = $("#txt_noBl").val() + $(this).find('td:eq(1)').html();
        ListPart.push({
            PART_ID: PartId,
            NO_BACKLOG: $("#txt_noBl").val(),
            DSTRCT_CODE: $("#txt_dstrct").val(),
            PART_NO: $(this).find('td:eq(1)').html(),
            FIG_NO: $(this).find('td:eq(4)').html(),
            INDEX_NO: $(this).find('td:eq(5)').html(),
            QTY: $(this).find('td:eq(6)').html(),
            STATUS: $(this).find('td:eq(9)').html()
        });
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_BacklogPart", //URI
        data: JSON.stringify(ListPart),
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
                        window.location.href = "/Backlog/ListBacklog";
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
    })
}