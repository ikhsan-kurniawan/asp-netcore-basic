$(document).ready(function () {
    console.log("ready!");
    $("#rolesTable").DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "ajax": {
            url: "https://localhost:7180/roles",
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
            { "data": "roleId" },
            { "data": "roleName" },
            {
                "render": function (data, type, row) {
                    return '<button onclick="editRoles(\'' + row.roleId + '\')" type="button" class="btn btn-warning" data-toggle="tooltip" data-placement="top" title="Edit data"><i class="fas fa-solid fa-pen"></i></button> ' +
                        '<button onclick="deleteRoles(\'' + row.roleId + '\')" type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="top" title="Hapus data"><i class=" fas fa-solid fa-trash"></i></button>';
                }
            }
        ],
        "initComplete": function () {
            $('[data-toggle="tooltip"]').tooltip()
        },
    }).buttons().container().appendTo('#universitiesTable_wrapper .col-md-6:eq(0)');


});

$('[data-tooltip="tooltip"]').tooltip();
function clearScreen() {
    $("#editButton").hide();
    $("#saveButton").show();
    clearForm();
}
function clearForm() {
    $("form").find(".form-control").val("");
    $("form").find(".form-control").removeClass("is-invalid");
}
function Save() {
    if ($("#roleForm").valid()) {
        var roles = new Object();
        roles.roleName = $("#inputNama").val();

        $.ajax({
            type: "POST",
            url: "https://localhost:7180/roles",
            data: JSON.stringify(roles),
            contentType: "application/json; charset=UTF-8",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem("userToken")
            }
        }).done(function (hasil) {
            Swal.fire({
                position: "center",
                icon: "success",
                title: hasil.message,
                showConfirmButton: false,
                timer: 1500
            });
            $('#rolesTable').DataTable().ajax.reload();
            $('#addRoleModal').modal('hide');
        }).fail(function (jqXHR) {
            console.log(jqXHR);
        });
    } else {
        console.log("invalid")
    }

}

function editRoles(roleId) {
    $("#editButton").show();
    $("#saveButton").hide();
    $("#universityForm").find(".is-invalid").removeClass("is-invalid");


    $.ajax({
        type: "GET",
        url: "https://localhost:7180/roles/" + roleId,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        }
    }).done(function (hasil) {
        $("#inputID").val(hasil.data.roleId);
        $("#inputNama").val(hasil.data.roleName);
        $('#addRoleModal').modal('show');
    }).fail(function (jqXHR) {
        console.log(jqXHR);
    });
}

function Edit() {
    if ($("#roleForm").valid()) {
        var roles = new Object();
        roles.roleId = $("#inputID").val();
        roles.roleName = $("#inputNama").val();

        $.ajax({
            type: "PUT",
            url: "https://localhost:7180/roles/" + roles.roleId,
            data: JSON.stringify(roles),
            contentType: "application/json; charset=UTF-8",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem("userToken")
            }
        }).done(function (hasil) {
            Swal.fire({
                position: "center",
                icon: "success",
                title: hasil.message,
                showConfirmButton: false,
                timer: 1500
            });
            $('#rolesTable').DataTable().ajax.reload();
            $('#addRoleModal').modal('hide');
        }).fail(function (jqXHR) {
            console.log(jqXHR.responseJSON.message);
        });
    }
}

function deleteRoles(roleId) {
    Swal.fire({
        title: "Yakin mau hapus?",
        text: "Data akan dihapus permanen!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, Hapus!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "https://localhost:7180/roles/" + roleId,
                type: "Delete",
                dataType: "json",
                headers: {
                    "Authorization": "Bearer " + localStorage.getItem("userToken")
                }
            }).done((hasil) => {
                Swal.fire({
                    title: "Deleted!",
                    text: hasil.message,
                    icon: "success"
                });
                $('#rolesTable').DataTable().ajax.reload();
            }).fail((err) => {
                Swal.fire({
                    title: "Error!",
                    text: err.responseJSON.message,
                    icon: "error"
                });
            });
        }
    });
}

$(function () {
    $('#roleForm').validate({
        rules: {
            inputNama: {
                required: true,
            },
        },
        messages: {
            inputNama: {
                required: "Nama harus diisi",
            },
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });
});