using System;
using System.Collections.Generic;

namespace WalletApp.DataLayer.Models;

public partial class Owner
{
    public int OwnerId { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerTaz { get; set; }

    public string? OwnerPassword { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
