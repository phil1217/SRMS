using Microsoft.EntityFrameworkCore;
using SRMS.Abstracts;
using SRMS.Models.Database;
using SRMS.Models.Temp;

namespace SRMS.Data.Repository
{
    public class AcademicMember:BaseRepository<AcademicMemberModel>
    {
        private readonly StudentRecordDbContext _context;

        public AcademicMember(StudentRecordDbContext context) :
            base(context, context.Member) {
            _context = context;
        }

        public override async Task<IEnumerable<AcademicMemberModel>> FindAll(QueryFilterOptions? filter)
        {
           
            var academicMembers = await _context.Member.Where(am =>
                                    (am.UserName.Contains(filter.Query) ||
                                     am.UserType.Contains(filter.Query) ||
                                     am.Name.Contains(filter.Query) || 
                                     am.UserId.ToString().Contains(filter.Query)))
                                       .Skip((filter.Page.Index ?? 0) * (filter.Page.Size ?? 0))
                                       .Take(filter.Page.Size ?? 0)
                                       .ToListAsync();

            return academicMembers;
        }

        public override async Task<int> Count(string? query)
        {
           
            return await _context.Member.Where(am =>
                                    (am.UserName.Contains(query) ||
                                     am.UserType.Contains(query) ||
                                     am.Name.Contains(query) ||
                                     am.UserId.ToString().Contains(query))).CountAsync();
        }

        public override async Task<bool> Has(AcademicMemberModel? member)
        {
          
            return await _context.Member
                .AnyAsync(m => m.UserType == member.UserType &&
                               (m.UserName == member.UserName || m.UserId == member.UserId));
        }

        public override async Task<bool> Authenticate(string? type,AuthenticationOptions? options)
        {

            return await _context.Member
                .AnyAsync(m => m.UserType == type && m.UserName == options.UserName && m.Password == options.Password);
        }

        public override async Task<bool> CanUpdate(AcademicMemberModel? member)
        {
           
            return !await _context.Member.AnyAsync(m=> m.Id != member.Id && m.UserType == member.UserType &&
                               (m.UserName == member.UserName || m.UserId == member.UserId));
        }

        public override async Task<string?> GetId(string? type, AuthenticationOptions? options)
        {
            var member = await _context.Member.FirstAsync(m=>m.UserType == type && m.UserName == options.UserName && m.Password == options.Password);

            return member?.UserId;
        }

        public override async Task<AcademicMemberModel> Get(string? type, AuthenticationOptions? options)
        {
            var member = await _context.Member.FirstAsync(m => m.UserType == type && m.UserName == options.UserName && m.Password == options.Password);

            return member;
        }
    }
}
