﻿$("document").ready(function () {
    getDetail();
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
        { data: 'PART_NO' },
        { data: 'STOCK_CODE' },
        { data: 'STK_DESC' },
        { data: 'FIG_NO' },
        { data: 'INDEX_NO' },
        { data: 'QTY' },
        { data: 'PART_CLASS' }
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
            $("#txt_compCode").val(dataBacklog.COMP_CODE);
            $("#txt_egi").val(dataBacklog.EGI);
            $("#txt_hm").val(dataBacklog.HM);
            $("#txt_blDesc").val(dataBacklog.BACKLOG_DESC);
            $("#txt_dInspecton").val(moment(dataBacklog.INSPECTON_DATE).format("YYYY-MM-DD"));
            $("#txt_inspector").val(dataBacklog.INSPECTOR);
            $("#txt_source").val(dataBacklog.SOURCE);
            $("#txt_wg").val(dataBacklog.WORK_GROUP);
            $("#txt_standJob").val(dataBacklog.STD_JOB);
            $("#txt_nrpGl").val(dataBacklog.NRP_GL);
            $("#txt_oriID").val(dataBacklog.ORIGINATOR_ID);
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

    let dataBacklog = new Object();
    dataBacklog.NO_BACKLOG = $("#txt_noBacklog").val();
    dataBacklog.UPDATED_BY = $("#hd_nrp").val();
    dataBacklog.REMARKS = $("#txt_note").val();
    dataBacklog.STATUS = postStatus;

    $.ajax({
        url: $("#web_link").val() + "/api/Backlog/Approve_Backlog", //URI
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
                        window.location.href = "/BacklogApproval/ListApproval";
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