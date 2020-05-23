function dateFromJsonNumber(value) {
    var date = new Date(parseInt(value.replace("/Date(", "").replace(")/", ""), 10));
    return date;
}

function formatDate(date) {
    let dd = date.getDate();
    let mm = date.getMonth() + 1;
    const yyyy = date.getFullYear();
    if (dd < 10) {
        dd = `0${dd}`;
    }
    if (mm < 10) {
        mm = `0${mm}`;
    }
    return `${yyyy}-${mm}-${dd}`;
}