using SempreEmPe.DataLayer;
using SempreEmPe.Models;
using System.Threading.Tasks;

namespace SempreEmPe.Services
{
    public interface IBuscaCep
    {
        Task<Endereco> BuscaEnderecoApi(string cep);
        Task<Endereco> BuscaEnderecoLocal(string cep, ICepBancoLocal cepBancoLocal);
        Task<Endereco> BuscaEnderecoLocalEntity(string cep, CepCorreiosPureContext context);
    }
}
