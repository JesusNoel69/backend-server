using System.Text.Json.Serialization;

namespace BackEnd_Server.Models
{
    public class TeamProject
    {
        public int TeamId { get; set; }
        public Team? Team { get; set; } 

        public int ProjectId { get; set; }
        [JsonIgnore]
        public Project? Project { get; set; }  
    }
}
