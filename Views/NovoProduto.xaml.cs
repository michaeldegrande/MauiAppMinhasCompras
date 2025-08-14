using MauiAppMinhasCompras.Models;
using System.Globalization;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();

        // Associa o evento para filtrar caracteres inválidos na entrada do preço
        txt_preco.TextChanged += OnPrecoTextChanged;
    }

    private void OnPrecoTextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null) return;

        // Permite somente números, '.' e ','
        string filtered = new string(entry.Text.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());

        if (entry.Text != filtered)
        {
            int cursorPos = entry.CursorPosition - 1;
            entry.Text = filtered;
            entry.CursorPosition = cursorPos < 0 ? 0 : cursorPos;
        }
    }

    private async void toolbaritem_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Substitui vírgula por ponto para converter corretamente
            string precoTratado = txt_preco.Text.Replace(',', '.');

            Produto p = new Produto
            {
                Descricao = txt_descricao.Text,
                Quantidade = Convert.ToDouble(txt_quantidade.Text, CultureInfo.InvariantCulture),
                Preco = Convert.ToDouble(precoTratado, CultureInfo.InvariantCulture)
            };

            await App.Db.InsertAsync(p);
            await DisplayAlert("Sucesso!", "Registro Inserido", "OK");
            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
