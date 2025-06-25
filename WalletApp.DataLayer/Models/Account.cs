using System;
using System.Collections.Generic;

namespace WalletApp.DataLayer.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int? OwnerId { get; set; }

    public string? AccountName { get; set; }

    public virtual ICollection<Balance> Balances { get; set; } = new List<Balance>();

    public virtual Owner? Owner { get; set; }

    public virtual ICollection<Transaction> TransactionAccountGets { get; set; } = new List<Transaction>();

    public virtual ICollection<Transaction> TransactionAccountPays { get; set; } = new List<Transaction>();
}
