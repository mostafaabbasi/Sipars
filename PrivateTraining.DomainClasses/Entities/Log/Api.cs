using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities.Log
{
    [Table("Api", Schema = "Log")]
    public class Api
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }

        [Display(Name = "text")] public string text { get; set; }

        [Display(Name = "detail")] public string detail { get; set; }
    }
}