using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Models;

namespace TestAPI.Controllers
{
    [Route("api/equip")]
    [ApiController]
    public class EquipsController : ControllerBase
    {
        private readonly WebApiContext _context;

        public EquipsController(WebApiContext context)
        {
            _context = context;
        }

        // GET: api/Equips
        [HttpGet]
        public IEnumerable<Equips> GetEquips()
        {
            return _context.Equips;
        }

        // GET: api/Equips/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEquips([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equips = await _context.Equips.FindAsync(id);

            if (equips == null)
            {
                return NotFound();
            }

            return Ok(equips);
        }

        // PUT: api/Equips/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquips([FromRoute] int id, [FromBody] Equips equips)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != equips.EquipID)
            {
                return BadRequest();
            }

            _context.Entry(equips).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EquipsExists(id))
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

        // POST: api/Equips
        [HttpPost]
        public async Task<IActionResult> PostEquips([FromBody] Equips equips)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Equips.Add(equips);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEquips", new { id = equips.EquipID }, equips);
        }

        // DELETE: api/Equips/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquips([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var equips = await _context.Equips.FindAsync(id);
            if (equips == null)
            {
                return NotFound();
            }

            _context.Equips.Remove(equips);
            await _context.SaveChangesAsync();

            return Ok(equips);
        }

        private bool EquipsExists(int id)
        {
            return _context.Equips.Any(e => e.EquipID == id);
        }
    }
}