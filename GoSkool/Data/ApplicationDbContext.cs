using System.Reflection.Emit;
using GoSkool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GoSkool.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<ClassEntity> Classes { get; set; }
        public DbSet<TeacherEntity> Teachers { get; set; }
        public DbSet<GoSkoolUser> GoSkoolUsers { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options) { }
        public DbSet<AdminEntity> Admin { get; set; }
        public DbSet<SectionEntity> Section { get; set; }
        public DbSet<StandardEntity> Standard { get; set; }
        public DbSet<SubjectEntity> Subject { get; set; }
        public DbSet<StudentEntity> Students { get; set; }
        public DbSet<AssignmentEntity> Assignment { get; set; }
        public DbSet<ExamEntity> Exam { get; set; } = default!;
        public DbSet<DriverEntity> Driver { get; set; }
        public DbSet<LocationEntity> Location { get; set; }
        public DbSet<AccountantEntity> Accountant { get; set; } = default!;
        public DbSet<TeacherScheduleEntity> TeachersSchedule { get; set; }

        public DbSet<AttendanceEntity> Attendance { get; set; }
        public DbSet<TeacherScheduleClassEntity> TeacherScheduleClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<TeacherScheduleClassEntity>()
                .HasOne(e => e.TeacherSchedule)
                .WithMany(e => e.TeacherScheduleClasses)
                .HasForeignKey(e => e.TeacherScheduleId);
            builder.Entity<TeacherScheduleClassEntity>()
                .HasOne(e => e.Class)
                .WithMany(e => e.TeacherScheduleClasses)
                .HasForeignKey(e => e.ClassId);
            builder.Entity<ClassScheduleTeacherEntity>()
                .HasOne(e => e.ClassSchedule)
                .WithMany(e => e.ClassScheduleTeachers)
                .HasForeignKey(e => e.ClassScheduleId);
            builder.Entity<ClassScheduleTeacherEntity>()
                .HasOne(e => e.Teacher)
                .WithMany(e => e.ClassScheduleTeachers)
                .HasForeignKey(e => e.TeacherId);

        }

        public DbSet<ClassScheduleEntity> ClassSchedule {  get; set; }
        public DbSet<ClassScheduleTeacherEntity> ClassScheduleTeachers { get; set; }




    }
}
