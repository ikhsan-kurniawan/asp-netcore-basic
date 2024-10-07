$(document).ready(function () {
    console.log("ready!");
    $("#accountrolesTable").DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "ajax": {
            url: "https://localhost:7180/accountroles",
            type: "GET",
            datatype: "json",
            "dataSrc": "data",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem("userToken")
            }
            //success: function (hasil) {
            //    console.log(hasil);
            //}
        },
        columnDefs: [{
            "defaultContent": "-",
            "targets": "_all"
        }],
        "columns": [
            {
                "data": null,
                "render": function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { "data": "accountRoleId" },
            { "data": "accountName" },
            { "data": "roleName" },
        ],
        "initComplete": function () {
            $('[data-toggle="tooltip"]').tooltip()
        },
    }).buttons().container().appendTo('#universitiesTable_wrapper .col-md-6:eq(0)');


});

$('[data-tooltip="tooltip"]').tooltip();