using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DisasterResourceAllocation.Models;
using DisasterResourceAllocation.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using Newtonsoft.Json;


namespace DisasterResourceAllocation.Controllers
{
    [Route("api/assignments")]
    [ApiController]
    public class AssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConnectionMultiplexer _redis;
        private const string cachedAssign = "cachedAssignments";
        public AssignmentController(ApplicationDbContext context,
        IConnectionMultiplexer redis
        )
        {
            _context = context;
            _redis = redis;
        }

        [HttpGet]
        public async Task<ActionResult> GetAssignments()
        {
            var db = _redis.GetDatabase();
            var cached = await db.StringGetAsync(cachedAssign);
            if (!cached.IsNullOrEmpty)
            {
                var assignments = JsonConvert.DeserializeObject<ResultAssignment>(cached.ToString());
                return Ok(assignments);
            }
            return Ok( new ResultAssignment());
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<Truck>> GetTruckById(string id)
        // {
        //     try
        //     {
        //         var truck = await _context.Trucks.FindAsync(id);
        //         return Ok(truck);
        //     }
        //     catch (KeyNotFoundException)
        //     {
        //         return NotFound();
        //     }
        // }

        [HttpPost]
        public async Task<ActionResult> PostAssignment()
        {
            var trucks = await _context.Trucks.ToListAsync();
            var areas = await _context.Areas.ToListAsync();
            var areas_sort = areas.OrderByDescending(a => a.UrgencyLevel).ToList();
            var trucksList = new List<Truck>(trucks);
            var matchedList = new List<(string AreaID, string TruckID)>();
            var assignmentValue = new List<Assignment>();
            var unmatchedAreas = new List<FailedAssignment>();
            foreach (var area in areas_sort)
            {
                bool areaMatched = false;
                var reasons = new List<string>();

                if (trucksList.Count == 0)
                {
                    reasons.Add("No Available Truck");
                    unmatchedAreas.Add(new FailedAssignment
                    {
                        AreaID = area.AreaID,
                        Reason = reasons
                    });
                    continue; 
                }

                foreach (var truck in trucksList.ToList())
                {
                    bool inTimeConstraint = truck.TravelTimeToArea.TryGetValue(area.AreaID, out int TravelTime) && TravelTime <= area.TimeConstraint;
                    bool hasResources = area.RequiredResources.All(rq => truck.AvailableResources.ContainsKey(rq.Key) && truck.AvailableResources[rq.Key] >= rq.Value);
                    if (inTimeConstraint && hasResources)
                    {
                        matchedList.Add((area.AreaID, truck.TruckID));
                        trucksList.Remove(truck);
                        areaMatched = true;
                        break;
                    }
                    else
                    {
                        if (!inTimeConstraint && !reasons.Contains("No trucks available to meet the time constraint")) reasons.Add("No trucks available to meet the time constraint");
                        if (!hasResources && !reasons.Contains("Insufficient resources")) reasons.Add("Insufficient resources");
                    }
                }
                if (!areaMatched)
                {
                    unmatchedAreas.Add(new FailedAssignment
                    {
                        AreaID = area.AreaID,
                        Reason = reasons
                    });
                }
            }
            foreach (var match in matchedList)
            {
                var area = areas.FirstOrDefault(a => a.AreaID == match.AreaID);
                var truck = trucks.FirstOrDefault(t => t.TruckID == match.TruckID);

                if (area != null && truck != null)
                {
                    assignmentValue.Add(new Assignment
                    {
                        AreaID = match.AreaID,
                        TruckID = match.TruckID,
                        ResourcesDelivery = area.RequiredResources
                    });

                }

            }
            var result = new ResultAssignment
            {
                Assignments = assignmentValue,
                FailedAssignments = unmatchedAreas
            };
            var db = _redis.GetDatabase();
            var JSON = JsonConvert.SerializeObject(result);
            await db.StringSetAsync(cachedAssign, JSON, TimeSpan.FromMinutes(30));
            return Ok(result);
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> PutTModel(int id, Truck truck)
        // {
        //     // TODO: Your code here
        //     await Task.Yield();

        //     return NoContent();
        // }

        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment()
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(cachedAssign);
            return NoContent();
        }
    }
}