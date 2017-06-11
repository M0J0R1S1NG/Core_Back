using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace Core.Controllers
{
    [Authorize(Roles="Admin")]
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(await _context.Vehicles.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View();
        }

        // POST: Vehicles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserGuid,Make,Model,Year,Details,Description,Status,LicensePlate,InsurancePolicy")] Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserGuid,Make,Model,Year,Details,Description,Status,LicensePlate,InsurancePolicy")] Vehicle vehicle)
        {
            if (id != vehicle.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.ID))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .SingleOrDefaultAsync(m => m.ID == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.SingleOrDefaultAsync(m => m.ID == id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.ID == id);
        }
    }
}
