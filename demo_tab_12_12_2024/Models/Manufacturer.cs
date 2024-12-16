using System;
using System.Collections.Generic;

namespace demo_tab_12_12_2024.Models;

public partial class Manufacturer
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateOnly? PatronageStart { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
