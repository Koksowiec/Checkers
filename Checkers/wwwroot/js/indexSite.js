$(document).ready(function () {
    $.ajax({
        url: '/Home/GetDashboard',
        type: 'GET',
        success: function (response) {
            // Iterate over the response and log user and count for each item
            for (var i = 0; i < response.length; i++) {
                var user = response[i].user;
                var count = response[i].count;
                $("#highscorePlayers").append("<span>" + user + ": " + count +"</span>")
            }
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
});