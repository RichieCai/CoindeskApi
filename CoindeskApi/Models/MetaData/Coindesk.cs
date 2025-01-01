using System;
using System.Collections.Generic;

namespace CoindeskApi.Models.MetaData;

public partial class Coindesk
{
    public string Code { get; set; } = null!;

    public string? CodeName { get; set; }

    public string? Description { get; set; }

    public string? DescriptionAes { get; set; }

    public decimal? RateFloat { get; set; }

    public DateTime? UpdateTime { get; set; }

    public string? Symbol { get; set; }
}
