
using Microsoft.Extensions.Configuration;
using SempreEmPe.DataLayer;
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
        private readonly IConfiguration _configuration;

        public BuscaCep(IConfiguration configuration)
        {
            client = new HttpClient();
            _configuration = configuration;
        }

        public async Task<Endereco> BuscaEnderecoApi(string cep)
        {
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            // Incluir mais webservices aqui trocando o texto onde recebe o cep pela interpolação de strings {cep}
            // Retorno do webservice deve ser JSON
            var dicUrls = new Dictionary<int, string>
            {
                {1,$"https://viacep.com.br/ws/{cep}/jsonwww/"},
                {2,$"https://ws.apicep.com/cep/{cep}.json"}
            };


            foreach (var url in dicUrls)
            {
                Uri myUri;
                if (Uri.TryCreate(url.Value, UriKind.RelativeOrAbsolute, out myUri))
                {
                    var response = await client.GetAsync(myUri.AbsoluteUri);
                    if (response.IsSuccessStatusCode)
                    {
                        return await ConverteEndereco(response, url.Key);
                    }
                    else
                    {
                        continue;
                    }
                } else
                {
                    throw new Exception($"Url configurada para o webservice {url.Value} inválida");
                }                               
            }

            async Task<Endereco> ConverteEndereco(HttpResponseMessage response, int modelo)
            {
                var conteudo = await response.Content.ReadAsStringAsync();
                dynamic enderecoResponse;

                switch (modelo)
                {
                    case 1: //viacep.com.br
                        enderecoResponse = JsonSerializer.Deserialize<EnderecoViaCep>(conteudo, jsonOptions);

                        endereco = new Endereco()
                        {
                            Cep = enderecoResponse.Cep,
                            Logradouro = enderecoResponse.Logradouro,
                            Complemento = enderecoResponse.Complemento,
                            Bairro = enderecoResponse.Bairro,
                            Cidade = enderecoResponse.Cidade,
                            Uf = enderecoResponse.Uf
                        };
                        break;
                    case 2: //ws.apicep.com/
                        enderecoResponse = JsonSerializer.Deserialize<EnderecoApiCep>(conteudo, jsonOptions);

                        endereco = new Endereco()
                        {
                            Cep = enderecoResponse.code,
                            Logradouro = enderecoResponse.address,                           
                            Bairro = enderecoResponse.district,
                            Cidade = enderecoResponse.city,
                            Uf = enderecoResponse.state
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

        public async Task<Endereco> BuscaEnderecoLocal(string cep, ICepBancoLocal cepBancoLocal)
        {
            EnderecoBancoLocal enderecoBancoLocal = await cepBancoLocal.BuscaEnderecoBancoLocal(cep);

            endereco = new Endereco()
            {
                Cep = enderecoBancoLocal.Cep,
                Logradouro = enderecoBancoLocal.Logradouro,
                Complemento = enderecoBancoLocal.Log_Complemento,
                Bairro = enderecoBancoLocal.Bai_No,
                Cidade = enderecoBancoLocal.Loc_no,
                Uf = enderecoBancoLocal.Ufe_Sg
            };

            return endereco;
        }

    }
}
