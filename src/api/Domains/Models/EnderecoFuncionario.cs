using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class EnderecoFuncionario
    {
        public int EnderecoFunId { get; set; }
        public int FuncionarioId { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }

    }
}
