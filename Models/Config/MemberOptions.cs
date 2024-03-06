using SRMS.Models.Temp;

namespace SRMS.Models.Config
{
    public class MemberOptions
    {
        public const string Member = "Member";
        public JwtOptions? Faculty { get; set; }

        public JwtOptions? Student { get; set; }
    }
}
