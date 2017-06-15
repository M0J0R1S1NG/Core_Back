


var map = null;

var myHome;
//var geocodeRequest2 = "https://dev.virtualearth.net/REST/v1/Locations?query=" + encodeURIComponent("21 Lakeview Ave. Toronto, Ontario, Canada") + "&jsonp=GeocodeCallback&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
//var geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations?query=" + encodeURIComponent("21 Lakeview Ave. Toronto, Ontario, Canada") + "&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
var pushpin;
top.addEventListener('resize', function() {
    var width = $(document).width();
    var height = $(document).height();
    map.setView({ width: width, height: height })
});
//    $(function () {
      
//         $('#myLink').on('click', function (e) {
//             e.preventDefault();
//             //top.document.getElementById("myLink").href = "Delivery/ModalAction/1?message=" +  encodeURI(top.document.getElementById("message").value) + "&title=" + encodeURI(top.document.getElementById("title").value);
//         });
//     });

function createFontPushpin(location, text, fontName, fontSizePx, color) {
    var c = document.createElement('canvas');
    var ctx = c.getContext('2d');

    //Define font style
    var font = fontSizePx + 'px ' + fontName;
    ctx.font = font

    //Resize canvas based on size of text.
    var size = ctx.measureText(text);
    c.width = size.width;
    c.height = fontSizePx;

    //Reset font as it will be cleared by the resize.
    ctx.font = font;
    ctx.textBaseline = 'top';
    ctx.fillStyle = color;

    ctx.fillText(text, 0, 0);

    return new Microsoft.Maps.Pushpin(location, {
        icon: c.toDataURL(),
        anchor: new Microsoft.Maps.Point(c.width / 2, c.height / 2) //Align center of pushpin with location.
    });
}

