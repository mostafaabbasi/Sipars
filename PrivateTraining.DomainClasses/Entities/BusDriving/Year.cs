using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.BusDriving
{
    [Table("Years", Schema = "BusDriving")]
    public class Year : BaseEntity
    {
        [Display(Name = " سال ")]
        [MaxLength(4)]
        public string Title { get; set; }

        public ICollection<Salary> Salaries { get; set; }

    }
}
