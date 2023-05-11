Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    DetailTableBody = $("#table_part tBody");
    let listData = `<tr class="odd text-center"><td valign="top" colspan="11" class="dataTables_empty"><input type="text" id="txt_cekRow" hidden value="1"/>No data available in table</td></tr>`;
    DetailTableBody.append(listData);

    DetailTableRepair = $("#table_repair tBody");
    let listDataRepair = `<tr class="odd text-center"><td valign="top" colspan="11" class="dataTables_empty"><input type="text" id="txt_cekRowRepair" hidden value="1"/>No data available in table</td></tr>`;
    DetailTableRepair.append(listDataRepair);

    getEqNumber();
    getCompCode();
    getSource();
    //getNRPGL();
    getOriID();
    getSTDJob();
    document.getElementById("txt_dInspecton").setAttribute("max", new Date().toISOString().split("T")[0]);
    document.getElementById("txt_planRD1").setAttribute("min", new Date().toISOString().split("T")[0]);
    
})

$("#txt_eqNumber").on("change", function () {
    let egi = $(this).find(':selected').attr('data-egi');
    $("#txt_egi").val(egi);
})

$("#txt_standJob").on("change", function () {
    let wg = $(this).find(':selected').attr('data-wg');
    $("#txt_wg").val(wg);
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

    tableAddPart.ajax.url($("#web_link").val() + "/api/Master/Get_PartNo?stckCode=&dsctrct=").load();
    $("#tblAddPart").hide();
});

//Table Add Part
var tableAddPart = $("#tblAddPart").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Master/Get_PartNo?stckCode=&dsctrct=",
        dataSrc: "Data",
    },
    scrollX: true,
    dom: '<t>',
    columnDefs: [
        { className: "text-center", "targets": [0] },
    ],
    columns: [
        {
            data: null,
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                return `<input type="checkbox" name="checkPart" data-mnemonic="${row.MNEMONIC}" value="${row.PART_NO}" onclick="checkPart(this)">`
            }
        },
        { data: 'STOCK_CODE' },
        { data: 'PART_NO' },
        { data: 'MNEMONIC' },
        { data: 'PREF_PART_IND' },
    ],
});

function checkPart(checkbox) {
    var checkboxes = document.getElementsByName('checkPart')
    checkboxes.forEach((item) => {
        if (item !== checkbox) item.checked = false
    })
    $("#txt_partNo").val(checkbox.value);
    $("#txt_Mnemonic").val(checkbox.getAttribute('data-mnemonic'));
}

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
        url: $("#web_link").val() + "/api/Master/Get_PartNo?stckCode=" + stckCode + "&dsctrct=" + $("#txt_dstrct").val(), //URI,
        type: "GET",
        cache: false,
        beforeSend: function () {
            $("#overlay").show();
        },
        success: function (result) {
            if (result.Data.length > 0) {
                tableAddPart.ajax.url($("#web_link").val() + "/api/Master/Get_PartNo?stckCode=" + stckCode + "&dsctrct=" + $("#txt_dstrct").val()).load();
                $("#tblAddPart").show();
            } else {
                Swal.fire(
                    'Warning!',
                    'Stock Code atau Part No Belum Terdaftar',
                    'warning'
                );
                tableAddPart.ajax.url($("#web_link").val() + "/api/Master/Get_PartNo?stckCode=&dsctrct=").load();
            }
            $("#overlay").hide();
        }
    });
}

function getEqNumber() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_EqNumber/" + $("#hd_site").val() + "/" + $("#hd_nrp").val(), //URI,
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_eqNumber').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.EQUIP_NO + '" data-egi="' + val.EQUIP_GRP_ID + '">' + val.EQUIP_NO + '</option>';
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
                text += '<option value="' + val.COMP_CODE + '">' + val.COMP_CODE + ' - ' + val.COMP_DESC + '</option>';
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
                text += '<option value="' + val.SOURCE + '">' + val.SOURCE + '</option>';
            });
            $("#txt_source").append(text);
        }
    });
}

function getNRPGL() {
    $.ajax({
        url: $("#web_link").val() + "/api/Master/Get_NRPGL2/" + $("#hd_site").val() + "/" + $("#hd_nrp").val(),
        type: "GET",
        cache: false,
        success: function (result) {
            $('#txt_nrpGl').empty();
            text = '<option></option>';
            $.each(result.Data, function (key, val) {
                text += '<option value="' + val.EMPLOYEE_ID + '">' + val.EMPLOYEE_ID + ' - ' + val.NAME +'</option>';
            });
            $("#txt_nrpGl").append(text);
        }
    });
}

function getOriID() {
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
                text += '<option value="' + val.STD_JOB_NO + '" data-wg="' + val.WORK_GROUP +'">' + val.STD_JOB_NO + ' - ' + val.STD_JOB_DESC +'</option>';
            });
            $("#txt_standJob").append(text);
        }
    });
}

