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
    public class SiteSettingsController : Controller
    {
        private readonly SeatBookingDBContext _context;

        public SiteSettingsController(SeatBookingDBContext context)
        {
            _context = context;
        }

        // GET: SiteSettings
        public async Task<IActionResult> Index()
        {
            return View(await _context.SiteSettings.ToListAsync());
        }

        // GET: SiteSettings/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var siteSettings = await _context.SiteSettings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siteSettings == null)
            {
                return NotFound();
            }

            return View(siteSettings);
        }

        // GET: SiteSettings/Create
        public IActionResult Create()
        {
            return RedirectToAction("Edit");
        }

        // POST: SiteSettings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,maxSeats,nearlyFullSeats,plentySeats")] SiteSettings siteSettings)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(siteSettings);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            return RedirectToAction("Edit");
        }

        // GET: SiteSettings/Edit/5
        public IActionResult Edit(string id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var siteSettings = _context.SiteSettings.FirstOrDefault();
            //var siteSettings = await _context.SiteSettings.FindAsync(id);
            //if (siteSettings == null)
            //{
            //    return NotFound();
            //}
            return View(siteSettings);
        }

        // POST: SiteSettings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,maxSeats,nearlyFullSeats,plentySeats")] SiteSettings siteSettings)
        {
            if (id != siteSettings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    //get the existing ID so we don't make a new row
                    var rowid = _context.SiteSettings.Select(a => a.Id).FirstOrDefault();
                    siteSettings.Id = rowid;

                    _context.Update(siteSettings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteSettingsExists(siteSettings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Create", "SeatBookings");
            }
            return View(siteSettings);
        }

        // GET: SiteSettings/Delete/5
        public IActionResult Delete(string id)
        {

            return RedirectToAction("Edit");
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var siteSettings = await _context.SiteSettings
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (siteSettings == null)
            //{
            //    return NotFound();
            //}

            //return View(siteSettings);
        }

        // POST: SiteSettings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {

            return RedirectToAction("Edit");
            //var siteSettings = await _context.SiteSettings.FindAsync(id);
            //_context.SiteSettings.Remove(siteSettings);
            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
        }

        private bool SiteSettingsExists(string id)
        {
            return _context.SiteSettings.Any(e => e.Id == id);
        }
    }
}
