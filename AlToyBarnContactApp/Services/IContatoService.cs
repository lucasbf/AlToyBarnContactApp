using AlToyBarnContactApp.Models;

namespace AlToyBarnContactApp.Services
{
    public interface IContatoService : IService<Contato>
    {
        public ICollection<Contato> FindFromCliente(Cliente cliente);
    }
}
