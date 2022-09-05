$(document).ready(function () {
    $('.validation-summary-errors ul li').css('padding-bottom', '1rem');
    $('.validation-summary-errors ul').css('padding', '0');
    $('.main-upload, .detail-upload').css('border', '1px dashed black');
    $('.main-upload, .detail-upload').css('background-color', '#e2e2e2');
    $('.custom-input-container').css('border', '1px dashed black');


    $(document).on('click', '.addinputs', function (e) {
        $('.custom-input-container').css('padding', '1rem 0');
        $('.custom-input-container').css('margin', '1rem 0');
        $('.custom-input-container').css('border', '1px dashed black');

        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.inputsContainer').append(data)
            })
    });
})