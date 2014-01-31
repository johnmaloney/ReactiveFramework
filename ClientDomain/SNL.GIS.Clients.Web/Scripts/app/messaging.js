$(function () {

    var openMap = new OpenLayersMap('map');
    var authHub = $.connection.authenticationHub;
    var layersHub = $.connection.layersHub;
    var searchHub = $.connection.searchHub;

    // assign the managers of the TOPICs 
    authHub.client.authenticated = topicReceived;
    layersHub.client.layersInitialized = userLayersInitialized;
    layersHub.client.layerReceived = layerResultReceived;
    searchHub.client.searchResult = topicReceived;

    $.connection.hub.start();
    
    var userAuthentication = new UserAuthentication($.connection.authenticationHub);
    $('#authenticateUser').click(function () {
        userAuthentication.login();
    });

    $('#searchData').click(function () {
    });

    $('#retrieveShapeLayers').click(function () {

        layersHub.server.retrieveLayer('shapes');

    });

    $('#retrievePointLayers').click(function () {
        
        layersHub.server.retrieveLayer('points');

    });

    $('#changeBaseLayer').click(function () {
    });
    
    function userLayersInitialized(msg) {

        topicReceived(msg);
        
        var newMessage = JSON.parse(msg);

        for (var i in newMessage.Layers) {

            console.log('Iterating layer# ' + i + ':' + newMessage.Layers[i]);

            console.log(openMap);
            openMap.addOpenGeoImage({ 'LAYERS': newMessage.Layers[i]});
        }
    };

    function layerResultReceived(msg) {

        topicReceived(msg);

        var newMessage = JSON.parse(msg);

        if (newMessage.LayerId === 'points') {
            openMap.addGeoJson(newMessage.Layer);
        }

        if (newMessage.LayerId === 'shapes') {
            openMap.addGeoJsonShapes('');
        }
    };

    function topicReceived(msg) {
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