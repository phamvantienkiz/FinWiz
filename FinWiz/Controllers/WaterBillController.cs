using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinWiz.Models;

namespace FinWiz.Controllers
{
    public class WaterBillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WaterBillController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WaterBill
        public async Task<IActionResult> Index()
        {
            return View(await _context.WaterBill.ToListAsync());
        }

        // GET: WaterBill/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBill
                .FirstOrDefaultAsync(m => m.WaterId == id);
            if (waterBill == null)
            {
                return NotFound();
            }

            return View(waterBill);
        }

        // GET: WaterBill/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WaterBill/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WaterId,UserId,UserName,Address,FirstReading,LastReading,TotalWater,Amount,TotalAmount,Note,Date")] WaterBill waterBill)
        {
            if (ModelState.IsValid)
            {
                _context.Add(waterBill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(waterBill);
        }

        // GET: WaterBill/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBill.FindAsync(id);
            if (waterBill == null)
            {
                return NotFound();
            }
            return View(waterBill);
        }

        // POST: WaterBill/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WaterId,UserId,UserName,Address,FirstReading,LastReading,TotalWater,Amount,TotalAmount,Note,Date")] WaterBill waterBill)
        {
            if (id != waterBill.WaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(waterBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WaterBillExists(waterBill.WaterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(waterBill);
        }

        // GET: WaterBill/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var waterBill = await _context.WaterBill
                .FirstOrDefaultAsync(m => m.WaterId == id);
            if (waterBill == null)
            {
                return NotFound();
            }

            return View(waterBill);
        }

        // POST: WaterBill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var waterBill = await _context.WaterBill.FindAsync(id);
            if (waterBill != null)
            {
                _context.WaterBill.Remove(waterBill);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WaterBillExists(int id)
        {
            return _context.WaterBill.Any(e => e.WaterId == id);
        }
    }
}
