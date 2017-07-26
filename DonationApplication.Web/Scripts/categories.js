$(function () {


    $("#add").on('click', function () {
        $("#category-name").val('');
        $(".modal").modal();
    });


    $(".delete").on('click', function () {
        var id = $(this).data('category-id');
        $.post('/admin/DeleteCategory', { categoryId: id }, function () {
            window.location.reload();
        });
    });

    $(".edit").on('click', function () {
        var id = $(this).data('category-id');
        $(`#view-category-${id}`).hide();
        $(`#edit-category-${id}`).show();
    });

    $(".update").on('click', function () {
        var id = $(this).data('category-id');
        var name = $(`#category-name-${id}`).val();
        $.post('/admin/updatecategory', {id :id, name: name}, function() {
            window.location.reload();
        });
    });
});