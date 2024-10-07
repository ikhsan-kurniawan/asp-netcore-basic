$(document).ready(function () {
    console.log("ready!");
    $("#universitiesTable").DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "buttons": ["copy", "csv", "excel", "pdf", "print", "colvis"],
        "ajax": {
            url: "https://localhost:7180/universities",
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
            { "data": "id" },
            { "data": "name" },
            {
                "render": function (data, type, row) {
                    return '<button onclick="editUniversitas(\'' + row.id + '\')" type="button" class="btn btn-warning" data-toggle="tooltip" data-placement="top" title="Edit data"><i class="fas fa-solid fa-pen"></i></button> ' +
                        '<button onclick="deleteUniversitas(\'' + row.id + '\')" type="button" class="btn btn-danger" data-toggle="tooltip" data-placement="top" title="Hapus data"><i class=" fas fa-solid fa-trash"></i></button>';
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
    if ($("#universityForm").valid()) {
        var universitas = new Object();
        universitas.name = $("#inputNama").val();

        $.ajax({
            type: "POST",
            url:"https://localhost:7180/universities",
            data: JSON.stringify(universitas),
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
            $('#universitiesTable').DataTable().ajax.reload();
            $('#addUniversityModal').modal('hide');
        }).fail(function (jqXHR) {
            console.log(jqXHR.responseJSON.message);
        });
    } else {
        console.log("invalid")
    }

}

function editUniversitas(univId) {
    $("#editButton").show();
    $("#saveButton").hide();
    $("#universityForm").find(".is-invalid").removeClass("is-invalid");


    $.ajax({
        type: "GET",
        url: "https://localhost:7180/universities/" + univId,
        dataType: "json",
        contentType: "application/json; charset=UTF-8",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        }
    }).done(function (hasil) {
        $("#inputID").val(hasil.data.id);
        $("#inputNama").val(hasil.data.name);
        $('#addUniversityModal').modal('show');
    }).fail(function (jqXHR) {
        console.log(jqXHR);
    });
}

function Edit() {
    if ($("#universityForm").valid()) {
        var universitas = new Object();
        universitas.id = $("#inputID").val();
        universitas.name = $("#inputNama").val();

        $.ajax({
            type: "PUT",
            url: "https://localhost:7180/universities/" + universitas.id,
            data: JSON.stringify(universitas),
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
            $('#universitiesTable').DataTable().ajax.reload();
            $('#addUniversityModal').modal('hide');
        }).fail(function (jqXHR) {
            console.log(jqXHR.responseJSON.message);
        });
    }
}

function deleteUniversitas(univId) {
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
                url: "https://localhost:7180/universities/" + univId,
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
                $('#universitiesTable').DataTable().ajax.reload();
            });
        }
    });
}

$(function () {
    $('#universityForm').validate({
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