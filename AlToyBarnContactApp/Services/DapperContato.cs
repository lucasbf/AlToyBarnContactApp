using AlToyBarnContactApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;

namespace AlToyBarnContactApp.Services
{
    public class DapperContato : IContatoService
    {
        private readonly IConfiguration _configuration;
        private string StrConnection
        {
            get { return _configuration.GetConnectionString("ContatosDatabase")!; }
        }

        public DapperContato(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void NoSelectCommand(string sql, Contato entity)
        {
            using (var conexao = new SqlConnection(StrConnection)!)
            {
                conexao.Execute(sql, entity);
            }
        }

        private IEnumerable<Contato> SelectCommand(string sql)
        {
            using (var conexao = new SqlConnection(StrConnection)!)
            {
                var contatos = conexao.Query<Contato, Cliente, Contato>(sql, (contato, cliente) =>
                {
                    cliente.Contatos.Add(contato);
                    contato.IdClienteNavigation = cliente;
                    return contato;
                }, splitOn: "Id,Id");
                return contatos;
            }
        }

        public void Create(Contato entity)
        {
            NoSelectCommand("INSERT INTO Contato (Perfil, Tipo, IdCliente) VALUES (@Perfil, @Tipo, @IdCliente)", entity);
        }

        public void Delete(Contato entity)
        {
            NoSelectCommand("DELETE FROM Contato WHERE Id = @Id AND IdCliente = @IdCliente", entity);
        }

        public Contato? Find(Contato entity)
        {
            var sql = "SELECT * FROM Contato ct INNER JOIN Cliente c ON ct.IdCliente = c.Id "+ 
                      "WHERE Id = " + entity.Id + " AND IdCliente = " + entity.IdCliente; ;
            return SelectCommand(sql).FirstOrDefault();
        }

        public ICollection<Contato> FindAll()
        {
            var sql = "SELECT * FROM Contato ct INNER JOIN Cliente c ON ct.IdCliente = c.Id";
            return SelectCommand(sql).ToList();
        }

        public ICollection<Contato> FindFromCliente(Cliente cliente)
        {
            var sql = "SELECT * FROM Contato ct INNER JOIN Cliente c ON ct.IdCliente = c.Id " +
                "WHERE ct.IdCliente = " + cliente.Id;
            return SelectCommand(sql).ToList();
        }

        public void Update(Contato entity)
        {
            NoSelectCommand("UPDATE Contato SET Perfil = @Perfil, Tipo = @Tipo WHERE Id = @Id AND IdCliente = @IdCliente", entity);
        }
    }
}
