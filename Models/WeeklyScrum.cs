using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackEnd_Server.Models
{
    public class WeeklyScrum
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        [StringLength(50, ErrorMessage = "La longitud máxima es de 50 caracteres.")]
        public string? Information { get; set; }
        public int TaskId { get; set; }
        public Task? Task { get; set; } 

        public int DeveloperId { get; set; }
        public Developer? Developer { get; set; } 
    }
}
