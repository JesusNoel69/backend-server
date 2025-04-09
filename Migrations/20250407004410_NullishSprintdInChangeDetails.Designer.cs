﻿// <auto-generated />
using System;
using BackEnd_Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackEnd_Server.Migrations
{
    [DbContext(typeof(AplicationDbContext))]
    [Migration("20250407004410_NullishSprintdInChangeDetails")]
    partial class NullishSprintdInChangeDetails
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BackEnd_Server.Models.ChangeDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<int?>("SprintNumber")
                        .HasColumnType("int");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.Property<string>("TaskInformation")
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)");

                    b.Property<string>("UserData")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SprintId");

                    b.HasIndex("TaskId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ChangeDetails");
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductBacklog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .HasColumnType("longtext");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("ProductBacklog");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ProjectNumber")
                        .HasColumnType("int");

                    b.Property<string>("Repository")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("ServerImage")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Project");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Sprint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Goal")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectNumber")
                        .HasColumnType("int");

                    b.Property<string>("Repository")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Sprint");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<int?>("DeveloperId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int?>("ProductBacklogId")
                        .HasColumnType("int");

                    b.Property<int?>("SprintId")
                        .HasColumnType("int");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("ProductBacklogId");

                    b.HasIndex("SprintId");

                    b.ToTable("TaskEntity");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int>("ProductOwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductOwnerId")
                        .IsUnique();

                    b.ToTable("Team");
                });

            modelBuilder.Entity("BackEnd_Server.Models.TeamProject", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("TeamId", "ProjectId");

                    b.HasIndex("ProjectId");

                    b.ToTable("TeamProject");
                });

            modelBuilder.Entity("BackEnd_Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Account")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("Rol")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("User");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("BackEnd_Server.Models.WeeklyScrum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("DeveloperId")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .HasMaxLength(200)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.HasIndex("TaskId");

                    b.ToTable("WeeklyScrum");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Developer", b =>
                {
                    b.HasBaseType("BackEnd_Server.Models.User");

                    b.Property<string>("NameSpecialization")
                        .HasMaxLength(30)
                        .HasColumnType("varchar(30)");

                    b.Property<int?>("TeamId")
                        .HasColumnType("int");

                    b.HasIndex("TeamId");

                    b.ToTable("Developer", (string)null);
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductOwner", b =>
                {
                    b.HasBaseType("BackEnd_Server.Models.User");

                    b.Property<string>("StakeHolderContact")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.ToTable("ProductOwner", (string)null);
                });

            modelBuilder.Entity("BackEnd_Server.Models.ChangeDetails", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Sprint", "Sprint")
                        .WithMany("ChangeDetails")
                        .HasForeignKey("SprintId");

                    b.HasOne("BackEnd_Server.Models.Task", "TaskEntity")
                        .WithMany("ChangeDetails")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd_Server.Models.User", "User")
                        .WithOne("ChangeDetails")
                        .HasForeignKey("BackEnd_Server.Models.ChangeDetails", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sprint");

                    b.Navigation("TaskEntity");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductBacklog", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Project", "Project")
                        .WithOne("ProductBacklog")
                        .HasForeignKey("BackEnd_Server.Models.ProductBacklog", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Sprint", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Project", "Project")
                        .WithMany("Sprints")
                        .HasForeignKey("ProjectId");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Task", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId");

                    b.HasOne("BackEnd_Server.Models.ProductBacklog", "ProductBacklog")
                        .WithMany("Tasks")
                        .HasForeignKey("ProductBacklogId");

                    b.HasOne("BackEnd_Server.Models.Sprint", "Sprint")
                        .WithMany("Tasks")
                        .HasForeignKey("SprintId");

                    b.Navigation("Developer");

                    b.Navigation("ProductBacklog");

                    b.Navigation("Sprint");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Team", b =>
                {
                    b.HasOne("BackEnd_Server.Models.ProductOwner", "ProductOwner")
                        .WithOne("Team")
                        .HasForeignKey("BackEnd_Server.Models.Team", "ProductOwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductOwner");
                });

            modelBuilder.Entity("BackEnd_Server.Models.TeamProject", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Project", "Project")
                        .WithMany("TeamProjects")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd_Server.Models.Team", "Team")
                        .WithMany("TeamProjects")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BackEnd_Server.Models.WeeklyScrum", b =>
                {
                    b.HasOne("BackEnd_Server.Models.Developer", "Developer")
                        .WithMany("WeeklyScrums")
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd_Server.Models.Task", "Task")
                        .WithMany("WeeklyScrums")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Developer");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Developer", b =>
                {
                    b.HasOne("BackEnd_Server.Models.User", null)
                        .WithOne()
                        .HasForeignKey("BackEnd_Server.Models.Developer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackEnd_Server.Models.Team", "Team")
                        .WithMany("Developers")
                        .HasForeignKey("TeamId");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductOwner", b =>
                {
                    b.HasOne("BackEnd_Server.Models.User", null)
                        .WithOne()
                        .HasForeignKey("BackEnd_Server.Models.ProductOwner", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductBacklog", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Project", b =>
                {
                    b.Navigation("ProductBacklog");

                    b.Navigation("Sprints");

                    b.Navigation("TeamProjects");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Sprint", b =>
                {
                    b.Navigation("ChangeDetails");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Task", b =>
                {
                    b.Navigation("ChangeDetails");

                    b.Navigation("WeeklyScrums");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Team", b =>
                {
                    b.Navigation("Developers");

                    b.Navigation("TeamProjects");
                });

            modelBuilder.Entity("BackEnd_Server.Models.User", b =>
                {
                    b.Navigation("ChangeDetails");
                });

            modelBuilder.Entity("BackEnd_Server.Models.Developer", b =>
                {
                    b.Navigation("WeeklyScrums");
                });

            modelBuilder.Entity("BackEnd_Server.Models.ProductOwner", b =>
                {
                    b.Navigation("Team");
                });
#pragma warning restore 612, 618
        }
    }
}
