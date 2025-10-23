using Meos_API.Data;
using Meos_Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Meos_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
         private readonly AppDbContext _dbcontext;

        public UsersController(AppDbContext context)
        {
            _dbcontext = context;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<UsersClass>>> GetUsers()
        {
            var users = await _dbcontext.Users
                .AsNoTracking()
                .ToListAsync();

            return Ok(users);
        }
    }
}
