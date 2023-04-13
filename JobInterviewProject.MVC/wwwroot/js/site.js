// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function confirmDelete() {
    if (confirm("Do you really want to delete?")) {
        return true; 
    } else {
        return false;
    }
}