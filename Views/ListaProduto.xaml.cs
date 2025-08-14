using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class ListaProduto : ContentPage
    {
        private ObservableCollection<Produto> _produtos;
        private double startY;
        private bool isDragging = false;

        public ListaProduto()
        {
            InitializeComponent();
            _produtos = new ObservableCollection<Produto>();
            collectionViewProdutos.ItemsSource = _produtos;

            // PanGesture para o efeito bounce
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;
            StackContent.GestureRecognizers.Add(panGesture);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarProdutos();
        }

        private async System.Threading.Tasks.Task CarregarProdutos()
        {
            var produtos = await App.Db.GetAllAsync();
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
                ? await App.Db.GetAllAsync()
                : await App.Db.SearchAsync(query);

            _produtos.Clear();
            foreach (var p in produtos)
            {
                _produtos.Add(p);
            }
        }

        // Swipe para editar
        private async void EditarProduto_Swipe(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var produto = swipeItem?.BindingContext as Produto;
            if (produto != null)
            {
                await Navigation.PushAsync(new EditarProduto(produto));
            }
        }

        // Swipe para excluir
        private async void ExcluirProduto_Swipe(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            var produto = swipeItem?.BindingContext as Produto;
            if (produto != null)
            {
                bool confirm = await DisplayAlert("Confirmação",
                    $"Deseja realmente excluir {produto.Descricao}?", "Sim", "Não");
                if (confirm)
                {
                    await App.Db.DeleteAsync(produto.Id);
                    _produtos.Remove(produto);
                }
            }
        }

        private async void OnSomarClicked(object sender, EventArgs e)
        {
            var produtos = await App.Db.GetAllAsync();
            var total = produtos.Sum(p => p.Preco * p.Quantidade);
            await DisplayAlert("Total", $"R$ {total:F2}", "OK");
        }

        private async void OnAddButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NovoProduto());
        }

        // Função do PanGesture para bounce
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    startY = StackContent.TranslationY;
                    isDragging = true;
                    break;

                case GestureStatus.Running:
                    if (isDragging)
                        StackContent.TranslationY = startY + e.TotalY / 2; // metade da distância
                    break;

                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    StackContent.TranslateTo(0, 0, 250, Easing.SpringOut); // volta com elasticidade
                    isDragging = false;
                    break;
            }
        }
    }
}
