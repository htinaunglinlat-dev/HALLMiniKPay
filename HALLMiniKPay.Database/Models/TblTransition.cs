using System;
using System.Collections.Generic;

namespace HALLMiniKPay.Database.Models;

public partial class TblTransition
{
    public int TransitionId { get; set; }

    public string FromPhone { get; set; } = null!;

    public string ToPhone { get; set; } = null!;

    public long Amount { get; set; }

    public string Note { get; set; } = null!;

    public DateTime? Date { get; set; }
}
