using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("MessageSendeds", Schema = "PrivateTraining")]
    public class MessageSended:BaseEntity
    {
        public int UserId { get; set; }

        public string SenderNumber { get; set; }

        public string ReceiverNumber { get; set; }

        public string Content { get; set; }

        public string Status { get; set; }

        public byte StatusType { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

    }
}
