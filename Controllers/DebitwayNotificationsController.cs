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
using Microsoft.AspNetCore.Identity;

namespace Core.Controllers
{[Authorize(Roles="Admin")]
    public class DebitWayNotificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DebitWayNotificationsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: DebitWayNotifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.DebitWayNotifications.ToListAsync());
        }

        // GET: DebitWayNotifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWayNotification = await _context.DebitWayNotifications
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWayNotification == null)
            {
                return NotFound();
            }

            return View(debitWayNotification);
        }

        // GET: DebitWayNotifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DebitWayNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,identifier,business,WebSiteId,vericode,item_name,action,merchant_transaction_id,transaction_id,transaction_status,transaction_result,transaction_date,transaction_type,result,net,amount,quantity,gross,discount_fee,additional_fee,processing_rate,item_code,custom,shipment,language,email,first_name,last_name,phone,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,errors,errors_meaning,customer_errors_meaning,issuer_name,issuer_confirmation,status,time_stamp")] DebitWayNotification debitWayNotification)
        {
            if (ModelState.IsValid)
            {
                _context.Add(debitWayNotification);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(debitWayNotification);
        }

        // GET: DebitWayNotifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWayNotification = await _context.DebitWayNotifications.SingleOrDefaultAsync(m => m.ID == id);
            if (debitWayNotification == null)
            {
                return NotFound();
            }
            return View(debitWayNotification);
        }

        // POST: DebitWayNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,identifier,business,WebSiteId,vericode,item_name,action,merchant_transaction_id,transaction_id,transaction_status,transaction_result,transaction_date,transaction_type,result,net,amount,quantity,gross,discount_fee,additional_fee,processing_rate,item_code,custom,shipment,language,email,first_name,last_name,phone,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,errors,errors_meaning,customer_errors_meaning,issuer_name,issuer_confirmation,status,time_stamp")] DebitWayNotification debitWayNotification)
        {
            if (id != debitWayNotification.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debitWayNotification);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebitWayNotificationExists(debitWayNotification.ID))
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
            return View(debitWayNotification);
        }

        // GET: DebitWayNotifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWayNotification = await _context.DebitWayNotifications
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWayNotification == null)
            {
                return NotFound();
            }

            return View(debitWayNotification);
        }

        // POST: DebitWayNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var debitWayNotification = await _context.DebitWayNotifications.SingleOrDefaultAsync(m => m.ID == id);
            _context.DebitWayNotifications.Remove(debitWayNotification);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DebitWayNotificationExists(int id)
        {
            return _context.DebitWayNotifications.Any(e => e.ID == id);
        }
    }
}
