using System.ComponentModel.DataAnnotations;

namespace API.Domains.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Id")]
        public int ProdutoId { get; set; }
        [Required]
        [Display(Name = "Codigo")]     
        public int Codigo { get; set; }
        [Required]
        [Display(Name = "Product")]
        [StringLength(25, ErrorMessage = "O nome do produto deve ter entre 1 até 25 caracteres")]
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public double Valor { get; set; }        
        public string Tipo { get; set; }
        public bool Ativo { get; set; }
    }
}
