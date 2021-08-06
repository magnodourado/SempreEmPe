using System;
using System.Collections.Generic;

#nullable disable

namespace SempreEmPe.Models
{
    public partial class LogGrandeUsuario
    {
        public int GruNu { get; set; }
        public string UfeSg { get; set; }
        public int LocNu { get; set; }
        public int BaiNu { get; set; }
        public int? LogNu { get; set; }
        public string GruNo { get; set; }
        public string GruEndereco { get; set; }
        public string Cep { get; set; }
        public string GruNoAbrev { get; set; }

        public virtual LogBairro LogBairro { get; set; }
        public virtual LogLocalidade LogLocalidade { get; set; }
        public virtual LogLogradouro LogLogradouro { get; set; }
    }
}
