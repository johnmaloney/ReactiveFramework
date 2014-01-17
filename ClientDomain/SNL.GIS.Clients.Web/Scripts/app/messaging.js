$(function () {

    var hub = $.connection.authenticationHub;
    $.connection.hub.start();
    
    var userAuthentication = new UserAuthentication($.connection.authenticationHub);
    $('#authenticateUser').click(function () {
        userAuthentication.login();
    });

    hub.client.authenticated = function (msg) {
        console.log(msg);
        var newMessage = JSON.parse(msg);

        var messages = $('#messages');
        var messagesBody = messages.find('tbody');

        var newRow = '<tr> ' +
                        '<td>' + newMessage.Identifier + '</td>'
                     '</tr>'

        messagesBody.append(newRow);
    };
});