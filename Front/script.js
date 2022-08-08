$(document).ready(function () {
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 10) {
            $('#center-header').addClass('center-header-sticky');
        } else {
            $('#center-header').removeClass('center-header-sticky');
        }
    });

    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 100) {
            $('.button-container').addClass('button-container-scroll');
        } else {
            $('.button-container').removeClass('button-container-scroll');
        }
    });
    $('.categories').slick({
        dots: false,
        infinite: false,
        arrows: true,
        nextArrow: '<button class="slider-button-next"></button>',
        prevArrow: '<button class="slider-button-prev"></button>',
        speed: 500,
        slidesToShow: 4,
        slidesToScroll: 4,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });
    $('.favorites').slick({
        dots: false,
        infinite: true,
        arrows: true,
        nextArrow: '<button class="favorites-button-next"></button>',
        prevArrow: '<button class="favorites-button-prev"></button>',
        speed: 500,
        slidesToShow: 4,
        slidesToScroll: 4,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });
    $('.section4-slider').slick({
        dots: true,
        infinite: true,
        arrows: false,
        speed: 500,
        slidesToShow: 1,
        slidesToScroll: 1,
        responsive: [
            {
                breakpoint: 1024,
                settings: {
                    slidesToShow: 3,
                    slidesToScroll: 3,
                    infinite: true,
                    dots: true
                }
            },
            {
                breakpoint: 600,
                settings: {
                    slidesToShow: 2,
                    slidesToScroll: 2
                }
            },
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
            // You can unslick at a given breakpoint now by adding:
            // settings: "unslick"
            // instead of a settings object
        ]
    });
    $(document).on('click', '.slider-button-next', function(){
        $('.slider-button-prev').addClass('show-slider-button');
        $('.slider-button-next').addClass('hide-slider-button');
    })
    $(document).on('click', '.slider-button-prev', function(){
        $('.slider-button-next').removeClass('hide-slider-button');
        $('.slider-button-prev').removeClass('show-slider-button');

    })
    $(document).on('click', '.dropdowns', function(e){
        console.log($(this).children('.lists').toggleClass('dropdowns-visible'))
    })
    
})

