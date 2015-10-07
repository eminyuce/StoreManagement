function isEmpty(str) {
    return (!str || 0 === str.length);
}
function sortInputFirst(input, data) {
    var first = [];
    var others = [];
    for (var i = 0; i < data.length; i++) {
        if (data[i].text.toLowerCase().indexOf(input.toLowerCase()) == 0) {
            first.push(data[i]);
        } else {
            others.push(data[i]);
        }
    }
    first.sort();
    others.sort();
    return (first.concat(others));
}