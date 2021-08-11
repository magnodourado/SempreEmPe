using System;
using System.Collections.Generic;

#nullable disable

namespace SempreEmPe.Models
{
    public partial class LogBairro
    {
        public LogBairro()
        {
            LogGrandeUsuarios = new HashSet<LogGrandeUsuario>();
            LogLogradouros = new HashSet<LogLogradouro>();
        }

        public int BaiNu { get; set; }
        public string UfeSg { get; set; }
        public int LocNu { get; set; }
        public string BaiNo { get; set; }
        public string BaiNoAbrev { get; set; }

        public virtual ICollection<LogGrandeUsuario> LogGrandeUsuarios { get; set; }
        public virtual ICollection<LogLogradouro> LogLogradouros { get; set; }
    }
}
