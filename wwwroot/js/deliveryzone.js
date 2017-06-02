    function LoadMap() {
        Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', {
            callback: function () {
                var manager = new Microsoft.Maps.AutosuggestManager({
                    placeSuggestions: false
                });
                manager.attachAutosuggest('#DeliveryAddress', '#searchBoxContainer', selectedSuggestion);
            },
            errorCallback: function(msg){
                alert(msg);
            },
            credentials: 'AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA' 
        });
    }
 
    function selectedSuggestion(result) {
        //Populate the address textbox values.
        if (top.document.getElementById('StreetName')!=null){
            top.document.getElementById('StreetName').value = result.address.addressLine;
            top.document.getElementById('City').value = result.address.locality;
            top.document.getElementById('Province').value = result.address.adminDistrict;
            top.document.getElementById('PostalCode').value = result.address.postalCode;
            top.document.getElementById('Country').value = result.address.countryRegion;
        }

       

         var exteriorRing = [
                new Microsoft.Maps.Location( 43.58852173036803, -79.53497284334303),
                new Microsoft.Maps.Location(43.58842227813971, -79.53634613435865),
                new Microsoft.Maps.Location(43.61378152265527, -79.55213898103834),
                new Microsoft.Maps.Location(43.67241489267107, -79.57548492830396),
                new Microsoft.Maps.Location(43.70667399177409, -79.5555722085774),
                new Microsoft.Maps.Location(43.731487197930384, -79.4457089273274),
                new Microsoft.Maps.Location(43.76234047426849, -79.39764374178053),
                new Microsoft.Maps.Location(43.767299325631875, -79.3633114663899),
                new Microsoft.Maps.Location(43.767299325631875, -79.34271210115553),
                new Microsoft.Maps.Location(43.78812201249422, -79.23010223787428),
                new Microsoft.Maps.Location(43.72960174598528, -79.21224945467115),
                new Microsoft.Maps.Location(43.70379498564007, -79.23834198396803),
                new Microsoft.Maps.Location(43.675990660512994, -79.27816742342115),
                new Microsoft.Maps.Location(43.65711609928246, -79.31661957185865),
                new Microsoft.Maps.Location(43.64618600915746, -79.35919159334303),
                new Microsoft.Maps.Location(43.632272123628894, -79.41549652498365),
                new Microsoft.Maps.Location(43.6362478374456, -79.45806854646803),
                new Microsoft.Maps.Location(43.628296116453576, -79.4786679117024),
                new Microsoft.Maps.Location(43.59647871168799, -79.49102753084303),
                new Microsoft.Maps.Location(43.58155850872511, -79.52673309724928)
            ];

            //Create a polygon
                 polygon = new Microsoft.Maps.Polygon(exteriorRing, {
                    fillColor: 'rgba(0, 255, 0, 0.5)',
                    strokeColor: 'red',
                    strokeThickness: 2
                });

        var points = polygon.getLocations();
        var InDeliveryArea =  pointInPolygon(points,result.location.latitude,result.location.longitude) 
        //map.setView({ bounds: result.bestView });
        var myAnswer = confirm(InDeliveryArea ? "This address qulaifies for immediate delivery and will be dispatched ASAP" : "Sorry this address does not qulaify for immediate delivery your order will ship via Canada Post tonight");
            if (myAnswer){
                top.document.getElementById("DeliveryAddress").value=""
            }else{
                top.document.getElementById("DeliveryAddress").value=""
            }
    }

function pointInPolygon(points,lat,lon) 
{
  var i;
  var j=points.length-1;
  var inPoly=false;

  for (i=0; i<points.length; i++) 
  {
    if (points[i].longitude<lon && points[j].longitude>=lon 
      || points[j].longitude<lon && points[i].longitude>=lon) 
    {
      if (points[i].latitude+(lon-points[i].longitude)/ 
        (points[j].longitude-points[i].longitude)*(points[j].latitude 
          -points[i].latitude)<lat) 
      {
        inPoly=!inPoly; 
      }
    }
    j=i; 
  }
  return inPoly; 
}


