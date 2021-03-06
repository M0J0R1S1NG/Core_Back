using System;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles="Admin")]
    public class PartnersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartnersController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Partners
        public async Task<IActionResult> Index()
        {
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
          
            return View(await _context.Partners.ToListAsync());
        }

        // GET: Partners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners
                .SingleOrDefaultAsync(m => m.Id == id);
            if (partner == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
          
            return View(partner);
        }

        // GET: Partners/Create
        public IActionResult Create()
        {
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.Name).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
                        
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");

            return View();
        }

        // POST: Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GUID,StreetAddress,EmailAddress,City,Province,PostalCode,PhoneNumber,SpecialInstructions,Status,DeliveryArea,SMSNumber,ShippingAddress,Name,Company,TaxId")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(partner);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(partner);
        }

        // GET: Partners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners.SingleOrDefaultAsync(m => m.Id == id);
            if (partner == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
          
            return View(partner);
        }

        // POST: Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GUID,StreetAddress,EmailAddress,City,Province,PostalCode,PhoneNumber,SpecialInstructions,Status,DeliveryArea,SMSNumber,ShippingAddress,Name,Company,TaxId")] Partner partner)
        {
            if (id != partner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(partner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartnerExists(partner.Id))
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
            return View(partner);
        }

        // GET: Partners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var partner = await _context.Partners
                .SingleOrDefaultAsync(m => m.Id == id);
            if (partner == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
            ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            var deliveryareaGuids = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name });
            ViewBag.deliveryAreas = new SelectList(deliveryareaGuids, "Id", "Value");
          
            return View(partner);
        }

        // POST: Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partner = await _context.Partners.SingleOrDefaultAsync(m => m.Id == id);
            _context.Partners.Remove(partner);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PartnerExists(int id)
        {
            return _context.Partners.Any(e => e.Id == id);
        }
    }
}
