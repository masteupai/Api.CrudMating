using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Domains.Models
{
    public class User
    {
        [Key]
        [Display(Name = "Id")]
        public int IdProfile { get; set; }
        [Required]
        [Display(Name = "User")]
        [StringLength(25, ErrorMessage = "O nome deve ter entre 1 até 25 caracteres")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "E-Mail")]
        [StringLength(50, ErrorMessage = "O E-Mail deve ter entre 1 até 50 caracteres")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Document")]
        [StringLength(11, ErrorMessage = "O Documento deve ter 11 caracteres")]
        public string Document { get; set; }
        [Required]
        [Display(Name = "Birthdate")]
        public DateTime Birthdate { get; set; }
        [Required]
        [Display(Name = "Country")]
        public Country IdCountry { get; set; }
        [Required]
        [Display(Name = "Profile")]
        public Profile Profile { get; set; }
        [Required]
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}
