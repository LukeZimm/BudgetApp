using BudgetApp.Context;
using BudgetApp.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Controllers
{
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]
    public class DatabaseController : ControllerBase
    {
        private readonly ApplicationContext _dbcontext;
        public DatabaseController(ApplicationContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        [HttpPost("api/auth")]
        public async Task<AuthResponse> Auth(AuthRequest request)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(m => m.Email == request.email);
            return new AuthResponse { accountExists = user != null };
            //return new Value() { value = request.email };
        }
    }
}
