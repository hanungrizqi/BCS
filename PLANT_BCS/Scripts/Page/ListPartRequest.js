var table = $("#tbl_backlog").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Logistic/Get_BacklogPart/" + $("#hd_site").val(),
        dataSrc: "Data",
    },
    buttons: ['excel'],
    dom: "<'row'<'col-sm-12'<'text-center bg-body-light py-2 mb-2'B>>>" +
        "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>><'row'<'col-sm-12'tr>><'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
    "columnDefs": [
        { "className": "dt-center", "targets": '_all' },
        { "className": "dt-nowrap", "targets": '_all'}
    ],
    scrollX: true,
    columns: [
        { data: 'NO_BACKLOG' },
        { data: 'DSTRCT_CODE' },
        { data: 'EQP_NUMBER' },
        { data: 'COMP_CODE' },
        { data: 'HM' },
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
        { data: 'PART_NO' },
        { data: 'STOCK_CODE' },
        { data: 'STK_DESC' },
        { data: 'QTY' },
        { data: 'FIG_NO' },
        { data: 'INDEX_NO' },
        { data: 'AVAILABLE_STOCK' },
        { data: 'LOCATION_ON_STOCK' },
        {
            data: 'ETA_SUPPLY',
            render: function (data, type, row) {
                let tanggal = moment(data).format("DD/MM/YYYY");
                if (data == null) {
                    tanggal = "dd/mm/yyyy"
                }
                return tanggal;
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
                table.ajax.reload();
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