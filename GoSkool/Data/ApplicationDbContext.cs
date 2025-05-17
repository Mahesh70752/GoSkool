using GoSkool.Models;
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
        public DbSet<GoSkool.Models.ExamEntity> Exam { get; set; } = default!;
        public DbSet<ClassScheduleEntity> classSchedule { get; set; }
    }
}
