using System.ComponentModel.DataAnnotations;

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
        public ProductOwner? ProductOwner { get; set; }
        public List<Developer>? Developers { get; set; }
        public TeamProject? TeamProject { get; set; }
    }
}
