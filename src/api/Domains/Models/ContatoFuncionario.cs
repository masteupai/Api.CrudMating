using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class ContatoFuncionario
    {
        public int ContatoFunId { get; set; }
        public int ClienteId { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
