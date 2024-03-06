using Microsoft.EntityFrameworkCore;
using SRMS.Abstracts;
using SRMS.Models.Database;
using SRMS.Models.Temp;

namespace SRMS.Data.Repository
{
    public class StudentRecord : BaseRepository<StudentRecordModel>
    {
        private readonly StudentRecordDbContext _context;

        public StudentRecord(StudentRecordDbContext context) :
            base(context, context.Record) {
            _context = context;
        }

        public override async Task<IEnumerable<StudentRecordModel>> FindAll(QueryFilterOptions? filter)
        {

            var studentRecord = await _context.Record.Where(sr =>
                                    (sr.Course.Contains(filter.Query) ||
                                     sr.Name.Contains(filter.Query) ||
                                     sr.Description.Contains(filter.Query) ||
                                     sr.Grade.ToString().Contains(filter.Query) ||
                                     sr.StudentId.ToString().Contains(filter.Query)) ||
                                     sr.Semester.ToString().Contains(filter.Query) ||
                                     sr.Unit.ToString().Contains(filter.Query))
                                       .Skip((filter.Page.Index ?? 0) * (filter.Page.Size ?? 0))
                                       .Take(filter.Page.Size ?? 0)
                                       .ToListAsync();

            return studentRecord;
        }

        public override async Task<IEnumerable<StudentRecordModel>> FindAll(string? type,string? id,QueryFilterOptions? filter)
        {

            var studentRecord = new List<StudentRecordModel>();

            switch (type.ToLower())
            {
                case "faculty":


                   studentRecord = await _context.Record.Where(sr =>
                                            (sr.Course.Contains(filter.Query) ||
                                             sr.Name.Contains(filter.Query) ||
                                             sr.Description.Contains(filter.Query) ||
                                             sr.Grade.ToString().Contains(filter.Query) ||
                                             sr.StudentId.ToString().Contains(filter.Query) ||
                                             sr.Semester.ToString().Contains(filter.Query) ||
                                             sr.Unit.ToString().Contains(filter.Query)) && sr.FacultyId == id)
                                               .Skip((filter.Page.Index ?? 0) * (filter.Page.Size ?? 0))
                                               .Take(filter.Page.Size ?? 0)
                                               .ToListAsync();

                    break;

                case "student":

                    studentRecord = await _context.Record.Where(sr =>
                                            (sr.Course.Contains(filter.Query) ||
                                             sr.Name.Contains(filter.Query) ||
                                             sr.Description.Contains(filter.Query) ||
                                             sr.Grade.ToString().Contains(filter.Query) ||
                                             sr.StudentId.ToString().Contains(filter.Query) ||
                                             sr.Semester.ToString().Contains(filter.Query) ||
                                             sr.Unit.ToString().Contains(filter.Query)) && sr.StudentId == id)
                                               .Skip((filter.Page.Index ?? 0) * (filter.Page.Size ?? 0))
                                               .Take(filter.Page.Size ?? 0)
                                               .ToListAsync();

                    break;

                default: break;
            }


            return studentRecord;
        }

        public override async Task<IEnumerable<StudentRecordModel>> GetAll(string? type, string? id, PagingOptions? page)
        {
            switch (type.ToLower())
            {
                case "faculty":

                    return await dbSet.Where(sr => sr.FacultyId == id).Skip((page.Index ?? 0) * (page.Size ?? 0))
                                       .Take(page.Size ?? 0)
                                       .ToListAsync();

                case "student":

                    return await dbSet.Where(sr => sr.StudentId == id).Skip((page.Index ?? 0) * (page.Size ?? 0))
                                       .Take(page.Size ?? 0)
                                       .ToListAsync();

                default: break;
            }


            return null;
        }

        public override async Task<int> Count(string? query)
        {
           
            return await _context.Record.Where(sr =>
                                    (sr.Course.Contains(query) ||
                                    sr.Name.Contains(query) ||
                                     sr.Description.Contains(query) ||
                                     sr.Grade.ToString().Contains(query) ||
                                     sr.StudentId.ToString().Contains(query)) ||
                                     sr.Semester.ToString().Contains(query) ||
                                     sr.Unit.ToString().Contains(query)).CountAsync();
        }

        public override async Task<int?> Count(string? type, string? id,string? query)
        {

            switch (type.ToLower())
            {
                case "faculty":

                    return await _context.Record.Where(sr =>
                                            (sr.Course.Contains(query) ||
                                            sr.Name.Contains(query) ||
                                             sr.Description.Contains(query) ||
                                             sr.Grade.ToString().Contains(query) ||
                                             sr.StudentId.ToString().Contains(query) ||
                                             sr.Semester.ToString().Contains(query) ||
                                             sr.Unit.ToString().Contains(query)) && sr.FacultyId == id).CountAsync();
                   
                case "student":

                    return await _context.Record.Where(sr =>
                                            (sr.Course.Contains(query) ||
                                            sr.Name.Contains(query) ||
                                             sr.Description.Contains(query) ||
                                             sr.Grade.ToString().Contains(query) ||
                                             sr.Semester.ToString().Contains(query) ||
                                             sr.Unit.ToString().Contains(query)) && sr.StudentId == id).CountAsync();

                default: break;
            }

            return null;
        }

        public override async Task<int?> Count(string? type, string? id)
        {

            switch (type.ToLower())
            {
                case "faculty":

                    return await dbSet.CountAsync(sr=>sr.FacultyId == id);

                case "student":

                    return await dbSet.CountAsync(sr => sr.StudentId == id);

                default: break;
            }

            return null;
        }

        public override async Task<bool> Has(StudentRecordModel record)
        {

            return await _context.Record
                .AnyAsync(sr => sr.StudentId == record.StudentId &&
                               sr.Course == record.Course &&
                               sr.Semester == record.Semester);
        }

        public override async Task Delete(string? type, string? id)
        {

            var entitiesToDelete = new List<StudentRecordModel>();

            switch (type.ToLower())
            {
                case "faculty":

                    entitiesToDelete = await _context.Record.Where(sr => sr.FacultyId == id).ToListAsync();

                    break;

                case "student":

                    entitiesToDelete = await _context.Record.Where(sr => sr.FacultyId == id).ToListAsync();

                    break;

                default: break;
            }
            

            _context.Record.RemoveRange(entitiesToDelete);

            await _context.SaveChangesAsync();
        }

        public override async Task<bool> CanUpdate(StudentRecordModel record)
        {
           
            return !await _context.Record.AnyAsync(sr => sr.Id != record.Id && sr.StudentId == record.StudentId &&
                               (sr.Course == record.Course));
        }

    }
}
