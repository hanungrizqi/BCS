Codebase.helpersOnLoad(['jq-select2']);

$("document").ready(function () {
    getDetail();
})

var tablePart = $("#table_part").DataTable();

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

function saveBacklog(dataStatus) {
    var ListPart = [];
    ListPart.length = 0;

    $.each($("#table_part tbody tr"), function () {
        ListPart.push({
            PART_ID: $(this).find('input[name="txt_partid').val(),
            AVAILABLE_STOCK: $(this).find('input[name="txt_availableStock').val(),
            LOCATION_ON_STOCK: $(this).find('[name="txt_los').val(),
            ETA_SUPPLY: $(this).find('input[name="txt_ETASupply').val(),
        });
    });

    $.ajax({
        url: $("#web_link").val() + "/api/Logistic/Update_ETASupply", //URI
        data: JSON.stringify(ListPart),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data.Remarks == true) {
                if (dataStatus == "SAVED") {
                    Swal.fire(
                        'Saved!',
                        'Data has been saved',
                        'success'
                    );
                }
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

function submitBacklog(status) {
    let tRow = $("#table_part >tbody >tr").length;
    $.each($("#table_part tbody tr"), function (index) {
        let AVAILABLE_STOCK = $(this).find('input[name="txt_availableStock').val(),
            LOCATION_ON_STOCK = $(this).find('[name="txt_los').val(),
            ETA_SUPPLY = $(this).find('input[name="txt_ETASupply').val();

        if (AVAILABLE_STOCK == "" || LOCATION_ON_STOCK == "" || ETA_SUPPLY == "") {
            Swal.fire(
                'Warning!',
                'Pastikan data Recommended Part sudah terisi semua',
                'warning'
            );
            return false;
        } else {
            let getin = index + 1;
            if (getin == tRow) {
                submitChanges(status);
            }
        }
    });
}

function submitChanges(status) {
    if ($("#txt_note").val() == "" || $("#txt_note").val() == null) {
        Swal.fire(
            'Warning',
            'Mohon sertakan Note!',
            'warning'
        );
        return false;
    }

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBacklog").val();
    dataBacklog.UPDATED_BY = $("#hd_nrp").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.STATUS = status;
    dataBacklog.POSISI_BACKLOG = "Planner1";

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Approve_Backlog", //URI
        data: JSON.stringify(dataBacklog),
        dataType: "json",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        beforeSend: function () {
            saveBacklog(status);
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
                        window.location.href = "/Logistic/index";
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
