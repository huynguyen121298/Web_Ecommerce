function ShowImagePreview(imageUploader, PreviewImage) {
    if (imageUploader.flle && imageUploader.flle[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(PreviewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.flle[0]);
    }
}