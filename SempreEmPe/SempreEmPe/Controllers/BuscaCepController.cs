using Microsoft.AspNetCore.Mvc;
using SempreEmPe.DataLayer;
using SempreEmPe.Models;
using SempreEmPe.Services;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SempreEmPe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuscaCepController : ControllerBase
    {
        private readonly IBuscaCep _buscaCep;
        private readonly ICepBancoLocal _cepBancoLocal;
        private readonly CepCorreiosPureContext _context;

        public BuscaCepController(IBuscaCep buscaCep, ICepBancoLocal cepBancoLocal, CepCorreiosPureContext context)
        {
            _buscaCep = buscaCep;
            _cepBancoLocal = cepBancoLocal;
            _context = context;
        }

        // GET api/<BuscaCepController>/5
        [HttpGet("{cep}")]
        public Task<Endereco> Get(string cep)
        {
            cep = cep.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("_", "");

            if (cep.Length != 8 || String.IsNullOrEmpty(cep))
            {
                throw new Exception("CEP inválido.");
            }

            //return _buscaCep.BuscaEnderecoApi(cep);

            //return _buscaCep.BuscaEnderecoLocal(cep, _cepBancoLocal);

            return _buscaCep.BuscaEnderecoLocalEntity(cep, _context);
        }
    }
}
