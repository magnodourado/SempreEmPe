
using SempreEmPe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SempreEmPe.Services
{
    public class BuscaCep : IBuscaCep
    {
        private readonly HttpClient client;

        public BuscaCep()
        {
            client = new HttpClient();
        }

        public async Task<Endereco> BuscaEndereco(string cep)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            string[] dicUrls = new string[]
            {
                "https://viacep.com.br/ws/inputcephere/jsonwww/",
                "https://viacep.com.br/ws/inputcephere/json/",
                "https://ws.apicep.com/cep/inputcephere.json"
            };

            
            foreach (var url in dicUrls)
            {
                var urlcompleta = url.Replace("inputcephere", cep);

                var response = await client.GetAsync(urlcompleta);
                if (response.IsSuccessStatusCode)
                {
                    var conteudo = await response.Content.ReadAsStringAsync();

                    var endereco = JsonSerializer.Deserialize<Endereco>(conteudo, jsonOptions);
                    
                    return endereco;
                }
                else
                {
                    continue;
                }

            }

            throw new Exception("Cep Invalido");           
        }
 
    }
}
