$(document).ready(function () {
    $('.clickmeget').click(function () {
        $.ajax({
            url: 'api/values',
            context: document.body
        }).done(function (data) {
            for (var q = 0; q < data.length; q++) {
                alert(data[q]);
            }
        });
    });

    $('.clickmepost').click(function () {
        $.ajax({
            type: 'POST',
            url: 'api/values',
            context: document.body,
            data: {
                value1: 'testing value 1',
                value2: 'testing value 2'
            }
        }).done(function () {
            alert('Success');
        });
    });

    var closureValue = 'Hello there!!';

    $('.clickmepostandclosure').click(function () {
        $.ajax({
            type: 'POST',
            url: 'api/values',
            context: document.body,
            data: {
                value1: 'testing value 1',
                value2: 'testing value 2'
            }
        }).done(function () {
            alert(closureValue);
            //global variable
            window.nbScope = 'lol';
        });
    });

    $('.clickmepostandclosure2').click(function () {
        $.ajax({
            type: 'POST',
            url: 'api/values',
            context: document.body,
            data: {
                value1: 'testing value 1',
                value2: 'testing value 2'
            }
        }).done(closureResponse);
    });

    var closureResponse = function () {
        alert(closureValue + ' with no-block-scope!!! ' + window.nbScope);
    };

});

