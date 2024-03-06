using SRMS.Models.Temp;

namespace SRMS.Models.Config
{
    public class AdminOptions
    {
        private string? _username;
        public const string Admin = "Admin";
        
        public string? UserName
         {
             get => _username;
             set => _username = value?.ToLower();
         }

        public string? Password { get; set; }

        public JwtOptions? Jwt {  get; set; }
    
    }
}
