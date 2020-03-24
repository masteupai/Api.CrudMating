using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Models
{
    public class ServicoProduto
    {
        public int ServicoProdutoId { get; set; }
        public int ProdutoId { get; set; }
        public int ServicoId { get; set; }
        public int Quantidade { get; set; }
    }
}
