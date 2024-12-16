using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using demo_tab_12_12_2024.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace demo_tab_12_12_2024;

public partial class AddWindow : Window
{
    private List<Manufacturer> _Manufacturers = Helper.DBContext.Manufacturers.OrderBy(x => x.Id).ToList();
    private ObservableCollection<Product> _Products = new ObservableCollection<Product>(Helper.DBContext.Products.OrderBy(x => x.Id));
    private ObservableCollection<Product> _AttachedProducts = [];
    private Product _SelectedProduct;
    

    public AddWindow()
    {
        _SelectedProduct = new();
        InitializeComponent();
        ComboboxInit();
        grid_addwindow.DataContext = _SelectedProduct;
        cbox_manufacturer.SelectedIndex = 0;
        tblock_id.IsVisible = false;
        tgswitch_isActive.IsChecked = false;
        btn_toSales.IsEnabled = false;
    }
    public AddWindow(Product product)
    {
        _SelectedProduct = product;
        InitializeComponent();

        grid_addwindow.DataContext = _SelectedProduct;
        ComboboxInit();
        cbox_manufacturer.SelectedIndex = _SelectedProduct.Manufacturer - 1;

        btn_delete.IsVisible = true;
        btn_delImage.IsVisible = _SelectedProduct.Image != null ? true : false;

        _AttachedProducts = new ObservableCollection<Product>(_SelectedProduct.AttachedProduct1s);
        lbox_attachedItems.ItemsSource = new ObservableCollection<Product>(_SelectedProduct.AttachedProduct1s);
        AttachedProdsCheck();
    }

    private void ComboboxInit()
    {
        cbox_manufacturer.Items.Clear();

        foreach (Manufacturer manufacturer in _Manufacturers)
            cbox_manufacturer.Items.Add(manufacturer.Name);
    }

    private void BackToMain(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void AttachedProdsCheck()
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>(Helper.DBContext.Products.OrderBy(x => x.Id));
        foreach (Product product in products)
        {
            foreach (Product attProd in _SelectedProduct.AttachedProduct1s)
                if (product.Id != attProd.Id)
                    _Products.Add(product);
        }
        lbox_allItems.ItemsSource = _Products.ToList();
    }

    private bool PriceCheck(string price)
    {
        try
        {
            float convertedprice = (float)Convert.ToDouble(price);
            if (convertedprice < 0 || price != "0" && convertedprice == 0)
                return false;
            else
                return true;
        }
        catch
        {
            return false;
        }
    }

    private void Delete(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Helper.DBContext.Remove(_SelectedProduct);
        Helper.DBContext.SaveChanges();

        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Close();
    }

    private void Confirm(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (PriceCheck(numupdown_price.Text) == true && tbox_name.Text != "")
        {
            _SelectedProduct.Manufacturer = cbox_manufacturer.SelectedIndex + 1;
            if (_SelectedProduct.Id == 0)
            {
                Helper.DBContext.Add(_SelectedProduct);
            }
            _SelectedProduct.AttachedProduct1s = _AttachedProducts;

            Helper.DBContext.SaveChanges();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }
    }

    private void ToSales(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        SalesWindow salesWindow = new SalesWindow(_SelectedProduct);
        salesWindow.Show();
        Close();
    }

    private void AddAttachedProduct(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (lbox_allItems.SelectedItem != null)
        {
            Product product = lbox_allItems.SelectedItem as Product;
            _AttachedProducts.Add(product);
            _Products.Remove(product);

            lbox_attachedItems.ItemsSource = null;
            lbox_attachedItems.ItemsSource = _AttachedProducts;

            lbox_allItems.ItemsSource = null;
            lbox_allItems.ItemsSource = _Products;
        }
    }

    private void DeleteAttachedProduct(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (lbox_attachedItems.SelectedItem != null)
        {
            Product product = lbox_attachedItems.SelectedItem as Product;
            _Products.Add(product);
            _AttachedProducts.Remove(product);

            lbox_attachedItems.ItemsSource = null;
            lbox_attachedItems.ItemsSource = _AttachedProducts;

            lbox_allItems.ItemsSource = null;
            lbox_allItems.ItemsSource = _Products;
        }
    }
}