$(function () {
    var placeholderElement = $('#modal-placeholder');

    $(document).on('click', 'button[data-toggle="ajax-modal"]', function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {
        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = new FormData(form.get(0));

        $.ajax({ url: actionUrl, method: 'post', data: dataToSend, processData: false, contentType: false }).done(function (data) {
            var newBody = $('.modal-body', data);
            placeholderElement.find('.modal-body').replaceWith(newBody);
            //choose correct table 
            var isValid = newBody.find('[name="IsValid"]').val() == 'True';
            if (actionUrl.includes("Delete")) {
                isValid = "True";
            }
            var tableName = actionUrl.split('/')[1];
            if (isValid) {
                var notificationsPlaceholder = $('#notification');
                var notificationsUrl = notificationsPlaceholder.data('url');
                $.get(notificationsUrl).done(function (notifications) {
                    notificationsPlaceholder.html(notifications);
                });

                //choose correct table to update
                switch (tableName) {
                    case "Meal":
                        var tableElement = $('#meals');
                        break;
                    case "Product":
                        var tableElement = $('#products');
                        break;
                    case "Day":
                        var tableElement = $('#days');
                        break;
                    case "DietPlan":
                        var tableElement = $('#dietPlans');
                        break;
                }
                var tableUrl = tableElement.data('url');
                $.get(tableUrl).done(function (table) {
                    tableElement.replaceWith(table);
                });
                placeholderElement.find('.modal').modal('hide');
            }
            if (!isValid) {

                switch (tableName) {
                    case "Meal":
                        $('#productsList').multiselect('rebuild');
                        break;
                    case "Day":
                        $('#mealsList').multiselect('rebuild');
                        break;
                    case "DietPlan":
                        $('#daysList').multiselect('rebuild');
                        break;
                }      
            }
        });
    });
});

$(function () {
    $('.bs-timepicker').timepicker();
});
