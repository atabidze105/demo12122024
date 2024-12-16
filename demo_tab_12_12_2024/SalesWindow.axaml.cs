using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_tab_12_12_2024.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace demo_tab_12_12_2024;

public partial class SalesWindow : Window
{
    private Product _SelectedProduct;
    private List<Product> _Products = Helper.DBContext.Products.Include(x => x.AttachedProduct1s).Include(x => x.Sales).Include(x => x.ManufacturerNavigation).OrderBy(x => x.Id).ToList();

    public SalesWindow()
    {
        InitializeComponent();
        lbox_products.ItemsSource = _Products.ToList();
    }
    
    public SalesWindow(Product product)
    {
        _SelectedProduct = product;
        InitializeComponent();
        lbox_products.ItemsSource = _Products.ToList();
        lbox_sales.ItemsSource = _SelectedProduct.Sales.ToList();
    }

    private void Border_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
    {
        _SelectedProduct = lbox_products.SelectedItem as Product;
        lbox_sales.ItemsSource = _SelectedProduct.Sales.ToList();
    }

    private void BackToMain(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }
}