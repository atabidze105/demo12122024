using System;
using System.Collections.Generic;

namespace demo_tab_12_12_2024.Models;

public partial class Sale
{
    public int Id { get; set; }

    public int Product { get; set; }

    public int Quantity { get; set; }

    public DateTime SaleDate { get; set; }

    public virtual Product ProductNavigation { get; set; } = null!;
}