function addPartToTable() {

    let PART_NO = $("#txt_partNo").val(),
        MNEMONIC = $("#txt_Mnemonic").val(),
        FIG_NO = $("#txt_fiqNo").val(),
        INDEX_NO = $("#txt_indexNo").val(),
        QTY = $("#txt_qty").val(),
        STCK_CODE = $("#txt_stckCode").val()

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

                $.ajax({
                    url: $("#web_link").val() + "/api/Backlog/Get_PartDetail?partNO=" + PART_NO + "&site=" + $("#hd_site").val(), //URI,
                    type: "GET",
                    cache: false,
                    beforeSend: function () {
                        $("#overlay").show();
                    },
                    success: function (result) {
                        let tRow = $("#table_part >tbody >tr").length;
                        let noRow = tRow + 1;
                        if ($("#txt_cekRow").val() == "1") {
                            noRow = 1;
                        }
                        DetailTableBody = $("#table_part tBody");

                        if (result.Remarks == true) {
                            $('#table_part tr.odd').remove();
                            if (result.Data != null) {
                                var listData = '<tr>' +
                                    '<td>' + noRow + '<input type="text" name="txt_mnemonicPart" value="' + MNEMONIC + '" hidden></td>' +
                                    '<td>' + result.Data.PART_NO + '</td>' +
                                    '<td>' + result.Data.STOCK_CODE + '</td>' +
                                    '<td>' + result.Data.STK_DESC + '</td>' +
                                    '<td>' + FIG_NO + '</td>' +
                                    '<td>' + INDEX_NO + '</td>' +
                                    '<td>' + QTY + '</td>' +
                                    '<td>' + result.Data.UOM + '</td>' +
                                    '<td>' + result.Data.PART_CLASS + '</td>' +
                                    '<td class="text-success">ACTIVE</td>' +
                                    '<td><button onclick="removeList(this)" type="button" class="btn"><i class="fa fa-times text-danger"></i></button></td>' +
                                    /*'<td><button onclick="savePartTemp(this)" type="button" class="btn"><span class="badge bg-success"><i class="fa fa-check-circle opacity-50 me-1"></i> LOCK PART</span></button></td>' +*/
                                    '</tr>';
                                /*savePartTemp();*/
                            } else {
                                var listData = '<tr>' +
                                    '<td>' + noRow + '<input type="text" name="txt_mnemonicPart" value="' + MNEMONIC + '" hidden></td>' +
                                    '<td>' + PART_NO + '</td>' +
                                    '<td>' + STCK_CODE + '</td>' +
                                    '<td></td>' +
                                    '<td>' + FIG_NO + '</td>' +
                                    '<td>' + INDEX_NO + '</td>' +
                                    '<td>' + QTY + '</td>' +
                                    '<td></td>' +
                                    '<td></td>' +
                                    '<td class="text-danger">NONE</td>' +
                                    '<td><button onclick="removeList(this)" type="button" class="btn"><i class="fa fa-times text-danger"></i></button></td>' +
                                    '</tr>';
                            }
                        } else {
                            Swal.fire(
                                'Warning!',
                                'Part Number belum terdaftar',
                                'warning'
                            );
                            $("#overlay").hide();
                            return false;
                        }

                        DetailTableBody.append(listData);
                        $("#overlay").hide();
                        $('#modal').modal('hide');
                    }
                });
            }
        }
    });
}

function removeList(element) {
    var item = element.parentNode.parentNode.rowIndex;
    document.getElementById("table_part").deleteRow(item);


    let tRow = $("#table_part >tbody >tr").length;
    if (tRow == 0) {

        DetailTableBody = $("#table_part tBody");
        let listData = `<tr class="odd text-center"><td valign="top" colspan="11" class="dataTables_empty"><input type="text" id="txt_cekRow" hidden value="1"/>No data available in table</td></tr>`;
        DetailTableBody.append(listData);
    }
}

function addRepairToTable() {
    let PROBLEM_DESC = $("#text_problemDesc").val(),
        ACTIVITY_REPAIR = $("#txt_activityRepair").val()

    /*saveRepairTemp();*/
    DetailTableBody = $("#table_repair tBody");
    $('#table_repair tr.odd').remove();

    let tRow = $("#table_repair >tbody >tr").length;
    let noRow = tRow + 1;

    var listData = '<tr>' +
        '<td>' + noRow + '</td>' +
        '<td>' + PROBLEM_DESC + '</td>' +
        '<td>' + ACTIVITY_REPAIR + '</td>' +
        '<td><button onclick="removeListRepair(this)" type="button" class="btn"><i class="fa fa-times text-danger"></i></button></td>' +
        /*'<td><button onclick="saveRepairTemp(this)" type="button" class="btn"><span class="badge bg-success"><i class="fa fa-check-circle opacity-50 me-1"></i> LOCK REPAIR</span></button></td>' +*/
        '</tr>';
    /*saveRepairTemp();*/
    DetailTableBody.append(listData);
    $('#modal_repair').modal('hide');
}

