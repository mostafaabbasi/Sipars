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
    [Table("CommentPrivates", Schema = "PrivateTraining")]
    public class CommentPrivate:BaseEntity
    {
        #region Property

        public CommentPrivate()
        {
            Accept = false;
        }

        public int SenderUserId { get; set; }
        public int ReciverUserId { get; set; }
        public int ServiceId { get; set; }


        [Required]
        [MaxLength(100)]
        [Display(Name = "موضوع")]
        public string Subject { get; set; }

        [MaxLength(1000)]
        [Display(Name = "متن پیام")]
        public string Desc { get; set; }

        [MaxLength(20)]
        public string Date { get; set; }

        [MaxLength(20)]
        public string Time { get; set; }

        public bool Accept { get; set; }

        #endregion

        #region navigation

        [ForeignKey("SenderUserId")]
        [InverseProperty("CommentPrivates")]
        public virtual ApplicationUser SenderUsers { get; set; }

        [ForeignKey("ReciverUserId")]
        public virtual ApplicationUser ReciverUsers { get; set; }


        #endregion

    }
}
