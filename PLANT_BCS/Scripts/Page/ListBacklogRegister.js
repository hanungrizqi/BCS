var table = $("#tbl_register").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/MOK/Get_ListBacklogRegister/" + $("#hd_site").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 2, 3, 5, 6, 7] },
        { "className": "dt-nowrap", "targets": '_all' },
        {
            "targets": 0, // Kolom pertama
            "width": "700px", // Lebar yang diinginkan
        }
    ],
    scrollX: true,
    columns: [
        { data: 'NO_REGISTER' },
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
            data: 'CREATED_DATE',
            render: function (data, type, row) {
                const tanggal = moment(data).format("DD/MM/YYYY");
                return tanggal;
            }
        },
        {
            data: 'STATUS',
            render: function (data, type, row) {
                text = '';
                text = `<span class="badge bg-info">${data}</span>`;
                return text;
            }
        },
        {
            data: 'NO_REGISTER',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Backlog/RegisterPlanet?noregister=${data}" class="btn btn-sm btn-primary">Detail</a>`;
                action += `</div>`;
                return action;
            }
        }
    ]
});