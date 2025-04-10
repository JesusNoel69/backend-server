using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BackEnd_Server.Models;
/*
mydomaint6b
vE3jI2y7
*/
/*
    "DefaultConnection": "server=localhost;port=3306;database=scrum_db;user=root;password=6016681;"

*/

namespace BackEnd_Server.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Developer>().ToTable("Developer");
            modelBuilder.Entity<ProductOwner>().ToTable("ProductOwner");

            modelBuilder.Entity<TeamProject>()
                .HasKey(tp => new { tp.TeamId, tp.ProjectId });

            modelBuilder.Entity<TeamProject>()
                .HasOne(tp => tp.Team)
                .WithMany(t => t.TeamProjects)
                .HasForeignKey(tp => tp.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductOwner>()
                .HasOne(po => po.Team)
                .WithOne(t => t.ProductOwner)
                .HasForeignKey<Team>(t => t.ProductOwnerId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<TeamProject>()
                .HasOne(tp => tp.Project)
                .WithMany(p => p.TeamProjects)
                .HasForeignKey(tp => tp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WeeklyScrum>()
                    .HasOne(ws => ws.Developer)
                    .WithMany(d => d.WeeklyScrums)
                    .HasForeignKey(ws => ws.DeveloperId)
                    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<WeeklyScrum>()
                    .HasOne(wsd => wsd.Task)
                    .WithMany(ws => ws.WeeklyScrums)
                    .HasForeignKey(wsd => wsd.TaskId)
                    .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Project>()
                .HasOne(p => p.ProductBacklog)
                .WithOne(pb => pb.Project)
                .HasForeignKey<ProductBacklog>(pb => pb.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);;

           // Relación entre ChangeDetails, Task, Developer, y Sprint
            // modelBuilder.Entity<ChangeDetails>()
            //     .HasOne(cd => cd.Task)
            //     .WithMany() // Asumiendo que Task puede tener varios cambios
            //     .HasForeignKey(cd => cd.TaskId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<ChangeDetails>()
            //     .HasOne(cd => cd.Developer)
            //     .WithMany() // Asumiendo que Developer puede tener varios cambios
            //     .HasForeignKey(cd => cd.DeveloperId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<ChangeDetails>()
            //     .HasOne(cd => cd.Sprints)
            //     .WithMany()
            //     .HasForeignKey(cd => cd.SprintId)
            //     .OnDelete(DeleteBehavior.Cascade);

            // modelBuilder.Entity<ChangeDetails>()
            //     .HasIndex(c => c.DeveloperId)
            //     .IsUnique(true); // Esto asegura que no haya un índice único en UserId

            // modelBuilder.Entity<ChangeDetails>()
            //     .HasIndex(c => c.TaskId)
            //     .IsUnique(true); // Esto asegura que no haya un índice único en TaskId

            // modelBuilder.Entity<ChangeDetails>()
            //     .HasIndex(c => c.SprintId)
            //     .IsUnique(true); // Esto asegura que no haya un índice único en SprintId

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<ChangeDetails> ChangeDetails { get; set; }
        public DbSet<Developer> Developer { get; set; }
        public DbSet<ProductBacklog> ProductBacklog { get; set; }
        public DbSet<ProductOwner> ProductOwner { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Sprint> Sprint { get; set; }
        public DbSet<Models.Task> TaskEntity { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamProject> TeamProject { get; set; }
        public DbSet<WeeklyScrum> WeeklyScrum { get; set; }
    }
}
