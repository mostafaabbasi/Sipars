using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.Utils;


#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// </summary>
    public class mBaseController : BaseApiController
    {
        #region DI

        private readonly ILocation _Location;
        private readonly IUnitOfWork _uow;

        private IDbSet<City> _City;

        private IDbSet<State> _State;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;

        private readonly IServiceLevel _ServiceLevel;


        public BaseController(IUnitOfWork uow, ILocation location, IServiceLevel servicelevel)
        {
            _uow = uow;
            _City = _uow.Set<City>();

            _State = _uow.Set<State>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _ServiceLevelList = _uow.Set<ServiceLevelList>();
            _ServiceLevel = servicelevel;

            _Location = location;
        }

        #endregion

        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ServiceLevelList(JObject input)
        {
            var serviceId = input["serviceId"].Value<int?>();
            if (serviceId == null )
            {
                return Ok(new {result = "error", message = "سطح مورد نظر یافت نشد !"});
            }

            var items = _ServiceLevelList.Where(List => List.ServicePropertiesId == serviceId).DistinctBy(ServiceLevelList => ServiceLevelList.ServiceLevelId).ToList().Select(list => new
            {
                list.ServiceLevels.Title,
                list.ServiceLevels.ServiceId,
                list.ServiceLevelId,
                list.PercentServiceLevel,
                list.ServicePropertiesId,
            });
                
//             _ServiceLevel.GetAllServiceLevel2()
//                .Where(serviceLevel => serviceLevel.ServiceId == serviceId)
//                .ToList().Select(a => new
//            {
//                a.Title,
//                a.ServiceId,
//                a.IsEnable,
//                list = a.ServiceLevelLists.Select(b => new
//                {
//                    b.ServiceLevelId,
//                    b.ServicePropertiesId,
//                    b.PercentServiceLevel,
//                    b.ServiceLevels.Title,
//                    b.ServiceLevels.ServiceId,
//                    b.ServiceProperties.Id,
//                    b.ServiceProperties.selected,
//                })
//            });

            return Ok(new {result = "done", items});
        }


        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> WorkUnitList(JObject input)
        {
            var serviceLocationId = input["serviceLocationId"].Value<int?>();
            if (serviceLocationId == 0)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }

            var items = _ServiceLocationWorkUnit
                .Where(workUnit => workUnit.IsEnable && workUnit.ServiceLocationId == serviceLocationId)
                .ToList().Select(a => new
                {
                    id = a.Id,
                    workUnitId = a.WorkUnitId,
                    priceWorkUnit = a.PriceWorkUnit,
                    serviceLocationId = a.ServiceLocationId,
                    workUnitSelected = a.WorkUnits.selected,
                    isEnable = a.IsEnable,
                    workUnitTitle = a.WorkUnits.Title,
//                    workUnit_Id = a.WorkUnits.Id,
//                    calculationPriceServiceLocationWorkUnit = a.CalculationPriceServiceLocationWorkUnit,
                });

            return Ok(new {result = "done", items});
        }

        
            
            /// <summary>
            /// نمایش شهر ها
            /// </summary>
            /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ServiceLocation(JObject input)
        {
            var serviceId = input["serviceId"].Value<int?>();
            var locationId = input["locationId"].Value<int?>();
            if (serviceId == null || locationId == null)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }
            
            var serviceLocationId = ApiUtils.GetServiceLocationId(serviceId??-1, locationId??-1);
            if (serviceLocationId == 0)
            {
                return Ok(new {result = "error", message = "سرویس مورد نظر یافت نشد !"});
            }
            return Ok(new {result = "done", item = new {serviceLocationId = serviceLocationId}});
        }
            
        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> StateList()
        {
            var items = _State.Select(a => new
            {
                id = a.Id,
                name = a.Name
            });

            return Ok(new {result = "done", items});
        }

        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CityList()
        {
            var items = _City.Select(a => new
            {
                id = a.Id,
                name = a.Name
            });

            return Ok(new {result = "done", items});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> LocationList()
        {
            var items = _Location.GetAllLocations().Select(location =>
                new
                {
                    location.Name,
                    location.CityId,
                    location.Id
                });

            return Ok(new {result = "done", items});
        }
    }
}