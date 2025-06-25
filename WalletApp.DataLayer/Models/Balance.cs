using System;
using System.Collections.Generic;

namespace WalletApp.DataLayer.Models;

public partial class Balance
{
    public int BalanceId { get; set; }

    public int? AccountId { get; set; }

    public decimal? BalanceValue { get; set; }

    public DateTime? BalanceTime { get; set; }

    public int? TransactionId { get; set; }

    public virtual Account? Account { get; set; }
}
