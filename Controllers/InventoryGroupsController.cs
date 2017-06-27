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
    public class InventoryGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryGroupsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: InventoryGroups
        public async Task<IActionResult> Index()
        { var InventoryIds = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label  + " " + x.Supplier});
            ViewBag.inventorys = new SelectList(InventoryIds, "Id", "Value");

            var deliveryareaIds = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name + " " + x.Description });
            ViewBag.deliveryAreas = new SelectList(deliveryareaIds, "Id", "Value");
            return View(await _context.InventoryGroups.ToListAsync());
        }

        // GET: InventoryGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryGroup = await _context.InventoryGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (inventoryGroup == null)
            {
                return NotFound();
            }
            var InventoryIds = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label + " " + x.Supplier });
            ViewBag.inventorys = new SelectList(InventoryIds, "Id", "Value");

            var deliveryareaIds = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name + " " + x.Description });
            ViewBag.deliveryAreas = new SelectList(deliveryareaIds, "Id", "Value");
            return View(inventoryGroup);
        }

        // GET: InventoryGroups/Create
        public IActionResult Create()
        {
            var InventoryIds = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label + " " + x.Supplier });
            ViewBag.inventorys = new SelectList(InventoryIds, "Id", "Value");

            var deliveryareaIds = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name  + " " + x.Description});
            ViewBag.deliveryAreas = new SelectList(deliveryareaIds, "Id", "Value");
            return View();
        }

        // POST: InventoryGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,InventoryId,DeliveryAreaId")] InventoryGroup inventoryGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(inventoryGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inventoryGroup);
        }

        // GET: InventoryGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryGroup = await _context.InventoryGroups.SingleOrDefaultAsync(m => m.Id == id);
            if (inventoryGroup == null)
            {
                return NotFound();
            }
             var InventoryIds = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label  + " " + x.Supplier});
            ViewBag.inventorys = new SelectList(InventoryIds, "Id", "Value");

            var deliveryareaIds = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name + " " + x.Description });
            ViewBag.deliveryAreas = new SelectList(deliveryareaIds, "Id", "Value");
            return View(inventoryGroup);
        }

        // POST: InventoryGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,InventoryId,DeliveryAreaId")] InventoryGroup inventoryGroup)
        {
            if (id != inventoryGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryGroupExists(inventoryGroup.Id))
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
            return View(inventoryGroup);
        }

        // GET: InventoryGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventoryGroup = await _context.InventoryGroups
                .SingleOrDefaultAsync(m => m.Id == id);
            if (inventoryGroup == null)
            {
                return NotFound();
            }
             var InventoryIds = _context.Inventorys.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Label  + " " + x.Supplier});
            ViewBag.inventorys = new SelectList(InventoryIds, "Id", "Value");

            var deliveryareaIds = _context.DeliveryAreas.OrderBy(c => c.ID).Select(x => new { Id = x.ID, Value = x.Name + " " + x.Description });
            ViewBag.deliveryAreas = new SelectList(deliveryareaIds, "Id", "Value");
            return View(inventoryGroup);
        }

        // POST: InventoryGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inventoryGroup = await _context.InventoryGroups.SingleOrDefaultAsync(m => m.Id == id);
            _context.InventoryGroups.Remove(inventoryGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool InventoryGroupExists(int id)
        {
            return _context.InventoryGroups.Any(e => e.Id == id);
        }
    }
}
