using System.ComponentModel.DataAnnotations;

namespace BackEnd_Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public bool Rol { get; set; }
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? Name { get; set; }
        [StringLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres.")]
        public string? Account { get; set; }
        [StringLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres.")]
        public string? Password { get; set; }
        public ProductOwner? ProductOwner { get; set; }
        public Developer? Developer { get; set; }
        public ChangeDetails? ChangeDetails { get; set; }
    }
}
