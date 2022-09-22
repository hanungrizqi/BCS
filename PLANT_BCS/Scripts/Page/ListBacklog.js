var table = $("#tbl_backlog").DataTable({
    ajax: {
        url: $("#web_link").val() + "/api/Backlog/Get_ListBacklog/" + $("#hd_site").val(),
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
        { data: 'STATUS' },
        {
            data: 'NO_BACKLOG',
            targets: 'no-sort', orderable: false,
            render: function (data, type, row) {
                action = `<div class="btn-group">`
                if (row.STATUS == "SAVED") {
                    action += `<a href="/Backlog/EditBacklog?noBacklog=${data}" class="btn btn-sm btn-info">Edit</a>`
                }
                action += `<button type="button" onclick="deleteBacklog(${data})" class="btn btn-sm btn-danger" title="Delete">Delete
                                </button>`
                action += `</div>`
                return action;
            }
        }
    ],

});

function deleteBacklog(noBacklog) {
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
                url: $("#web_link").val() + "/api/Backlog/Delete_Backlog?noBacklog=" + noBacklog, //URI
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