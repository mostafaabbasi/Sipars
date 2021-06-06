using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.FrameWork
{
    [Table("Messages", Schema = "Framework")]
    public class Message : BaseEntity
    {
        #region Property

        public Message()
        {
            ReadMessage = false;
        }

        public int SenderUserId { get; set; }
        public int ReciverUserId { get; set; }

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

        public bool ReadMessage { get; set; }

        #endregion

        #region navigation

        [ForeignKey("SenderUserId")]
        [InverseProperty("Messages")]
        public virtual ApplicationUser SenderUsers { get; set; }

        [ForeignKey("ReciverUserId")]
        public virtual ApplicationUser ReciverUsers { get; set; }


        #endregion



    }
}
