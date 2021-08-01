using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SempreEmPe.Models
{
    public class EnderecoApiCep
    {
        public string code { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string address { get; set; }
    }
}
