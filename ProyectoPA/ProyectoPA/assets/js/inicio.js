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

});
