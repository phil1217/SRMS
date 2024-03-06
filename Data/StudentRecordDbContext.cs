using Microsoft.EntityFrameworkCore;
using SRMS.Models.Database;

namespace SRMS.Data
{
    public class StudentRecordDbContext:DbContext
    {
        public StudentRecordDbContext() : base() { }
        public StudentRecordDbContext(DbContextOptions<StudentRecordDbContext> options) : base(options) { }

        public DbSet<AcademicMemberModel> Member { get; set; }

        public DbSet<StudentRecordModel> Record { get; set; }

        public DbSet<ProfileDetailsModel> Profile { get; set; }

    }
}
