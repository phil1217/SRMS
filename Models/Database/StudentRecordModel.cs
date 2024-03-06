using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SRMS.Models.Database
{
    public class StudentRecordModel
    {
        private string? _course;
        private string? _name;
        private string? _description;
        private string? _remark;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? StudentId { get; set;}

        [Required]
        [StringLength(50)]
        public string? Name
        {
            get => _name;
            set => _name = value?.ToLower();
        }


        [Required]
        [StringLength(50)]
        public string? Course {
            get => _course; 
            set => _course = value?.ToLower();
        }

        [Required]
        [StringLength(50)]
        public string? Description {
            get => _description;
            set => _description = value?.ToLower();
        }

        [Required]
        public int? Grade{get;set;}

        [Required]
        public int? Semester { get; set; }

        [Required]
        public int? Unit { get; set; }

        [Required]
        [StringLength(50)]
        public string? FacultyId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Remark
        {
            get => _remark;
            set => _remark = value?.ToLower();
        }

    }
}
