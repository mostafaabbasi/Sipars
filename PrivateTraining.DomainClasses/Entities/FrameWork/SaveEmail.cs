using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{

    [Table("SaveEmails", Schema = "Framework")]
    public class SaveEmail : BaseEntity
    {
        [StringLength(500)]
        public string Subject { get; set; }

        [StringLength(1000)]
        public string Body { get; set; }

        [StringLength(50)]
        public string FromEmail { get; set; }

        [StringLength(50)]
        public string ToEmail { get; set; }

        [StringLength(15)]
        public string Date { get; set; }

        [StringLength(500)]
        public string Status { get; set; }
    }
}
