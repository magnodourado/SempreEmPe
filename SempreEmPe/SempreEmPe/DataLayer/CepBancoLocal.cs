using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SempreEmPe.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SempreEmPe.DataLayer
{
    public class CepBancoLocal : ICepBancoLocal
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CepBancoLocal(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<EnderecoBancoLocal> BuscaEnderecoBancoLocal(string cep)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = @"SELECT * FROM " +
                    "(SELECT A.CEP, A.TLO_TX + ' ' + A.LOG_NO AS LOGRADOURO, A.LOG_COMPLEMENTO, B.BAI_NO," +
                        "C.LOC_NO, C.UFE_SG FROM LOG_LOGRADOURO A " +
                        "JOIN LOG_BAIRRO B ON B.BAI_NU = A.BAI_NU_INI AND A.UFE_SG = B.UFE_SG AND A.LOC_NU = B.LOC_NU " +
                        "JOIN LOG_LOCALIDADE C ON A.LOC_NU = C.LOC_NU AND A.UFE_SG = C.UFE_SG UNION " +
                        "SELECT A.CEP, B.TLO_TX + ' ' + B.LOG_NO AS LOGRADOURO, B.LOG_COMPLEMENTO, " +
                        "C.BAI_NO, D.LOC_NO, D.UFE_SG FROM LOG_GRANDE_USUARIO A " +
                        "JOIN LOG_LOGRADOURO B ON B.LOG_NU = A.LOG_NU AND A.UFE_SG = B.UFE_SG " +
                        "JOIN LOG_BAIRRO C ON A.BAI_NU = C.BAI_NU AND A.UFE_SG = C.UFE_SG " +
                        "JOIN LOG_LOCALIDADE D ON A.LOC_NU = D.LOC_NU AND A.UFE_SG = D.UFE_SG " +
                    ") AS BASE_CEP " +
                    "WHERE BASE_CEP.CEP = @cep";

                await connection.OpenAsync();

                IEnumerable<EnderecoBancoLocal> query = await connection.QueryAsync<EnderecoBancoLocal>(cmd, new { cep }, commandTimeout: 300);
                EnderecoBancoLocal endereco = query.SingleOrDefault();

                return endereco;
            }
        }


    }
}
