using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;

namespace Core.Controllers
{
    public class DriversController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DriversController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Drivers
        public async Task<IActionResult> Index()
        {
             var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
                     var vehicleGuids = _context.Vehicles.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Make });
                    ViewBag.vehicles = new SelectList(vehicleGuids, "Id", "Value");
            return View(await _context.Drivers.ToListAsync());
        }

        // GET: Drivers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            }
                 var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
                     var vehicleGuids = _context.Vehicles.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Make });
                    ViewBag.vehicles = new SelectList(vehicleGuids, "Id", "Value");
            return View(driver);
        }

        // GET: Drivers/Create
        public IActionResult Create()
        {
             var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
                    var vehicleGuids = _context.Vehicles.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Make });
                    ViewBag.vehicles = new SelectList(vehicleGuids, "Id", "Value");
            return View();
        }

        // POST: Drivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,GUID,UserGuid,GeocodedAddress,Address1,PostalCode,Details,SpecialInstructions,Status,PhoneNumber,VehicleGuid")] Driver driver)
        {
            if (ModelState.IsValid)
            {
                _context.Add(driver);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        // GET: Drivers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers.SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            }
             var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
                      var vehicleGuids = _context.Vehicles.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Make });
                    ViewBag.vehicles = new SelectList(vehicleGuids, "Id", "Value");
            return View(driver);
        }

        // POST: Drivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,GUID,UserGuid,GeocodedAddress,Address1,PostalCode,Details,SpecialInstructions,Status,PhoneNumber,VehicleGuid")] Driver driver)
        {
            if (id != driver.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(driver);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DriverExists(driver.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(driver);
        }

        // GET: Drivers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var driver = await _context.Drivers
                .SingleOrDefaultAsync(m => m.ID == id);
            if (driver == null)
            {
                return NotFound();
            }
                    var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
                    var vehicleGuids = _context.Vehicles.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Make });
                    ViewBag.vehicles = new SelectList(vehicleGuids, "Id", "Value");
            return View(driver);
        }

        // POST: Drivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var driver = await _context.Drivers.SingleOrDefaultAsync(m => m.ID == id);
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DriverExists(int id)
        {
            return _context.Drivers.Any(e => e.ID == id);
        }
    }
}
