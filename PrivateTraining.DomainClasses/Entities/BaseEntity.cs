using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PrivateTraining.DomainClasses.Entities
{
    public class BaseEntity
    {

      
        public BaseEntity()
        {
            IsEnable = true;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id{ get; set; }

        public bool IsEnable { get; set; }
    }
}
