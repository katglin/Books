$(document).ready(function () {
    // file upload events
    $(".book-title-input").change(function () {
        //var bookId = +$(this).parent().parent().parent().children(':first')[0].innerText;
        var row = $(this).parent().parent().parent()[0];
        var img = $(row).find("img");
        var bookId = $(row).find("td")[0].innerText;

        var formData = new FormData();
        var totalFiles = this.files.length;
        if (totalFiles > 0) {
            var file = this.files[0];
            if (file.size > 512000) { // 500 KB
                alert('Sorry, only files up to 500 KB are allowed');
                return;
            }
            formData.append("BookTitleInput", file);
            formData.append("BookId", bookId);
        }
        $.ajax({
            type: "POST",
            url: '/Book/UploadImage/' + bookId,
            data: formData,
            dataType: "text",
            contentType: false,
            processData: false,
            success: function (url) {
                if (url && img) {
                    img.attr('src', url);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('File was not saved')
            }
        });
    });


    // modal events
    $("#btnShowBookModal").click(function () {
        $("#bookModal").modal('show');
    });

    $("#btnHideBookModal").click(function () {
        $("#bookModal").modal('hide');
    });

    $('#bookModal').on('hidden.bs.modal', function () {
        $('#bookModalForm').trigger("reset");
        $('#AuthorIds').val("").trigger('change');
    });

    $("#booksList").on("click", ".edit-book", function () {
        var bookId = +$(this).parent().parent().children(':first')[0].innerText;
        $.ajax({
            type: "GET",
            url: "/Book/Get",
            data: { id: bookId },
            success: function (data) {
                $("#bookModal").modal('show');
                $("#bookModalForm #Id").val(bookId);
                $("#bookModalForm #Name").val(data.Name);
                $("#bookModalForm #ReleaseDate").val(formatDate(dateFromJsonNumber(data.ReleaseDate)));
                $("#bookModalForm #AuthorIds").val(data.AuthorIds).trigger("change");
                $("#bookModalForm #Rate").val(data.Rate);
                $("#bookModalForm #PageNumber").val(data.PageNumber);
            }
        });
    });

    $("#booksList").on("click", ".delete-book", function () {
        var bookId = +$(this).parent().parent().children(':first')[0].innerText;
        var row = $(this).parent().parent()[0];
        $.ajax({
            type: "POST",
            url: "/Book/Delete",
            data: { id: bookId },
            success: function () {
                row.remove();
            },
            error: function (data) {
                $('#AuthorIds').validate();
            }
        });
    });
});