using System.ComponentModel.DataAnnotations;

namespace BackEnd_Server.Models
{
    public class Developer : User
    {
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? NameSpecialization { get; set; }
        public Team? Team { get; set; }

    }
}
