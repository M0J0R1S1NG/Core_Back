using System;
using Core.Models;
using Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;


namespace Core.Services{



public class GeoFunctions{
       public  bool pointInPolygon(List<Points> points,double lat,double lon) {
            var i=0;
            var j=points.Count()-1;
            var inPoly=false;
            for (i=0; i<points.Count(); i++) 
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
    }
}
