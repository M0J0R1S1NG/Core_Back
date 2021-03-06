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
    public class FranchiseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FranchiseController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Franchise
        public async Task<IActionResult> Index()
        {
            return View(await _context.Franchise.ToListAsync());
        }

        // GET: Franchise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var franchise = await _context.Franchise
                .SingleOrDefaultAsync(m => m.Id == id);
            if (franchise == null)
            {
                return NotFound();
            }

            return View(franchise);
        }

        // GET: Franchise/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Franchise/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,City,ContactDate,HasVehicle,VehicleType,VehicleYear,Consent,StartDate")] Franchise franchise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(franchise);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(franchise);
            
        }

        // GET: Franchise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var franchise = await _context.Franchise.SingleOrDefaultAsync(m => m.Id == id);
            if (franchise == null)
            {
                return NotFound();
            }
            return View(franchise);
        }

        // POST: Franchise/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppUser,City,ContactDate,HasVehicle,VehicleType,VehicleYear,Consent,StartDate")] Franchise franchise)
        {
            if (id != franchise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(franchise);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FranchiseExists(franchise.Id))
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
            return View(franchise);
        }

        // GET: Franchise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var franchise = await _context.Franchise
                .SingleOrDefaultAsync(m => m.Id == id);
            if (franchise == null)
            {
                return NotFound();
            }

            return View(franchise);
        }

        // POST: Franchise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var franchise = await _context.Franchise.SingleOrDefaultAsync(m => m.Id == id);
            _context.Franchise.Remove(franchise);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool FranchiseExists(int id)
        {
            return _context.Franchise.Any(e => e.Id == id);
        }
    }
}
