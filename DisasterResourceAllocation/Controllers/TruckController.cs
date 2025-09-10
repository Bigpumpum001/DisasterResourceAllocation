using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DisasterResourceAllocation.Models;
using DisasterResourceAllocation.Data;
using Microsoft.EntityFrameworkCore;

namespace DisasterResourceAllocation.Controllers
{
    [Route("api/trucks")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TruckController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Truck>>> GetTrucks()
        {
            return await _context.Trucks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Truck>> GetTruckById(string id)
        {
            try
            {
                var truck = await _context.Trucks.FindAsync(id);
                return Ok(truck);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Truck>> PostTruck(Truck truck)
        {
            await _context.Trucks.AddAsync(truck);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTruckById), new { id = truck.TruckID }, truck);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTModel(int id, Truck truck)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return NoContent();
        // }

        // [HttpDelete("{id}")]
        // public async Task<ActionResult<Truck>> DeleteTruckById(int id)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return null;
        // }
    }
}