using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    readonly ObservableCollection<Produto> Lista = new ObservableCollection<Produto>();
	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = Lista;
	}

	protected async override void OnAppearing()
	{
		try
		{
			Lista.Clear();

			List<Produto> tmp = await App.Db.GetAll();

			tmp.ForEach(i => Lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}

    private void Toolbaritem_Clicked(object sender, EventArgs e)
	{
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

	private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
	{

		try
		{
			string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            Lista.Clear();

			List<Produto> tmp = await App.Db.Search(q);

			tmp.ForEach(i => Lista.Add(i));
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
	}


	private void Toolbaritem_Clicked_1(object sender, EventArgs e)
	{
		try
		{
			double soma = Lista.Sum(i => i.Total);

			string msg = $"O Total é {soma:C}";

			DisplayAlert("Total dos Produtos", msg, "OK");
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
		try //Todo o código precisa ser envolvido por Try-Catch para não fechar do nada quando houver um bug.
		{
			MenuItem selecionado = sender as MenuItem;

			Produto p = (Produto)selecionado.BindingContext;//Selecionando o item para excluir.

			bool confirm = await DisplayAlert("Tem certeza?",$"Remover {p.Descricao}?", "Sim?", "Não");//Variável para confirmar exclusão.

            if (confirm)//Comando para excluir produto.
            {
				await App.Db.Delete(p.Id);
				Lista.Remove(p);//Comando para excluir o produto da observable collection.
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
		try
		{
			Produto p = e.SelectedItem as Produto;

			Navigation.PushAsync(new Views.EditarProduto
			{
				BindingContext = p,
			});
		}
		catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
	}

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {
            Lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => Lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
		finally
		{
			lst_produtos.IsRefreshing = false;
		}
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            // Verifica se tem algum item selecionado
            if (lst_produtos.SelectedItem == null)
            {
                await DisplayAlert("Atenção", "Selecione um produto para excluir.", "OK");
                return;
            }

            // Confirma se o usuário realmente quer excluir
            bool confirmar = await DisplayAlert("Confirmação", "Deseja excluir este produto?", "Sim", "Não");
            if (!confirmar)
                return;

            // Pega o item selecionado
            var itemSelecionado = lst_produtos.SelectedItem;

            // Remove da lista (supondo que sua lista esteja no ItemsSource)
            var lista = lst_produtos.ItemsSource as ObservableCollection<string>; // ou o tipo que você está usando
            if (lista != null)
            {
                lista.Remove((string)itemSelecionado); // Cast se for string, ajusta se for outro objeto
            }

            // Limpa a seleção
            lst_produtos.SelectedItem = null;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}