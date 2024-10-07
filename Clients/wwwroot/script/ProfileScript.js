$(document).ready(function () {
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

    $.ajax({
        type: "GET",
        url: "https://localhost:7180/accounts/" + localStorage.getItem("nik"),
        dataType: 'json',
        contentType: "application/json; charset=UTF-8",
        headers: {
            "Authorization": "Bearer " + localStorage.getItem("userToken")
        }
    }).done(function (hasil) {
        $('#nik').text(hasil.data.nik);
        $('#fullName').text(hasil.data.firstName + " " + hasil.data.lastName);
        $('#phone').text(hasil.data.phone);
        $('#email').text(hasil.data.email);
        // Format tanggal
        const birthDate = new Date(hasil.data.birthDate);
        const options = { day: 'numeric', month: 'long', year: 'numeric' };
        $('#birthDate').text(birthDate.toLocaleDateString('en-US', options));
        $('#universityName').text(hasil.data.universityName);
        $('#gpa').text(hasil.data.gpa);
        $('#degree').text(hasil.data.degree);
        $('#roles').text(hasil.data.roles);


        $('#inputNik').val(hasil.data.nik);
        $('#inputNamaDepan').val(hasil.data.firstName);
        $('#inputNamaBelakang').val(hasil.data.lastName);
        $('#inputTelepon').val(hasil.data.phone);
        $('#inputTanggalLahir').val(hasil.data.birthDate.split('T')[0]);
        $('#inputEmail').val(hasil.data.email);
        $('#inputUniversity').val(hasil.data.universityId);
        $('#inputDegree').val(hasil.data.enumDegree.toString());
        $('#inputGpa').val(hasil.data.gpa);

        $('#inputPasswordNik').val(hasil.data.nik);
        console.log(hasil.data)

        $(".edit").show();

    }).fail(function (jqXHR) {
        console.log(jqXHR.responseJSON.message);
    });
});

function clearForm() {
    $("form").find(".form-control").removeClass("is-invalid");
}

function Edit() {
    if ($("#editForm").valid()) {
        // Mengambil data dari form
        var account = {
            nik: $("#inputNik").val(),
            firstName: $("#inputNamaDepan").val(),
            lastName: $("#inputNamaBelakang").val(),
            phone: $("#inputTelepon").val(),
            birthDate: $("#inputTanggalLahir").val(),
            email: $("#inputEmail").val(),
            university_Id: $("#inputUniversity").val(),
            degree: parseInt($("#inputDegree").val()),
            gpa: $("#inputGpa").val(),
        };

        $.ajax({
            type: "PUT",
            url: "https://localhost:7180/accounts/" + localStorage.getItem("nik"),
            data: JSON.stringify(account),
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
            }).then(() => {
                window.location.href = "https://localhost:7170/profile";
            });
        }).fail(function (jqXHR) {
            console.log(jqXHR);
        });
    }
}

function EditPassword() {
    if ($("#editPasswordForm").valid()) {
        // Mengambil data dari form
        var account = {
            nik: $("#inputPasswordNik").val(),
            oldPassword: $("#inputPasswordLama").val(),
            newPassword: $("#inputPasswordBaru").val(),
            newPasswordConfirmation: $("#inputPasswordKonfirmasi").val(),
        };

        $.ajax({
            type: "PUT",
            url: "https://localhost:7180/accounts/editpassword/" + localStorage.getItem("nik"),
            data: JSON.stringify(account),
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
            }).then(() => {
                window.location.href = "https://localhost:7170/profile";
            });
        }).fail(function (jqXHR) {
            Swal.fire({
                position: "center",
                icon: "error",
                title: jqXHR.responseJSON.message,
                showConfirmButton: false,
                timer: 1500
            })
            console.log(jqXHR);
        });
    }
}


$(function () {
    $('#editForm').validate({
        rules: {
            inputNamaDepan: {
                required: true,
            },
            inputNamaBelakang: {
                required: true,
            },
            inputTelepon: {
                required: true,
            },
            inputTanggalLahir: {
                required: true,
            },
            inputEmail: {
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
            },
            inputTanggalLahir: {
                required: "Tangga lahir harus diisi",
            },
            inputEmail: {
                required: "Email harus diisi",
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

$(function () {
    $('#editPasswordForm').validate({
        rules: {
            inputPasswordLama: {
                required: true,
            },
            inputPasswordBaru: {
                required: true,
            },
            inputPasswordKonfirmasi: {
                required: true,
            },
        },
        messages: {
            inputPasswordLama: {
                required: "Password lama harus diisi",
            },
            inputPasswordBaru: {
                required: "Password baru harus diisi",
            },
            inputPasswordKonfirmasi: {
                required: "Password konfirmasi harus diisi",
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