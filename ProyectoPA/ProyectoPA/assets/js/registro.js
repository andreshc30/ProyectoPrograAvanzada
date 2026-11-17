$(function () {

    $('#registroForm').validate({
        rules: {
            Nombre: { required: true },
            PrimerApellido: { required: true },
            SegundoApellido: { required: true },
            Cedula: { required: true },
            Correo: { required: true, email: true },
            Contrasena: { required: true, minlength: 8 }
        },
        messages: {
            Nombre: { required: 'El nombre es obligatorio' },
            PrimerApellido: { required: 'El primer apellido es obligatorio' },
            SegundoApellido: { required: 'El segundo apellido es obligatorio' },
            Cedula: { required: 'La cédula es obligatoria' },
            Correo: {
                required: 'El correo electrónico es obligatorio',
                email: 'Ingrese un correo electrónico válido'
            },
            Contrasena: {
                required: 'La contraseña es obligatoria',
                minlength: 'Debe tener al menos 8 caracteres'
            }
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