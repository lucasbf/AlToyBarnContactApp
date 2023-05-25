using AlToyBarnContactApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace AlToyBarnContactApp.Services
{
    public class DapperCliente : IClienteService
    {
        private readonly IConfiguration _configuration;

        private string StrConnection
        {
            get { return _configuration.GetConnectionString("ContatosDatabase")!; }
        }

        public DapperCliente(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void NoSelectCommand(string sql, Cliente entity)
        {
            using (var conexao = new SqlConnection(StrConnection)!)
            {
                conexao.Execute(sql, entity);
            }
        }

        private IEnumerable<Cliente> SelectCommand(string sql)
        {
            using (var conexao = new SqlConnection(StrConnection)!)
            {
                var clientes = conexao.Query<Cliente, Contato, Cliente>(sql, (cliente, contato) =>
                {
                    if (contato != null)
                    {
                        cliente.Contatos.Add(contato);
                        contato.IdClienteNavigation = cliente;
                    }
                    return cliente;
                }, splitOn: "IdCliente");
                return clientes;
            }
        }

        public void Create(Cliente entity)
        { 
            NoSelectCommand("INSERT INTO Cliente (Nome, Endereco) VALUES (@Nome, @Endereco)", entity);
        }

        public void Delete(Cliente entity)
        {
            NoSelectCommand("DELETE FROM Cliente WHERE Id = @Id", entity);
        }

        public Cliente? Find(Cliente entity)
        {
            var sql = "SELECT c.Id AS Id, c.Nome AS Nome, c.Endereco AS Endereco, ct.Id AS IdContato, " +
                "ct.Perfil AS Perfil, ct.Tipo AS Tipo, ct.IdCliente AS IdCliente FROM Cliente c " +
                "LEFT JOIN Contato ct ON c.Id = ct.IdCliente WHERE Id = " + entity.Id;

            return SelectCommand(sql).FirstOrDefault();
        }

        public ICollection<Cliente> FindAll()
        {
            var sql = "SELECT c.Id AS Id, c.Nome AS Nome, c.Endereco AS Endereco, ct.Id AS IdContato, " +
                "ct.Perfil AS Perfil, ct.Tipo AS Tipo, ct.IdCliente AS IdCliente FROM Cliente c " +
                "LEFT JOIN Contato ct ON c.Id = ct.IdCliente";
            
            return SelectCommand(sql).GroupBy(c => c.Id).Select(c => c.First()).ToList();
        }

        public void Update(Cliente entity)
        {
            NoSelectCommand("UPDATE Cliente SET Nome = @Nome, Endereco = @Endereco", entity);
        }
    }
}
