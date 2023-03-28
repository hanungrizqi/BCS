new bootstrap.Toast(document.getElementById('toast-example-2')).show();

var table = $("#tbl_backlog").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_ListBacklogReview/" + $("#hd_site").val(),
        dataSrc: "Data",
    },

    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 4, 6, 7, 8, 9] },
        { "className": "dt-nowrap", "targets": '_all' }
    ],
    scrollX: true,
    columns: [
        { data: 'NO_BACKLOG' },
        { data: 'WO_NO' },
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
        {
            data: 'PLAN_REPAIR_DATE_1',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        {
            data: 'PLAN_REPAIR_DATE_2',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        { data: 'POSISI_BACKLOG' },
        { data: 'CREATED_DATE' },
        {
            data: 'STATUS',
            render: function (data, type, row) {
                //text = '';
                //if (data == "PLANNER APPROVED") {
                //    text = `<span class="badge bg-primary">${data}</span>`;
                //} else if (data == "PLANNER CANCEL") {
                //    text = `<span class="badge bg-danger">${data}</span>`;
                //} else if (data == "PROGRESS") {
                //    text = `<span class="badge bg-warning">${data}</span>`;
                //} else if (data == "CLOSE") {
                //    text = `<span class="badge bg-success">${data}</span>`;
                //} else {
                //    text = `<span class="badge bg-info">${data}</span>`;
                //}
                //return text;
                text = '';
                if (data == "CLOSE") {
                    text = `<span class="badge bg-success">${data}</span>`;
                } else if (data == "PLANNER CANCEL") {
                    text = `<span class="badge bg-danger">${data}</span>`;
                } else {
                    text = `<span class="badge bg-info">${data}</span>`;
                }
                return text;
            }
        },
        {
            data: 'NO_BACKLOG',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Review/DetailBacklog?noBacklog=${data}" class="btn btn-sm btn-primary">Detail</a>`;     
                action += `</div>`;
                return action;
            }
        }
    ],

    initComplete: function () {
        this.api()
            .columns(9)
            .every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm" style="width:200px; display:inline-block; margin-left: 10px;"><option value="">-- STATUS --</option></select>')
                    .appendTo($("#tbl_backlog_filter.dataTables_filter"))
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());

                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                    });

                column
                    .data()
                    .unique()
                    .sort()
                    .each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>');
                    });
            });
    }
});