function createRequested() {
    var myRequestLocs = [
        new Microsoft.Maps.Location(43.54095685778117, -79.6159970132649),
        new Microsoft.Maps.Location(43.53697482983541, -79.63934296053053),
        new Microsoft.Maps.Location(43.62054218765287, -79.5940243570149),
        new Microsoft.Maps.Location(43.557877542615095, -79.59814423006178),
        new Microsoft.Maps.Location(43.56384841488275, -79.58029144685865),
        new Microsoft.Maps.Location(43.52004827719623, -79.61187714021803),
        new Microsoft.Maps.Location(43.521044088251635, -79.62972992342115),
        new Microsoft.Maps.Location(43.53398813629408, -79.62286346834303),
        new Microsoft.Maps.Location(43.53896587668115, -79.64552277010084),
        new Microsoft.Maps.Location(43.55389663236335, -79.65513580721021),
        new Microsoft.Maps.Location(43.55290136370817, -79.67024200838209),
        new Microsoft.Maps.Location(43.556882339707556, -79.61531036775709),
        new Microsoft.Maps.Location(43.569818665048004, -79.65788238924146),
        new Microsoft.Maps.Location(43.59269925852399, -79.61805694978834),
        new Microsoft.Maps.Location(43.60861106448856, -79.56175201814771),
        new Microsoft.Maps.Location(43.63445877375317, -79.60295074861646),
        new Microsoft.Maps.Location(43.80841549566984, -79.11021393221023),
        new Microsoft.Maps.Location(43.796521745142385, -79.13218658846023),
        new Microsoft.Maps.Location(43.82129771998934, -79.1363064615071),
        new Microsoft.Maps.Location(43.82228854521496, -79.11021393221023),
        new Microsoft.Maps.Location(43.830214524803466, -79.05802887361648),
        new Microsoft.Maps.Location(43.8183251154098, -79.06077545564773),
        new Microsoft.Maps.Location(43.83516774651179, -79.0429226724446),
        new Microsoft.Maps.Location(43.8411110699021, -79.0319363443196),
        new Microsoft.Maps.Location(43.86190803975845, -79.04429596346023),
        new Microsoft.Maps.Location(43.83615834152088, -79.08824127596023),
        new Microsoft.Maps.Location(43.82030684808147, -79.15965240877273),
        new Microsoft.Maps.Location(43.81733422440822, -79.2187039224446),
        new Microsoft.Maps.Location(43.80246888614087, -79.25990265291335),
        new Microsoft.Maps.Location(43.82526089199238, -79.27088898103835),
        new Microsoft.Maps.Location(43.83813948220627, -79.27500885408523),
        new Microsoft.Maps.Location(43.858937488070545, -79.3450466958821),
        new Microsoft.Maps.Location(43.85497652225992, -79.39860504549148),
        new Microsoft.Maps.Location(43.88269775743937, -79.40409820955398),
        new Microsoft.Maps.Location(43.857947271285, -79.42881744783523),
        new Microsoft.Maps.Location(43.88764662128292, -79.43568390291335),
        new Microsoft.Maps.Location(43.831205202033445, -79.51808136385085),
        new Microsoft.Maps.Location(43.85002494499142, -79.46864288728835),
        new Microsoft.Maps.Location(43.880718096784605, -79.50572174471023),
        new Microsoft.Maps.Location(43.80048656149813, -79.3889920083821),
        new Microsoft.Maps.Location(43.808415465425476, -79.44117706697585),
        new Microsoft.Maps.Location(43.78264267992381, -79.4549099771321),
        new Microsoft.Maps.Location(43.80048656149813, -79.4823757974446),
        new Microsoft.Maps.Location(43.76578526358328, -79.49748199861648),
        new Microsoft.Maps.Location(43.748923065086444, -79.4384304849446),
        new Microsoft.Maps.Location(43.76280992678586, -79.42607086580398),
        new Microsoft.Maps.Location(43.77371877156877, -79.45216339510085),
        new Microsoft.Maps.Location(43.7776851006763, -79.41783111971023),
        new Microsoft.Maps.Location(43.768760422133255, -79.41508453767898),
        new Microsoft.Maps.Location(43.778676649411594, -79.3999783365071),
        new Microsoft.Maps.Location(43.78462559652218, -79.36976593416335),
        new Microsoft.Maps.Location(43.794539192929, -79.4109646646321),
        new Microsoft.Maps.Location(43.803460023797065, -79.37113922517898),
        new Microsoft.Maps.Location(43.8183251154098, -79.44255035799148),
        new Microsoft.Maps.Location(43.82625165144268, -79.39585846346023),
        new Microsoft.Maps.Location(43.73106379328994, -79.55104034822585),
        new Microsoft.Maps.Location(43.743962690617316, -79.56889313142898),
        new Microsoft.Maps.Location(43.73304841989255, -79.59498566072585),
        new Microsoft.Maps.Location(43.71915465257977, -79.64579742830398),
        new Microsoft.Maps.Location(43.70922852980625, -79.66227692049148),
        new Microsoft.Maps.Location(43.72213212942967, -79.69798248689773),
        new Microsoft.Maps.Location(43.73304838961005, -79.71446197908523),
        new Microsoft.Maps.Location(43.743962660340344, -79.73643463533523),
        new Microsoft.Maps.Location(43.72709431249919, -79.73231476228835),
        new Microsoft.Maps.Location(43.71617696717839, -79.73918121736648),
        new Microsoft.Maps.Location(43.71022121304014, -79.76115387361648),
        new Microsoft.Maps.Location(43.735032950446715, -79.74742096346023),
        new Microsoft.Maps.Location(43.71915462229025, -79.7240750161946),
        new Microsoft.Maps.Location(43.723124629214105, -79.5537869302571),
        new Microsoft.Maps.Location(43.736025236483364, -79.56889313142898),
        new Microsoft.Maps.Location(43.71915465257977, -79.59635895174148),
        new Microsoft.Maps.Location(43.73007145532609, -79.5043484536946),
        new Microsoft.Maps.Location(43.73900187481839, -79.48649567049148),
        new Microsoft.Maps.Location(43.75289103843096, -79.50572174471023),
        new Microsoft.Maps.Location(43.77272711035753, -79.51258819978835),
        new Microsoft.Maps.Location(43.80147773204121, -79.51121490877273),
        new Microsoft.Maps.Location(43.81733422440822, -79.51670807283523),
        new Microsoft.Maps.Location(43.83516774651179, -79.4713894693196),
        new Microsoft.Maps.Location(43.85200562523269, -79.43980377596023),
        new Microsoft.Maps.Location(43.84804419897186, -79.41371124666335),
        new Microsoft.Maps.Location(43.78363414644449, -79.31895416658523),
        new Microsoft.Maps.Location(43.81138853307959, -79.31483429353835),
        new Microsoft.Maps.Location(43.80940650442047, -79.28187530916335),
        new Microsoft.Maps.Location(43.82625165144268, -79.29698151033523),
        new Microsoft.Maps.Location(43.80445117525613, -79.21630291037074),
        new Microsoft.Maps.Location(43.793547937534456, -79.27260784201137),
        new Microsoft.Maps.Location(43.839130058111316, -79.25612834982387),
    ];
    for (var i = myRequestLocs.length - 1; i >= 0; i--) {
        var myPush = myRequestLocs[i];
        // var newpushpin = new Microsoft.Maps.Pushpin(myPush,null);
        //map.entities.push(newpushpin);

        var pin = createFontPushpin(myPush, '\uF015', 'FontAwesome', 13, 'red');
        //Add the pushpin to the map
        map.entities.push(pin);
    }

}

