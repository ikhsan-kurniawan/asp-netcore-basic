//(function cekToken() {
//    if (localStorage.getItem('userToken')) {
//        // Redirect ke dashboard jika token ada
//        window.location.href = 'dashboard'; // Ganti dengan URL dashboard yang sesuai
//    }
//})();


function Login() {

    var account = new Object();
    account.emailOrNik = $("#inputEmail").val();
    account.password = $("#inputPassword").val();

    $.ajax({
        type: "POST",
        url: "https://localhost:7180/accounts/login",
        data: JSON.stringify(account),
        contentType: "application/json; charset=UTF-8",
    }).done(function (hasil) {

        // SET LOCAL STORAGE
        localStorage.setItem('userToken', hasil.data.token);
        let payload = parseJwt(hasil.data.token);
        localStorage.setItem('username', payload.Username);
        localStorage.setItem('nik', payload.NIK);
        localStorage.setItem('roles', JSON.stringify(payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']));
        localStorage.setItem('message', "Selamat datang " + payload.Username);


        // SET COOKIES ATAU APALAH DI ASP NET CORE CONTROLLER
        let roles = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
        // Pastikan roles selalu berupa array
        if (typeof roles === 'string') {
            roles = [roles];
        } else if (!Array.isArray(roles)) {
            roles = [];
        }
        let session = {
            nik: payload.NIK,
            nama: payload.Username,
            roles: roles
        }

        $.ajax({
            type: "POST",
            url: "https://localhost:7170/Session/SetSession",
            data: JSON.stringify(session),
            contentType: "application/json; charset=UTF-8",
        }).done((result) => {
            console.log(result);
            Swal.fire({
                position: "center",
                icon: "success",
                title: hasil.message,
                showConfirmButton: false,
                timer: 1500
            }).then(() => {
                if (localStorage.getItem('returnUrl')) {
                    window.location.href = localStorage.getItem("returnUrl");
                    localStorage.removeItem("returnUrl");
                } else {
                    window.location.href = _REDIRECTURL;
                }
            });
        }).fail((err) => {
            console.log(err)
        });


    }).fail(function (jqXHR) {
        Swal.fire({
            position: "center",
            icon: "error",
            title: jqXHR.responseJSON.message,
            showConfirmButton: false,
            timer: 1500
        });
    });


}

function Logout() {
    localStorage.clear();

    $.ajax({
        type: "POST",
        url: "https://localhost:7170/Session/RemoveSession",
        contentType: "application/json; charset=UTF-8",
    }).done((hasil) => {
        console.log(hasil);
        window.location.href = "https://localhost:7170/login";
    }).fail((err) => {
        console.log(err)
    });

}

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}