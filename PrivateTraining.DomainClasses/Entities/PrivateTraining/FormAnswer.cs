using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("FormAnswers", Schema = "PrivateTraining")]
    public class FormAnswer : BaseEntity
    {
        //امتیاز1=1 و امتیاز2=2 و .....

        [Required]
        public byte TypeScore { get; set; }

        [Required]
        public int ValueScore { get; set; }

        [Required]
        public int FormQuestionId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [Required]
        public int ServiceReceiverId { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public int ServiceProviderId { get; set; }

        [ForeignKey("ServiceReceiverId")]
        [InverseProperty("FormAnswers")]
        public virtual ApplicationUser ApplicationReceiverUsers { get; set; }

        [ForeignKey("ServiceProviderId")]
        public virtual ApplicationUser ApplicationProviderUsers { get; set; }

        [ForeignKey("ServiceId")]
        [InverseProperty("FormAnswers")]
        public virtual ServiceProperties Services { get; set; }

    }
}
