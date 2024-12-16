using Avalonia.Controls;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;

namespace demo_tab_12_12_2024.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public float Price { get; set; }

    public string? Image { get; set; }

    public int Manufacturer { get; set; }

    public string? Description { get; set; }

    public virtual Manufacturer ManufacturerNavigation { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

    public virtual ICollection<Product> AttachedProduct1s { get; set; } = new List<Product>();

    public virtual ICollection<Product> MainProducts { get; set; } = new List<Product>();

    public Bitmap ProductImage => new Bitmap(Image != null ? $"Assets/{Image}" : "Assets/image.png");

    public string ProductBackgroundProperty => IsActive == true ? "White" : "Gray" ;
}
