using Meos_API.Data;
using Meos_Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public VehiclesController(AppDbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleClass>>> GetVehicles()
        {
            var vehicles = await _dbcontext.Vehicles
                .Include(v => v.Notes)
                .AsNoTracking()
                .ToListAsync();

            return Ok(vehicles);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<VehicleClass>>> SearchVehicles(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await _dbcontext.Vehicles
                    .AsNoTracking()
                    .ToListAsync();
            }

            var lowerQ = query.Trim();

            var results = await _dbcontext.Vehicles
                .AsNoTracking()
                .Where(v =>
                    EF.Functions.Like(v.Plate!, $"%{lowerQ}%"))
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("{plate}")]
        public async Task<ActionResult<VehicleClass>> GetVehicle(string plate)
        {
            var vehicle = await _dbcontext.Vehicles
                .AsNoTracking()
                .Include(v => v.Person) 
                .Include(v => v.Notes)
                .FirstOrDefaultAsync(v => v.Plate == plate);

            if (vehicle == null)
            {
                return NotFound();
            }

            return Ok(vehicle);

        }
        
        [HttpPost("{plate}/notes")]
        public async Task<ActionResult<VehicleNoteClass>> AddNoteToVehicle(string plate, [FromBody] VehicleNoteClass newNote)
        {
            var vehicleExists = await _dbcontext.Vehicles.AnyAsync(v => v.Plate == plate);
            if (!vehicleExists)
                return NotFound($"vehicle with identifier {plate} not found.");

            var note = new VehicleNoteClass
            {
                Plate = plate,
                Note = newNote.Note
            };

            _dbcontext.VehicleNotes.Add(note);
            await _dbcontext.SaveChangesAsync();

            return Ok(note);
        }

        [HttpGet("{plate}/notes")]
        public async Task<ActionResult<IEnumerable<VehicleNoteClass>>> GetPersonNote(string plate)
        {
            var vehicleExists = await _dbcontext.Vehicles.AnyAsync(v => v.Plate == plate);

            if (!vehicleExists)
            {
                return NotFound();
            }

            var notes = await _dbcontext.VehicleNotes
                .Where(n => n.Plate == plate)
                .OrderByDescending(n => n.Id)
                .ToListAsync();

            return Ok(notes);
        }
    }
}
