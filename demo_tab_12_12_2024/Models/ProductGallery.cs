using System;
using System.Collections.Generic;

namespace demo_tab_12_12_2024.Models;

public partial class ProductGallery
{
    public int ProductId { get; set; }

    public string Image { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
