// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

M.AutoInit();
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.dropdown-trigger');
    var options = {
        "constrainWidth": false
    }
    M.Dropdown.init(elems, options);
});
document.addEventListener('DOMContentLoaded', function () {
    var elems = document.querySelectorAll('.datepicker');
    var options = {
        "defaultDate": Date.now(),
        "setDefaultDate": true,
        "autoClose": true,
        "disableWeekends": true,
    }
    var instances = M.Datepicker.init(elems, options);
});