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
    public class DebitWayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DebitWayController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: DebitWay
        public async Task<IActionResult> Index()
        {
            return View(await _context.DebitWay.ToListAsync());
        }

        // GET: DebitWay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }

            return View(debitWay);
        }

        // GET: DebitWay/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DebitWay/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public async Task<IActionResult> Create([Bind("ID,identifier,vericode,website_unique_id,return_url,transaction_id,merchant_transaction_id,transaction_status,transaction_result,transaction_date,transaction_type,item_name,amount,quantity,item_code,language,email,phone,custom,shipment,first_name,last_name,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,customer_errors_meaning,errors,issuer_name,issuer_confirmation,status,time_stamp")] DebitWay debitWay)
        {
            if (ModelState.IsValid)
            {
                debitWay.transaction_date_datetime=DateTime.Now;
                _context.Add(debitWay);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(debitWay);
        }

        // GET: DebitWay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay.SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }
            return View(debitWay);
        }

        // POST: DebitWay/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,identifier,vericode,website_unique_id,return_url,transaction_id,merchant_transaction_id,transaction_status,transaction_result,transaction_date,transaction_date_datetime,transaction_type,item_name,amount,quantity,item_code,language,email,phone,custom,shipment,first_name,last_name,address,city,state_or_province,zip_or_postal_code,country,shipping_address,shipping_city,shipping_state_or_province,shipping_zip_or_postal_code,shipping_country,customer_errors_meaning,errors,issuer_name,issuer_confirmation,status,time_stamp")] DebitWay debitWay)
        {
            if (id != debitWay.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(debitWay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DebitWayExists(debitWay.ID))
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
            return View(debitWay);
        }

        // GET: DebitWay/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var debitWay = await _context.DebitWay
                .SingleOrDefaultAsync(m => m.ID == id);
            if (debitWay == null)
            {
                return NotFound();
            }

            return View(debitWay);
        }

        // POST: DebitWay/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var debitWay = await _context.DebitWay.SingleOrDefaultAsync(m => m.ID == id);
            _context.DebitWay.Remove(debitWay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool DebitWayExists(int id)
        {
            return _context.DebitWay.Any(e => e.ID == id);
        }
    }
}
