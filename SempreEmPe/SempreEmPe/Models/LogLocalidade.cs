using System;
using System.Collections.Generic;

#nullable disable

namespace SempreEmPe.Models
{
    public partial class LogLocalidade
    {
        public LogLocalidade()
        {
            LogGrandeUsuarios = new HashSet<LogGrandeUsuario>();
            LogLogradouros = new HashSet<LogLogradouro>();
        }

        public int LocNu { get; set; }
        public string UfeSg { get; set; }
        public string LocNo { get; set; }
        public string Cep { get; set; }
        public string LocInSit { get; set; }
        public string LocInTipoLoc { get; set; }
        public int? LocNuSub { get; set; }
        public string LocNoAbrev { get; set; }
        public int? MunNu { get; set; }

        public virtual ICollection<LogGrandeUsuario> LogGrandeUsuarios { get; set; }
        public virtual ICollection<LogLogradouro> LogLogradouros { get; set; }
    }
}
