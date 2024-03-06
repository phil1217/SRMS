using Microsoft.EntityFrameworkCore;
using SRMS.Abstracts;
using SRMS.Models.Database;

namespace SRMS.Data.Repository
{
    public class ProfileDetails : BaseRepository<ProfileDetailsModel>
    {
        public ProfileDetails(StudentRecordDbContext context) :
            base(context, context.Profile)
        {

        }

        public override async Task Delete(string? type, string? id)
        {
            var entitiesToRemove = dbSet.Where(p => p.AcadMemberType == type && p.AcadMemberId == id);
            dbContext.RemoveRange(entitiesToRemove);
            await dbContext.SaveChangesAsync();

        }

        public override async Task<ProfileDetailsModel> Get(string? type, string? id)
        {
            return await dbSet.FirstOrDefaultAsync(p => p.AcadMemberType == type && p.AcadMemberId == id);
        }

    }
}
