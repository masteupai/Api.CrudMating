using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class ContatoCliente
    {
        public int ContatoCliId { get; set; }
        public int ClienteId { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
    }
}
