$(function () {
    fillTable();

    function fillTable(categoryId) {
        $("table tr:gt(0)").remove();
        $.get("/admin/getpendingapplications", {categoryId: categoryId}, function(result) {
            result.forEach(function(item) {
                $("table").append(`<tr>
                    <td>${item.category}</td>
                    <td>
                        <a href="/admin/userhistory?userId=${item.userId}">${item.email}</a>
                    </td>
                    <td>${item.firstName}</td>
                    <td>${item.lastName}</td>
                    <td>${item.amount}</td>
                    <td><button data-id="${item.id}" class="btn btn-success">Approve</button>
                    <button data-id="${item.id}" class="btn btn-danger">Reject</button>
                    </td>
                    </tr>`);    
            });
        });
    }

    $("#category-filter").on('change', function() {
        fillTableBaseOnDropdown();
    });

    $("table").on('click', '.btn-danger', function() {
        var id = $(this).data('id');
        $.post('/admin/UpdateApplicationStatus', {id :id, isApproved: false}, function() {
            fillTableBaseOnDropdown();
        });
    });

    $("table").on('click', '.btn-success', function () {
        var id = $(this).data('id');
        $.post('/admin/UpdateApplicationStatus', { id: id, isApproved: true }, function () {
            fillTableBaseOnDropdown();
        });
    });

    function fillTableBaseOnDropdown() {
        var value = $("#category-filter").val();
        if (value == "0") {
            fillTable();
        } else {
            fillTable(value);
        }
    }
});