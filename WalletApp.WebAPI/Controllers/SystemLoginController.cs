using Microsoft.AspNetCore.Mvc;
using WalletApp.DataLayer.Models;

namespace WalletApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemLoginController : ControllerBase
    {
        private readonly WalletDbContext _context;

        // Constructor injection - the dependency injection system provides the DbContext
        public SystemLoginController(WalletDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<bool> Get(string adminTaz, string adminPassword)
        {
            try
            {
                // Check if admin credentials exist in database
                var admin = _context.Admins
                    .FirstOrDefault(a => a.AdminTaz == adminTaz && a.AdminPassword == adminPassword);

                // Return true if admin found, false otherwise
                return Ok(admin != null);
            }
            catch (Exception ex)
            {
                // Log error and return false for any database issues
                return Ok(false);
            }
        }
    }
}