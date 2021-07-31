using System.Text.Json.Serialization;

namespace SempreEmPe.Models
{
    public class Endereco{
        public string Cep { get; set; }
        
        public string Logradouro { get; set; }

        public string Complemento { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Uf { get; set; }

        public string Ibge { get; set; }

        public string Ddd { get; set; }
    }
}
