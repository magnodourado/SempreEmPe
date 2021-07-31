using SempreEmPe.Models;
using System.Threading.Tasks;

namespace SempreEmPe.Services
{
    public interface IBuscaCep
    {
        Task<Endereco> BuscaEndereco(string cep);
    }
}
