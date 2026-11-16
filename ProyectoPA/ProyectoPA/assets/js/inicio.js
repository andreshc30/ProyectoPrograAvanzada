document.addEventListener('DOMContentLoaded', function () {


  const carouselEl = document.getElementById('cdpCarruselPlacas');
  if (carouselEl && window.bootstrap) {
    const carousel = window.bootstrap.Carousel.getOrCreateInstance(carouselEl, {
      interval: 5500,
      pause: false,
      touch: true
    });

    carouselEl.addEventListener('mouseenter', () => carousel.pause());
    carouselEl.addEventListener('mouseleave', () => carousel.cycle());
  }

  const gridDestacados = document.getElementById('cdpGridDestacados');
  const destacadosVacio = document.getElementById('cdpDestacadosVacio');

  if (gridDestacados) {
    cargarDestacados();
  }

  function formatearPrecio(precio) {
    return '₡' + Number(precio).toLocaleString('es-CR');
  }

  function rutaImagen(nombreImagen) {
    return nombreImagen ? ROOT_URL + 'uploads/' + nombreImagen : '';
  }

  function cargarDestacados() {
    fetch(`${BASE_URL}Home/ApiDestacados`)
      .then((res) => res.json())
      .then((productos) => {
        if (!productos.length) {
          destacadosVacio.classList.remove('d-none');
          return;
        }
        renderizarDestacados(productos);
      })
      .catch((error) => {
        console.error('Error cargando destacados:', error);
        destacadosVacio.textContent = 'Ocurrió un error cargando los destacados.';
        destacadosVacio.classList.remove('d-none');
      });
  }

  function renderizarDestacados(productos) {
    gridDestacados.innerHTML = '';

    productos.forEach((producto) => {
      const precio = formatearPrecio(producto.precio);
      const imagen = rutaImagen(producto.imagen_nombre);

      const col = document.createElement('div');
      col.className = 'col';

      col.innerHTML = `
        <div class="bg-white h-100 d-flex flex-column" style="border: 1px solid #D4AF37;">
            <img src="${imagen}" class="img-fluid w-100" alt="${producto.nombre}" style="height: 220px; object-fit: cover;">
            <div class="p-3 d-flex flex-column flex-grow-1">
                <h5 class="mb-3">${producto.nombre}</h5>
                <div class="d-flex justify-content-between align-items-center mt-auto">
                    <span class="text-primary font-work-sans fs-5">${precio}</span>
                    <a href="${BASE_URL}Catalogo" class="btn btn-sm-square btn-outline-dark" title="Ver en catálogo">
                        <i class="bi bi-eye"></i>
                    </a>
                </div>
            </div>
        </div>
      `;

      gridDestacados.appendChild(col);
    });
  }

});
