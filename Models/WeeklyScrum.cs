using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_Server.Models
{
    public class WeeklyScrum
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        [StringLength(50, ErrorMessage = "La longitud máxima es de 30 caracteres.")]
        public string? Information { get; set; }
        public int TaskId { get; set; }
        public int DeveloperId { get; set; }
    }
}