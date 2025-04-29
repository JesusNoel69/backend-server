using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_Server.DTOs
{
   public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int State { get; set; }
        public int? DeveloperId { get; set; }
        public string? WeeklyScrum { get; set; }
    }
}