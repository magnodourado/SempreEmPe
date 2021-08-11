
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SempreEmPe.DataLayer;
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

        public async Task<Endereco> BuscaEnderecoLocalEntity(string cep, CepCorreiosPureContext context)
        {
            Endereco endereco;
            try
            {
                // Desabilitando o change tracker
                context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                endereco = await context.LogLogradouros
                                           .Include(l => l.LogBairro)
                                           .Include(l => l.LogLocalidade)
                                           .Where(l => l.Cep == cep)
                                           .Select(l => new Endereco
                                           {
                                               Cep = l.Cep,
                                               Logradouro = l.TloTx + " " + l.LogNo,
                                               Complemento = l.LogComplemento,
                                               Bairro = l.LogBairro.BaiNo,
                                               Cidade = l.LogLocalidade.LocNo,
                                               Uf = l.UfeSg
                                           })
                                           .FirstOrDefaultAsync();

                if (endereco == null)
                {
                    //Filtro where aplicado em uma tabela incluída

                    //enderecoLocal = await context.LogLogradouros
                    //                       .Include(l => l.LogBairro)
                    //                       .Include(l => l.LogLocalidade)
                    //                       .Include(l => l.LogGrandeUsuarios
                    //                                .Where(lg => lg.Cep == cep))
                    //                       .FirstOrDefaultAsync();

                    endereco = await context.LogGrandeUsuarios
                                            .Include(lgu => lgu.LogLogradouro)
                                            .Include(lgu => lgu.LogBairro)
                                            .Include(lgu => lgu.LogLocalidade)
                                            .Where(lgu => lgu.Cep == cep)
                                            .Select(lgu => new Endereco
                                            {
                                                Cep = lgu.Cep,
                                                Logradouro = lgu.LogLogradouro.TloTx + " " + lgu.LogLogradouro.LogNo,
                                                Complemento = lgu.LogLogradouro.LogComplemento,
                                                Bairro = lgu.LogBairro.BaiNo,
                                                Cidade = lgu.LogLocalidade.LocNo,
                                                Uf = lgu.UfeSg
                                            })
                                            .FirstOrDefaultAsync();
                }
            } catch
            {
                return null;
            }     

            return endereco;
        }

    }
}
