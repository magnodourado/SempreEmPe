using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public BuscaCepController(IBuscaCep buscaCep, ICepBancoLocal cepBancoLocal, CepCorreiosPureContext context, IConfiguration configuration)
        {
            _buscaCep = buscaCep;
            _cepBancoLocal = cepBancoLocal;
            _context = context;
            _configuration = configuration;
        }

        // GET api/<BuscaCepController>/5
        [HttpGet("{cep}")]
        public async Task<IActionResult> Get(string cep)
        {

            Endereco endereco;
            cep = cep.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("_", "");

            if (cep.Length != 8 || String.IsNullOrEmpty(cep))
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "O cep recebido não é válido.");
            }

            if (bool.Parse(_configuration["ConsultaBancoLocal"])){
                endereco = await _buscaCep.BuscaEnderecoLocalEntity(cep, _context);
                if (endereco == null)
                {
                    endereco = await _buscaCep.BuscaEnderecoApi(cep);
                }
            }
            else
            {
                endereco = await _buscaCep.BuscaEnderecoApi(cep);
            }
                        
            //Consulta com comando SQL 
            //return _buscaCep.BuscaEnderecoLocal(cep, _cepBancoLocal);

            if (endereco == null)
            {
                return NotFound($"O cep {cep} não foi encontrado.");
            }

            return Ok(endereco);
        }
    }
}
