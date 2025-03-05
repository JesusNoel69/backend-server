using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_Server.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Rol { get; set; }
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? Name { get; set; }
        [StringLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres.")]
        public string? Account { get; set; }
        [StringLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres.")]
        public string? Password { get; set; }
        public ChangeDetails? ChangeDetails { get; set; }
    }
}
