$(document).ready(function () {
    $("#accountTable").DataTable({
        "responsive": false,
        "lengthChange": true,
        "autoWidth": false,
        "paging": true,
        "searching": true,
        "ordering": true,
        "info": true,
        "scrollX": true,
        "ajax": {
            url: "https://localhost:7180/accounts",
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
            { "data": "nik" },
            {
                "data": null,
                "render": function (data, type, row) {
                    return row.firstName + ' ' + row.lastName; // Gabungkan firstName dan lastName
                }
            },
            { "data": "email" },
            { "data": "phone" },
            { "data": "birthDate" },
            { "data": "universityName" },
            { "data": "gpa" },
            { "data": "degree" },
        ],
        "initComplete": function () {
            $('[data-toggle="tooltip"]').tooltip()
        },
    });

    $.ajax({
        url: "https://localhost:7180/universities",
        type: "GET",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        },
        success: function (res) {
            $.each(res.data, function (k, v) {
                $("#inputUniversity").append($('<option></option>').val(v.id).text(v.name));
            });
        }
    });
});

$('[data-tooltip="tooltip"]').tooltip();

function clearScreen() {
    $("#editButton").hide();
    $("#saveButton").show();
    clearForm();
}

function clearForm() {
    $("form").find("input.form-control").val("");
    $("form").find(".form-control").removeClass("is-invalid");
}

function Save() {
    if ($("#accountForm").valid()) {

        var account = new Object();
        account.firstName = $("#inputNamaDepan").val();
        account.lastName = $("#inputNamaBelakang").val();
        account.phone = $("#inputTelepon").val();
        account.birthDate = $("#inputTanggalLahir").val();
        account.email = $("#inputEmail").val();
        account.password = $("#inputPassword").val();
        account.university_Id = $("#inputUniversity").val();
        account.degree = parseInt($("#inputDegree").val());
        account.gpa = $("#inputGpa").val();
        $.ajax({
            type: "POST",
            url: "https://localhost:7180/accounts/register",
            data: JSON.stringify(account),
            contentType: "application/json; charset=utf-8",
            headers: {
                "Authorization": "Bearer " + localStorage.getItem("userToken")
            }
        }).done((result) => {
            if (result.statusCode == 201) {
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: result.message,
                    showConfirmButton: false,
                    timer: 1500
                });
                $('#accountTable').DataTable().ajax.reload();
                $('#addAccountModal').modal('hide');
            } else {
                alert(result.message)
            }
        }).fail(function (jqXHR) {
            console.log(jqXHR);
            Swal.fire({
                position: "center",
                icon: "error",
                title: jqXHR.responseJSON.message,
                showConfirmButton: false,
                timer: 1500
            });
        });

    }
}

$(function () {
    $('#accountForm').validate({
        rules: {
            inputNamaDepan: {
                required: true,
            },
            inputNamaBelakang: {
                required: true,
            },
            inputTelepon: {
                required: true,
                digits: true,

            },
            inputTanggalLahir: {
                required: true,
            },
            inputEmail: {
                required: true,
            },
            inputPassword: {
                required: true,
            },
            inputUniversity: {
                required: true,
            },
            inputDegree: {
                required: true,
            },
            inputGpa: {
                required: true,
            },
        },
        messages: {
            inputNamaDepan: {
                required: "Nama depan harus diisi",
            },
            inputNamaBelakang: {
                required: "Nama belakang harus diisi",
            },
            inputTelepon: {
                required: "Telepon harus diisi",
                digits: "Telepon ngga boleh huruf",
            },
            inputTanggalLahir: {
                required: "Tangga lahir harus diisi",
            },
            inputEmail: {
                required: "Email harus diisi",
            },
            inputPassword: {
                required: "Password harus diisi",
            },
            inputUniversity: {
                required: "Universitas harus diisi",
            },
            inputDegree: {
                required: "Degree harus diisi",
            },
            inputGpa: {
                required: "GPA harus diisi",
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