using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Microsoft.AspNetCore.Authorization;
using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Controllers
{
        [Authorize(Roles="Admin")]
    public class DeliveryAreasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryAreasController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;   
            _userManager = userManager; 
        }

        // GET: DeliveryAreas
        public async Task<IActionResult> Index()
        {         
            // DeliveryArea query =
	        //         from d in _context.DeliveryAreas  
	        //         select new DeliveryArea { 
            //             Status = d.Status,
            //             Description = d.Description, 
            //             Open = d.Open, 
            //             ClosedTime = d.ClosedTime,
            //             OpenTime =  d.OpenTime,
            //             Points = d.Points,
            //             Name = d.Name,
            //             Partner = d.Partner,
            //             CreatedDate = d.CreatedDate,
            //             CreatedBy = d.CreatedBy,
            //              Name = (from f in _context.Users
            //             where (f.Id == d.CreatedBy.ToString())
            //             select new  {  UserName = f.Name }).First()
            //     	};
            

             var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(await _context.DeliveryArea.ToListAsync());
        }

        // GET: DeliveryAreas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryArea = await _context.DeliveryArea
                .SingleOrDefaultAsync(m => m.ID == id);
            if (deliveryArea == null)
            {
                return NotFound();
            }
                    var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(deliveryArea);
        }

        // GET: DeliveryAreas/Create
        public IActionResult Create()
        {
                    var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
           
           
            
            ViewBag.Createdby = _userManager.GetUserId(User);
            return View();
        }

        // POST: DeliveryAreas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CreatedBy,CreatedDate,Partner,Points,Name,Description,OpenTime,ClosedTime,Open,Status")] DeliveryArea deliveryArea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(deliveryArea);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(deliveryArea);
        }

        // GET: DeliveryAreas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryArea = await _context.DeliveryArea.SingleOrDefaultAsync(m => m.ID == id);
            if (deliveryArea == null)
            {
                return NotFound();
            }
            var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(deliveryArea);
        }

        // POST: DeliveryAreas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CreatedBy,CreatedDate,Partner,Points,Name,Description,OpenTime,ClosedTime,Open,Status")] DeliveryArea deliveryArea)
        {
            if (id != deliveryArea.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deliveryArea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DeliveryAreaExists(deliveryArea.ID))
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
            return View(deliveryArea);
        }

        // GET: DeliveryAreas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryArea = await _context.DeliveryArea
                .SingleOrDefaultAsync(m => m.ID == id);
            if (deliveryArea == null)
            {
                return NotFound();
            }
var userGuids = _context.Users.OrderBy(c => c.Id).Select(x => new { Id = x.Id, Value = x.UserName });
                    ViewBag.userGuids = new SelectList(userGuids, "Id", "Value");
            return View(deliveryArea);
        }

        // POST: DeliveryAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deliveryArea = await _context.DeliveryArea.SingleOrDefaultAsync(m => m.ID == id);
            _context.DeliveryArea.Remove(deliveryArea);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DeliveryAreaExists(int id)
        {
            return _context.DeliveryArea.Any(e => e.ID == id);
        }
    }





}


