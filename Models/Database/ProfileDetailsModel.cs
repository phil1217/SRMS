using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SRMS.Models.Database
{
    public class ProfileDetailsModel
    {
        private string? _type;
        private string? _name;
        private string? _gender;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? AcadMemberId { get; set; }

        [Required]
        [StringLength(50)]
        public string? AcadMemberType {
            get => _type;
            set => _type = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? Name
        {
            get => _name;
            set => _name = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? Gender
        {
            get => _gender;
            set => _gender = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? BirthDate { get; set; }

    }
}
