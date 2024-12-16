using Avalonia.Controls;
using demo_tab_12_12_2024.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace demo_tab_12_12_2024
{
    public partial class MainWindow : Window
    {
        private List<Product> _Products = Helper.DBContext.Products.Include(x => x.AttachedProduct1s).Include(x => x.Sales).Include(x => x.ManufacturerNavigation).OrderBy(x => x.Id).ToList();
        private List<Manufacturer> _Manufacturers = Helper.DBContext.Manufacturers.OrderBy(x => x.Id).ToList();
        private List<Product> _FoundedProducts = [];

        public MainWindow()
        {
            InitializeComponent();
            lbox_products.ItemsSource = _Products.ToList();
            ComboboxInit();
            tblock_count.Text = $"{_Products.Count}/{_Products.Count}";
        }
        private void ComboboxInit()
        {
            cbox_filtration.Items.Add("Все элементы");
            foreach (Manufacturer manufacturer in _Manufacturers)
            {
                cbox_filtration.Items.Add(manufacturer.Name);
            }

            cbox_sorting.Items.Add("По умолчанию");
            cbox_sorting.Items.Add("По возрастанию стоимости");
            cbox_sorting.Items.Add("По убыванию стоимости");

            cbox_filtration.SelectedIndex = 0;
            cbox_sorting.SelectedIndex = 0;
        }

        private List<Product> Filtration(List<Product> products)
        {
            if (cbox_filtration.SelectedIndex != 0)
            {
                List<Product> filtratedProducts = new List<Product>();
                filtratedProducts.AddRange(products.Where(x => x.ManufacturerNavigation.Name == cbox_filtration.SelectedItem.ToString()));
                return filtratedProducts;
            }
            return products;
        }


        private List<Product> SearchingSorting(List<Product> products)
        {
            List<Product> searched = [];
            if (tbox_searchbar.Text != "")
            {
                List<Product> priorityLess = [];
                List<Product> priorityMore = [];
                foreach (Product product in products)
                {
                    int i = 0;
                    if (product.Name.Trim().ToLower().Contains(tbox_searchbar.Text.Trim().ToLower()) == true)
                        i++;
                    if(product.Description != null)
                        if (product.Description.Trim().ToLower().Contains(tbox_searchbar.Text.Trim().ToLower()) == true)
                            i++;
                    switch(i)
                    {
                        case 1:
                            priorityLess.Add(product);
                            break;
                        case 2:
                            priorityMore.Add(product);
                            break;
                    }
                }
                searched.AddRange(priorityMore);
                searched.AddRange(priorityLess);
            }
            else
                searched.AddRange(products);

            switch (cbox_sorting.SelectedIndex)
            {
                case 1:
                    return searched.OrderBy(x => x.Price).ToList();
                case 2:
                    return searched.OrderByDescending(x => x.Price).ToList();
            }
            
            return searched;
        }

        private void SortingEvent()
        {
            _FoundedProducts.Clear();
            _FoundedProducts.AddRange(SearchingSorting(Filtration(_Products)));
            if (_FoundedProducts.Count > 0 || tbox_searchbar.Text != "" || cbox_filtration.SelectedIndex != 0)
            {
                lbox_products.ItemsSource = _FoundedProducts.ToList();
                tblock_count.Text = $"{_FoundedProducts.Count}/{_Products.Count}";
            }
            else
            {
                lbox_products.ItemsSource = _Products.ToList();
                tblock_count.Text = $"{_Products.Count}/{_Products.Count}";
            }
        }

        private void SearchingActivity(object? sender, Avalonia.Input.KeyEventArgs e)
        {
            if (tbox_searchbar.Text != null)
            {
                SortingEvent();
            }
        }

        private void FiltrationChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (cbox_filtration.SelectedItem != null)
            {
                SortingEvent();
            }
        }

        private void SortingChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            if (cbox_sorting.SelectedItem != null)
            {
                SortingEvent();
            }
        }

        private void ToAddWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow();
            addWindow.Show();
            Close();
        }

        private void ToSales(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            SalesWindow salesWindow = new SalesWindow();
            salesWindow.Show();
            Close();
        }

        private void Border_DoubleTapped(object? sender, Avalonia.Input.TappedEventArgs e)
        {
            Product selectedProduct = lbox_products.SelectedItem as Product;
            AddWindow addWindow = new AddWindow(selectedProduct);
            addWindow.Show();
            Close();
        }
    }
}