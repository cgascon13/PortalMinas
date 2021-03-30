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
        if ($('#btnLogin').text() == 'Ingresar') {
            RedirectUrl();
        }
        else {
            $.post(ROOT + 'Login/VerifyData', {
                usuario: $('#usuario').val(),
                password: $('#password').val()
            }, "json").done(function (data) {
                if (typeof (data) === 'object') {                    
                    if (data.length == 1) {
                        $('#empresa').append(
                            $('<option value="' + data[0].Id + '">' + data[0].NomEmpresa + '</option>')
                        );
                        RedirectUrl();
                    }
                    else if (data.length > 1) {
                        $('#btnLogin').text('Ingresar').prop('disabled', false);
                        $('.div-empresa').css('display', 'block');
                        $.each(data, function (index, item) {
                            $('#empresa').append(
                                $('<option value="' + item.Id + '">' + item.NomEmpresa + '</option>')
                            );
                        });
                        $('#usuario, #password').prop('readonly', true);
                    }
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

    function RedirectUrl() {
        if ($('#usuario').val() == '' || $('#password').val() == '') {
            $('#btnLogin').prop('disabled', false);
            return false;
        }

        $.post(ROOT + 'Login/Login', {
            usuario: $('#usuario').val(),
            password: $('#password').val(),
            empresa: $('#empresa').text()
        }, "json").done(function (data) {
            if (data.indexOf('error: ') != -1) {
                console.log("ErrorLogin: " + data);
                alert(data);
            }
            else { //Todo OK
                window.location.href = window.location.protocol + "//" + window.location.host + ROOT + data;
            }
        }).fail(function (data) {
            console.log("ErrorLogin: " + data);
            console.log("ErrorLogin: " + data.statusText);
            alert('Ocurrió un error');
        });
    }

    $('#usuario').focus();
}) //FIN DEL READY