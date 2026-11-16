document.addEventListener('DOMContentLoaded', function () {

  const buscador = document.getElementById('cdpBuscarPlaca');
  const sinResultados = document.getElementById('cdpSinResultados');
  const productos = document.querySelectorAll('#cdpGridProductos .cdp-producto');
  const filtros = document.querySelectorAll('.cdp-filtro');

  let filtroActivo = 'todos';

  function aplicarFiltro() {
    const texto = buscador.value.trim().toLowerCase();
    let visibles = 0;

    productos.forEach((tarjeta) => {
      const coincideMaterial = (filtroActivo === 'todos' || tarjeta.dataset.material === filtroActivo);
      const coincideTexto = tarjeta.dataset.nombre.toLowerCase().includes(texto);
      const mostrar = coincideMaterial && coincideTexto;

      tarjeta.classList.toggle('d-none', !mostrar);
      if (mostrar) visibles++;
    });

    sinResultados.classList.toggle('d-none', visibles > 0);
  }

  filtros.forEach((btn) => {
    btn.addEventListener('click', () => {
      filtros.forEach((b) => b.classList.remove('active'));
      btn.classList.add('active');
      filtroActivo = btn.dataset.filtro;
      aplicarFiltro();
    });
  });

  if (buscador) {
    buscador.addEventListener('input', aplicarFiltro);
  }

  const modalDetalle = document.getElementById('cdpModalDetalle');

  if (modalDetalle) {
    modalDetalle.addEventListener('show.bs.modal', (event) => {
      const boton = event.relatedTarget;
      const tarjeta = boton.closest('.cdp-producto');

      document.getElementById('cdpModalImagen').src = tarjeta.dataset.imagen;
      document.getElementById('cdpModalImagen').alt = tarjeta.dataset.nombre;
      document.getElementById('cdpModalNombre').textContent = tarjeta.dataset.nombre;
      document.getElementById('cdpModalPrecio').textContent = tarjeta.dataset.precio;
      document.getElementById('cdpModalDescripcion').textContent = tarjeta.dataset.descripcion;
    });
  }

});
