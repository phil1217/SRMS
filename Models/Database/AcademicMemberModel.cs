using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRMS.Models.Database
{
    public class AcademicMemberModel
    {
        private string? _name;
        private string? _username;
        private string? _usertype;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? UserId { get; set;}

        [Required]
        [StringLength(50)]
        public string? Name {
            get => _name; 
            set => _name = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? UserType {
            get =>_usertype;
            set => _usertype = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? UserName
        {
            get => _username;
            set => _username = value?.ToLower();
        }

        [Required]
        public string? Password { get; set; }
    }
}
