; (function (window) {
    'use strict'

    var imagebase64 = function (file, width, height, callback) {
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function (e) {
            var img = new Image;
            img.src = this.result;

            var canvas = document.createElement("canvas");
            var drawer = canvas.getContext("2d");

            if (height > width) {
                canvas.width = width;
                canvas.height = width * (img.height / img.width);
            } else {
                canvas.height = height;
                canvas.width = height * (img.width / img.height);
            }

            drawer.drawImage(img, 0, 0, canvas.width, canvas.height);
            var dataURL = canvas.toDataURL('image/png');
            callback(dataURL);
        }
    }

    window.imagebase64 = imagebase64;
}(window))