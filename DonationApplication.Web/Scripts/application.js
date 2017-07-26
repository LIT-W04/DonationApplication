$(function () {
    $("#category-select").on('change', function () {
        var value = $("#category-select").val();
        $("#submit-button").prop('disabled', value === "0");
    });
})