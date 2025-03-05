using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackEnd_Server.Models
{
    public class Sprint
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public int State { get; set; }
        [StringLength(100, ErrorMessage = "La longitud máxima es de 100 caracteres.")]
        public string? Repository { get; set; }
        [StringLength(200, ErrorMessage = "La longitud máxima es de 200 caracteres.")]
        public string? Goal { get; set; }
        [StringLength(250, ErrorMessage = "La longitud máxima es de 250 caracteres.")]
        public string? Description { get; set; }
        public int ProjectNumber { get; set; }
        public List<ChangeDetails>? ChangeDetails { get; set; }
        public List<Task>? Tasks { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }
    }
}
