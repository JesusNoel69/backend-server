using System.ComponentModel.DataAnnotations;

namespace BackEnd_Server.Models
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int State { get; set; }
        [StringLength(30, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? Repository { get; set; }
        [StringLength(200, ErrorMessage = "La longitud máxima es de 200 caracteres.")]
        public string? ServerImage { get; set; }
        public int ProjectNumber { get; set; }
        public List<Sprint>? Sprints { get; set; }
        public TeamProject? TeamProject { get; set; }
        public ProductBacklog? ProductBacklog { get; set; }
    }
}
