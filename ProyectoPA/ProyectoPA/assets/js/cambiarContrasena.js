$(function () {

    $('#cambiarContrasenaForm').validate({
        rules: {
            Contrasena: { required: true, minlength: 8 },
            ConfirmarContrasena: { required: true, equalTo: '#Contrasena' }
        },
        messages: {
            Contrasena: {
                required: 'La contraseña es obligatoria',
                minlength: 'Debe tener al menos 8 caracteres'
            },
            ConfirmarContrasena: { required: 'Debe confirmar la contraseña', equalTo: 'Las contraseñas no coinciden' }
        },
        errorElement: 'div',
        errorClass: 'login-error-message',
        errorPlacement: function (error, element) {
            var grupo = element.closest('.campo-clave');
            error.insertAfter(grupo.length ? grupo : element);
        },
        highlight: function (element) {
            $(element).addClass('login-input-invalid');
        },
        unhighlight: function (element) {
            $(element).removeClass('login-input-invalid');
        }
    });

});