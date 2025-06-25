using System;
using System.Collections.Generic;

namespace WalletApp.DataLayer.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? AccountPayId { get; set; }

    public int? AccountGetId { get; set; }

    public DateTime? TransactionTime { get; set; }

    public decimal? TransactionAmount { get; set; }

    public virtual Account? AccountGet { get; set; }

    public virtual Account? AccountPay { get; set; }
}
