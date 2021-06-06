using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PrivateTraining.DomainClasses.Entities.PrivateTraining
{
    [Table("ServiceReceiverServiceLocations", Schema = "PrivateTraining")]
    public class ServiceReceiverServiceLocation
    {
        public ServiceReceiverServiceLocation()
        {
            IsEnable = true;
        }

        public int buyServiceId { get; set; }
        public int userCityId { get; set; }
        public string userCityTitle { get; set; }
        public string userAddress { get; set; }
        public string userMobile { get; set; }
        public string userDescription { get; set; }
        public string providerServiceLocationStatus { get; set; }
        public string attachmentPath { get; set; }
        public string providerType { get; set; }
        public string workPriceListJson { get; set; }

        public string date { get; set; }
        public string time { get; set; }
        public string timeRegister { get; set; }
        public bool dateTimeSyncByProvider { get; set; }
        
        public int serviceLevelListId { get; set; }
        
        public string statusChangeJson { get; set; }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal Id { get; set; }

        public bool IsEnable { get; set; }

        [Required] public int ServiceLocationId { get; set; }
        [Required] public int ServiceReceiverId { get; set; }
        [Required] public int ServiceProviderId { get; set; }
        [Required] public string DateRegister { get; set; }

        public string DateAcceptStatus { get; set; }

        public string TimeAcceptStatus { get; set; }

        public string DateCertainStatus { get; set; }

        public string TimeCertainStatus { get; set; }

        public Nullable<int> WhoChangeStatus { get; set; }
        public Nullable<int> WorkUnitId { get; set; }


        public int Status { get; set; }

        public int CalcPrice { get; set; }

        public int CalcPriceReceived { get; set; }

        public byte TypeProblem { get; set; }
        public Nullable<int> ReasonProblemByUserId { get; set; }
        public string ReasonProblem { get; set; }
        public string DateProblem { get; set; }
        public string TimeProblem { get; set; }

        [JsonIgnore]
        [ForeignKey("ServiceLocationId")]
        [InverseProperty("ServiceReceiverServiceLocations")]
        public virtual View_ServiceLocations ServiceLocations { get; set; }

        [JsonIgnore]
        [ForeignKey("ServiceReceiverId")]
        [InverseProperty("ServiceReceiverServiceLocations")]
        public virtual ApplicationUser ApplicationReceiverUsers { get; set; }
        //  public virtual ServiceReceiverInfo ApplicationReceiverUsers { get; set; }

        [JsonIgnore]
        [ForeignKey("WorkUnitId")]
        [InverseProperty("ServiceReceiverServiceLocations")]
        public virtual WorkUnit WorkUnits { get; set; }

        [JsonIgnore]
        [ForeignKey("ServiceProviderId")] public virtual ApplicationUser ApplicationProviderUsers { get; set; }
        //   public virtual ServiceProviderInfo ApplicationProviderUsers { get; set; }

        [JsonIgnore]
        [ForeignKey("ReasonProblemByUserId")] public virtual ApplicationUser ProblemUsers { get; set; }

        [JsonIgnore]
        [ForeignKey("WhoChangeStatus")] public virtual ApplicationUser ChangeStatusUsers { get; set; }

        [JsonIgnore]
        public virtual ICollection<ServiceReceiverRequest> ServiceReceiverRequests { get; set; }
       
        [JsonIgnore]
        public virtual ICollection<DebtServiceReceiverServiceLocation> DebtServiceReceiverServiceLocations { get; set; }
    }

    //public partial class test4 : DbMigration
    //{
    //    public override void Up()
    //    {
    //        AlterColumn("ServiceReceiverServiceLocations", "Id", c => c.Decimal(nullable: false, precision: 18, scale: 2, identity: true));
    //    }

    //    public override void Down()
    //    {
    //        AlterColumn("ServiceReceiverServiceLocations", "Id", c => c.Decimal(nullable: false, precision: 18, scale: 2));
    //    }
    //}
}