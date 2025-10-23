using Meos_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Meos_Shared;
using Microsoft.AspNetCore.Authorization;
using System.Collections;

namespace Meos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;

        public PersonsController(AppDbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonClass>>> GetPersons()
        {
            var persons = await _dbcontext.Persons
                .Include(p => p.Licenses)
                .Include(p => p.Vehicles)
                .Include(p => p.ArrestWarrant)
                .AsNoTracking()
                .ToListAsync();

            return Ok(persons);
        }

        [HttpGet("{identifier}")]
        public async Task<ActionResult<PersonClass>> GetPerson(string identifier)
        {
            var person = await _dbcontext.Persons
                .Include(p => p.Licenses)
                .Include(p => p.ArrestWarrant)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identifier == identifier);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<PersonClass>>> SearchPersons(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return await _dbcontext.Persons
                    .AsNoTracking()
                    .ToListAsync();
            }

            var lowerQ = query.Trim();

            var results = await _dbcontext.Persons
                .AsNoTracking()
                .Where(p =>
                    EF.Functions.Like(p.FirstName!, $"%{lowerQ}%") ||
                    EF.Functions.Like(p.LastName!, $"%{lowerQ}%") ||
                    EF.Functions.Like(p.Bsn!, $"%{lowerQ}%"))
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet("{identifier}/totalfines")]
        public async Task<ActionResult<TotalFinesDto>> GetPersonTotalFines(string identifier)
        {
            var totalAmount = await _dbcontext.Fines
                .Where(f => EF.Functions.Like(f.Identifier, identifier))
                .SumAsync(f => f.Amount);

            return Ok(new TotalFinesDto { TotalAmount = totalAmount });
        }

        [HttpGet("{identifier}/vehicles")]
        public async Task<ActionResult<VehicleClass>> GetPersonVehicles(string identifier)
        {
            var person = await _dbcontext.Persons
                .Include(p => p.Vehicles)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Identifier == identifier);

            if (person == null)
                return NotFound();

            return Ok(person.Vehicles.ToList());
        }

        [HttpPost("{identifier}/notes")]
        public async Task<ActionResult<PersonNoteClass>> AddNoteToPerson(string identifier, [FromBody] PersonNoteClass newNote)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);
            if (!personExists)
                return NotFound($"Person with identifier {identifier} not found.");

            var note = new PersonNoteClass
            {
                Identifier = identifier,
                Note = newNote.Note
            };

            _dbcontext.PersonNotes.Add(note);
            await _dbcontext.SaveChangesAsync();

            return Ok(note);
        }

        [HttpGet("{identifier}/notes")]
        public async Task<ActionResult<IEnumerable<PersonNoteClass>>> GetPersonNote(string identifier)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);

            if (!personExists)
            {
                return NotFound();
            }

            var notes = await _dbcontext.PersonNotes
                .Where(n => n.Identifier == identifier)
                .OrderByDescending(n => n.Id)
                .ToListAsync();

            return Ok(notes);
        }

        [HttpPost("{identifier}/arrestwarrants")]
        public async Task<ActionResult<ArrestWarrantClass>> AddArrestWarrant(string identifier, [FromBody] ArrestWarrantClass newArrestWarrant)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);
            if (!personExists)
                return NotFound();

            var arrestWarrant = new ArrestWarrantClass
            {
                Identifier = identifier,
                Agent = newArrestWarrant.Agent,
                Date = newArrestWarrant.Date,
                Message = newArrestWarrant.Message
            };

            _dbcontext.ArrestWarrants.Add(arrestWarrant);
            await _dbcontext.SaveChangesAsync();

            return Ok(arrestWarrant);
        }

        [HttpGet("{identifier}/arrestwarrants")]
        public async Task<ActionResult<IEnumerable<ArrestWarrantClass>>> GetPersonArrestWarrant(string identifier)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);

            if (!personExists)
            {
                return NotFound();
            }

            var arrestWarrant = await _dbcontext.ArrestWarrants
                .Where(a => a.Identifier == identifier)
                //.OrderByDescending(a => a.Id)
                .ToListAsync();

            return Ok(arrestWarrant);
        }

        [HttpGet("{identifier}/arrestwarrants/{id}")]
        public async Task<ActionResult<ArrestWarrantClass>> GetArrestWarrantId(int id)
        {
            var arrestWarrant = await _dbcontext.ArrestWarrants.FirstOrDefaultAsync(a => a.Id == id);

            if (arrestWarrant == null)
            {
                return NotFound();
            }
            return Ok(arrestWarrant);
        }

        [HttpDelete("{identifier}/arrestwarrants/{id}")]
        public async Task<ActionResult<ArrestWarrantClass>> DeleteArrestWarrant(int id)
        {
            var arrestWarrant = await _dbcontext.ArrestWarrants.FindAsync(id);

            if (arrestWarrant == null)
            {
                return NotFound();
            }

            _dbcontext.ArrestWarrants.Remove(arrestWarrant);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{identifier}/incidents")]
        public async Task<ActionResult<IncidentClass>> GetPersonIncidents(string identifier)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);

            if (!personExists)
            {
                return NotFound();
            }

            var incidents = await _dbcontext.Incidents
                .Where(i => i.Identifier == identifier)
                .ToListAsync();

            return Ok(incidents);
        }

        [HttpPost("{identifier}/incidents")]
        public async Task<ActionResult<IncidentClass>> AddPersonIncident(string identifier, [FromBody] IncidentClass newIncident)
        {
            var personExists = await _dbcontext.Persons.AnyAsync(p => p.Identifier == identifier);

            if (!personExists)
            {
                return NotFound();
            }

            var incident = new IncidentClass
            {
                Identifier = identifier,
                Artikelen = newIncident.Artikelen,
                IngenomenGoederen = newIncident.IngenomenGoederen,
                Rechten = newIncident.Rechten,
                Agent = newIncident.Agent,
                Datum = newIncident.Datum
            };

            _dbcontext.Incidents.Add(incident);
            await _dbcontext.SaveChangesAsync();

            return Ok(incident);
        }

        [HttpGet("{identifier}/incidents/{incidentid}")]
        public async Task<ActionResult<IncidentClass>> GetIncidentId(int incidentid)
        {
            var incident = await _dbcontext.Incidents.FirstOrDefaultAsync(i => i.IncidentId == incidentid);

            if (incident == null)
            {
                return NotFound();
            }

            return Ok(incident);
        }

        [HttpPost("{identifier}/incidents/{incidentid}/notes")]
        public async Task<ActionResult<IncidentNoteClass>> AddIncidentNote(int incidentid, [FromBody] IncidentNoteClass newNote)
        {
            var incidentExists = await _dbcontext.Incidents.AnyAsync(i => i.IncidentId == incidentid);
            if (!incidentExists)
                return NotFound($"Incident with identifier {incidentid} not found.");

            var note = new IncidentNoteClass
            {
                IncidentId = incidentid,
                Note = newNote.Note
            };

            _dbcontext.IncidentNotes.Add(note);
            await _dbcontext.SaveChangesAsync();

            return Ok(note);
        }

        [HttpGet("{identifier}/incidents/{incidentid}/notes")]
        public async Task<ActionResult<IEnumerable<IncidentNoteClass>>> GetIncidentNote(int incidentid)
        {
            var incidentExists = await _dbcontext.Incidents.AnyAsync(i => i.IncidentId == incidentid);
            if (!incidentExists)
                return NotFound();

            var notes = await _dbcontext.IncidentNotes
                .Where(n => n.IncidentId == incidentid)
                .OrderByDescending(n => n.Id)
                .ToListAsync();

            return Ok(notes);
        }

        [HttpPost("{identifier}/incidents/{incidentid}/users")]
        public async Task<ActionResult> AddIncidentUsers(string identifier, int incidentid, [FromBody] List<int> userIds)
        {
        var incident = await _dbcontext.Incidents
            .Include(i => i.Users) 
            .FirstOrDefaultAsync(i => i.IncidentId == incidentid);

        if (incident == null) return NotFound();

        var usersToAdd = await _dbcontext.Users
            .Where(u => userIds.Contains(u.Id))
            .ToListAsync();

        foreach (var user in usersToAdd)
        {
            if (!incident.Users.Contains(user))
            {
                incident.Users.Add(user);
            }
        }

        await _dbcontext.SaveChangesAsync();

        return Ok(incident);
        }



        [HttpGet("{identifier}/incidents/{incidentid}/users")]
        public async Task<ActionResult<IEnumerable<UsersClass>>> GetIncidentUsers(string identifier, int incidentid)
        {
            var incident = await _dbcontext.Incidents
                .Include(i => i.Users)
                .FirstOrDefaultAsync(i => i.IncidentId == incidentid);

            if (incident == null)
                return NotFound();

            return Ok(incident.Users);
        }
        
        [HttpPost("{identifier}/incidents/{incidentid}/sentence")]
        public async Task<ActionResult<IncidentNoteClass>> AddIncidentSentence(int incidentid, [FromBody] SentenceClass newSentence)
        {
            var incidentExists = await _dbcontext.Incidents.AnyAsync(i => i.IncidentId == incidentid);
            if (!incidentExists)
                return NotFound($"Incident with identifier {incidentid} not found.");

            var sentence = new SentenceClass
            {
                IncidentId = incidentid,
                Fine = newSentence.Fine,
                Type = newSentence.Type,
                Amount = newSentence.Amount
            };

            _dbcontext.Sentence.Add(sentence);
            await _dbcontext.SaveChangesAsync();

            return Ok(sentence);
        }

        [HttpGet("{identifier}/incidents/{incidentid}/sentence")]
        public async Task<ActionResult<IEnumerable<SentenceClass>>> GetIncidentSentence(int incidentid)
        {
            var incidentExists = await _dbcontext.Incidents.AnyAsync(i => i.IncidentId == incidentid);
            if (!incidentExists)
                return NotFound();

            var sentence = await _dbcontext.Sentence
                .Where(s => s.IncidentId == incidentid)
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            return Ok(sentence);
        }
    }
}
