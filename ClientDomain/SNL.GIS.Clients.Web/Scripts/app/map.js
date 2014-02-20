var MQMap = function (mapElement) {

    console.log("Created a new instance of MapQuest Map");

    var options = {
        elt: document.getElementById('map'),           /*ID of element on the page where you want the map added*/
        zoom: 13,                                      /*initial zoom level of the map*/
        latLng: { lat: 40.735383, lng: -73.984655 },   /*center of map in latitude/longitude */
        mtype: 'osm',                                  /*map type (osm)*/
        bestFitMargin: 0,                              /*margin offset from the map viewport when applying a bestfit on shapes*/
        zoomOnDoubleClick: true                        /*zoom in when double-clicking on map*/
    };

    mapelement = new MQA.TileMap(options);

    MQA.withModule('largezoom', 'viewoptions', 'geolocationcontrol', 'insetmapcontrol', 'mousewheel', function () {

        mapelement.addControl(
          new MQA.LargeZoom(),
          new MQA.MapCornerPlacement(MQA.MapCorner.TOP_LEFT, new MQA.Size(5, 5))
        );

        mapelement.addControl(new MQA.ViewOptions());

        mapelement.addControl(
          new MQA.GeolocationControl(),
          new MQA.MapCornerPlacement(MQA.MapCorner.TOP_RIGHT, new MQA.Size(10, 50))
        );

        /*Inset Map Control options */
        var options = {
            size: { width: 150, height: 125 },
            zoom: 3,
            mapType: 'osmsat',
            minimized: false
        };

        mapelement.addControl(
          new MQA.InsetMapControl(options),
          new MQA.MapCornerPlacement(MQA.MapCorner.BOTTOM_RIGHT)
        );

        mapelement.enableMouseWheelZoom();
    });
}

var OpenMap = function (mapElementName) {
    
    console.log("Created a new instance of OpenStreets Map");

    // Start a OpenLayer Map - http://docs.openlayers.org/library/introduction.html
    // Wiki examples - http://wiki.openstreetmap.org/wiki/OpenLayers_Simple_Example
    var map = new OpenLayers.Map(mapElementName);

    var fromProjection = new OpenLayers.Projection("EPSG:4326");   // Transform from WGS 1984
    var toProjection = new OpenLayers.Projection("EPSG:900913"); // to Spherical Mercator Projection
    var position = new OpenLayers.LonLat(13.41, 52.52).transform(fromProjection, toProjection);
    var zoom = 3;

    map.addLayer(new OpenLayers.Layer.OSM());
    map.setCenter(position, zoom);
}

var OpenLayersMap = function (elementName) {

    var attribution = new ol.Attribution({
        html: 'Tiles &copy; <a href="http://services.arcgisonline.com/ArcGIS/' +
            'rest/services/World_Topo_Map/MapServer">ArcGIS</a>'
    });


    var map = new ol.Map({
        target: elementName,
        //layers: [
        //    new ol.layer.Tile({
        //        source: new ol.source.MapQuest({ layer: 'sat' })
        //    })
        //],
        layers: [
            new ol.layer.Tile({
                source: new ol.source.XYZ({
                    attributions: [attribution],
                    url: 'http://server.arcgisonline.com/ArcGIS/rest/services/' +
                        'World_Topo_Map/MapServer/tile/{z}/{y}/{x}'
                })
            })
        ],
        renderer: 'canvas',
        view: new ol.View2D({
            center: ol.proj.transform([-100.1, 35.5], 'EPSG:4326', 'EPSG:3857'),
            zoom: 2
        })
    });

    this.map = map;

    // USAGE EXAMPLES

    // Loads a MapQuest Map instance into the #map element //
    //var mqMap = new MQMap($('#map'));

    //openMap.addTile('http://api.tiles.mapbox.com/v3/mapbox.va-quake-aug.jsonp');
    //openMap.addTile('http://api.tiles.mapbox.com/v3/mapbox.geography-class.jsonp');
    //openMap.addTile('https://a.tiles.mapbox.com/v3/maloney1.h4am381i.jsonp');
    //openMap.addOpenGeoImage({ 'LAYERS': 'topp:states' });
    //openMap.addOpenGeoImage({ 'LAYERS': 'usgs:tracts' });
    //openMap.addOpenGeoImage({ 'LAYERS': 'topp:tasmania_state_boundaries' });
    ////openMap.addKMLVector('');
    //openMap.addGeoJsonShapes('');
    //openMap.addGeoJson('');

    //console.log(openMap);

};

