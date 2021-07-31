using System.Text.Json.Serialization;

namespace SempreEmPe.Models
{
    public class Endereco{

        [JsonPropertyName("cep")]
        public string Cep { get; set; }
        
        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; }

        [JsonPropertyName("complemento")]
        public string Complemento { get; set; }

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; }

        [JsonPropertyName("localidade")]
        public string Cidade { get; set; }

        [JsonPropertyName("uf")]
        public string Uf { get; set; }

        [JsonPropertyName("ibge")]
        public string Ibge { get; set; }

        [JsonPropertyName("ddd")]
        public string Ddd { get; set; }
    }
}
