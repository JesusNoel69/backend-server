using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd_Server.Models;

namespace BackEnd_Server.DTOs
{
    public class UpdateTaskDTO
    {
        public Models.Task? Task { get; set; }
        public int UserId { get; set; }
    }
}