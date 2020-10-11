$(document).ready(function () {
    // file upload events
    $(".book-title-input").change(function () {
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

    $(".book-attachment-input").change(function () {
        var row = $(this).parent().parent()[0];
        var bookId = $(row).find("td")[0].innerText;
        var attachments = $(row).find(".attachments-list");
        var fileName = this.files[0].name;

        var formData = new FormData();
        var totalFiles = this.files.length;
        if (totalFiles > 0) {
            var file = this.files[0];
            if (file.size > 512000) { // 500 KB
                alert('Sorry, only files up to 500 KB are allowed');
                return;
            }
            formData.append("BookAttachmentInput", file);
            formData.append("BookId", bookId);
        }
        $.ajax({
            type: "POST",
            url: '/Book/UploadAttachment/' + bookId,
            data: formData,
            dataType: "text",
            contentType: false,
            processData: false,
            success: function (res) {
                if (res) {
                    var data = JSON.parse(res);
                    attachments.append(
                        '<a data-attachment="' + data.FileS3Key+'" class="attachment-file attachment-info" href="' + data.FileUrl + '">' + fileName + '</a>'+
                        '<span data-attachment="' + data.FileS3Key+'" class="glyphicon glyphicon-remove remove-attachment attachment-info"></span></br>'
                    );
                    var files = $(row).find(".attachment-file");
                    if (files.length > 2) {
                        var uploadBtn = $(row).find(".book-attachment-input");
                        $(uploadBtn).remove();
                    }
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('File was not saved')
            }
        });
    });

    // download attachment
    $(document).on('click', '.attachment-file', function (e) {
        e.preventDefault();
        e.stopPropagation();
        var link = $(this)[0];

        fetch(link.href).then(function (res) {
            return res.blob().then((b) => {
                var a = document.createElement("a");
                a.href = URL.createObjectURL(b);
                a.setAttribute("download", link.innerText);
                a.click();
            });
        });
    })

    // remove attachment
    $(document).on('click', '.remove-attachment', function (e) {
        var fileKey = $(this)[0].attributes["data-attachment"].value;
        var currentIcon = this;
        $.ajax({
            type: "POST",
            url: "/Book/DeleteAttachment",
            data: { fileKey: fileKey },
            success: function () {
                var row = $(currentIcon).parent().parent()[0];
                var attachments = $(row).find(".attachments-list");
                var links = $(attachments).find(".attachment-info");
                links.each(function (i) {
                    if ($(this)[0].attributes["data-attachment"].value == fileKey) {
                        $(this).remove();
                    }
                });
            },
            error: function (data) {
                alert('Sorry, failed to delete attachment. Try again later');
            }
        });
    })


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
        var row = $(this).parent().parent()[0];
        var bookId = $(row).find("td")[0].innerText;
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
        var row = $(this).parent().parent()[0];
        var bookId = $(row).find("td")[0].innerText;
        $.ajax({
            type: "POST",
            url: "/Book/DeleteAsync",
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