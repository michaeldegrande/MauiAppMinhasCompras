namespace MauiAppMinhasCompras
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("AppShell constructor chamado.");
#endif
            // Remova ou comente a linha abaixo, pois InitializeComponent não existe sem o arquivo AppShell.xaml correspondente.
            // this.InitializeComponent();
        }
    }
}
