$(document).ready(function () {
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