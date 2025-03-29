using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd_Server.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? Name { get; set; }
        [StringLength(10, ErrorMessage = "La longitud máxima es de 10 caracteres.")]
        public string? Code { get; set; }
        public int ProductOwnerId { get; set; }
        [JsonIgnore]
        public ProductOwner? ProductOwner { get; set; }
        [JsonIgnore]
        public List<Developer>? Developers { get; set; }
        [JsonIgnore]
        public List<TeamProject>? TeamProjects { get; set; }
    }
}
