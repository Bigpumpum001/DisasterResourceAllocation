using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DisasterResourceAllocation.Models;
using DisasterResourceAllocation.Data;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace DisasterResourceAllocation.Controllers
{
    [Route("api/areas")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AreaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Area>>> GetAreas()
        {
            return await _context.Areas.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Area>> GetAreaById(string id)
        {
            try
            {
                var area = await _context.Areas.FindAsync(id);
                return Ok(area);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Area>> PostArea(Area area)
        {
            await _context.Areas.AddAsync(area);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAreaById), new { id = area.AreaID }, area);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTModel(int id, Area area)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Area>> DeleteAreaById(int id)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return null;
        // }
    }
}
