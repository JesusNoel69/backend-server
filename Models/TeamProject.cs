namespace BackEnd_Server.Models
{
    public class TeamProject
    {
        public int TeamId { get; set; }
        public int ProjectId { get; set; }
        public List<Team>? Teams { get; set; }
        public List<Project>? Projects{ get; set; }
    }
}
