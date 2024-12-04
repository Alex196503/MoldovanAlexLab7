using MoldovanAlexLab7.Models;

namespace MoldovanAlexLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var product = (Product)BindingContext;
        await App.Database.SaveProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var product = listView.SelectedItem as Product;
        await App.Database.DeleteProductAsync(product);
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });

    }
    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        Product p;
        if (listView.SelectedItem != null)
        {
            p = listView.SelectedItem as Product;
            var lp = new ListProduct()
            {
                ShopListID = ((ShopList)this.BindingContext).ID,  // Fix: Use BindingContext here
                ProductID = p.ID
            };
            await App.Database.SaveListProductAsync(lp);
            p.ListProducts = new List<ListProduct> { lp };
            await Navigation.PopAsync();
        }
    }
    void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Example logic
        var selectedItem = e.SelectedItem;
        if (selectedItem != null)
        {
            DisplayAlert("Selected Item", selectedItem.ToString(), "OK");
        }
    }
    async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        // Ob?ine produsul selectat din ListView
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            // Confirmare de la utilizator
            bool confirm = await DisplayAlert("Confirm Delete",
                $"Are you sure you want to delete {selectedProduct.Description}?", "Yes", "No");

            if (confirm)
            {
                await App.Database.DeleteProductAsync(selectedProduct);

                listView.ItemsSource = await App.Database.GetProductsAsync();
            }
        }
        else
        {
            await DisplayAlert("Error", "No item selected for deletion.", "OK");
        }
    }

}