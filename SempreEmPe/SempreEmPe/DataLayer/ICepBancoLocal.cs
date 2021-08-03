using SempreEmPe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SempreEmPe.DataLayer
{
    public interface ICepBancoLocal
    {
        Task<EnderecoBancoLocal> BuscaEnderecoBancoLocal(string cep);
    }
}
