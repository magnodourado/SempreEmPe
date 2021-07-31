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
        
        // GET api/<BuscaCepController>/5
        [HttpGet("{cep}")]
        public Task<Endereco> Get(string cep)
        {
            // Trocar por injeção de dependencia aqui
            BuscaCep servico = new BuscaCep();

            return servico.BuscaEndereco(cep);
        }

    }
}
