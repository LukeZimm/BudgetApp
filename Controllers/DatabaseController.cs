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
        [HttpPost("api/db/createuser")]
        public async Task<Models.User> CreateUser(Models.User user)
        {
            while (true)
            {
                user.Id = Guid.NewGuid().ToString();
                // check if user.Id already exists in database
                if (!_dbcontext.Users.Any(m => m.Id == user.Id)) break;
            }
            await _dbcontext.Users.AddAsync(user);
            await _dbcontext.SaveChangesAsync();
            return user;
        }
        [HttpPost("api/db/access_token")]
        public async Task<String?> AccessToken(string id)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user != null) return user.AccessToken;
            else return null;
        }
        [HttpPost("api/db/user_id")]
        public async Task<String?> UserId(string email)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(m => m.Email == email);
            if (user != null) return user.Id;
            else return null;
        }
    }
}
