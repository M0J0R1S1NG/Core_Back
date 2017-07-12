using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Core.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Models.ManageViewModels;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;


using Microsoft.AspNetCore.Mvc.Rendering;


using Core.Models.AccountViewModels;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Twilio;
using Twilio.TwiML;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Core.Helpers.BingMapTypes;
using Core.Controllers;
using System.Text.Encodings;


namespace Core.Controllers
{
    [Authorize]
    public class DeliveryController : Controller
    {   
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly string _externalCookieScheme;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;



        public DeliveryController(ApplicationDbContext context,
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<IdentityCookieOptions> identityCookieOptions,
          IEmailSender emailSender,
          ISmsSender smsSender,
          ILoggerFactory loggerFactory)
        {   _context = context;   
            _userManager = userManager;
            _signInManager = signInManager;
            _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<ManageController>();
        }
        [AllowAnonymous]
        public async Task <IActionResult> Index()
        {   
            ViewBag.emailAddress=ViewBag.emailAddress;
            ViewBag.searchBox=ViewBag.searchBox;
           ViewBag.IsAdmin=User.IsInRole("Admin");           
            List<DeliveryArea> deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0).ToList();
            return View(deliveryareas);
        }
        [AllowAnonymous]
        public IActionResult Default()
        {
            return View();
        }
        
         [HttpGet]
         [AllowAnonymous]
         public ActionResult ModalAction(int Id=0,string title="No Title",string message="No Message")
        {

            ViewBag.Id = Id;
            ViewBag.Message=message;
            ViewBag.Title=title;
            BootstrapModel myBoot= new BootstrapModel { };
            myBoot.OnlyCancelButton=true;
            return PartialView("ModalContent",  myBoot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task <IActionResult> AddEmail(string searchBox, string emailAddress)
        {
           TempData["emailAddress"] =emailAddress;

           
            Core.Models.Address newadd = new Core.Models.Address();
            newadd.Name = emailAddress;
            newadd.FullAddress = searchBox;
            newadd.CreatedUser= Guid.NewGuid();
            newadd.Status = 3;
            newadd.Geocoded = true;
            await _context.Addresses.AddAsync(newadd);
            await _context.SaveChangesAsync();

            List<DeliveryArea> deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0).ToList();
            return RedirectToAction("index");

        }
        public async Task <IActionResult>  CheckAddress(string address){
            string DeliveryAddress=address;
            string vaout="";
            using (var client = new HttpClient())
                {
                    try
                    {
                            var geocodeRequest="";
                            geocodeRequest = "https://dev.virtualearth.net/REST/v1/Locations?query=";
                            geocodeRequest+=DeliveryAddress;
                            //var identityTypes='&includeEntityTypes=Address,Neighborhood,PopulatedPlace,Postcode1,AdminDivision1,AdminDivision2,CountryRegion';
                            //geocodeRequest+= identityTypes ;
                            geocodeRequest+= "&includeNeighborhood=true";
                            geocodeRequest+= "&jsonp=GeocodeCallbackGeo&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA";
                           // CallRestService( encodeURI(geocodeRequest));


                           
                        client.BaseAddress = new Uri("https://dev.virtualearth.net");
                        var response = await client.GetAsync($"/REST/v1/Locations?Query={DeliveryAddress}&key=AsBPQXiDKMHud6u68TPcW7rq2UpVmTegFhU7Im1eLE_pFgiEbGLXtoa4xSu2R5fA&includeNeighborhood=true");
                        response.EnsureSuccessStatusCode();
                        var stringResult = await response.Content.ReadAsStringAsync();
                        var  retLocation = JsonConvert.DeserializeObject<Core.Helpers.BingMapTypes.RootObject>(stringResult);
                         Response.ContentType="text/plain";

                           var   addressName=retLocation.resourceSets[0].resources[0].name;
                           var   addressLat= retLocation.resourceSets[0].resources[0].point.coordinates[0];
                           var   addressLon=  retLocation.resourceSets[0].resources[0].point.coordinates[1];
                           var   formattedAddress= retLocation.resourceSets[0].resources[0].address.formattedAddress;
                           var   postalCode=  retLocation.resourceSets[0].resources[0].address.postalCode;
                           var   adminDistrict =retLocation.resourceSets[0].resources[0].address.adminDistrict;
                           var   adminDistrict2 = retLocation.resourceSets[0].resources[0].address.adminDistrict2;
                           var   countryRegion = retLocation.resourceSets[0].resources[0].address.countryRegion;
                           var   locality = retLocation.resourceSets[0].resources[0].address.locality;
                           var   neighbourhood = retLocation.resourceSets[0].resources[0].entityType[0];
                           var   addressLine =retLocation.resourceSets[0].resources[0].address.addressLine;
                               
                            


                           List<DeliveryArea> DeliveryAreas = _context.DeliveryAreas.Where(x=> x.Status>=0).ToList();
                            Points dumbPoint =new Points();
                            List<Points> newPoints = new List<Points>{
                                dumbPoint
                            };
                            newPoints.RemoveAt(0);
                            
                            double valLat=0,valLon=0;
                            char[]  delimiters = {')',','};
                            int oddeven=0;
                        foreach(var thisArea in DeliveryAreas){
                              var  rectPoints=thisArea.Points.Replace("new Microsoft.Maps.Location", "").Split(delimiters);  
                                foreach (var str in rectPoints){
                                    
                                    if (str!=""){
                                        if (oddeven==0){
                                            valLat=  Convert.ToDouble(str.Replace(" ","").Replace("(",""));
                                            oddeven=1;
                                        }else{
                                            valLon=  Convert.ToDouble(str.Replace(" ","").Replace("(",""));
                                            Points thisPoint =new Points();
                                            thisPoint.latitude = valLat;
                                            thisPoint.longitude =valLon;
                                            newPoints.Add(thisPoint);
                                            oddeven=0;

                                        }
                                    }
                                }
                                
                                
                                var headstr="";
                                
                                GeoFunctions geoFuncs = new GeoFunctions();
                               bool InDeliveryArea= geoFuncs.pointInPolygon(newPoints,addressLat ,addressLon);
                                newPoints.RemoveRange(0,newPoints.Count());
                                    if(InDeliveryArea ){ 
                                        headstr = $"All set you are in a 30 minute delivery area Goto UberDuber.com to create an account then come back here to place an order or COME BACK SOON we are working on sms automated registration"+ (char)10;
                                        
                                        return View(DeliveryAreas);
                                    }else{
                                        
                                        headstr= $"Sorry you are not in a delivery area.  We are expanding daily go to UberDuber.com and enter you information and we will notify you when your area opens up."+ (char)10;
                                        return View(DeliveryAreas);
                                    }
                                    
                        }
                          
                        
                            
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        byte[] byteArray = Encoding.UTF8.GetBytes(httpRequestException.Message);
                        //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
                        MemoryStream stream = new MemoryStream(byteArray);
                        //StreamReader reader = new StreamReader(stream);
                        //string text = reader.ReadToEnd();
                        IActionResult myoutRes = StatusCode(200);
                        Response.Body= stream;
                        Response.ContentType="text/plain";
                        return myoutRes;  
                    }
                    
                    return View(_context.DeliveryAreas.Where(c=> c.Status>=0).ToList());
                }


            }
 
    }
}

