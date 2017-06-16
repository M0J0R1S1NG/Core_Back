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

    public class SMSController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SMSController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: SMS
        public async Task<IActionResult> Index()
        {
            return View(await _context.SMS.ToListAsync());
        }

        // GET: SMS/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sMS = await _context.SMS
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sMS == null)
            {
                return NotFound();
            }

            return View(sMS);
        }

        // GET: SMS/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppUser,SentTo,SentFrom,DateSent,DateRecieved")] SMS sMS)
        {
            if (ModelState.IsValid)
            {
                sMS.Id = Guid.NewGuid();
                _context.Add(sMS);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sMS);
        }

        // GET: SMS/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sMS = await _context.SMS.SingleOrDefaultAsync(m => m.Id == id);
            if (sMS == null)
            {
                return NotFound();
            }
            return View(sMS);
        }

        // POST: SMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AppUser,SentTo,SentFrom,DateSent,DateRecieved")] SMS sMS)
        {
            if (id != sMS.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sMS);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SMSExists(sMS.Id))
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
            return View(sMS);
        }

        // GET: SMS/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sMS = await _context.SMS
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sMS == null)
            {
                return NotFound();
            }

            return View(sMS);
        }

        // POST: SMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var sMS = await _context.SMS.SingleOrDefaultAsync(m => m.Id == id);
            _context.SMS.Remove(sMS);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SMSExists(Guid id)
        {
            return _context.SMS.Any(e => e.Id == id);
        }
    }
}
