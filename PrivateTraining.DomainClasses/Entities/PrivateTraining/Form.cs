using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("Forms", Schema = "PrivateTraining")]
    public class Form : BaseEntity
    {
        [Display(Name = "نام فرم")]
        [Required]
        public string Name { get; set; }

        [Display(Name = " امتیاز عالی ")]
        [Required]
        public int Score1 { get; set; }

        [Display(Name = "امتیاز  خوب ")]
        [Required]
        public int Score2 { get; set; }

        [Display(Name = "امتیاز متوسط ")]
        [Required]
        public int Score3 { get; set; }

        [Display(Name = " امتیاز ضعیف")]
        [Required]
        public int Score4 { get; set; }

        [Display(Name = " امتیاز خیلی ضعیف")]
        [Required]
        public int Score5 { get; set; }

        public virtual ICollection<FormQuestion> FormQuestions { get; set; }
    }
}
