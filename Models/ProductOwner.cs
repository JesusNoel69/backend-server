using System.ComponentModel.DataAnnotations;

namespace BackEnd_Server.Models
{
    public class ProductOwner : User
    {
        [StringLength(100, ErrorMessage = "La longitud máxima es de 100 caracteres.")]
        public string? StakeHolderContact { get; set; }
        public Team? Teams { get; set; }
    }
}
