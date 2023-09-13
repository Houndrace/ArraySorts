using System;
using System.Collections.Generic;

namespace ArraySorts.Models;

public partial class Sorting
{
    public int Id { get; set; }

    public int TypeId { get; set; }

    public DateTime StartDate { get; set; }

    public TimeSpan StartTime { get; set; }

    public int TimeResult { get; set; }

    public int OriginalArrayId { get; set; }

    public virtual Array OriginalArray { get; set; } = null!;

    public virtual SortingType Type { get; set; } = null!;
}
