using MauiAppMinhasCompras.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiAppMinhasCompras.Helpers
{
    public class SQLiteDatabaseHelper
    {
        readonly SQLiteAsyncConnection _conn;

        public SQLiteDatabaseHelper(string path)
        {
            _conn = new SQLiteAsyncConnection(path);
            _conn.CreateTableAsync<Produto>().ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public Task<int> InsertAsync(Produto p)
        {
            return _conn.InsertAsync(p);
        }

        public Task<int> UpdateAsync(Produto p)
        {
            return _conn.ExecuteAsync("UPDATE Produto SET Descricao=?, Quantidade=?, Preco=?, Categoria=? WHERE Id=?",
                p.Descricao, p.Quantidade, p.Preco, p.Categoria, p.Id);
        }

        public Task<int> DeleteAsync(int id)
        {
            return _conn.Table<Produto>().DeleteAsync(i => i.Id == id);
        }

        public Task<List<Produto>> GetAllAsync()
        {
            return _conn.Table<Produto>().ToListAsync();
        }

        public Task<List<Produto>> SearchAsync(string q)
        {
            string sql = "SELECT * FROM Produto WHERE Descricao LIKE ?";
            return _conn.QueryAsync<Produto>(sql, "%" + q + "%");
        }
    }
}
