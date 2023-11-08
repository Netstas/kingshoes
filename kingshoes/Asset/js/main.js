$(".slick-carousel").slick({
    infinite: true,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: true,
    dots: true,
    prevArrow: '<button class="prev"><i class="fa-solid fa-chevron-left"></i></button>',
    nextArrow: '<button class="next"><i class="fa-solid fa-chevron-right"></i></button>',
});
$(".carousel").slick({
    infinite: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    arrows: true,
    dots: false,
    prevArrow: '<button class="prev shared"><i class="fa-solid fa-chevron-left"></i></button>',
    nextArrow: '<button class="next shared"><i class="fa-solid fa-chevron-right"></i></button>',
});
$(".carousel-items").slick({
    infinite: true,
    slidesToShow: 5,
    slidesToScroll: 1,
    arrows: false,
    dots: false,
    draggable: true
});