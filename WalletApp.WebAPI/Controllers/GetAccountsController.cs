using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletApp.DataLayer.Models;

namespace WalletApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAccountsController : ControllerBase
    {
        private readonly WalletDbContext _context;

        public GetAccountsController(WalletDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<AccountResponse>> Get()
        {
            try
            {
                // Include Owner data to access owner properties
                var accounts = _context.Accounts
                    .Include(a => a.Owner) // This loads related Owner data
                    .Select(a => new AccountResponse
                    {
                        OwnerName = a.Owner.OwnerName,
                        OwnerTaz = a.Owner.OwnerTaz,
                        AccountID = a.AccountId,
                        AccountName = a.AccountName
                    })
                    .ToList();

                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest("Error retrieving accounts");
            }
        }
    }

    // Response data structure as specified in requirements
    public class AccountResponse
    {
        public string OwnerName { get; set; }
        public string OwnerTaz { get; set; }
        public int AccountID { get; set; }
        public string AccountName { get; set; }
    }
}