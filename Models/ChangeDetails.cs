using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_Server.Models
{
    public class ChangeDetails
    {
        [Key]
        public int Id { get; set; }

        public int? SprintNumber { get; set; }

        public string? UserData { get; set; }

        [StringLength(150, ErrorMessage = "La longitud máxima es de 150 caracteres.")]
        public string? TaskInformation { get; set; }

        public int? TaskId { get; set; }
        public List<Task>? Task { get; set; } 

        // public List<int>? DeveloperId { get; set; }
        // public List<Developer>? Developer { get; set; } 
        public int? SprintId { get; set; }
        public List<Sprint>? Sprints { get; set; }  // Cambié de un solo Sprint a una lista de Sprints

    }
}
