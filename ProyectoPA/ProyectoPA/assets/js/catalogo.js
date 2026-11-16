document.addEventListener('DOMContentLoaded', function () {

  const buscador = document.getElementById('cdpBuscarPlaca');
  const filtrosContenedor = document.getElementById('cdpFiltros');
  const grid = document.getElementById('cdpGridProductos');
  const sinResultados = document.getElementById('cdpSinResultados');
  const cargando = document.getElementById('cdpCargando');

  let productosData = [];
  let filtroActivo = 'todos';

  function renderizarProductos(lista) {
    grid.innerHTML = '';

    lista.forEach((producto) => {
      const precio = '₡' + Number(producto.precio).toLocaleString('es-CR');
      const imagen = producto.imagen_nombre ? ROOT_URL + 'uploads/' + producto.imagen_nombre : '';

      const col = document.createElement('div');
      col.className = 'col cdp-producto';
      col.dataset.material = producto.material;
      col.dataset.nombre = producto.nombre;
      col.dataset.precio = precio;
      col.dataset.imagen = imagen;
      col.dataset.descripcion = producto.descripcion;

      col.innerHTML = `
        <div class="bg-white h-100 d-flex flex-column" style="border: 1px solid #D4AF37;">
            <img src="${imagen}" class="img-fluid w-100" alt="${producto.nombre}" style="height: 220px; object-fit: cover;">
            <div class="p-3 d-flex flex-column flex-grow-1">
                <h5 class="mb-3">${producto.nombre}</h5>
                <div class="d-flex justify-content-between align-items-center mt-auto">
                    <span class="text-primary font-work-sans fs-5">${precio}</span>
                    <div class="d-flex gap-2">
                        <button class="btn btn-sm-square btn-outline-dark"
                            title="Ver detalle"
                            data-bs-toggle="modal"
                            data-bs-target="#cdpModalDetalle">
                            <i class="bi bi-eye"></i>
                        </button>
                        <form action="${BASE_URL}Carrito/Agregar/${producto.id}" method="POST">
                            <button type="submit"
                                class="btn btn-sm-square btn-outline-dark"
                                title="Agregar al carrito">
                                <i class="bi bi-cart-plus"></i>
                            </button>
                        </form>
                    </div>
                </div>
            </div>
        </div>
      `;

      grid.appendChild(col);
    });
  }

  function renderizarFiltros() {
    const materiales = [...new Set(productosData.map((p) => p.material))];

    materiales.forEach((material) => {
      const btn = document.createElement('button');
      btn.type = 'button';
      btn.className = 'btn btn-sm btn-outline-dark text-uppercase cdp-filtro';
      btn.dataset.filtro = material;
      btn.textContent = material.charAt(0).toUpperCase() + material.slice(1);
      filtrosContenedor.appendChild(btn);
    });

    inicializarFiltros();
  }

  function aplicarFiltro() {
    const texto = buscador.value.trim().toLowerCase();

    const filtrados = productosData.filter((producto) => {
      const coincideMaterial = (filtroActivo === 'todos' || producto.material === filtroActivo);
      const coincideTexto = producto.nombre.toLowerCase().includes(texto);
      return coincideMaterial && coincideTexto;
    });

    renderizarProductos(filtrados);
    sinResultados.classList.toggle('d-none', filtrados.length > 0);
  }

  function inicializarFiltros() {
    const filtros = document.querySelectorAll('.cdp-filtro');

    filtros.forEach((btn) => {
      btn.addEventListener('click', () => {
        filtros.forEach((b) => b.classList.remove('active'));
        btn.classList.add('active');
        filtroActivo = btn.dataset.filtro;
        aplicarFiltro();
      });
    });
  }

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

  fetch(`${BASE_URL}Catalogo/ApiList`)
    .then((res) => res.json())
    .then((data) => {
      productosData = data;
      if (cargando) cargando.classList.add('d-none');
      renderizarProductos(productosData);
      renderizarFiltros();
    })
    .catch((error) => {
      console.error('Error cargando el catálogo:', error);
      if (cargando) cargando.textContent = 'Ocurrió un error cargando el catálogo.';
    });

});
