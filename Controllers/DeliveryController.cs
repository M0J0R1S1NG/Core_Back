using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Core.Data;

namespace Core.Controllers
{
    
    public class DeliveryController : Controller
    {   
        private readonly ApplicationDbContext _context;
        public DeliveryController(ApplicationDbContext context){

        _context = context;
        }
        public IActionResult Index()
        {   
            ViewBag.emailAddress=ViewBag.emailAddress;
            ViewBag.searchBox=ViewBag.searchBox;

            List<DeliveryArea> deliveryareas = _context.DeliveryAreas.Where(c=> c.Status>=0).ToList();
            return View(deliveryareas);
        }
        
        public IActionResult Default()
        {
            return View();
        }
         [HttpGet]
         public ActionResult ModalAction(int Id=0,string title="No Title",string message="No Message")
        {

            ViewBag.Id = Id;
            ViewBag.Message=message;
            ViewBag.Title=title;
            return PartialView("ModalContent", new BootstrapModel { });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> AddEmail(string searchBox, string emailAddress)
        {
           TempData["emailAddress"] =emailAddress;

           
            Address newadd = new Core.Models.Address();
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
    }
}

