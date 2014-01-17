$(function () {

    var hub = $.connection.queueHub;

    hub.client.queueUpdated = function (msg) {
        var newMessage = JSON.parse(msg);

        console.log("finished server call of hub");
        var messages = $('#messages');
        var messagesBody = messages.find('tbody');

        var newRow = '<tr> ' +
                        '<td colspan="3">' + newMessage.MessageType + '</td>' +
                        '<td><a class="align-right" id="'+ newMessage.Identifier +'" href="#" data-command=\'' + msg + '\'><i class="fa fa-wrench"></i> PROCESS</a></td>' +
                     '</tr>'

        messagesBody.append(newRow);

        var link = messagesBody.find('#' + newMessage.Identifier);
        link.click(function () {
            var message = $(this).data('command');
            var json = JSON.stringify(message);
            console.log(json);
            hub.server.processQueueItem(json);
            console.log("Finished server call");
        });
    };
    
    $('#more').click(function () {
        response();
    });

   

    $.connection.hub.start().done(function(){

        console.log("finished start of hub");

        
    });
    

});