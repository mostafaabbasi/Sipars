using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateTraining.DomainClasses.Entities
{
    [Table("Chat", Schema = "Msg")]
    public class Chat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int buyServiceId { get; set; }

        public string text { get; set; }

        public string type { get; set; }
        public int senderId { get; set; }
        public int receiverId { get; set; }
        public string date { get; set; }
        public string time { get; set; }
    }
}