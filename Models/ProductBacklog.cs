using System.ComponentModel.DataAnnotations;

namespace BackEnd_Server.Models
{
    public class ProductBacklog
    {
        [Key]
        public int Id { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Comment { get; set; }
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? UpdatedBy { get; set; }
        public Project? Project { get; set; }
        public List<Task>? Tasks { get; set; }

    }
}
