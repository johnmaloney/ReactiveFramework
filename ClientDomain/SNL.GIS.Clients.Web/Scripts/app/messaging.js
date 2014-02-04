$(function () {

    var openMap = new OpenLayersMap('map');
    var authHub = $.connection.authenticationHub;
    var layersHub = $.connection.layersHub;
    var searchHub = $.connection.searchHub;

    // assign the managers of the TOPICs 
    authHub.client.authenticated = topicReceived;
    layersHub.client.layersInitialized = userLayersInitialized;
    layersHub.client.layerReceived = layerResultReceived;
    searchHub.client.searchResult = searchResultsReceived;

    $.connection.hub.start();
    
    var userAuthentication = new UserAuthentication($.connection.authenticationHub);
    $('#authenticateUser').click(function () {
        userAuthentication.login();
    });

    $('#searchData').click(function () {

        var criteria = $('#searchCriteria').val();
        searchHub.server.search(criteria);

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

    function searchResultsReceived(msg) {

        topicReceived(msg);

        var messages = $('#searchResults');
        var messagesBody = messages.find('tbody');
        messagesBody.empty();

        var newMessage = JSON.parse(msg);
        for (var branch in newMessage.Results) {

            var newRow =
            '<tr> ' +
                '<td>' + newMessage.Results[branch].keyBranch + '</td>' +
                '<td>' + newMessage.Results[branch].description + '</td>' +
                '<td>' + newMessage.Results[branch].street1 + '  ' + newMessage.Results[branch].zip + '</td>' +
                '<td>' + newMessage.Results[branch].latitude + '</td>' +
                '<td>' + newMessage.Results[branch].longitude + '</td>' +
            '</tr>';

            messagesBody.append(newRow);

        };
    };

    function topicReceived(msg) {
        console.log(msg);
        var newMessage = JSON.parse(msg);

        var messages = $('#messages');
        var messagesBody = messages.find('tbody');

        var newRow = '<tr> ' +
                        '<td>' + newMessage.Details + '</td>' +
                    '</tr>';

        messagesBody.append(newRow);
    };
});