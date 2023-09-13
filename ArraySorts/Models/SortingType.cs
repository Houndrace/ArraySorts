using System;
using System.Collections.Generic;

namespace ArraySorts.Models;

public partial class SortingType
{
    public int Id { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Sorting> Sortings { get; set; } = new List<Sorting>();
}
