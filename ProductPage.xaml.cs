using MoldovanAlexLab7.Models;

namespace MoldovanAlexLab7;

public partial class ProductPage : ContentPage
{
    ShopList sl;
    private ShopList? slist;

    public ProductPage(ShopList slist)
	{

		InitializeComponent();
        sl = slist;

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
        listView.ItemsSource = await App.Database.GetProductsAsync();
    }
    private void OnAddButtonClicked(object sender, EventArgs e)
    {
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            DisplayAlert("Info", $"{selectedProduct.Description} added to shopping list.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Please select a product to add.", "OK");
        }
    }
}