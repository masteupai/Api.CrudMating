using API.Domains.Models.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class Veiculo
    {
        public int VeiculoId { get; set; }
        public int ClienteId { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Ano { get; set; }
        public string Placa { get; set; }
        public string Cor { get; set; }
    }
}