function removeListRepair(element) {
    var item = element.parentNode.parentNode.rowIndex;
    document.getElementById("table_repair").deleteRow(item);

    let tRow = $("#table_repair >tbody >tr").length;
    if (tRow == 0) {

        DetailTableBody = $("#table_repair tBody");
        let listData = `<tr class="odd text-center"><td valign="top" colspan="11" class="dataTables_empty"><input type="text" id="txt_cekRowRepair" hidden value="1"/>No data available in table</td></tr>`;
        DetailTableBody.append(listData);
    }
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

        let tRow = $("#table_part >tbody >tr").length;
        if ($("#txt_cekRow").val() == "1" && $("#txt_cekRowRepair").val() == "1") {
            Swal.fire(
                'Submit error!',
                'Mohon untuk menginput list Recommended Part atau Recommended Repair',
                'warning'
            );
            return false;
        }

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

    // disable submit button
    //$('#submit_button').attr('disabled', 'disabled');
    /*console.log('submit_button');*/

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
            MNEMONIC: $(this).find('[name="txt_mnemonicPart"]').val(),
            FIG_NO: $(this).find('td:eq(4)').html(),
            INDEX_NO: $(this).find('td:eq(5)').html(),
            QTY: $(this).find('td:eq(6)').html()
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
                saveRepair();
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

function saveRepair() {
    var ListRepair = [];
    ListRepair.length = 0;

    //let partId = ListPart.PART_ID;
    //console.log(partId);

    $.each($("#table_repair tbody tr"), function () {
        ListRepair.push({
            NO_BACKLOG: $("#txt_noBl").val(),
            PROBLEM_DESC: $(this).find('td:eq(1)').html(),
            ACTIVITY_REPAIR: $(this).find('td:eq(2)').html(),
        });
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_BacklogRepair", //URI
        data: JSON.stringify(ListRepair),
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
        },
        //complete: function () {
        //    // enable submit button after AJAX request is complete
        //    $('#submit_button').removeAttr('disabled');
        //}
    });
}

// log submit button
console.log($('#submit_button'));

function savePartTemp() {
    var ListPart = [];
    ListPart.length = 0;

    $.each($("#tblAddPart tbody tr"), function () {
        let PartId = $("#txt_noBl").val() + $("#txt_partNo").val() + + $("#txt_indexNo").val() + $("#txt_fiqNo").val();

        ListPart.push({
            PART_ID: PartId,
            NO_BACKLOG: $("#txt_noBl").val(),
            DSTRCT_CODE: $("#txt_dstrct").val(),
            PART_NO: $("#txt_partNo").val(),
            STOCK_CODE: $("#txt_stckCode").val(),
            /*STK_DESC: $(this).find('td:eq(3)').html(),*/
            /*MNEMONIC: $(this).find('[name="txt_mnemonicPart"]').val(),*/
            FIG_NO: $("#txt_fiqNo").val(),
            INDEX_NO: $("#txt_indexNo").val(),
            QTY: $("#txt_qty").val(),
            //UOM: $(this).find('td:eq(7)').html(),
            //PART_CLASS: $(this).find('td:eq(8)').html(),
        });
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_BacklogPartTemp", //URI
        data: JSON.stringify(ListPart),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been locked!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
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

function deletePartTemp(partId) {
    PART_ID: partId,
    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/deletePartTemp?partId=" + partId, //URI
        type: "POST",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been locked!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
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

function saveRepairTemp() {
    var ListRepair = [];
    ListRepair.length = 0;

    $.each($("#table_repair tbody tr"), function () {
        let PartId = $("#txt_noBl").val() + $("#txt_activityRepair").val();
        ListRepair.push({
            REPAIR_ID: PartId,
            NO_BACKLOG: $("#txt_noBl").val(),
            PROBLEM_DESC: $("#text_problemDesc").val(),
            ACTIVITY_REPAIR: $("#txt_activityRepair").val(),
        });
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Create_RepairTemp", //URI
        data: JSON.stringify(ListRepair),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been locked!",
                    icon: 'success',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    allowOutsideClick: false,
                    allowEscapeKey: false
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

function deleteRepairTemp(partId) {

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/deleteRepairTemp?partId=" + partId, //URI
        type: "POST",
        success: function (data) {
            if (data.Remarks == true) {
                Swal.fire({
                    title: 'Saved',
                    text: "Your data has been saved!!",
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