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

    // MIXINS!!!!

    $('.clickmixins').click(function () {

        var rectangle = {
            setWidth: function (w) {
                this.width = w;
            },
            setHeight: function (h) {
                this.height = h;
            },
            draw: function () {
                alert('building a rectangle with height: ' + this.height + ' and width: ' + this.width + ' and text: ' + this.text);
            }
        };

        var button = {
            setText: function (t) {
                this.text = t;
            }
        };

        var onclickControl = {
            callback: function (fn) {
                fn();
            }
        };

        var rectangleClickButton = function (w, h, text, callback) {
            this.setWidth(w);        // from rectangle mixin
            this.setHeight(h);        // from rectangle mixin
            this.setText(text);        // from button mixin
            this.callback(callback);    // from onclickControl mixin
        };

        rectangleClickButton.addMixin(rectangle);
        rectangleClickButton.addMixin(button);
        rectangleClickButton.addMixin(onclickControl);

        var btn = new rectangleClickButton(100, 100, 'some text', function () {
            alert('confused.com!!');
        });

        btn.draw();
    });


});

Object.prototype.addMixin = function (mixin) {
    for (var prop in mixin) {
        if (mixin.hasOwnProperty(prop)) {
            this.prototype[prop] = mixin[prop];
        }
    }
};