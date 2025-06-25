using Microsoft.AspNetCore.Mvc;
using WalletApp.DataLayer.Models;

namespace WalletApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {
        private readonly WalletDbContext _context;

        public CreateAccountController(WalletDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody] CreateAccountRequest request)
        {
            try
            {
                // Validate that initial balance is not negative
                if (request.InitialBalance < 0)
                {
                    return Ok(false); // Don't allow accounts to start with negative balances
                }

                // Create new Owner record (this part stays the same)
                var newOwner = new Owner
                {
                    OwnerName = request.OwnerName,
                    OwnerTaz = request.OwnerTaz,
                    OwnerPassword = request.OwnerPassword
                };

                _context.Owners.Add(newOwner);
                _context.SaveChanges(); // This assigns the auto-generated OwnerID

                // Create new Account record (this part stays the same)
                var newAccount = new Account
                {
                    OwnerId = newOwner.OwnerId,
                    AccountName = request.OwnerName + "Osh"
                };

                _context.Accounts.Add(newAccount);
                _context.SaveChanges(); // This assigns the auto-generated AccountID

                // Create initial Balance record with the specified starting balance
                var initialBalance = new Balance
                {
                    AccountId = newAccount.AccountId,
                    BalanceValue = request.InitialBalance, // Use the provided initial balance
                    BalanceTime = DateTime.Now,
                    TransactionId = null // No transaction for initial balance
                };

                _context.Balances.Add(initialBalance);
                _context.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
    }

    // Data transfer object for request parameters
    public class CreateAccountRequest
    {
        public string OwnerName { get; set; }
        public string OwnerTaz { get; set; }
        public string OwnerPassword { get; set; }
        public decimal InitialBalance { get; set; }
    }
}