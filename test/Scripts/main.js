$(document).ready(function () {
    var pageNumber = 0;

    //AJAX обновление страницы 
    setInterval(function () {
        $.ajax({
            type: "POST",
            url: "Home/Index",
            data: "page=" + pageNumber,
            success: function (result) {
                $('#result').empty();
                $('#result').html(result);
            }
        });
    }, 1000);

    //обновление после like, dell
    var updateMessages = function () {
        $.ajax({
            type: "POST",
            url: "Home/Index",
            data: "",
            success: function (result) {
                $('#result').empty();
                $('#result').html(result);
            }
        });
    };

    // пейджинг
    $('#result').on('click', '.page', function (event) {
        pageNumber = $(this).attr('data-ajax');

        $.ajax({
            type: "POST",
            url:"Home/Index",
            data: "page=" + pageNumber,
            success: function (result) {
                $('#result').empty();
                $('#result').html(result);
            },
            error: function (xhr, status, p3) {
                alert(xhr.responseText);
            }
        });
        return false;
    })

    // AJAX сохранение формы
    $('#mesFormSubmit').on('click', function (e) {
        var postbody = $("#mesForm").serializeArray();
        var files = document.getElementById('uploadFile').files;
        if (window.FormData !== undefined) {
            var data = new FormData();
            for (var i = 0; i < postbody.length; i++) {
                data.append(postbody[i].name, postbody[i].value);
            }
            if (files.length > 0) {
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
            }
        };
        $.ajax({
            type: "POST",
            url: "Home/SaveMessage",
            contentType: false,
            processData: false,
            data: data,
            success: function (result) { },
            error: function (xhr, status, p3) {
                alert(xhr.responseText);
            }
        });
        $('#mesForm')[0].reset();
    });

    //добавление инпута
    $('.cross').on('click', function (e) {
        var el = $('.add');
        $('#uploadFile').before("<input type='url' name='Link' value='link'placeholder = 'link'>");
    });


})