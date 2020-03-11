using API.Domains.Models.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class Servico
    {
        public int ServicoId { get; set; }
        public int VeiculoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public Situacao Situacao { get; set; }
        public string Quilometragem { get; set; }
        public double PrecoTotal { get; set; }
    }
}
