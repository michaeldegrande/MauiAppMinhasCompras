using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = Lista;
	}

    protected async override void OnAppearing()
    {
        List<Produto> tmp = await App.Db.GetAll();

		tmp.ForEach(i => Lista.Add(i));
	}

    private void toolbaritem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}
}