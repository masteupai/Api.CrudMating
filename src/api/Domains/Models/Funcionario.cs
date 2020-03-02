using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class Funcionario
    {
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public double Salario { get; set; }
        public string Documento { get; set; }
        public bool Ativo { get; set; }
    }
}
