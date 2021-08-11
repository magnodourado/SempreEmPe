using System;
using System.Collections.Generic;

#nullable disable

namespace SempreEmPe.Models
{
    public partial class LogLogradouro
    {
        public LogLogradouro()
        {
            LogGrandeUsuarios = new HashSet<LogGrandeUsuario>();
        }

        public int LogNu { get; set; }
        public string UfeSg { get; set; }
        public int LocNu { get; set; }
        public int? BaiNuIni { get; set; }
        public int? BaiNuFim { get; set; }
        public string LogNo { get; set; }
        public string LogComplemento { get; set; }
        public string Cep { get; set; }
        public string TloTx { get; set; }
        public string LogStaTlo { get; set; }
        public string LogNoAbrev { get; set; }

        public virtual LogBairro LogBairro { get; set; }
        public virtual LogLocalidade LogLocalidade { get; set; }
        public virtual ICollection<LogGrandeUsuario> LogGrandeUsuarios { get; set; }
    }
}
