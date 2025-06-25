using System;
using System.Collections.Generic;

namespace WalletApp.DataLayer.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string AdminName { get; set; } = null!;

    public string AdminTaz { get; set; } = null!;

    public string AdminPassword { get; set; } = null!;
}
