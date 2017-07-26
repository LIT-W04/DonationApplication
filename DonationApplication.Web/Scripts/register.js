$(function () {
    var isEmailValid = false;
    var isPasswordMatch = false;
    $("#email").on('keyup', function() {
        $.get('/account/CheckEmailExists', {email: $("#email").val()}, function(result) {
            isEmailValid = !result.exists;
            if (isEmailValid) {
                $("#email-error").hide();
            } else {
                $("#email-error").show();
            }
            setSubmitButton();
        });
    });

    $("#password, #password-match").on('keyup', function () {
        isPasswordMatch = $("#password").val() === $("#password-match").val();
        if (isPasswordMatch) {
            $("#password-error").hide();
        } else {
            $("#password-error").show();
        }
        setSubmitButton();
    });

    function setSubmitButton() {
        $("#submit-button").prop('disabled', !isEmailValid || !isPasswordMatch);
    }
});