$("document").ready(function () {
    getDetail();
})

$('#modal').on('hidden.bs.modal', function () {
    $("#txt_partNo").val("");
    $("#txt_stckCode").val("");
    $("#txt_fiqNo").val("");
    $("#txt_indexNo").val("");
    $("#txt_qty").val("");

    tableAddPart.ajax.url($("#web_link").val() + "/api/Master/Get_PartNo?stckCode=&dsctrct=").load();
    $("#tblAddPart").hide();
});

var table = $("#table_part").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogPart/" + $("#txt_noBacklog").val(),
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
        url: $("#web_link").val() + "/api/Backlog/Get_BacklogRepair/" + $("#txt_noBacklog").val(),
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
            $("#txt_mp").val(dataBacklog.MANPOWER);
            $("#txt_hEst").val(dataBacklog.HOUR_EST);
            $("#txt_posBL").val(dataBacklog.POSISI_BACKLOG);
            $("#txt_inspector").val(dataBacklog.CREATED_BY);
            $("#txt_note").val(dataBacklog.REMARKS);
        }
    });
}

function submitApproval(postStatus) {

    if ($("#txt_note").val() == "" || $("#txt_note").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Note Approval!',
            'warning'
        );
        return;
    }

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBacklog").val();
    dataBacklog.UPDATED_BY = $("#hd_nrp").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.POSISI_BACKLOG = "Logistic";
    dataBacklog.STATUS = postStatus;

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Approve_Backlog", //URI
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
                        window.location.href = "/BacklogApproval/ListApproval";
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
        QTY = $("#txt_qty").val(),
        MNEMONIC = $("#txt_Mnemonic").val()

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

                let PartId = $("#txt_noBacklog").val() + PART_NO;
                ListPart.push({
                    PART_ID: PartId,
                    NO_BACKLOG: $("#txt_noBacklog").val(),
                    DSTRCT_CODE: $("#txt_dstrct").val(),
                    PART_NO: PART_NO,
                    FIG_NO: FIG_NO,
                    INDEX_NO: INDEX_NO,
                    QTY: QTY,
                    MNEMONIC: MNEMONIC
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
        NO_BACKLOG: $("#txt_noBacklog").val(),
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
        NO_BACKLOG: $("#txt_noBacklog").val(),
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