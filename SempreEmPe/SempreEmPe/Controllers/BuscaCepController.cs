using Microsoft.AspNetCore.Mvc;
using SempreEmPe.Models;
using SempreEmPe.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SempreEmPe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuscaCepController : ControllerBase
    {
        private readonly IBuscaCep _buscaCep;
        public BuscaCepController(IBuscaCep buscaCep)
        {
            _buscaCep = buscaCep;
        }

        // GET api/<BuscaCepController>/5
        [HttpGet("{cep}")]
        public Task<Endereco> Get(string cep)
        {
            cep = cep.Replace("-", "").Replace(" ", "").Replace(".", "").Replace("_", "");

            if(cep.Length != 8 || String.IsNullOrEmpty(cep))
            {
                throw new Exception("CEP inválido.");
            }

            return _buscaCep.BuscaEndereco(cep);
        }
    }
}
