
using SempreEmPe.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SempreEmPe.Services
{
    public class BuscaCep : IBuscaCep
    {
        private readonly HttpClient client;
        private Endereco endereco;

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

            var dicUrls = new Dictionary<int, string>
            {
                {1,"https://viacep.com.br/ws/inputcephere/json/"},
                {2,"https://viacep.com.br/ws/inputcephere/json/"},
                {3,"https://ws.apicep.com/cep/inputcephere.json"}
            };


            foreach (var url in dicUrls)
            {
                var urlcompleta = url.Value.Replace("inputcephere", cep);

                var response = await client.GetAsync(urlcompleta);
                if (response.IsSuccessStatusCode)
                {
                    return await ConverteEndereco(response, url.Key);
                }
                else
                {
                    continue;
                }

            }

            async Task<Endereco> ConverteEndereco(HttpResponseMessage response, int modelo)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                EnderecoViaCep enderecoResponse;

                switch (modelo)
                {
                    case 1:
                        enderecoResponse = JsonSerializer.Deserialize<EnderecoViaCep>(conteudo, jsonOptions);

                        endereco = new Endereco()
                        {
                            Cep = enderecoResponse.Cep,
                            Logradouro = enderecoResponse.Logradouro,
                            Complemento = enderecoResponse.Complemento,
                            Bairro = enderecoResponse.Bairro,
                            Cidade = enderecoResponse.Cidade,
                            Uf = enderecoResponse.Uf,
                            Ibge = enderecoResponse.Ibge,
                            Ddd = enderecoResponse.Ddd
                        };
                        break;
                    default:
                        throw new Exception("Não foi encontrado modelo configurado para o serviço.");

                }

                if (endereco == null)
                {
                    throw new Exception("Não foi encontrado modelo configurado para o serviço.");
                }

                return endereco;
            }

            throw new Exception("Cep Inválido");
        }

    }
}
