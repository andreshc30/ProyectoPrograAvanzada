$(function () {

    $('#recuperarForm').validate({
        rules: {
            Cedula: { required: true }
        },
        messages: {
            Cedula: { required: 'La cédula es obligatoria' }
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