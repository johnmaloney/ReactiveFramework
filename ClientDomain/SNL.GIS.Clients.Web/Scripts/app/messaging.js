$(function () {

    var hub = $.connection.authenticationHub;
    $.connection.hub.start();
    
    var userAuthentication = new UserAuthentication($.connection.authenticationHub);
    $('#authenticateUser').click(function () {
        userAuthentication.login();
    });

    // Authenticated takes the message from the ServiceBus that
    // tells us this user has been logged in
    hub.client.authenticated = function (msg) {
        console.log(msg);
        var newMessage = JSON.parse(msg);

        var messages = $('#messages');
        var messagesBody = messages.find('tbody');

        var newRow = '<tr> ' +
                        '<td>' + newMessage.Details + '</td>'
                     '</tr>'

        messagesBody.append(newRow);
    };
});