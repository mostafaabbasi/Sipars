using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig
{
    public class View_ServiceLocations
    {
        [Key]
        public int Id { get; set; }
        public int ServiceLocationid { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public List<View_ServiceInLocation> Services { get; set; }

        public string ServiceName { get; set; }
        public int LocationCode { get; set; }
        public int ServiceId { get; set; }
        public int StateId { get; set; }

        public int CityId { get; set; }
        public int ServiceCode { get; set; }
        public string ServiceLocationCode { get; set; }
        public int MinCapacityService { get; set; }
        public int MaxCapacityService { get; set; }
       // public int PercentCountReservationService { get; set; }
        public double PercentCountReservationService { get; set; }
        public double PercentPriceWorkUnitServiceLocation { get; set; }
        public double PercentPriceRegisterServiceProvider { get; set; }
        //public int CapacityServiceProvider { get; set; }
        // public int PercentOfShares { get; set; }
        public double PercentOfShares { get; set; }
        public double CalculationPriceRegisterServiceProvider { get; set; }
        public double CalculationPricePercentOfShares { get; set; }



    }

    public class View_ServiceInLocation
    {
        public int ServiceLocationid { get; set; }
        public int ServiceId { get; set; }
        public string Servicename { get; set; }
        public bool Selected { get; set; }
    }


}
