using System;
using System.Collections.Generic;

namespace ArraySorts.Models;

public partial class Array
{
    public int Id { get; set; }

    public string Data { get; set; } = null!;

    public int ItemQuantity { get; set; }

    public virtual ICollection<Sorting> Sortings { get; set; } = new List<Sorting>();
}
