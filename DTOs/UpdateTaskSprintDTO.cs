using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd_Server.DTOs
{
//    public class UpdateTasksSprintDTO
// {
//     public required List<Models.Task> BacklogTasks { get; set; }
//     public int BacklogId { get; set; }
//     public required List<Models.Task> SprintTasks { get; set; }
//     public int SprintId { get; set; }
// }

public class BacklogTasksDTO
{
    public List<Models.Task> Tasks { get; set; } = new();
    public int BacklogId { get; set; }
}

public class SprintTasksDTO
{
    public List<Models.Task> Tasks { get; set; } = new();
    public int SprintId { get; set; }
}

public class UpdateTasksSprintDTO
{
    public BacklogTasksDTO? Backlog { get; set; }
    public SprintTasksDTO? Sprint { get; set; }
}

}