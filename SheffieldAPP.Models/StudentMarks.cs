using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheffieldAPP.Models
{
    public class StudentMarks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public double Score { get; set; } = 0;
        [Required]
        public int StudentSubjectId { get; set; }
        [ForeignKey("StudentSubjectId")]
        public StudentSubject StudentSubject { get; set; }
    }
}
