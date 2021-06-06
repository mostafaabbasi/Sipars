using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("Settings", Schema = "Framework")]
    public class Setting : BaseEntity
    {
        [StringLength(50)]
        public string Subject { get; set; }

        [StringLength(500)]
        public string Value1 { get; set; }

        [StringLength(50)]
        public string Value2 { get; set; }

        [StringLength(50)]
        public string Value3 { get; set; }

        [StringLength(50)]
        public string Value4 { get; set; }

    }
}
