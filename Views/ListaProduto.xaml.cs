using MauiAppMinhasCompras.Helpers;
using MauiAppMinhasCompras.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks; // para Task

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    private SQLiteDatabaseHelper _dbHelper;
    private ObservableCollection<Produto> _produtos;

    public ListaProduto()
    {
        InitializeComponent();

        // Usar a instância singleton do App.Db
        _dbHelper = App.Db;

        _produtos = new ObservableCollection<Produto>();
        listViewProdutos.ItemsSource = _produtos;

        // Removido CarregarProdutos daqui, vai ser chamado no OnAppearing
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await CarregarProdutos();
    }

    private async Task CarregarProdutos()
    {
        var produtos = await _dbHelper.GetAllAsync();
        _produtos.Clear();
        foreach (var p in produtos)
        {
            _produtos.Add(p);
        }
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var query = e.NewTextValue;
        var produtos = string.IsNullOrWhiteSpace(query)
            ? await _dbHelper.GetAllAsync()
            : await _dbHelper.SearchAsync(query);

        _produtos.Clear();
        foreach (var p in produtos)
        {
            _produtos.Add(p);
        }
    }

    private async void OnProdutosRefreshing(object sender, EventArgs e)
    {
        await CarregarProdutos();
        listViewProdutos.IsRefreshing = false;
    }

    private async void OnRemoverClicked(object sender, EventArgs e)
    {
        if (sender is MenuItem item && item.CommandParameter is int id)
        {
            await _dbHelper.DeleteAsync(id);
            await CarregarProdutos();
        }
    }

    private void OnProdutoSelecionado(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Produto produto)
        {
            // Implementar navegação ou ação
        }

        listViewProdutos.SelectedItem = null;
    }

    private async void OnSomarClicked(object sender, EventArgs e)
    {
        var produtos = await _dbHelper.GetAllAsync();
        var total = produtos.Sum(p => p.Preco * p.Quantidade);
        await DisplayAlert("Total", $"R$ {total:F2}", "OK");
    }

    private async void OnAdicionarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MauiAppMinhasCompras.Views.NovoProduto());
    }

    private async void OnEditarClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MauiAppMinhasCompras.Views.EditarProduto());
    }

}

