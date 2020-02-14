using System.ComponentModel.DataAnnotations;

namespace API.Domains.Models
{
    public class Product
    {
        [Key]
        [Display(Name = "Id")]
        public int IdProduct { get; set; }
        [Required]
        [Display(Name = "Codigo")]     
        public int CodProduct { get; set; }
        [Required]
        [Display(Name = "Product")]
        [StringLength(25, ErrorMessage = "O nome do produto deve ter entre 1 até 25 caracteres")]
        public string ProductName { get; set; }
        public string Descricao { get; set; }
        public int Quant { get; set; }
        public double Value { get; set; }        
        public string ProdType { get; set; }
        public bool Active { get; set; }
    }
}
