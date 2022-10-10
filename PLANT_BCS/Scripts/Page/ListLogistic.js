var table = $("#tbl_backlog").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_ListLogistic/" + $("#hd_site").val(),
        dataSrc: "Data",
    },
    "columnDefs": [
        { "className": "dt-center", "targets": [0, 1, 6, 7] }
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
        {
            data: 'STATUS',
            render: function (data, type, row) {
                text = `<span class="badge bg-success">${data}</span>`;
                return text;
            } 
        },
        {
            data: 'NO_BACKLOG',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                action += `<a href="/Logistic/ETASupply?noBacklog=${data}" class="btn btn-sm btn-info">Detail</a>`
                return action;
            }
        }
    ]
});


function uploadExcel() {
    if (document.getElementById("file").files.length == 0) {
        Swal.fire(
            'Warning!',
            'No file uploaded',
            'warning'
        );
        return false;
    }

    var form = new FormData(document.getElementById("form_upload_excel"));
    $(document).ajaxStart(function () {
        $("#overlay").show();
    }).ajaxStop(function () {
        $('#form_upload_excel').trigger("reset");
        $("#overlay").hide('slow');
    });
    $.ajax({
        url: "/logistic/Upload2",
        data: form,
        cache: false,
        type: "POST",
        contentType: false,
        processData: false,
        dataType: "json",
        success: function (result) {

            if (result.Remarks == true) {
                Swal.fire(
                    'Saved!',
                    'Data has been Saved.',
                    'success'
                );
            }
            else {
                Swal.fire(
                    'Error!',
                    'Message : ' + result.Message,
                    'error'
                );
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function downloadTemplate() {
    window.open("/Content/Document/Template_logistic_ETA_Supply.xlsx");
}
