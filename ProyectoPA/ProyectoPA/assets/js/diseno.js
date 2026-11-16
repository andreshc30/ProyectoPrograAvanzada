document.addEventListener('DOMContentLoaded', () => {

    const IVA = 0.13;
    const TAMANO_MAXIMO = 4 * 1024 * 1024;

    const formulario = document.getElementById('frmDiseno');
    const selectMaterial = document.getElementById('categorias');
    const selectTamano = document.getElementById('tamano');
    const inputImagen = document.getElementById('inputImagenPlaca');
    const imgPreview = document.getElementById('imgPreviewPlaca');
    const btnAgregarCarrito = document.getElementById('btnAgregarCarrito');

    const resumenMaterial = document.getElementById('resumenMaterial');
    const resumenTamano = document.getElementById('resumenTamano');
    const resumenSubtotal = document.getElementById('resumenSubtotal');
    const resumenImpuestos = document.getElementById('resumenImpuestos');
    const resumenTotal = document.getElementById('resumenTotal');
    const inputMensaje = document.getElementById('mensajePlaca');
    const previewMensaje = document.getElementById('previewMensaje');

    function formatearPrecio(valor) {
        return '₡' + Math.round(valor).toLocaleString('es-CR');
    }

    function opcionSeleccionada(select) {
        return select.value ? select.options[select.selectedIndex] : null;
    }

    function actualizarResumen() {
        const material = opcionSeleccionada(selectMaterial);
        const tamano = opcionSeleccionada(selectTamano);

        const precioMaterial = material ? Number(material.dataset.precio) : 0;
        const precioTamano = tamano ? Number(tamano.dataset.precio) : 0;

        resumenMaterial.textContent = material
            ? `${material.dataset.nombre} - ${formatearPrecio(precioMaterial)}`
            : 'Sin seleccionar';

        resumenTamano.textContent = tamano
            ? `${tamano.dataset.nombre} - ${formatearPrecio(precioTamano)}`
            : 'Sin seleccionar';

        const subtotal = precioMaterial + precioTamano;
        const impuestos = subtotal * IVA;
        const total = subtotal + impuestos;

        resumenSubtotal.textContent = formatearPrecio(subtotal);
        resumenImpuestos.textContent = formatearPrecio(impuestos);
        resumenTotal.textContent = formatearPrecio(total);

        btnAgregarCarrito.disabled = !(material && tamano);
    }

    function actualizarMensaje() {
        const texto = inputMensaje.value.trim();

        if (texto === '') {
            previewMensaje.textContent = 'Su mensaje aparecerá aquí.';
            previewMensaje.classList.add('cdp-disenar-placeholder', 'text-secondary');
        } else {
            previewMensaje.textContent = texto;
            previewMensaje.classList.remove('cdp-disenar-placeholder', 'text-secondary');
        }
    }

    selectMaterial.addEventListener('change', actualizarResumen);
    selectTamano.addEventListener('change', actualizarResumen);
    inputMensaje.addEventListener('input', actualizarMensaje);

    inputImagen.addEventListener('change', () => {
        const archivo = inputImagen.files[0];

        if (!archivo) {
            imgPreview.src = imgPreview.dataset.defecto;
            return;
        }

        if (!archivo.type.startsWith('image/')) {
            Swal.fire('Archivo inválido', 'El archivo seleccionado no es una imagen.', 'error');
            inputImagen.value = '';
            imgPreview.src = imgPreview.dataset.defecto;
            return;
        }

        if (archivo.size > TAMANO_MAXIMO) {
            Swal.fire('Archivo muy grande', 'La imagen no debe superar los 4 MB.', 'warning');
            inputImagen.value = '';
            imgPreview.src = imgPreview.dataset.defecto;
            return;
        }

        const lector = new FileReader();
        lector.onload = (evento) => {
            imgPreview.src = evento.target.result;
        };
        lector.readAsDataURL(archivo);
    });

    formulario.addEventListener('submit', (evento) => {
        if (!selectMaterial.value || !selectTamano.value || !inputMensaje.value.trim()) {
            evento.preventDefault();
            Swal.fire('Campos incompletos', 'Debe completar todos los campos antes de continuar.', 'warning');
        }
    });

    actualizarResumen();
    actualizarMensaje();

});
