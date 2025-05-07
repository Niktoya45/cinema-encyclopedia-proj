
// provide references to corresponding pagination elements preserving other query parameters

var pageNums = document.querySelectorAll("#pages .page-item");

var locationSearch = window.location.search;
var locationPlain = window.location.pathname;
const currentSearch = new URLSearchParams(locationSearch);

if (currentSearch.has("pageNum")) {
    currentSearch.delete("pageNum");
}
if (currentSearch.size) {

    pageNums.forEach(function (pageNum) {

        var ref = pageNum.firstElementChild;

        if (ref) { 
            ref.href = ref.href + "&" + currentSearch.toString();
        }
    });
}