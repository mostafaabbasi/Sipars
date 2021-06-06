using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("FormQuestions", Schema = "PrivateTraining")]
    public class FormQuestion:BaseEntity
    {
        [Display(Name = "عنوان سوال")]
        [Required]
        public string Title { get; set; }

        [Display(Name = "شماره فرم")]
        [Required]
        public int FormId { get; set; }

        [ForeignKey("FormId")]
        [InverseProperty("FormQuestions")]
        public virtual  Form Forms { get; set; }
    }
}
