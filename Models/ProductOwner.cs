using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd_Server.Models
{
    public class ProductOwner : User
    {
        [StringLength(100, ErrorMessage = "La longitud máxima es de 100 caracteres.")]
        public string? StakeHolderContact { get; set; }
        [JsonIgnore]
        public Team? Team { get; set; }
    }
}
