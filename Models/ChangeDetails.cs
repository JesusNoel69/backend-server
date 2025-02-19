using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_Server.Models
{
    public class ChangeDetails
    {
        [Key]
        public int Id { get; set; }

        public int SprintNumber { get; set; }

        public string? UserData { get; set; }

        [StringLength(150, ErrorMessage = "La longitud máxima es de 150 caracteres.")]
        public string? TaskInformation { get; set; }

  
        public int UserId { get; set; }  
        public User? User { get; set; }  

        // evitar conflictos con System.Threading.Tasks.Task
        public int TaskId { get; set; }
        public Models.Task? TaskEntity { get; set; }

        public int SprintId { get; set; }
        public Sprint? Sprint { get; set; }
    }
}
