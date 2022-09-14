$(document).ready(function () {
    $('.validation-summary-errors ul li').css('padding-bottom', '1rem');
    $('.validation-summary-errors ul').css('padding', '0');
    $('.main-upload, .detail-upload').css('border', '1px dashed black');
    $('.main-upload, .detail-upload').css('background-color', '#e2e2e2');
    $('.custom-input-container').css('border', '1px dashed black');


    $(document).on('click', '.addinputs', function (e) {
        $('.new-input-container').css('padding', '1rem 0');
        $('.new-input-container').css('margin', '1rem 0');
        $('.new-input-container').css('border', '1px dashed black');

        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.inputsContainer').append(data)
            })
    });
    
    $(document).on('click', '.unit-update', function (e) {
        e.preventDefault();

        let No = $(this).attr('data-value');

        let id = $('.unit-list-container.'+No).attr('data-value');

        if (id == No) {

            fetch($(this).attr('href'))
                .then(res => res.text())
                .then(data => {
                    $('.unit-list-container.'+No).append(data)
                })
        }
        
    });
    $(document).on('submit', '#update-sub', function (e) {
        e.preventDefault();

        const form = document.getElementById('update-sub');
        const payload = new FormData(form);

        fetch('/Manage/Product/UpdateUnitPost', {
            method: 'POST',
            body: payload,
        })
            .then(res => res.text())
            .then(data => {
                $('.product-options-container').html(data);
            });
    })

    //$(document).on('click', '.unit-submit', function (e) {
    //    e.preventDefault();

    //    $('.product-options-container').empty();

    //    fetch('/Manage/Product/UpdateUnitPost', {
    //        method: 'POST'
    //    })
    //        .then(res => res.text())
    //        .then(data => {
    //            $('.product-options-container').append(data)
    //        })

    //});
    $(document).on('click', '.det-img-close', function (e) {
        e.preventDefault();

        $('.detail-images-container').empty();
        fetch($(this).parent().attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.detail-images-container').append(data)
            })

    });
    $(document).on('click', '.det-order-close', function (e) {
        e.preventDefault();

        fetch($(this).parent().attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.orderitems-container').html(data)
            })

    });

    $(document).on('click', '.unit-update-close', function (e) {
        e.preventDefault();

        $(this).parent().parent().parent().parent().parent().parent().remove();
       

    });

    $(document).on('click', '.deleteBtn', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.tblContent').html(data)
            })

    });

    $(document).on('click', '.restoreBtn', function (e) {
        e.preventDefault();

        let url = $(this).attr('href');
        fetch(url)
            .then(res => res.text())
            .then(data => {
                $('.tblContent').html(data)
            })

    });
    
    
})