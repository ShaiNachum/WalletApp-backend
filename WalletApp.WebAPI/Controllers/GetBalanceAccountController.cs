using Microsoft.AspNetCore.Mvc;
using WalletApp.DataLayer.Models;

namespace WalletApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetBalanceAccountController : ControllerBase
    {
        private readonly WalletDbContext _context;

        public GetBalanceAccountController(WalletDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<BalanceResponse> Get(int accountId)
        {
            try
            {
                // Get the latest balance record for the specified account
                var latestBalance = _context.Balances
                    .Where(b => b.AccountId == accountId)
                    .OrderByDescending(b => b.BalanceTime)
                    .FirstOrDefault();

                if (latestBalance == null)
                {
                    return NotFound("Account not found or has no balance history");
                }

                var response = new BalanceResponse
                {
                    BalanceValue = latestBalance.BalanceValue.Value,
                    BalanceTime = latestBalance.BalanceTime.Value
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving balance");
            }
        }
    }

    public class BalanceResponse
    {
        public decimal BalanceValue { get; set; }
        public DateTime BalanceTime { get; set; }
    }
}