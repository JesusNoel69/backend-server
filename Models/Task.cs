﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
        [JsonIgnore]
        public Sprint? Sprint { get; set; }
        [JsonIgnore]
        public ProductBacklog? ProductBacklog { get; set; }
        public List<WeeklyScrum>? WeeklyScrums { get; set; }
        public Developer? Developer{ get; set; }//agregada
    }
}
