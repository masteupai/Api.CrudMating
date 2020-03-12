using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class ServicoFuncionario
    {
        public int ServicoFuncionarioId { get; set; }
        public int FuncionarioId { get; set; }
        public int ServicoId { get; set; }
        public double Comissao { get; set; }
    }
}
