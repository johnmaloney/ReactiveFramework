$(function () {

    var hub = $.connection.messageHub;

    function updateMessageEvent(msg) {
        var newMessage = JSON.parse(msg);
        var messages = $('#messages');
        var messagesBody = messages.find('tbody');

        var newRow = '<tr> ' +
                        '<td>' + newMessage.LastUpdated + '</td>' +
                        '<td>' + newMessage.Message + '</td>' +
                        '<td>' + newMessage.PublishedBy + '</td>' +
                        '<td>' + newMessage.TransportedBy + '</td>' +
                     '</tr>'

        messagesBody.append(newRow);
    };

    hub.client.newMessageEvent = function (msg) {
        updateMessageEvent(msg);
    };

    var messages = $('#messages');
    var messagesBody = messages.find('tbody');
    messagesBody.empty();
    messagesBody.append('<tr><td colspan="4">Loading.......</td></tr>');

    $.connection.hub.start();
});