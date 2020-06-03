$(document).ready(function () {
    // modal init
    var today = new Date();
    $("#ReleaseDate").prop("max", formatDate(today));

    var authors = [];
    $.ajax({
        cache: false,
        url: '/Author/GetAuthors',
        type: 'GET',
        success: function (result) {
            authors = result;

            $.each(authors, function (index, value) {
                $('#AuthorIds').append(
                    '<option value="' + authors[index].Id + '">' + authors[index].Text + '</option>'
                );
            });
        }
    });

    $('#AuthorIds').select2({
        placeholder: 'Select authors'
    });

    // modal submit
    $("#bookModalForm").submit(function (e) {
        e.preventDefault();
        var form = $(this);
        var id = +$("#Id").val();
        var url = id == 0 ? "/Book/Create" : "/Book/Edit";
        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(),
            success: function (data) {
                $("#bookModal").modal('hide');
                $('#AuthorIds').val("").trigger('change');
                var releaseDate = dateFromJsonNumber(data.ReleaseDate);
                var authors = data.Authors.map(function (author) {
                    return `<a  href="/Author/Edit/${author.Id}?firstName=${author.FirstName}&lastName=${author.LastName}">${author.FirstName} ${author.LastName}</a><br/>`;
                });
                if (id) { // update book row
                    $("#booksList tr").each(function (row) {
                        if (row != 0) {
                            $row = $(this);
                            var rowId = $row.find("td:first")[0].innerText;
                            if (rowId == id) {
                                $row[0].children[1].innerText = data.Name;
                                $row[0].children[2].innerHTML = authors.join('');
                                $row[0].children[3].innerText = formatDate(releaseDate);
                                $row[0].children[4].innerText = data.Rate;
                                $row[0].children[5].innerText = data.PageNumber;
                                return false;
                            }
                        }
                    });
                }
            }
        });
    });
});