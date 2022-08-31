$(document).ready(function () {
    $('.validation-summary-errors ul li').css('padding-bottom', '1rem');
    $('.validation-summary-errors ul').css('padding', '0');



    $(document).on('click', '.addinputs', function (e) {
        e.preventDefault();

        fetch($(this).attr('href'))
            .then(res => res.text())
            .then(data => {
                $('.inputsContainer').append(data)
            })
    });
})