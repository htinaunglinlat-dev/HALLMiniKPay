using System;
using System.Collections.Generic;

namespace HALLMiniKPay.Database.Models;

public partial class TblWallet
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Pin { get; set; } = null!;

    public long Amount { get; set; }
}
