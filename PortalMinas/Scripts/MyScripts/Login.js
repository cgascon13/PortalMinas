$(document).ready(function () {
    $('#usuario').keyup(function (e) {
        if (e.which == 13)
            $('#password').focus();
    });
    $('#password').keyup(function (e) {       
        if (e.which == 13)
            $('#btnLogin').click();
    });

    $('#btnLogin').click(function () {
        console.log($('#usuario').val());
        if ($('#usuario').val() == '' && $('#password').val() == '')
            return false;
        else {
            if ($('#usuario').val() == '') {
                alert('Ingrese un usuario');
                return false;
            }
            if ($('#password').val() == '') {
                alert('Ingrese una contraseña');
                return false;
            }
        }
        iniciaSession();
    });

    let iniciaSession = () => {
        let user = document.getElementById("usuario").value;
        let pass = document.getElementById("password").value;

        if (user == "" || pass == "") {
            $('#btnLogin').prop('disabled', false);
            return false;
        }
        if ($('#btnLogin').text() == 'Ingresar') {
            console.log($('#empresa').text());
            $.post(ROOT + 'Login/Login', {
                usuario: user,
                password: pass,
                empresa: $('#empresa').text()
            }, "json").done(function (data) {
                if (data.indexOf('error: ') != -1) {
                    alert(data);
                }
                else {
                    window.location.href = window.location.protocol + "//" + window.location.host + ROOT + data;
                }
            }).fail(function (data) {
                console.log("ErrorLogin: " + data);
                console.log("ErrorLogin: " + data.statusText);
                alert('Ocurrió un error');
            });
        }
        else {
            $.post(ROOT + 'Login/VerifyData', {
                usuario: user,
                password: pass
            }, "json").done(function (data) {
                if (typeof (data) === 'object') {
                    $('.div-empresa').css('display', 'block');
                    $('#btnLogin').text('Ingresar').prop('disabled', false);
                    $.each(data, function (index, item) {
                        $('#empresa').append(
                            $('<option value="' + item.Id + '">' + item.NomEmpresa + '</option>')
                        );
                    });
                    $('#usuario, #password').prop('readonly', true);
                }
                else {
                    alert(data);
                }
            }).fail(function (data) {
                console.log("ErrorLogin: " + data);
                console.log("ErrorLogin: " + data.statusText);
                alert('Ocurrió un error');
            });
        }
    }

    $('#usuario').focus();
}) //FIN DEL READY