function showRequests() {
    if (top.document.getElementById("showRequests").value != "Show Requested Addresses") {
        clearPushpins();
        top.document.getElementById("showRequests").value = "Show Requested Addresses";
    } else {
        createRequested();
        top.document.getElementById("showRequests").value = "Clear Requested Addresses";
    }
}



function pointInPolygon(points, lat, lon) {
    var i;
    var j = points.length - 1;
    var inPoly = false;

    for (i = 0; i < points.length; i++) {
        if (points[i].longitude < lon && points[j].longitude >= lon ||
            points[j].longitude < lon && points[i].longitude >= lon) {
            if (points[i].latitude + (lon - points[i].longitude) /
                (points[j].longitude - points[i].longitude) * (points[j].latitude -
                    points[i].latitude) < lat) {
                inPoly = !inPoly;
            }
        }
        j = i;
    }
    return inPoly;
}

function loadMapScenario() {
    myHome = new Microsoft.Maps.Location(43.64996210156162, -79.42098968904615);
    var navigationBarMode = Microsoft.Maps.NavigationBarMode;
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: 'AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA',
        center: new Microsoft.Maps.Location(43.64996210156162, -79.42098968904615),
        mapTypeId: Microsoft.Maps.MapTypeId.aerial,
        navigationBarMode: navigationBarMode.compact,
        zoom: 10,
    });

    //mybox = map.LocationRect(myHome,1,1);
    //if (mybox.contains(myHome)){alert("mybox contains myHome");}
    Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', function() {
        var manager = new Microsoft.Maps.AutosuggestManager({ map: map });
        manager.attachAutosuggest('#searchBox', '#searchBoxContainer', selectedSuggestion);
    });

    var tempHolder2 = [];
    var tempHolder = [];

    // Microsoft.Maps.Events.addHandler(map, 'click', function (e) {;
    //         //if (e.targetType == "map") {
    //         var point = new Microsoft.Maps.Point(e.getX(), e.getY());
    //         var loc = e.target.tryPixelToLocation(point);
    //         var location = new Microsoft.Maps.Location(loc.latitude, loc.longitude);
    //         top.document.getElementById("tempHolderSpan").innerHTML += "<li>new Microsoft.Maps.Location(" + loc.latitude + ",  " + loc.longitude + "),</li>";
    //          tempHolder = tempHolder2.concat(location,tempHolder);
    //            polygon = new Microsoft.Maps.Polygon(tempHolder, {
    //             fillColor: 'rgba(0, 255, 0, 0.5)',
    //             strokeColor: 'red',
    //             strokeThickness: 3
    //         });
    //             map.entities.pop(polygon)
    //             //Add the polygon to map
    //             map.entities.push(polygon);
    //         //}
    // });

    var center = map.getCenter();
    var polygon;
    //Create array of locations to form a ring.
    loadMaps();
}
// function createDeliveryArea2(Initing) {
//     var exteriorRing = [
// new Microsoft.Maps.Location(43.56003065539521, -79.61158674638868),
// new Microsoft.Maps.Location(43.54062192736565, -79.5854942170918),
// new Microsoft.Maps.Location(43.534150962462746, -79.59373396318556),
// new Microsoft.Maps.Location(43.52120694936544, -79.5964805452168),
// new Microsoft.Maps.Location(43.509256164195875, -79.595793899709),
// new Microsoft.Maps.Location(43.49580870191352, -79.59922712724806),
// new Microsoft.Maps.Location(43.48109633810351, -79.61570661943556),
// new Microsoft.Maps.Location(43.46814094416184, -79.6342460481465),
// new Microsoft.Maps.Location(43.45817336656578, -79.64248579424024),
// new Microsoft.Maps.Location(43.475615548859274, -79.66926496904493),
// new Microsoft.Maps.Location(43.501520354489244, -79.66857832353712),
// new Microsoft.Maps.Location(43.55578475890212, -79.60884016435743)
//     ];
//  //Create a polygon
//     polygon2 = new Microsoft.Maps.Polygon(exteriorRing, {
//         fillColor: 'rgba(0, 255, 0, 0.5)',
//         strokeColor: 'red',
//         strokeThickness: 2
//     });
//      map.entities.push(polygon2);
//     if (Initing == false) {
//         resizeMapWindow();
//     };
// }
// function createDeliveryArea(Initing) {
//     var exteriorRing = [
//         new Microsoft.Maps.Location(43.58852173036803, -79.53497284334303),
//         new Microsoft.Maps.Location(43.58842227813971, -79.53634613435865),
//         new Microsoft.Maps.Location(43.61378152265527, -79.55213898103834),
//         new Microsoft.Maps.Location(43.67241489267107, -79.57548492830396),
//         new Microsoft.Maps.Location(43.70667399177409, -79.5555722085774),
//         new Microsoft.Maps.Location(43.731487197930384, -79.4457089273274),
//         new Microsoft.Maps.Location(43.76234047426849, -79.39764374178053),
//         new Microsoft.Maps.Location(43.767299325631875, -79.3633114663899),
//         new Microsoft.Maps.Location(43.767299325631875, -79.34271210115553),
//         new Microsoft.Maps.Location(43.78812201249422, -79.23010223787428),
//         new Microsoft.Maps.Location(43.72960174598528, -79.21224945467115),
//         new Microsoft.Maps.Location(43.70379498564007, -79.23834198396803),
//         new Microsoft.Maps.Location(43.675990660512994, -79.27816742342115),
//         new Microsoft.Maps.Location(43.65711609928246, -79.31661957185865),
//         new Microsoft.Maps.Location(43.64618600915746, -79.35919159334303),
//         new Microsoft.Maps.Location(43.632272123628894, -79.41549652498365),
//         new Microsoft.Maps.Location(43.6362478374456, -79.45806854646803),
//         new Microsoft.Maps.Location(43.628296116453576, -79.4786679117024),
//         new Microsoft.Maps.Location(43.59647871168799, -79.49102753084303),
//         new Microsoft.Maps.Location(43.58155850872511, -79.52673309724928)
//     ];