OpenLayersMap.prototype = {
    
    addTile : function (layerInfo) {

        console.log("Adding a new Layer...");

        this.map.addLayer(new ol.layer.Tile({
            source: new ol.source.TileJSON({
                url: layerInfo,
                crossOrigin: 'anonymous'
            })
        }))
    },

    addOpenGeoImage: function (paramInfo) {

        console.log('Adding a new Image...');

        this.map.addLayer(
            new ol.layer.Image({
            source: new ol.source.ImageWMS({
                url: 'http://demo.opengeo.org/geoserver/wms',
                params: paramInfo,
                serverType: /** @type {ol.source.wms.ServerType} */ ('geoserver'),
                extent: [-13884991, 2870341, -7455066, 6338219]
            })
        }));

    },

    addKMLVector : function(kmlSource){

        console.log("Adding KML image...");

        var vector = new ol.layer.Vector({
            source: new ol.source.KML({
                reprojectTo: 'EPSG:3857',
                url: 'http://ol3js.org/en/master/examples/data/kml/2012_Earthquakes_Mag5.kml'
            })
        });

    },

    addGeoJson : function(info) {
        
        console.log("Adding Geo Json...");

        var image = new ol.style.Circle({
            radius: 5,
            fill: null,
            stroke: new ol.style.Stroke({ color: 'red', width: 1 })
        });

        var styles = {
            'Point': [new ol.style.Style({
                image: image
            })]
        };

        var styleFunction = function (feature, resolution) {
            return styles[feature.getGeometry().getType()];
        };

        var vectorSource = new ol.source.GeoJSON({
            reprojectTo: 'EPSG:3857',
            url: info
        });

        var vectorLayer = new ol.layer.Vector({
            source: vectorSource,
            styleFunction: styleFunction
        });

        this.map.addLayer(vectorLayer);

    },

    addGeoJsonShapes: function (info) {

        var image = new ol.style.Circle({
            radius: 5,
            fill: null,
            stroke: new ol.style.Stroke({ color: 'red', width: 1 })
        });

        var styles = {
            'Point': [new ol.style.Style({
                image: image
            })],
            'LineString': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'green',
                    width: 1
                })
            })],
            'MultiLineString': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'green',
                    width: 1
                })
            })],
            'MultiPoint': [new ol.style.Style({
                image: image
            })],
            'MultiPolygon': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'yellow',
                    width: 1
                }),
                fill: new ol.style.Fill({
                    color: 'rgba(255, 255, 0, 0.1)'
                })
            })],
            'Polygon': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'blue',
                    lineDash: [4],
                    width: 3
                }),
                fill: new ol.style.Fill({
                    color: 'rgba(0, 0, 255, 0.1)'
                })
            })],
            'GeometryCollection': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'magenta',
                    width: 2
                }),
                fill: new ol.style.Fill({
                    color: 'magenta'
                }),
                image: new ol.style.Circle({
                    radius: 10,
                    fill: null,
                    stroke: new ol.style.Stroke({
                        color: 'magenta'
                    })
                })
            })],
            'Circle': [new ol.style.Style({
                stroke: new ol.style.Stroke({
                    color: 'red',
                    width: 2
                }),
                fill: new ol.style.Fill({
                    color: 'rgba(255,0,0,0.2)'
                })
            })]
        };

        var styleFunction = function (feature, resolution) {
            return styles[feature.getGeometry().getType()];
        };

        var vectorSource = new ol.source.GeoJSON(
            /** @type {olx.source.GeoJSONOptions} */({
                object: {
                    'type': 'FeatureCollection',
                    'crs': {
                        'type': 'name',
                        'properties': {
                            'name': 'EPSG:3857'
                        }
                    },
                    'features': [
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'Point',
                              'coordinates': [0, 0]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'LineString',
                              'coordinates': [[4e6, -2e6], [8e6, 2e6]]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'LineString',
                              'coordinates': [[4e6, 2e6], [8e6, -2e6]]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'Polygon',
                              'coordinates': [[[-5e6, -1e6], [-4e6, 1e6], [-3e6, -1e6]]]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'MultiLineString',
                              'coordinates': [
                                [[-1e6, -7.5e5], [-1e6, 7.5e5]],
                                [[1e6, -7.5e5], [1e6, 7.5e5]],
                                [[-7.5e5, -1e6], [7.5e5, -1e6]],
                                [[-7.5e5, 1e6], [7.5e5, 1e6]]
                              ]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'MultiPolygon',
                              'coordinates': [
                                [[[-5e6, 6e6], [-5e6, 8e6], [-3e6, 8e6], [-3e6, 6e6]]],
                                [[[-2e6, 6e6], [-2e6, 8e6], [0e6, 8e6], [0e6, 6e6]]],
                                [[[1e6, 6e6], [1e6, 8e6], [3e6, 8e6], [3e6, 6e6]]]
                              ]
                          }
                      },
                      {
                          'type': 'Feature',
                          'geometry': {
                              'type': 'GeometryCollection',
                              'geometries': [
                                {
                                    'type': 'LineString',
                                    'coordinates': [[-5e6, -5e6], [0e6, -5e6]]
                                },
                                {
                                    'type': 'Point',
                                    'coordinates': [4e6, -5e6]
                                },
                                {
                                    'type': 'Polygon',
                                    'coordinates': [[[1e6, -6e6], [2e6, -4e6], [3e6, -6e6]]]
                                }
                              ]
                          }
                      }
                    ]
                }
            }));

        vectorSource.addFeature(new ol.Feature(new ol.geom.Circle([5e6, 7e6], 1e6)));

        var vectorLayer = new ol.layer.Vector({
            source: vectorSource,
            styleFunction: styleFunction
        });

        this.map.addLayer(vectorLayer);
    }
};

