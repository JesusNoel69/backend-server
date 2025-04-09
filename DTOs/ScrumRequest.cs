using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_Server.DTOs
{
    public class ScrumRequest
    {
        public int DeveloperId { get; set; }
        public int TaskId { get; set; }
        public string Content { get; set; }
    }    
}