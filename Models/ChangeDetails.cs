using System.ComponentModel.DataAnnotations;

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
        public Task? Task { get; set; }
        public User? User { get; set; }
        public Sprint? Sprint { get; set; }
    }
}
