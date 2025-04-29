using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd_Server.Models;

namespace BackEnd_Server.DTOs
{
    public class ChangeDetailWithTaskNameDto
{
    public int Id { get; set; }
    public int? SprintNumber { get; set; }
    public string? UserData { get; set; }
    public string? TaskInformation { get; set; }
    public int TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
}