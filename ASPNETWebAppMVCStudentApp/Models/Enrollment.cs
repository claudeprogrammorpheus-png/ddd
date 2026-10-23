using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETWebAppMVCStudentApp.Models
{
    public class Enrollment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Enrollment ID")]
        public int EnrollmentID { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseID { get; set; }

        [Required]
        [Display(Name = "Student")]
        public int StudentID { get; set; }

        [Range(0, 10, ErrorMessage = "Grade must be between 0 and 10")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal? Grade { get; set; }

        [ForeignKey("CourseID")]
        public virtual Course Course { get; set; }

        [ForeignKey("StudentID")]
        public virtual Student Student { get; set; }
    }
}