//     //Create a polygon
//     polygon = new Microsoft.Maps.Polygon(exteriorRing, {
//         fillColor: 'rgba(0, 255, 0, 0.5)',
//         strokeColor: 'red',
//         strokeThickness: 2
//     });

//     // Add the polygon to map
//     map.entities.push(polygon);
//     if (Initing == false) {
//         resizeMapWindow();
//     };
//     //resizeMapWindow();
// }

function resizeMapWindow() {
    if (pushpin == null || polygon == null) { return; }
    var polyLocs = polygon.getLocations();
    var pushLocs = pushpin.getLocation();
    allLocs = polyLocs.concat(pushLocs);
    var rect = Microsoft.Maps.LocationRect.fromLocations(allLocs);
    map.setView({

        center: location,
        bounds: rect,
        padding: 80,
    });
}

function toggleDeliveryArea() {
    if (top.document.getElementById("deliveryArea").value == "Show Delivery Area") {
        createDeliveryArea(false);
        top.document.getElementById("deliveryArea").value = "Hide Delivery Area";
    } else {
        for (var i = map.entities.getLength() - 1; i >= 0; i--) {
            var myPoly = map.entities.get(i);
            if (myPoly instanceof Microsoft.Maps.Polygon) {
                map.entities.removeAt(i);
            }
        }
        top.document.getElementById("deliveryArea").value = "Show Delivery Area";
    }
    resizeMapWindow();
}

function clearPushpins() {
    for (var i = map.entities.getLength() - 1; i >= 0; i--) {
        var myPush = map.entities.get(i);
        if (myPush instanceof Microsoft.Maps.Pushpin) {
            map.entities.removeAt(i);
        }
    }
    resizeMapWindow();
}

