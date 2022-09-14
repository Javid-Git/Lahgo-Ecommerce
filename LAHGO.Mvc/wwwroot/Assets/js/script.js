
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
   
    $(document).on('click', '.slider-button-next', function () {
        $('.slider-button-prev').addClass('show-slider-button');
        $('.slider-button-next').addClass('hide-slider-button');
    })
    $(document).on('click', '.slider-button-prev', function () {
        $('.slider-button-next').removeClass('hide-slider-button');
        $('.slider-button-prev').removeClass('show-slider-button');

    })
    //$(document).on('click', '.dropdowns', function (e) {
    //    console.log($(this).children('.lists').toggleClass('dropdowns-visible'))
    //})
    
    $(document).on('click', ('#minicart-close', '.minicart-wraper'), function () {
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')
    })
    $(document).on('click', '#minicart-close', function () {
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')
    })
    $(document).on('click', '#mobile-menu-trigger', function () {
        $('.sidebar').toggleClass('show-sidebar')
        $('.sidebar-wraper').toggleClass('show-sidebar')
    })
    $(document).on('click', '.sidebar-wraper', function () {
        $('.sidebar').toggleClass('show-sidebar')
        $('.sidebar-wraper').toggleClass('show-sidebar')
    })

    $(document).on('click', '#mobile-sort', function () {
        $('.mobile-sort-list').toggleClass('show-mobile-sort')
    })
    $(document).on('click', '#mobile-filter', function () {
        $('.mobile-filter-big').toggleClass('show-mobile-filter')
        $('.mobile-sorting').toggleClass('mobile-sorting-top')
    })
    $(document).on('click', '#cancel-filtering, #apply-filtering', function () {
        $('.mobile-filter-big').toggleClass('show-mobile-filter')
        $('.mobile-sorting').toggleClass('mobile-sorting-top')
    })

    
    $(document).on('click', '.detail-size-container', function (e) {
        let size = $(this).attr('value');
        console.log(size);
        $('.selected-size').empty();
        $('.selected-size').append(size);
    })
    
    
    $(document).on('click', '.small-colors-optional', function (e) {
        let size = $(this).attr('value');
        console.log(size);
        $('.selected-color').empty();
        $('.selected-color').append(size);
    })

    $(document).on('click', '.small-colors-optional', function (e) {
        $('.small-colors-optional').removeClass('active-selection');
        $(this).addClass('active-selection');
    })

    $(document).on('click', '.detail-size-container', function (e) {
        $('.detail-size-container').removeClass('active-selection');
        $(this).addClass('active-selection');
    })

    $(document).on({
        mouseenter: function () {
            $('.tooltip-content').addClass('show-tooltip-content')
        },
        mouseleave: function () {
            $('.tooltip-content').removeClass('show-tooltip-content')

        }
    }, ".svg-tooltip");

    $(".slide-toggle").click(function (e) {
        $(this).children('.toggle-list').slideToggle(400, function () {
            // Animation complete.
        });
    });

    $(document).on('click','.popup-pic', function(e){
        let picsrc = $(this).children('img').attr('src');
        $('.popup-wraper').css('background-image', 'url(' + picsrc + ')');
    })

    $(document).on('click', '.add-pic', function (e) {
        let bcpic = $(this).children('img').attr('src')
        $('.popup-wraper').css('background-image', 'url(' + bcpic + ')');
        $('.popup-pic-list').toggle();
        console.log(bcpic)
    })
    $(document).on('click', '#product-images-popup-close, .popup-modal', function () {
        $('.popup-pic-list').toggle();
    })

   

    $(document).on('click', '.mobile-product-list', function () {
        $('.checkout1-right-mobile').slideToggle();
    })

    $(document).on('click', '.contact1-ordering', function (e) {
        $(this).children('.faq-slider-list').slideToggle();
        $(this).children('.slider-main-button').children('.contact-plus-button').toggleClass('contact-mult-button');
    })
    

    $(document).on('click', '.help-faqs, .faq-btn', function () {
        $('.contact1-middle').hide();
        $('.contact1-right').hide();
        $('.contact1-faqs').show();

    })
    
    $(document).on('click', '.help-contact, .contact-btn', function () {
        $('.contact1-middle').show();
        $('.contact1-right').show();
        $('.contact1-faqs').hide();

    })

    $(document).on('click', '.account-options-button', function (e) {
        $('.account-options-button').children('a').removeClass('active-link');

        $(this).children('a').addClass('active-link');

    })

    $(document).on('click', '.account-option-button', function () {
        $('.account-option-button').next().toggle();
    })

    $(document).on('click', '.account-details', function () {
        $('.account1-middle').hide();
        $('#account-details').show();
    })

    $(document).on('click', '.account-option', function () {
        $('.account1-middle').show();
        $('#account-details').hide();
    })

    $(document).on('click', '.account-option-mobile', function () {
        $(this).parent().toggle();
    })
    $(document).on('click', '.header-search-pane-trigger', function () {
        $('.search-mobile-container').toggleClass('search-mobile-container-visible')
        $('svg-search-mobile').toggle();
        $('svg-close-mobile').toggle();
    })

    ///////////////////////////////////////////////  Basket   ///////////////////////////////////////////////


    $(document).on('click', '.item-quantity-decrease', function (e) {
        e.preventDefault();
        let inputCount = $(this).next().val();

        if (inputCount >= 2) {
            inputCount--;
            $(this).next().val(inputCount);
            let url = $(this).attr('href') + '/?count=' + inputCount;
            console.log('sub');
            fetch(url)
                .then(res => res.text())
                .then(data => {
                    $('').html(data);
                    fetch('/basket/openbasket')
                        .then(res => res.text())
                        .then(data => {
                            $('.minicart').html(data);
                            fetch('/cart/updatesummary')
                                .then(res => res.text())
                                .then(data => {
                                    $('.cart-right').html(data);
                                });
                        });
                });
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
        let url = $(this).attr('href') + '/?count=' + inputCount;

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('').html(data);
                fetch('/basket/openbasket')
                    .then(res => res.text())
                    .then(data => {
                        $('.minicart').html(data);
                        fetch('/cart/updatesummary')
                            .then(res => res.text())
                            .then(data => {
                                $('.cart-right').html(data);
                            });
                    });
            });


    })

    $(document).on('click', '.deletefromcart', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.minicart').html(data);
                fetch('/basket/DeleteUpdate')
                    .then(res => res.json())
                    .then(data => {
                        $('.notification').html(data);
                    });
            });
    })

    ///////////////////////////////////////////////   Minicart   ///////////////////////////////////////////////

    $(document).on('click', '.minicart-button', function (e) {
        e.preventDefault();
        $('#minicart').toggleClass('minicart-visible')
        $('.minicart-wraper').toggleClass('minicart-visible')
        let url = $(this).attr('href');

        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.minicart').html(data);
            });

    })

    $(document).on('click', '.deletefrombasket', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.cart-products').html(data);
                fetch('/basket/DeleteUpdate')
                    .then(res => res.json())
                    .then(data => {
                        $('.notification').html(data);
                        fetch('/cart/updatesummary')
                            .then(res => res.text())
                            .then(data => {
                                $('.cart-right').html(data);
                            });
                    });
            });
    })

    ///////////////////////////////////////////////   Get sizes   ///////////////////////////////////////////////

    $(document).on('click', '.smcolor-container', function (e) {
        e.preventDefault();

        let all_rad_input = $('.smcolor-container').prev();
        $('.radio-select').removeClass('radio-select')
        let url = $(this).attr('href');
        let rad_input = $(this).prev();

        if (rad_input.prop('checked', true)) {
            $(this).addClass('radio-select')
        }
        let color = $(this).attr('data-name');
        $('.selected-color').empty();
        $('.selected-color').append(color);

        $('.size-selection-container').empty();
        $('.detail-sizes-container').empty();
        fetch(url)
            .then(response => response.text())
            .then(data => {
                $($(this).parent().siblings('.quick-add').children([2])).append(data)
                fetch(url)
                    .then(response => response.text())
                    .then(data => {
                        $('.detail-sizes-container').append(data)

                    })
            })
    })
    
    
    /////////////////////////////////////////////   Search   /////////////////////////////////////////////

    $('.search-input').keyup(function () {
        let value = $(this).val();
        console.log(value);

        let url = $(this).data("url");
        url = '/product/search/?search=' + value.trim();
        console.log(url);

        if (value) {
            fetch(url)
                .then(response => response.text())

                .then(data => {
                    $('.search-body').html(data)

                })
        }
        else {
            $('.search-body').html('')
        }

    })

    $(document).on('click', '.accordion-toggle', function (e) {
        $(this).next().children([1]).children([1]).slideToggle();
    })

    ///////////////////////////////////////////////   Loaders   /////////////////////////////////////////////

    $(window).on("load", function () {
        setTimeout(function () {
            $(".loader").fadeOut("slow");
        }, 2000);
        
    });
    $(window).on("load", function () {
        setTimeout(function () {
            $(".loader-div").fadeOut("slow");
        }, 1000);

    });
   

    ///////////////////////////////////////////////   Filters   /////////////////////////////////////////////

    $(document).on('submit', '#filter', function (e) {
        e.preventDefault();

        const form = document.getElementById('filter');
        const payload = new FormData(form);

        fetch('/shop/filter', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.text())
            .then(data => {
                $('.products').html(data);
            });
    })

    $(document).on('submit', '#loadmore', function (e) {
        e.preventDefault();

        const form = document.getElementById('loadmore');
        const payload = new FormData(form);

        fetch('/shop/filter', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.text())
            .then(data => {
                $('.products').html(data);
            });
    })

    $(document).on('submit', '#filtermobile', function (e) {
        e.preventDefault();

        const form = document.getElementById('filtermobile');
        const payload = new FormData(form);

        fetch('/shop/filter', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.text())
            .then(data => {
                $('.products').html(data);
            });
    })

    $(document).on('click', '.size-option-select', function (e) {
        $(this).attr('selected', 'selected')
    })

    $(document).on('change', '.color-filter', function (e) {
        var color = $(this).find('option:selected').data('color')
        console.log(color)
        $(this).css('background-color', color)
    })

    ///////////////////////////////////////////////   Toaster   /////////////////////////////////////////////

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": true,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    /////////////////////////////////////////////   Add to basket   /////////////////////////////////////////////

    $(document).on('submit', '.basket-add-form', function (e) {
        e.preventDefault();

        const form = e.target;
        console.log(form)


        let size_id = $('.sizes').find("input[type='radio']:checked").val();
        if (size_id == undefined) {
            $(function () {
                toastr.error("Size should be selected", "Error")
            })
        }
        console.log(size_id)
       

        const payload = new FormData(form);

        fetch('/basket/addtobasket', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.json())
            .then(data => {
                $('.notification').html(data);

            })
            .then(function () {
                if (size_id != undefined) {
                    toastr.success("Product was successfully added to basket ", "Done")
                }
            });
    })

    $(document).on('submit', '#addtobasket-detail', function (e) {
        e.preventDefault();

        const form = e.target;
        console.log(form)


        let size_id = $('.detail-sizes-container').find("input[type='radio']:checked").val();
        if (size_id == undefined) {
            $(function () {
                toastr.error("Size should be selected", "Error")
            })
        }
        console.log(size_id)


        const payload = new FormData(form);

        fetch('/basket/addtobasket', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.json())
            .then(data => {
                $('.notification').html(data);

            })
            .then(function () {
                if (size_id != undefined) {
                    toastr.success("Product was successfully added to basket ", "Done")
                }
            });
    })

    $(document).on('click', '.size-label', function (e) {
        $(this).prev().trigger('click')
        $('.size-label').removeClass('size-label-selected');
        if ($(this).prev().attr('checked', true)) {
            $(this).addClass('size-label-selected')
        }
    })  

   
    ///////////////////////////////////////////////   Slick Sliders    ///////////////////////////////////////////////

    $('.detail1-left-mobile').slick({
        dots: false,
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
                    dots: false
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
})
     

