namespace SRMS.Models.Temp
{
    public class AuthenticationOptions
    {
        private string? _username;
        public string? UserName
        { 
            get => _username;
            set => _username = value?.ToLower();
        }

        public string? Password { get; set; }
    }
}
