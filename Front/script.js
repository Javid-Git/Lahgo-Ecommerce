$(document).ready(function () {
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 10) {
            $('#center-header').addClass('center-header-sticky');   
            $('.sidebar').addClass('sidebar-long');   
            
        } else {
            $('#center-header').removeClass('center-header-sticky');
            $('.sidebar').removeClass('sidebar-long');   

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
                breakpoint: 576,
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
    $(document).on('click', '.minicart-button', function(e){
        e.preventDefault();
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')

    })
    $(document).on('click', ('#minicart-close', '.minicart-wraper'), function(){
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')
    })
    $(document).on('click', '#minicart-close', function(){
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')
    })
    $(document).on('click', '#mobile-menu-trigger', function(){
        $('.sidebar').toggleClass('show-sidebar')
        $('.sidebar-wraper').toggleClass('show-sidebar')
    })
    $(document).on('click','.sidebar-wraper', function(){
        $('.sidebar').toggleClass('show-sidebar')
        $('.sidebar-wraper').toggleClass('show-sidebar')
    })
    $(document).on('click', '.item-quantity-decrease', function (e) {
        e.preventDefault();
        let inputCount = $(this).next().val();

        if (inputCount >= 2) {
            inputCount--;
            $(this).next().val(inputCount);
        }
    })

    $(document).on('click', '.item-quantity-increase', function (e) {
        e.preventDefault();
        let inputCount = $(this).prev().val();

        if (inputCount > 0) {
            inputCount++;
        } else {
            inputCount = 1;
        }
        $(this).prev().val(inputCount);
    })
    $(document).on('click','#mobile-sort', function(){
        $('.mobile-sort-list').toggleClass('show-mobile-sort')
    })
    $(document).on('click','#mobile-filter', function(){
        $('.mobile-filter-big').toggleClass('show-mobile-filter')
        $('.mobile-sorting').toggleClass('mobile-sorting-top')
    })
    $(document).on('click','#cancel-filtering', function(){
        $('.mobile-filter-big').toggleClass('show-mobile-filter')
        $('.mobile-sorting').toggleClass('mobile-sorting-top')
    })

    $('.about4-items').slick({
        dots: true,
        infinite: true,
        arrows: false,
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
                breakpoint: 576,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
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
})

