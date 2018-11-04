using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using ASPNZBat.Models;

namespace ASPNZBat.Controllers
{
    public class AdminDatasController : Controller
    {
        private readonly SeatBookingDBContext _context;

        public AdminDatasController(SeatBookingDBContext context)
        {
            _context = context;
        }

        // GET: AdminDatas
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdminData.ToListAsync());
        }

        // GET: AdminDatas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminData = await _context.AdminData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminData == null)
            {
                return NotFound();
            }

            return View(adminData);
        }

        // GET: AdminDatas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminDatas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,isVisibleS1L,isVisibleS2L,isVisibleS3L,isVisibleS4L,isVisibleS5L,isVisibleS6L,isVisibleS7L,isVisibleS8L,isVisibleS9L,isVisibleS10L,isVisibleS11L,isVisibleS12L,isVisibleS13L,isVisibleS14L,isVisibleS15L,isVisibleS16L")] AdminData adminData)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adminData);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adminData);
        }

        // GET: AdminDatas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminData = await _context.AdminData.FindAsync(id);
            if (adminData == null)
            {
                return NotFound();
            }
            return View(adminData);
        }

        // POST: AdminDatas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,isVisibleS1L,isVisibleS2L,isVisibleS3L,isVisibleS4L,isVisibleS5L,isVisibleS6L,isVisibleS7L,isVisibleS8L,isVisibleS9L,isVisibleS10L,isVisibleS11L,isVisibleS12L,isVisibleS13L,isVisibleS14L,isVisibleS15L,isVisibleS16L")] AdminData adminData)
        {
            if (id != adminData.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adminData);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminDataExists(adminData.Id))
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
            return View(adminData);
        }

        // GET: AdminDatas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adminData = await _context.AdminData
                .FirstOrDefaultAsync(m => m.Id == id);
            if (adminData == null)
            {
                return NotFound();
            }

            return View(adminData);
        }

        // POST: AdminDatas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var adminData = await _context.AdminData.FindAsync(id);
            _context.AdminData.Remove(adminData);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminDataExists(string id)
        {
            return _context.AdminData.Any(e => e.Id == id);
        }
    }
}
