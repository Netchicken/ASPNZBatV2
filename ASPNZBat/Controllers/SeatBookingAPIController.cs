using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNZBat.Data;
using ASPNZBat.Models;

namespace ASPNZBat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatBookingAPIController : ControllerBase
    {
        private readonly SeatBookingDBContext _context;

        public SeatBookingAPIController(SeatBookingDBContext context)
        {
            _context = context;
        }

        // GET: api/SeatBookingAPI
        [HttpGet]
        public IEnumerable<SeatBooking> GetSeatBooking()
        {
            return _context.SeatBooking;
        }

        // GET: api/SeatBookingAPI/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeatBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seatBooking = await _context.SeatBooking.FindAsync(id);

            if (seatBooking == null)
            {
                return NotFound();
            }

            return Ok(seatBooking);
        }

        // PUT: api/SeatBookingAPI/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeatBooking([FromRoute] int id, [FromBody] SeatBooking seatBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seatBooking.ID)
            {
                return BadRequest();
            }

            _context.Entry(seatBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatBookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SeatBookingAPI
        [HttpPost]
        public async Task<IActionResult> PostSeatBooking([FromBody] SeatBooking seatBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SeatBooking.Add(seatBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeatBooking", new { id = seatBooking.ID }, seatBooking);
        }

        // DELETE: api/SeatBookingAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeatBooking([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seatBooking = await _context.SeatBooking.FindAsync(id);
            if (seatBooking == null)
            {
                return NotFound();
            }

            _context.SeatBooking.Remove(seatBooking);
            await _context.SaveChangesAsync();

            return Ok(seatBooking);
        }

        private bool SeatBookingExists(int id)
        {
            return _context.SeatBooking.Any(e => e.ID == id);
        }
    }
}