function SetupModal(){

var xhttp = new XMLHttpRequest();
    var datevar = Date().substr(0,Date().indexOf("GMT")-10);
    xhttp.onreadystatechange = function() {
        //alert("got return from xhttp readyState:" + this.readyState + " status:" + this.status + " text:" + this.responseText);
        if (this.readyState == 4 && this.status == 200) {
           
            document.getElementById("demo").innerHTML =  "Order was posted to orders table";
        }
    };
    
    xhttp.open("POST", "/delivery/ModalAction", true);
    xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    //xhttp.send("fname=Henry&lname=Ford");
    //return;
    var buildstr2 = "";
    for (var i in simpleCart.items()){
        buildstr2 +=  simpleCart.items()[i].get('quantity') + "x" + simpleCart.items()[i].get('name') + " at: $" + simpleCart.items()[i].get('price') + " for a total of: $" +  simpleCart.items()[i].get('total') +  '  \n\r';
    }
    buildstr2 += "Grand Total: $" + simpleCart.grandTotal();
     var buildStr = ""
    //buildStr += "OrderDate='" + datevar + "'&";
    //buildStr += "DeliveryDate='" + datevar + "'&";
    buildStr += "Total=" +  simpleCart.grandTotal() + "&";    //"1" + "&";  
    buildStr += "GeocodedAddress='" + top.document.getElementById("DeliveryAddress").value + "'&";
    //buildStr += "Weight=" + "7" + "&";
    buildStr += "PaymentType=" + "1" + "&";
    buildStr += "Details='" +  buildstr2 + "'&";   // "test details" + "'&";   
    buildStr += "SpecialInstructions='" + top.document.getElementById("SpecialInstructions").value + "'&";
    buildStr += "Status=" + "1" + "&";
    //buildStr += "DriverId=" + "1" + "&";
    //buildStr += "CustomerId=" + "1" ;
    xhttp.send(buildStr); 






}
//       function CheckAddress(){
//            var address = encodeURIComponent( top.document.getElementById("tags").value);
//             geocodeRequest2 = "https://dev.virtualearth.net/REST/v1/Locations?query=" + address + "&jsonp=GeocodeCallback&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
//           CallRestService2(geocodeRequest2);

//       };
//       function  VerifyAddress(){
//             geocodeRequest2 = "https://dev.virtualearth.net/REST/v1/Locations?query=" + encodeURIComponent(top.document.getElementById('address').value) + "&jsonp=GeocodeCallback&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
//           CallRestService2(geocodeRequest2);
//           //CallRestService(geocodeRequest, GeocodeCallback);
//                      };
//       function CallRestService2(request) {
//             var script = document.createElement("script");
//             script.setAttribute("id", "myscript");
//             script.setAttribute("type", "text/javascript");
//             script.setAttribute("src", request);
//             document.body.appendChild(script);
//         }



//     function GeocodeCallback(result)  {
//          if (result.success=true){

//         //  alert(result.resourceSets[0].resources[0].geocodePoints[0].coordinates[0] + "   " + result.resourceSets[0].resources[0].geocodePoints[0].coordinates[1]);
//         var location = new Microsoft.Maps.Location(result.resourceSets[0].resources[0].geocodePoints[0].coordinates[0], result.resourceSets[0].resources[0].geocodePoints[0].coordinates[1]);
//          pushpin = new Microsoft.Maps.Pushpin(location, { icon: 'https://ecn.dev.virtualearth.net/mapcontrol/v7.0/7.0.20150902134620.61/i/poi_search.png',
//             anchor: new Microsoft.Maps.Point(12, 39) });
//          //var pushpin = new Microsoft.Maps.Pushpin(location, null);
//          map.entities.push(pushpin);
//          resizeMapWindow();
//         };
//     }


// function CallRestService(request, callback) {
//     $.ajax({
//         url: request,
//         dataType: "jsonp",
//         jsonp: "jsonp",
//         success: function (r) {
//             callback(r);
//         },
//         error: function (e) {
//             alert(e.statusText);
//         }
//     });
// }


//     $m = jQuery.noConflict();

//     $m(function () {
//         var availableTags = [
//           "ActionScript"
//         ];
//         $m("#tags").autocomplete({
//             source: function (request, response) {
//                 $.ajax({
//                     url: "https://dev.virtualearth.net/REST/v1/Locations",
//                     dataType: "jsonp",
//                     data: {
//                         key: "AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA",
//                         q: request.term
//                     },
//                     jsonp: "jsonp",
//                     success: function (data) {
//                         var result = data.resourceSets[0];
//                         if (result) {
//                             if (result.estimatedTotal > 0) {
//                                 response($.map(result.resources, function (item) {
//                                     return {
//                                         data: item,
//                                         label: item.name + ' (' + item.address.countryRegion + ')',
//                                         value: item.name
//                                     }
//                                 }));
//                             }
//                         }
//                     }
//                 });
//             },
//             messages: {
//                 noResults: '',
//                 results: function() {},
//             },

//             minLength: 1,
//             //change: function( event, ui ) {
//             //     alert( "Handler for change() finally called." );
//             //},
//         });
//         // $m( "#tags" ).on( "autocompletechange", function( event, ui ) {
//         //     CheckAddress();
//         // });

//         //$m("#tags").click(function() {
//         //    alert( "Handler for .click() called." );
//         //}).change();


//     });