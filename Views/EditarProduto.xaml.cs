using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views
{
    public partial class EditarProduto : ContentPage
    {
        private Produto _produto;

        public EditarProduto(Produto produto)
        {
            InitializeComponent();
            _produto = produto;
            BindingContext = _produto;

            txt_descricao.Text = _produto.Descricao;
            txt_quantidade.Text = _produto.Quantidade.ToString(CultureInfo.InvariantCulture);
            txt_preco.Text = _produto.Preco.ToString("F2", CultureInfo.InvariantCulture);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_descricao.Text) ||
                    string.IsNullOrWhiteSpace(txt_quantidade.Text) ||
                    string.IsNullOrWhiteSpace(txt_preco.Text))
                {
                    await DisplayAlert("Aviso", "Preencha todos os campos!", "OK");
                    return;
                }

                string quantidadeText = txt_quantidade.Text.Replace(',', '.');
                string precoText = txt_preco.Text.Replace(',', '.');

                if (!double.TryParse(quantidadeText, NumberStyles.Any, CultureInfo.InvariantCulture, out double quantidade) || quantidade <= 0)
                {
                    await DisplayAlert("Erro", "Quantidade inválida!", "OK");
                    return;
                }

                if (!double.TryParse(precoText, NumberStyles.Any, CultureInfo.InvariantCulture, out double preco) || preco < 0)
                {
                    await DisplayAlert("Erro", "Preço inválido!", "OK");
                    return;
                }

                _produto.Descricao = txt_descricao.Text.Trim();
                _produto.Quantidade = quantidade;
                _produto.Preco = preco;

                await App.Db.UpdateAsync(_produto);
                await DisplayAlert("Sucesso!", "Produto atualizado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }
        }

        // Permite apenas números, ponto e vírgula no preço
        private void TxtPreco_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            if (entry == null) return;

            string newText = "";
            foreach (char c in entry.Text)
            {
                if (char.IsDigit(c) || c == '.' || c == ',')
                    newText += c;
            }

            if (entry.Text != newText)
                entry.Text = newText;
        }
    }
}
