using SQLite;

namespace MauiAppMinhasCompras.Models
{
    public class Produto
    {
        private string _descricao = string.Empty;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Por favor, preencha a descrição.");
                }
                _descricao = value;
            }
        }

        public string Categoria { get; set; } = string.Empty;

        public double Quantidade { get; set; }

        public double Preco { get; set; }

        [Ignore] // Não será salvo no banco
        public double Total => Quantidade * Preco;
    }
}
