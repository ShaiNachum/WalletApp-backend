using Microsoft.AspNetCore.Mvc;
using WalletApp.DataLayer.Models;

namespace WalletApplication.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateTransactionController : ControllerBase
    {
        private readonly WalletDbContext _context;

        public CreateTransactionController(WalletDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult<bool> Post([FromBody] CreateTransactionRequest request)
        {
            try
            {
                // Validate that accounts are different
                if (request.AccountPayID == request.AccountGetID)
                {
                    return Ok(false); // Cannot transfer to same account
                }

                // Get current balances for both accounts
                var payAccountBalance = _context.Balances
                    .Where(b => b.AccountId == request.AccountPayID)
                    .OrderByDescending(b => b.BalanceTime)
                    .FirstOrDefault();

                var getAccountBalance = _context.Balances
                    .Where(b => b.AccountId == request.AccountGetID)
                    .OrderByDescending(b => b.BalanceTime)
                    .FirstOrDefault();

                if (payAccountBalance == null || getAccountBalance == null)
                {
                    return Ok(false); // One or both accounts don't exist
                }

                // Check if paying account has sufficient funds
                decimal newPayBalance = payAccountBalance.BalanceValue.Value - request.Amount;
                if (newPayBalance < 0)
                {
                    return Ok(false); // Insufficient funds
                }

                // Create the transaction record
                var transaction = new Transaction
                {
                    AccountPayId = request.AccountPayID,
                    AccountGetId = request.AccountGetID,
                    TransactionTime = DateTime.Now,
                    TransactionAmount = request.Amount
                };

                _context.Transactions.Add(transaction);
                _context.SaveChanges(); // Get the auto-generated TransactionID

                // Create new balance records for both accounts
                var newPayBalanceRecord = new Balance
                {
                    AccountId = request.AccountPayID,
                    BalanceValue = newPayBalance,
                    BalanceTime = DateTime.Now,
                    TransactionId = transaction.TransactionId
                };

                decimal newGetBalance = getAccountBalance.BalanceValue.Value + request.Amount;
                var newGetBalanceRecord = new Balance
                {
                    AccountId = request.AccountGetID,
                    BalanceValue = newGetBalance,
                    BalanceTime = DateTime.Now,
                    TransactionId = transaction.TransactionId
                };

                _context.Balances.Add(newPayBalanceRecord);
                _context.Balances.Add(newGetBalanceRecord);
                _context.SaveChanges();

                return Ok(true);
            }
            catch (Exception ex)
            {
                return Ok(false);
            }
        }
    }

    public class CreateTransactionRequest
    {
        public int AccountPayID { get; set; }
        public int AccountGetID { get; set; }
        public decimal Amount { get; set; }
    }
}