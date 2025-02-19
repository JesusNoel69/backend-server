using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_Server.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [StringLength(20, ErrorMessage = "La longitud máxima es de 20 caracteres.")]
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int State { get; set; }
        public int Order{ get; set; }
        public List<ChangeDetails>? ChangeDetails { get; set; }
        public Sprint? Sprint { get; set; }
        public ProductBacklog? ProductBacklog { get; set; }
        public List<WeeklyScrum>? WeeklyScrums { get; set; }
    }
}
