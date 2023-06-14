using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sample_crud_be_2.Models;

namespace sample_crud_be_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        
        public AuthenticationController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;          
        }

        [HttpGet("getUser")]
        public async Task<ActionResult> GetUser(string username, string password, string role)
        {
            if (_applicationDbContext.Users == null)
            {
                return NotFound();
            }

            var users =  _applicationDbContext.Users.ToList();

            User user = users.FirstOrDefault(u => u.Username == username && u.Password == password && u.Role == role);

            if (user == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
