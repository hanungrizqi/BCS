﻿var table = $("#tbl_backlog").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_ListApprovalBacklog/" + $("#hd_site").val(),
        dataSrc: "Data",
    },

    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 5, 6, 7] },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        { data: 'NO_BACKLOG' },
        { data: 'DSTRCT_CODE' },
        { data: 'EQP_NUMBER' },
        { data: 'COMP_CODE' },
        { data: 'BACKLOG_DESC' },
        {
            data: 'INSPECTON_DATE',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        { data: 'CREATED_DATE' },
        { data: 'STATUS' },
        {
            data: 'NO_BACKLOG',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/BacklogApproval/DetailBacklog?noBacklog=${data}" class="btn btn-sm btn-info">Detail</a>`
                return action;
            }
        }
    ],

});