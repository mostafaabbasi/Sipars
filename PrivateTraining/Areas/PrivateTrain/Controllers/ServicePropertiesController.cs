using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System.Data.Entity;
using System.Web.Http;
using Castle.Core.Internal;
using Fasterflect;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.DomainClasses.EntitiesView;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.Utils;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServicePropertiesController : Controller
    {
        // GET: PrivateTraining/ServiceProperties
        private readonly IServiceProperties _ServiceProperties;
        private readonly IUnitOfWork _uow;
        private IDbSet<ServiceProperties> _ServicePropertiesdb;
        private readonly IService _Service;
        private IDbSet<Service> _Servicedb;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnitdb;

        private readonly IWorkUnit _WorkUnit;

        //private IDbSet<ServiceWorkUnit> _ServiceWorkUnitDb;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private readonly IServiceLocation _ServiceLocation;
        private readonly ILocation _location;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private IDbSet<UserService> _UserService;
        private IDbSet<UserServiceLocation> _UserServiceLocation;
        private readonly IServiceLevel _ServiceLevel;
        private IDbSet<ServiceLevelList> _ServiceLevelList;

        List<int> ListServiceChild = new List<int>();
        string parents = "";

        public ServicePropertiesController(IUnitOfWork uow, IServiceProperties ServiceProperties, IService Service,
            IWorkUnit WorkUnit, IServiceLocation ServiceLocation, ILocation location, IServiceLevel servicelevel)
        {
            _uow = uow;
            _ServiceProperties = ServiceProperties;
            _ServicePropertiesdb = _uow.Set<ServiceProperties>();
            _Service = Service;
            _Servicedb = _uow.Set<Service>();
            _ServiceWorkUnitdb = _uow.Set<ServiceWorkUnit>();
            //_ServiceWorkUnitDb = _uow.Set<ServiceWorkUnit>();
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _ServiceLocation = ServiceLocation;
            _WorkUnit = WorkUnit;
            _location = location;
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _UserService = _uow.Set<UserService>();
            _UserServiceLocation = _uow.Set<UserServiceLocation>();
            _ServiceLevel = servicelevel;
            _ServiceLevelList = _uow.Set<ServiceLevelList>();
        }

        // GET: PrivateTraining/ServiceProperties
        /// <summary>
        /// صفحه مدیریت خدمات
        /// </summary>
        public async virtual Task<ActionResult> Index()
        {
            var listServices = _ServicePropertiesdb.Where(c => c.IsEnable == true).OrderBy(c => c.Id).ToList();
            var listWorkUnit = _ServiceWorkUnitdb.Where(c => c.IsEnable == true).ToList();

            PrivateTraining_View_ServicePropertiesWorkUnit ViewModel =
                new PrivateTraining_View_ServicePropertiesWorkUnit();
            ViewModel.ServicesProperties = listServices.Where(c => c.IsEnable == true).OrderBy(c => c.Id).ToList();
            ViewModel.WorkUnits = listWorkUnit.Where(c => c.IsEnable == true).ToList();
            if (ViewModel.ServicesProperties.Count() > 0)
                return View(ViewModel);
            else
                return View();
        }

        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> GetServicePropertiesList(ServiceProperties param)
        {
            try
            {
                var list = await _ServiceProperties.GetAllServiceProperties();
                if (list.Count() > 0)
                {
                    return Json(new {Result = true, list = list, Message = ""});
                }
                else
                    return Json(new {Result = false, Message = "وجود ندارد"});
            }
            catch
            {
                return Json(new {Result = false, Message = "خطا"});
            }
        }

        /// <summary>
        /// افزودن مشخصات خدمت
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> AddServiceProperties(ServiceProperties param, List<int> Workunits,
            List<int> PriceWorkUnits,
            List<int> ServiceLavels, List<int> PercentServiceLevel)
        {
            try
            {
                int Level = 0;
                if (param.ParentId != 0)
                {
                    var x = await _ServiceProperties.GetServiceProperties(param.ParentId);
                    Level = x.Level;
                }

                param.IsEnable = true;
                param.Level = Level + 1;
                await _ServiceProperties.AddServiceProperties(param);
                int Status = await _uow.SaveAllChangesAsync();

                //------ ثبت واحد کار
                if (Workunits != null)
                {
                    int WorkUnitId = 0;
                    int PriceWorkUnitId = 0;
                    for (int i = 0; i < Workunits.Count(); i++)
                    {
                        WorkUnitId = Workunits[i];
                        PriceWorkUnitId = PriceWorkUnits[i];

                        if (WorkUnitId != 0 && PriceWorkUnitId != 0)
                        {
                            ServiceWorkUnit tempServiceWorkUnit = new ServiceWorkUnit();
                            tempServiceWorkUnit.ServicePropertiesId = param.Id;
                            tempServiceWorkUnit.WorkUnitId = WorkUnitId;
                            tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId;
                            _ServiceWorkUnitdb.Add(tempServiceWorkUnit);
                            Status = await _uow.SaveAllChangesAsync();
                        }
                    }
                }

                //------ ثبت سطوح خدمت
                if (ServiceLavels != null)
                {
                    int serviceLevelId = 0;
                    int PercentserviceLevelId = 0;
                    for (int i = 0; i < ServiceLavels.Count(); i++)
                    {
                        serviceLevelId = ServiceLavels[i];
                        PercentserviceLevelId = ServiceLavels[i];

                        if (serviceLevelId != 0 && PercentserviceLevelId != 0)
                        {
                            ServiceLevelList temp = new ServiceLevelList();
                            temp.ServicePropertiesId = param.Id;
                            temp.ServiceLevelId = serviceLevelId;
                            temp.PercentServiceLevel = PercentserviceLevelId;
                            _ServiceLevelList.Add(temp);
                            Status = await _uow.SaveAllChangesAsync();
                        }
                    }
                }

                //--------------------- درج خدمت محل زمان افزودن لیست

                var AllLocation = await _location.GetAllLocation();
                foreach (var item in AllLocation.ToList())
                {
                    //---------------------- ثبت خصوصیات خدمت برای محل
                    ServiceProperties tempServiceProperties = param;
                    DomainClasses.Entities.PrivateTraining.View_ServiceLocations tempServiceLocation =
                        new DomainClasses.Entities.PrivateTraining.View_ServiceLocations();
                    tempServiceLocation.ServiceId = tempServiceProperties.Id;
                    tempServiceLocation.ServiceCode = tempServiceProperties.ServiceCode;
                    tempServiceLocation.LocationId = item.Id;
                    tempServiceLocation.LocationCode = item.LocationCode;
                    tempServiceLocation.ServiceLocationCode =
                        tempServiceLocation.LocationCode + "" + tempServiceLocation.ServiceCode;
                    tempServiceLocation.MaxCapacityService = tempServiceProperties.MaxCapacity;
                    tempServiceLocation.MinCapacityService = tempServiceProperties.MinCapacity;
                    tempServiceLocation.PercentCountReservationService = tempServiceProperties.PercentCountReservation;
                    tempServiceLocation.CityId = item.CityId;

                    tempServiceLocation.PercentPriceWorkUnitServiceLocation = item.PercentPriceWorkUnitServiceLocation;
                    tempServiceLocation.PercentPriceRegisterServiceProvider = item.PercentPriceRegisterServiceProvider;
                    //tempServiceLocation.CapacityServiceProvider = tempServiceProperties.CapacityServiceProvider;
                    tempServiceLocation.PercentOfShares =
                        item.PercentOfShares + (tempServiceProperties.PercentOfShares);
                    tempServiceLocation.CalculationPriceRegisterServiceProvider =
                        (tempServiceProperties.PriceRegisterServiceProvider) +
                        ((tempServiceLocation.PercentPriceRegisterServiceProvider / 100) *
                         (tempServiceProperties.PriceRegisterServiceProvider));

                    await _ServiceLocation.AddServiceLocation(tempServiceLocation);
                    Status = await _uow.SaveAllChangesAsync();

                    //---------------------- ثبت قیمت های واحد کار تعریف شده خدمت ؛ برای محل
                    var listServiceWorkUnit = _ServiceWorkUnit
                        .Where(c => c.ServicePropertiesId == tempServiceLocation.ServiceId).ToList();
                    foreach (var item2 in listServiceWorkUnit)
                    {
                        ServiceLocationWorkUnit tempServiceLocationWorkUnit = new ServiceLocationWorkUnit();
                        tempServiceLocationWorkUnit.ServiceLocationId = tempServiceLocation.Id;
                        tempServiceLocationWorkUnit.WorkUnitId = item2.WorkUnitId;
                        tempServiceLocationWorkUnit.PriceWorkUnit = item2.PriceWorkUnit;
                        tempServiceLocationWorkUnit.CalculationPriceServiceLocationWorkUnit =
                            item2.PriceWorkUnit + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) *
                            item2.PriceWorkUnit;
                        _ServiceLocationWorkUnit.Add(tempServiceLocationWorkUnit);
                        Status = await _uow.SaveAllChangesAsync();
                    }
                }

                return Json(new {Result = true, Messages = Status});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Messages = ""});
            }
        }

        /// <summary>
        /// حذف مشخصات خدمت
        /// </summary>
        /// <param name="ServicePropertiesId"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> DeleteServiceProperties(string Id)
        {
            try
            {
                var y = await _ServiceProperties.GetServiceProperties(Convert.ToInt32(Id));
                var intId = Convert.ToInt32(y.Id);
                var listT = _ServicePropertiesdb.Where(c => c.ParentId == y.Id).ToList();
                if (listT.Count() > 0)
                {
                    return Json(new {Result = false, Message = "بدلیل داشتن زیر خدمات امکان حذف وجود ندارد."});
                }

                var ListT2 = _UserService.Where(c => c.ServiceId == intId).ToList();
                if (listT.Count() > 0)
                {
                    return Json(new
                        {Result = false, Message = "خدمت موردنظر توسط کاربران انتخاب شده است.امکان حذف وجود ندارد."});
                }

                var ListT3 = _UserServiceLocation.Where(c => c.ServiceId == intId);
                if (ListT3.Count() > 0)
                {
                    return Json(new
                    {
                        Result = false, Message = "کاربران محل خدمت موردنظر را انتخاب کرده اند. امکان حذف وجود ندارد ."
                    });
                }

                //-------------------- حذف خدمت محل ها
                var L = await _ServiceLocation.GetAllServiceLocation();
                L = L.Where(c => c.ServiceId == intId);

                foreach (var item in L)
                {
                    var d = _ServiceLocation.DeleteServiceLocation(Convert.ToInt32(item.Id));
                }

                //----------------------  حذف واحد های کار خدمت
                var listworkDel = _ServiceWorkUnitdb.Where(c => c.ServicePropertiesId == intId).ToList();
                for (int i = 0; i <= listworkDel.Count() - 1; i++)
                {
                    var serviceworkid = listworkDel[i].Id;
                    ServiceWorkUnit ServiceWorkunit = _ServiceWorkUnitdb.Find(serviceworkid);
                    _ServiceWorkUnitdb.Remove(ServiceWorkunit);
                    await _uow.SaveAllChangesAsync();
                }

                //----------------- حذف خدمت
                await _ServiceProperties.DeleteServiceProperties(intId);
                var status = await _uow.SaveAllChangesAsync();

                return Json(new {Result = true, Message = "حذف با موفقیت انجام شد"});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false, Message = "امکان حذف خدمت موردنظر وجود ندارد." + ex.Message});
            }
        }

        public void EditServiceLevelList(List<int> ServiceLavels, List<int> PercentServiceLevel, List<Service> services,
            int Id = 0)
        {
            var listOld = _ServiceLevelList.Where(c => c.ServicePropertiesId == Id).ToList();

            int ServiceLavelId = 0;
            int PercentServiceLavelId = 0;

            if (ServiceLavels != null)
            {
                for (int i = 0; i < ServiceLavels.Count(); i++)
                {
                    ServiceLavelId = ServiceLavels[i];
                    PercentServiceLavelId = PercentServiceLevel[i];
                    var y = listOld.Where(c => c.ServiceLevelId == ServiceLavelId).ToList();

                    if (ServiceLavelId != 0 && PercentServiceLavelId != 0 && y.Count() > 0)
                    {
                        var d = listOld.Where(c => c.ServiceLevelId == ServiceLavelId).FirstOrDefault();
                        if (d != null)
                        {
                            d.ServicePropertiesId = Id;
                            d.ServiceLevelId = ServiceLavelId;
                            d.PercentServiceLevel = PercentServiceLavelId;
                        }
                    }
                    else
                    {
                        ServiceLevelList temp = new ServiceLevelList();
                        temp.ServicePropertiesId = Id;
                        temp.ServiceLevelId = ServiceLavelId;
                        temp.PercentServiceLevel = PercentServiceLavelId;
                        _ServiceLevelList.Add(temp);
                    }

                    var m = _uow.SaveAllChanges();
                }

                var listDel = listOld.Where(c => !ServiceLavels.Contains(c.ServiceLevelId)).ToList();
                foreach (var item in listDel)
                {
                    _ServiceLevelList.Remove(item);
                    var s = _uow.SaveAllChanges();
                }

                //----------------- ویرایش زیرمجموعه 
                RetrunListChild(Id, services);
                foreach (var item2 in ListServiceChild)
                {
                    var listOld2 = _ServiceLevelList.Where(c => c.ServicePropertiesId == item2).ToList();

                    int ServiceLavelId2 = 0;
                    int PercentServiceLavelId2 = 0;
                    for (int i = 0; i < ServiceLavels.Count(); i++)
                    {
                        ServiceLavelId2 = ServiceLavels[i];
                        PercentServiceLavelId2 = PercentServiceLevel[i];
                        var y = listOld2.Where(c => c.ServiceLevelId == ServiceLavelId2).ToList();

                        //   if (WorkUnitId2 != 0 && PriceWorkUnitId2 != 0 && y.Count() <= 0)
                        if (ServiceLavelId2 != 0 && PercentServiceLavelId2 != 0 && y.Count() > 0)
                        {
                            var d = listOld2.Where(c => c.ServiceLevelId == ServiceLavelId2).FirstOrDefault();
                            if (d != null)
                            {
                                d.ServicePropertiesId = item2;
                                d.ServiceLevelId = ServiceLavelId2;
                                d.PercentServiceLevel = PercentServiceLavelId2;
                            }
                        }
                        else
                        {
                            ServiceLevelList temp = new ServiceLevelList();
                            temp.ServicePropertiesId = item2;
                            temp.ServiceLevelId = ServiceLavelId2;
                            temp.PercentServiceLevel = PercentServiceLavelId2;
                            _ServiceLevelList.Add(temp);
                        }

                        var g = _uow.SaveAllChanges();
                    }

                    var listDel2 = listOld2.Where(c => !ServiceLavels.Contains(c.ServiceLevelId)).ToList();
                    foreach (var item1 in listDel2)
                    {
                        _ServiceLevelList.Remove(item1);
                        var d = _uow.SaveAllChanges();
                    }
                }
            }
        }

        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>         
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> serviceProperties(int Id)
        {
            ServiceProperties list = await _ServiceProperties.GetServiceProperties(Id);

            var listWorkUnit = await _WorkUnit.GetAllWorkUnit();
            var ListWorkUnits = listWorkUnit.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
            {
                WorkUnitId = c.Id,
                WorkUnitTitle = c.Title,
                selected = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id),
                PriceWorkUnit = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                    ? c.ServiceWorkUnits.FirstOrDefault(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                        .PriceWorkUnit
                    : 0
            }).ToList();

            ServiceProperties ListTemp = new ServiceProperties();
            ListTemp.Id = list.Id;
            ListTemp.Title = list.Title;
            ListTemp.ServiceCode = list.ServiceCode;
            ListTemp.MinCapacity = list.MinCapacity;
            ListTemp.MaxCapacity = list.MaxCapacity;
            ListTemp.ServiceCode = list.ServiceCode;
            ListTemp.HowPerform = list.HowPerform;
            ListTemp.PriceRegisterServiceProvider = list.PriceRegisterServiceProvider;
            ListTemp.PercentOfShares = list.PercentOfShares;
            //ListTemp.CapacityServiceProvider = list.CapacityServiceProvider;
            ListTemp.PercentCountReservation = list.PercentCountReservation;
            ListTemp.automation = list.automation;
            ListTemp.InitialVisit = list.InitialVisit;
            ListTemp.DescriptionUploud1 = list.DescriptionUploud1;
            ListTemp.DescriptionUploud2 = list.DescriptionUploud2;
            ListTemp.DescriptionUploud3 = list.DescriptionUploud3;

            return Json(new
                {ServiceProperties = ListTemp, WorkUnit = ListWorkUnits, ServiceLevel = await ListServiceLevel(Id)});
        }

        public class dynamicParam
        {
            public JObject value { get; set; }
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        [System.Web.Mvc.HttpPost]
        //[System.Web.Mvc.HttpGet]
        public virtual async Task<JsonResult> ServiceProperty(int id)
        {
            var service = await _ServiceProperties.GetServiceProperties(id);
            return Json(new
            {
                result = "done", serviceProperty = JsonConvert.SerializeObject(service, Formatting.None,
                    new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    })
            });
        }

        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> EditServiceExtraProperties(ServiceProperties editService,
            string childService)
        {
            var service = _ServicePropertiesdb.First(s => s.Id == editService.Id);
            //var service = await _ServiceProperties.GetServiceProperties(editService.Id);

            if (service == null)
            {
                return Json(new {result = "error", Messages = "service not found!"});
            }

            /*askCustomerSex 
            askProviderAddress 
            askProviderSex 
            askTime 
            forceAttach 
            multiPrice 
            
            pricingSipars 
            pricingProvider 
            pricingShared 
            
            providerSelectSipars 
            providerSelectCustomer 
            providerSelectProvider 
            
            
            serviceLocationCustomer 
            serviceLocationProvider 
            serviceLocationLess 
            
            multiProviderSelect 
            multiProviderOffer 
            
            payOnline 
            payMin 
            payMinPercent 
            
            serviceUnitName: '',
            
            baseOff: 0,
            minOff: 0,
            
            serviceDescription: '',
            priceDescription: ''
            
            */

            service.askCustomerSex = editService.askCustomerSex;
            service.askProviderAddress = editService.askProviderAddress;
            service.askProviderSex = editService.askProviderSex;
            service.askTime = editService.askTime;
            service.forceAttach = editService.forceAttach;
            service.multiPrice = editService.multiPrice;

            service.pricingSipars = editService.pricingSipars;
            service.pricingProvider = editService.pricingProvider;
            service.pricingShared = editService.pricingShared;

            service.providerSelectSipars = editService.providerSelectSipars;
            service.providerSelectCustomer = editService.providerSelectCustomer;
            service.providerSelectProvider = editService.providerSelectProvider;

            service.serviceLocationCustomer = editService.serviceLocationCustomer;
            service.serviceLocationProvider = editService.serviceLocationProvider;
            service.serviceLocationLess = editService.serviceLocationLess;

            service.multiProviderSelect = editService.multiProviderSelect;
            service.multiProviderOffer = editService.multiProviderOffer;

            service.payOnline = editService.payOnline;
            service.payMin = editService.payMin;
            service.payMinPercent = editService.payMinPercent;
            
             service.serviceUnitName = editService.serviceUnitName;
             service.baseOff = editService.baseOff;
             service.minOff = editService.minOff;
             service.serviceDescription = editService.serviceDescription;
             service.priceDescription = editService.priceDescription;
             

            if (!string.IsNullOrEmpty(childService))
            {
                var allChildServiceProperties = new List<ServiceProperties>();

                void SetChildsProps(int parentId)
                {
                    var TempService = _ServicePropertiesdb.Where(c => c.ParentId == parentId).ToList();
                    allChildServiceProperties.AddRange(TempService);
                    TempService.ForEach(s => SetChildsProps(s.Id));
                }

                SetChildsProps(editService.Id);

                allChildServiceProperties.ForEach(serviceProp =>
                {
                    JsonConvert.PopulateObject(childService, serviceProp);
                });
            }

            _uow.SaveAllChanges();

            return Json(new {result = "done", Messages = ""});
        }


        /// <summary>
        /// ویرایش خدمت
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> EditServiceProperties(ServiceProperties param, List<int> Workunits,
            List<int> PriceWorkUnits,
            List<int> ServiceLavels, List<int> PercentServiceLevel)
        {
            try
            {
                var services = await _Service.GetAllService();

                var list = await _ServiceProperties.GetServiceProperties(param.Id);
                var listSub = _ServicePropertiesdb.Where(c => c.ParentId == param.Id);
                if (list != null)
                {
                    list.Id = param.Id;
                    list.Title = param.Title;
                    list.MinCapacity = param.MinCapacity;
                    list.MaxCapacity = param.MaxCapacity;
                    list.ServiceCode = param.ServiceCode;
                    //      list.PriceWorkUnit = param.PriceWorkUnit;
                    list.PriceRegisterServiceProvider = param.PriceRegisterServiceProvider;
                    list.PercentOfShares = param.PercentOfShares;
                    list.PercentCountReservation = param.PercentCountReservation;
                    list.PriceRegisterServiceProvider = param.PriceRegisterServiceProvider;
                    //list.CapacityServiceProvider = param.CapacityServiceProvider;
                    list.automation = param.automation;
                    list.HowPerform = param.HowPerform;
                    list.InitialVisit = param.InitialVisit;
                    list.DescriptionUploud1 = param.DescriptionUploud1;
                    list.DescriptionUploud2 = param.DescriptionUploud2;
                    list.DescriptionUploud3 = param.DescriptionUploud3;
                    var e = _uow.SaveAllChanges();

                    //---------- ویرایش واحد کار
                    var listOldWorkUnit = _ServiceWorkUnitdb.Where(c => c.ServicePropertiesId == param.Id).ToList();

                    int WorkUnitId = 0;
                    int PriceWorkUnitId = 0;
                    for (int i = 0; i < Workunits.Count(); i++)
                    {
                        WorkUnitId = Workunits[i];
                        PriceWorkUnitId = PriceWorkUnits[i];
                        var y = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId).ToList();

                        //    if (WorkUnitId != 0 && PriceWorkUnitId != 0 && y.Count() <= 0)
                        if (WorkUnitId != 0 && PriceWorkUnitId != 0 && y.Count() > 0)
                        {
                            var d = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId).FirstOrDefault();
                            if (d != null)
                            {
                                d.ServicePropertiesId = param.Id;
                                d.WorkUnitId = WorkUnitId;
                                d.PriceWorkUnit = PriceWorkUnitId;
                            }
                        }
                        else
                        {
                            ServiceWorkUnit tempServiceWorkUnit = new ServiceWorkUnit();
                            tempServiceWorkUnit.ServicePropertiesId = param.Id;
                            tempServiceWorkUnit.WorkUnitId = WorkUnitId;
                            tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId;
                            _ServiceWorkUnitdb.Add(tempServiceWorkUnit);
                        }

                        var m = _uow.SaveAllChanges();
                    }

                    //------ 960603
                    var listDelt = listOldWorkUnit.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                    foreach (var item in listDelt)
                    {
                        _ServiceWorkUnitdb.Remove(item);
                        var s = _uow.SaveAllChanges();
                    }

                    // SetServiceWorkUnit(Workunits, PriceWorkUnits, param.Id);
                }

                //----------------- ویرایش زیرمجموعه خدمات
                RetrunListChild(param.Id, services.ToList());
                foreach (var item2 in ListServiceChild)
                {
                    var listchild = await _ServiceProperties.GetServiceProperties(item2);
                    listchild.MinCapacity = param.MinCapacity;
                    listchild.MaxCapacity = param.MaxCapacity;
                    //listchild.ServiceCode = param.ServiceCode;
                    //     listchild.PriceWorkUnit = param.PriceWorkUnit;
                    listchild.PriceRegisterServiceProvider = param.PriceRegisterServiceProvider;
                    listchild.PercentOfShares = param.PercentOfShares;
                    listchild.PercentCountReservation = param.PercentCountReservation;
                    listchild.PriceRegisterServiceProvider = param.PriceRegisterServiceProvider;
                    //listchild.CapacityServiceProvider = param.CapacityServiceProvider;
                    var k = _uow.SaveAllChanges();

                    var listOldWorkUnit2 = _ServiceWorkUnitdb.Where(c => c.ServicePropertiesId == item2).ToList();

                    int WorkUnitId2 = 0;
                    int PriceWorkUnitId2 = 0;
                    for (int i = 0; i < Workunits.Count(); i++)
                    {
                        WorkUnitId2 = Workunits[i];
                        PriceWorkUnitId2 = PriceWorkUnits[i];
                        var y = listOldWorkUnit2.Where(c => c.WorkUnitId == WorkUnitId2).ToList();

                        //   if (WorkUnitId2 != 0 && PriceWorkUnitId2 != 0 && y.Count() <= 0)
                        if (WorkUnitId2 != 0 && PriceWorkUnitId2 != 0 && y.Count() > 0)
                        {
                            var d = listOldWorkUnit2.Where(c => c.WorkUnitId == WorkUnitId2).FirstOrDefault();
                            if (d != null)
                            {
                                d.ServicePropertiesId = item2;
                                d.WorkUnitId = WorkUnitId2;
                                d.PriceWorkUnit = PriceWorkUnitId2;
                            }
                        }
                        else
                        {
                            ServiceWorkUnit tempServiceWorkUnit = new ServiceWorkUnit();
                            tempServiceWorkUnit.ServicePropertiesId = item2;
                            tempServiceWorkUnit.WorkUnitId = WorkUnitId2;
                            tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId2;
                            _ServiceWorkUnitdb.Add(tempServiceWorkUnit);
                        }

                        var g = _uow.SaveAllChanges();
                    }

                    var listDelt2 = listOldWorkUnit2.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                    foreach (var item1 in listDelt2)
                    {
                        _ServiceWorkUnitdb.Remove(item1);
                        var d = _uow.SaveAllChanges();
                    }

                    //SetServiceWorkUnit(Workunits, PriceWorkUnits, item2);
                }
                //------- ویرایش سطح خدمت

                EditServiceLevelList(ServiceLavels, PercentServiceLevel, services.ToList(), param.Id);

                //--------------------------------------------- ویرایش خدمت محل زمان افزودن لیست

                var AllLocation = _location.GetAllLocations();
                foreach (var item in AllLocation.ToList())
                {
                    var Status = 0;
                    //---------------------- ویرایش خصوصیات خدمت برای محل
                    ServiceProperties tempServiceProperties = param;
                    var tempServiceLocation =
                        _ServiceLocation.GetAllServiceLocationWirhServiceIds(tempServiceProperties.Id, item.Id);
                    if (tempServiceLocation == null)
                    {
                        //---------------------- ثبت خصوصیات خدمت برای محل
                        DomainClasses.Entities.PrivateTraining.View_ServiceLocations tempServiceLocation2 =
                            new DomainClasses.Entities.PrivateTraining.View_ServiceLocations();
                        tempServiceLocation2.ServiceId = tempServiceProperties.Id;
                        tempServiceLocation2.ServiceCode = tempServiceProperties.ServiceCode;
                        tempServiceLocation2.LocationId = item.Id;
                        tempServiceLocation2.LocationCode = item.LocationCode;
                        tempServiceLocation2.ServiceLocationCode =
                            tempServiceLocation2.LocationCode + "" + tempServiceLocation2.ServiceCode;
                        tempServiceLocation2.MaxCapacityService = tempServiceProperties.MaxCapacity;
                        tempServiceLocation2.MinCapacityService = tempServiceProperties.MinCapacity;
                        tempServiceLocation2.PercentCountReservationService =
                            tempServiceProperties.PercentCountReservation;
                        tempServiceLocation2.CityId = item.CityId;

                        tempServiceLocation2.PercentPriceWorkUnitServiceLocation =
                            item.PercentPriceWorkUnitServiceLocation;
                        tempServiceLocation2.PercentPriceRegisterServiceProvider =
                            item.PercentPriceRegisterServiceProvider;
                        //tempServiceLocation.CapacityServiceProvider = tempServiceProperties.CapacityServiceProvider;
                        tempServiceLocation2.PercentOfShares =
                            item.PercentOfShares + (tempServiceProperties.PercentOfShares);
                        tempServiceLocation2.CalculationPriceRegisterServiceProvider =
                            (tempServiceProperties.PriceRegisterServiceProvider) +
                            ((tempServiceLocation2.PercentPriceRegisterServiceProvider / 100) *
                             (tempServiceProperties.PriceRegisterServiceProvider));

                        await _ServiceLocation.AddServiceLocation(tempServiceLocation2);
                        Status = await _uow.SaveAllChangesAsync();

                        tempServiceLocation = tempServiceLocation2;
                    }

                    //tempServiceLocation.ServiceId = tempServiceProperties.Id;
                    //tempServiceLocation.ServiceCode = tempServiceProperties.ServiceCode;
                    //tempServiceLocation.LocationId = item.Id;
                    //   tempServiceLocation.LocationCode = item.LocationCode;
                    //  tempServiceLocation.ServiceLocationCode = tempServiceLocation.LocationCode + "" + tempServiceLocation.ServiceCode;
                    tempServiceLocation.MaxCapacityService = tempServiceProperties.MaxCapacity;
                    tempServiceLocation.MinCapacityService = tempServiceProperties.MinCapacity;
                    tempServiceLocation.PercentCountReservationService = tempServiceProperties.PercentCountReservation;
                    //  tempServiceLocation.CityId = item.CityId;

                    tempServiceLocation.PercentPriceWorkUnitServiceLocation = item.PercentPriceWorkUnitServiceLocation;
                    tempServiceLocation.PercentPriceRegisterServiceProvider = item.PercentPriceRegisterServiceProvider;
                    //tempServiceLocation.CapacityServiceProvider = tempServiceProperties.CapacityServiceProvider;
                    tempServiceLocation.PercentOfShares =
                        item.PercentOfShares + (tempServiceProperties.PercentOfShares);
                    tempServiceLocation.CalculationPriceRegisterServiceProvider =
                        (tempServiceProperties.PriceRegisterServiceProvider) +
                        ((tempServiceLocation.PercentPriceRegisterServiceProvider / 100) *
                         (tempServiceProperties.PriceRegisterServiceProvider));

                    var h = _uow.SaveAllChanges();

                    var listOldWorkUnit = _ServiceLocationWorkUnit
                        .Where(c => c.ServiceLocationId == tempServiceLocation.Id).ToList();

                    int WorkUnitId4 = 0;
                    int PriceWorkUnitId4 = 0;
                    for (int i = 0; i < Workunits.Count(); i++)
                    {
                        WorkUnitId4 = Workunits[i];
                        PriceWorkUnitId4 = PriceWorkUnits[i];
                        var y = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId4).ToList();

                        if (WorkUnitId4 != 0 && PriceWorkUnitId4 != 0 && y.Count() > 0)
                        {
                            var d = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId4).FirstOrDefault();
                            if (d != null)
                            {
                                d.ServiceLocationId = tempServiceLocation.Id;
                                d.WorkUnitId = WorkUnitId4;
                                d.PriceWorkUnit = PriceWorkUnitId4;
                                d.CalculationPriceServiceLocationWorkUnit =
                                    PriceWorkUnitId4 + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) *
                                    PriceWorkUnitId4;
                            }
                        }
                        else
                        {
                            ServiceLocationWorkUnit tempServiceWorkUnit = new ServiceLocationWorkUnit();
                            tempServiceWorkUnit.ServiceLocationId = tempServiceLocation.Id;
                            tempServiceWorkUnit.WorkUnitId = WorkUnitId4;
                            tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId4;
                            tempServiceWorkUnit.CalculationPriceServiceLocationWorkUnit =
                                PriceWorkUnitId4 + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) *
                                PriceWorkUnitId4;
                            _ServiceLocationWorkUnit.Add(tempServiceWorkUnit);
                        }

                        var f = _uow.SaveAllChanges();
                    }


                    //------ 960603
                    var listDelt = listOldWorkUnit.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                    foreach (var item2 in listDelt)
                    {
                        _ServiceLocationWorkUnit.Remove(item2);
                        var s = _uow.SaveAllChanges();
                    }
                    //    SetServiceLOcationWorkUnit(Workunits, PriceWorkUnits, tempServiceLocation);

                    //----------------

                    RetrunListChild(tempServiceProperties.Id, services.ToList());
                    foreach (var item2 in ListServiceChild)
                    {
                        var tempServiceLocation2 = _ServiceLocation.GetAllServiceLocationWirhServiceIds(item2, item.Id);

                        if (tempServiceLocation2 != null)
                        {
                            //tempServiceLocation.ServiceId = tempServiceProperties.Id;
                            //tempServiceLocation.ServiceCode = tempServiceProperties.ServiceCode;
                            //tempServiceLocation.LocationId = item.Id;
                            // tempServiceLocation.LocationCode = item.LocationCode;
                            //  tempServiceLocation.ServiceLocationCode = tempServiceLocation.LocationCode + "" + tempServiceLocation.ServiceCode;
                            tempServiceLocation2.MaxCapacityService = tempServiceProperties.MaxCapacity;
                            tempServiceLocation2.MinCapacityService = tempServiceProperties.MinCapacity;
                            tempServiceLocation2.PercentCountReservationService =
                                tempServiceProperties.PercentCountReservation;
                            //tempServiceLocation.CityId = item.CityId;

                            tempServiceLocation2.PercentPriceWorkUnitServiceLocation =
                                item.PercentPriceWorkUnitServiceLocation;
                            tempServiceLocation2.PercentPriceRegisterServiceProvider =
                                item.PercentPriceRegisterServiceProvider;
                            //tempServiceLocation.CapacityServiceProvider = tempServiceProperties.CapacityServiceProvider;
                            tempServiceLocation2.PercentOfShares =
                                item.PercentOfShares + (tempServiceProperties.PercentOfShares);
                            tempServiceLocation2.CalculationPriceRegisterServiceProvider =
                                (tempServiceProperties.PriceRegisterServiceProvider) +
                                ((tempServiceLocation2.PercentPriceRegisterServiceProvider / 100) *
                                 (tempServiceProperties.PriceRegisterServiceProvider));

                            var h1 = _uow.SaveAllChanges();

                            var listOldWorkUnit5 = _ServiceLocationWorkUnit
                                .Where(c => c.ServiceLocationId == tempServiceLocation2.Id).ToList();

                            int WorkUnitId3 = 0;
                            int PriceWorkUnitId3 = 0;
                            for (int i = 0; i < Workunits.Count(); i++)
                            {
                                WorkUnitId3 = Workunits[i];
                                PriceWorkUnitId3 = PriceWorkUnits[i];
                                var y = listOldWorkUnit5.Where(c => c.WorkUnitId == WorkUnitId3).ToList();

                                //   if (WorkUnitId2 != 0 && PriceWorkUnitId2 != 0 && y.Count() <= 0)
                                if (WorkUnitId3 != 0 && PriceWorkUnitId3 != 0 && y.Count() > 0)
                                {
                                    var d = listOldWorkUnit5.Where(c => c.WorkUnitId == WorkUnitId3).FirstOrDefault();
                                    if (d != null)
                                    {
                                        d.ServiceLocationId = tempServiceLocation2.Id;
                                        d.WorkUnitId = WorkUnitId3;
                                        d.PriceWorkUnit = PriceWorkUnitId3;
                                        d.CalculationPriceServiceLocationWorkUnit =
                                            PriceWorkUnitId3 +
                                            (tempServiceLocation2.PercentPriceWorkUnitServiceLocation / 100) *
                                            PriceWorkUnitId3;
                                    }
                                }
                                else
                                {
                                    ServiceLocationWorkUnit tempServiceWorkUnit = new ServiceLocationWorkUnit();
                                    tempServiceWorkUnit.ServiceLocationId = tempServiceLocation2.Id;
                                    tempServiceWorkUnit.WorkUnitId = WorkUnitId3;
                                    tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId3;
                                    tempServiceWorkUnit.CalculationPriceServiceLocationWorkUnit =
                                        PriceWorkUnitId3 +
                                        (tempServiceLocation2.PercentPriceWorkUnitServiceLocation / 100) *
                                        PriceWorkUnitId3;
                                    _ServiceLocationWorkUnit.Add(tempServiceWorkUnit);
                                }

                                var g = _uow.SaveAllChanges();
                            }

                            var listDelt5 = listOldWorkUnit5.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                            foreach (var item3 in listDelt5)
                            {
                                _ServiceLocationWorkUnit.Remove(item3);
                                var s = _uow.SaveAllChanges();
                            }

                            //  SetServiceLOcationWorkUnit(Workunits, PriceWorkUnits, tempServiceLocation2);
                        }
                    }
                }

                return Json(new {Result = true, Messages = ""});
            }
            catch (Exception ex)
            {
                return Json(new {Result = false});
            }
        }

        /// <summary>
        /// واحد کار خدمت
        /// </summary>
        /// <param name="Workunits"></param>
        /// <param name="PriceWorkUnits"></param>
        /// <param name="Id"></param>
        public void SetServiceWorkUnit(List<int> Workunits, List<int> PriceWorkUnits, int Id = 0)
        {
            var listOldWorkUnit = _ServiceWorkUnitdb.Where(c => c.ServicePropertiesId == Id).ToList();

            int WorkUnitId = 0;
            int PriceWorkUnitId = 0;
            for (int i = 0; i < Workunits.Count(); i++)
            {
                WorkUnitId = Workunits[i];
                PriceWorkUnitId = PriceWorkUnits[i];
                var y = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId).ToList();

                //    if (WorkUnitId != 0 && PriceWorkUnitId != 0 && y.Count() <= 0)
                if (WorkUnitId != 0 && PriceWorkUnitId != 0 && y.Count() > 0)
                {
                    var d = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId && c.ServicePropertiesId == Id)
                        .FirstOrDefault();
                    if (d != null)
                    {
                        d.ServicePropertiesId = Id;
                        d.WorkUnitId = WorkUnitId;
                        d.PriceWorkUnit = PriceWorkUnitId;
                    }
                }
                else
                {
                    ServiceWorkUnit tempServiceWorkUnit = new ServiceWorkUnit();
                    tempServiceWorkUnit.ServicePropertiesId = Id;
                    tempServiceWorkUnit.WorkUnitId = WorkUnitId;
                    tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId;
                    _ServiceWorkUnitdb.Add(tempServiceWorkUnit);
                }

                var m = _uow.SaveAllChanges();
            }

            //------ 960603
            var listDelt = listOldWorkUnit.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
            foreach (var item in listDelt)
            {
                _ServiceWorkUnitdb.Remove(item);
                var s = _uow.SaveAllChanges();
            }
        }


        /// <summary>
        /// واحد کار خدمت محل
        /// </summary>
        /// <param name="Workunits"></param>
        /// <param name="PriceWorkUnits"></param>
        /// <param name="Id"></param>
        public void SetServiceLOcationWorkUnit(List<int> Workunits, List<int> PriceWorkUnits,
            DomainClasses.Entities.PrivateTraining.View_ServiceLocations tempServiceLocation)
        {
            var listOldWorkUnit = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == tempServiceLocation.Id)
                .ToList();

            int WorkUnitId4 = 0;
            int PriceWorkUnitId4 = 0;
            for (int i = 0; i < Workunits.Count(); i++)
            {
                WorkUnitId4 = Workunits[i];
                PriceWorkUnitId4 = PriceWorkUnits[i];
                var y = listOldWorkUnit.Where(c => c.WorkUnitId == WorkUnitId4).ToList();

                if (WorkUnitId4 != 0 && PriceWorkUnitId4 != 0 && y.Count() > 0)
                {
                    var d = listOldWorkUnit
                        .Where(c => c.WorkUnitId == WorkUnitId4 && c.ServiceLocationId == tempServiceLocation.Id)
                        .FirstOrDefault();
                    if (d != null)
                    {
                        d.ServiceLocationId = tempServiceLocation.Id;
                        d.WorkUnitId = WorkUnitId4;
                        d.PriceWorkUnit = PriceWorkUnitId4;
                        d.CalculationPriceServiceLocationWorkUnit =
                            PriceWorkUnitId4 + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) *
                            PriceWorkUnitId4;
                    }
                }
                else
                {
                    ServiceLocationWorkUnit tempServiceWorkUnit = new ServiceLocationWorkUnit();
                    tempServiceWorkUnit.ServiceLocationId = tempServiceLocation.Id;
                    tempServiceWorkUnit.WorkUnitId = WorkUnitId4;
                    tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId4;
                    tempServiceWorkUnit.CalculationPriceServiceLocationWorkUnit =
                        PriceWorkUnitId4 + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) *
                        PriceWorkUnitId4;
                    _ServiceLocationWorkUnit.Add(tempServiceWorkUnit);
                }

                var f = _uow.SaveAllChanges();
            }

            //------ 960603
            var listDelt = listOldWorkUnit.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
            foreach (var item2 in listDelt)
            {
                _ServiceLocationWorkUnit.Remove(item2);
                var s = _uow.SaveAllChanges();
            }
        }


        public void RetrunListChild(int Id, List<Service> Service)
        {
            var TempService = Service.Where(c => c.ParentId == Id);
            foreach (var item in TempService)
            {
                if (ListServiceChild.IndexOf(item.Id) == -1)
                    ListServiceChild.Add(item.Id);
            }

            foreach (var item in TempService)
            {
                RetrunListChild(item.Id, Service);
            }

            // return List;
        }

        /// <summary>
        ///لیست زیر خدمات
        /// </summary>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> ListSubServiceProperties(int ServicePropertiesId, int DefaultId)
        {
            try
            {
                var ListSubServiceProperties = await _ServiceProperties.GetAllServiceProperties();
                var Listresult = ListSubServiceProperties.Where(c => c.ParentId == ServicePropertiesId).ToList().Select(
                    a => new ServiceProperties()
                    {
                        Id = a.Id,
                        Title = a.Title,
                        MinCapacity = a.MinCapacity,
                        MaxCapacity = a.MaxCapacity,
                        ServiceCode = a.ServiceCode,
                        //CapacityServiceProvider = a.CapacityServiceProvider,
                        //PriceWorkUnit = a.PriceWorkUnit,
                        PriceRegisterServiceProvider = a.PriceRegisterServiceProvider,
                        PercentOfShares = a.PercentOfShares,
                        PercentCountReservation = a.PercentCountReservation,
                    }).ToList();
                if (Listresult.Count() > 0)
                {
                    return Json(new {list = Listresult, Resualt = true});
                }
                else
                    return Json(new {Resualt = false});
            }
            catch (Exception)
            {
                return Json(new {Resualt = false});
            }
        }


        /// <summary>
        /// لود اطلاعات برای ویرایش اطلاعات
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>         
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> LoadEdit(int Id)
        {
            ServiceProperties list = await _ServiceProperties.GetServiceProperties(Id);

            var listWorkUnit = await _WorkUnit.GetAllWorkUnit();
            var ListWorkUnits = listWorkUnit.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
            {
                WorkUnitId = c.Id,
                WorkUnitTitle = c.Title,
                selected = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id),
                PriceWorkUnit = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                    ? c.ServiceWorkUnits.FirstOrDefault(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                        .PriceWorkUnit
                    : 0
            }).ToList();

            ServiceProperties ListTemp = new ServiceProperties();
            ListTemp.Id = list.Id;
            ListTemp.Title = list.Title;
            ListTemp.ServiceCode = list.ServiceCode;
            ListTemp.MinCapacity = list.MinCapacity;
            ListTemp.MaxCapacity = list.MaxCapacity;
            ListTemp.ServiceCode = list.ServiceCode;
            ListTemp.HowPerform = list.HowPerform;
            ListTemp.PriceRegisterServiceProvider = list.PriceRegisterServiceProvider;
            ListTemp.PercentOfShares = list.PercentOfShares;
            //ListTemp.CapacityServiceProvider = list.CapacityServiceProvider;
            ListTemp.PercentCountReservation = list.PercentCountReservation;
            ListTemp.automation = list.automation;
            ListTemp.InitialVisit = list.InitialVisit;
            ListTemp.DescriptionUploud1 = list.DescriptionUploud1;
            ListTemp.DescriptionUploud2 = list.DescriptionUploud2;
            ListTemp.DescriptionUploud3 = list.DescriptionUploud3;

            return Json(new
                {ServiceProperties = ListTemp, WorkUnit = ListWorkUnits, ServiceLevel = await ListServiceLevel(Id)});
        }

        /// <summary>
        /// لیست سطوح
        /// </summary>
        /// <returns></returns>
        public async Task<List<PrivateTraining_View_ServiceLevelList>> ListServiceLevel(int Id = 0)
        {
            ShowPlusMenuAnnuncement SP = new ShowPlusMenuAnnuncement();
            var NewId = SP.GetOneLevelServicePropertiesId(_ServiceProperties.GetAllServiceProperties2(), Id);

            var listServiceLevel = await _ServiceLevel.GetAllServiceLevel();
            var listServiceLevels = listServiceLevel.Where(m => m.ServiceId == Id || m.ServiceId == NewId).ToList()
                .Select(c => new PrivateTraining_View_ServiceLevelList
                {
                    ServiceLevelId = c.Id,
                    ServiceLevelTitle = c.Title,
                    selected = c.ServiceLevelLists.Any(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id),
                    PercentServiceLevel =
                        c.ServiceLevelLists.Any(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id)
                            ? c.ServiceLevelLists
                                .FirstOrDefault(b => b.ServiceLevelId == c.Id && b.ServicePropertiesId == Id)
                                .PercentServiceLevel
                            : 0
                }).ToList();
            return listServiceLevels;
        }

        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> LoadParentServiceProperties(int Id)
        {
            ServiceProperties list = await _ServiceProperties.GetServiceProperties(Id);

            var listWorkUnit = await _WorkUnit.GetAllWorkUnit();
            var ListWorkUnits = listWorkUnit.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
            {
                WorkUnitId = c.Id,
                WorkUnitTitle = c.Title,
                selected = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id),
                PriceWorkUnit = c.ServiceWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                    ? c.ServiceWorkUnits.FirstOrDefault(b => b.WorkUnitId == c.Id && b.ServicePropertiesId == Id)
                        .PriceWorkUnit
                    : 0
            }).ToList();

            ServiceProperties ListTemp = new ServiceProperties();
            ListTemp.MinCapacity = list.MinCapacity;
            ListTemp.MaxCapacity = list.MaxCapacity;
            ListTemp.PriceRegisterServiceProvider = list.PriceRegisterServiceProvider;
            ListTemp.PercentOfShares = list.PercentOfShares;
            //ListTemp.CapacityServiceProvider = list.CapacityServiceProvider;
            ListTemp.PercentCountReservation = list.PercentCountReservation;
            ListTemp.automation = list.automation;
            ListTemp.HowPerform = list.HowPerform;
            ListTemp.InitialVisit = list.InitialVisit;
            ListTemp.DescriptionUploud1 = list.DescriptionUploud1;
            ListTemp.DescriptionUploud2 = list.DescriptionUploud2;
            ListTemp.DescriptionUploud3 = list.DescriptionUploud3;

            return Json(new
            {
                ServiceProperties = ListTemp, ListWorkUnits = ListWorkUnits, ServiceLevel = await ListServiceLevel(Id)
            });
        }

        /// <summary>
        ///نمایش صفحه اختصاصی خدمت
        /// </summary>
        //   [HttpPost]
        public virtual ActionResult ViewfullServiceProperties(int Id)
        {
            var item = _ServicePropertiesdb.Where(c => c.Id == Id).FirstOrDefault();
            return View(item);
        }

        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> LoadWorkUnit()
        {
            var listWorkUnit = await _WorkUnit.GetAllWorkUnit();
            var ListWorkUnits = listWorkUnit.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
            {
                WorkUnitId = c.Id,
                WorkUnitTitle = c.Title,
            }).ToList();


            return Json(new {WorkUnit = ListWorkUnits});
        }

        #region service

        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> GetServiceList()
        {
            var list = await _Service.GetAllService();
            var ListServiceChild = list.ToList().Select(c => new Service
            {
                Id = c.Id,
                Title = c.Title,
            }).ToList();

            return Json(new {Result = true, list = ListServiceChild, Message = ""});
        }

        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public virtual async Task<JsonResult> ReturnParents(int[] Services, int UserId = 0)
        {
            var List = "";
            int LastId = 0;
            if (Services != null)
            {
                Array.Sort(Services);
                foreach (var item in Services)
                {
                    if (LastId == item) // حذف سرویس تکراری
                    {
                        var DelService = _UserService.Where(c => c.UserId == UserId && c.ServiceId == item)
                            .FirstOrDefault();
                        if (DelService != null)
                            _UserService.Remove(DelService);
                        //DelService.IsEnable = false;

                        var DelServiceL = _UserServiceLocation.Where(c => c.UserId == UserId && c.ServiceId == item)
                            .FirstOrDefault();
                        if (DelServiceL != null)
                            _UserServiceLocation.Remove(DelServiceL);
                        //DelServiceL.IsEnable = false;

                        var d = await _uow.SaveAllChangesAsync();
                    }
                    else
                    {
                        parents = _ServicePropertiesdb.Where(c => c.Id == item).FirstOrDefault().Title;
                        var ParentString = ReturnParentsService(item);
                        List += "<div>" + ParentString + "</div>";
                    }

                    LastId = item;
                }
            }

            return Json(new {Result = true, Path = List, Message = ""});
        }

        public string ReturnParentsService(int ServiceId)
        {
            if (ServiceId != 0)
            {
                var Tempservice = _ServicePropertiesdb.Where(c => c.Id == ServiceId).FirstOrDefault().ParentId;
                if (Tempservice != 0)
                {
                    var Title = _ServicePropertiesdb.Where(c => c.Id == Tempservice).FirstOrDefault().Title;
                    parents = Title + " <i class=\"fa fa-angle-left\"></i> " + parents;
                    parents = ReturnParentsService(Tempservice);
                }

                return parents;
            }
            else
            {
                return parents;
            }
        }

        /// <summary>
        /// نمایش لیست قیمت زمان ثبت درخواشت
        /// </summary>
        /// <param name="ServiceReceiverServiceLocationId"></param>
        /// <param name="ServiceId"></param>
        /// <param name="LocationId"></param>
        /// <returns></returns>
        [System.Web.Mvc.HttpPost]
        public virtual async Task<JsonResult> LoadServiceWorkUnit(int ServiceReceiverServiceLocationId = 0,
            int ServiceId = 0, int LocationId = 0,
            int ServiceLevelListId = 0)
        {
            try
            {
                var ServiceLocationId = 0;
                if (ServiceId != 0 && LocationId != 0)
                {
                    var AllServiceLocation = await _ServiceLocation.GetAllServiceLocation();
                    var Query = AllServiceLocation.Where(c => c.ServiceId == ServiceId && c.LocationId == LocationId)
                        .FirstOrDefault();
                    if (Query != null)
                        ServiceLocationId = Query.Id;
                }
                else
                {
                    var Query = _ServiceReceiverServiceLocations.Where(c => c.Id == ServiceReceiverServiceLocationId)
                        .FirstOrDefault();
                    if (Query != null)
                        ServiceLocationId = Query.ServiceLocationId;
                }

                //-------------  قیمت ها از جدول خدمت محل نمایش داده می شوند
                var list = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == ServiceLocationId).ToList();

                //----------------- سطح خدمت
                float Percent = 0;
                var ServiceLevel = _ServiceLevelList.Where(c => c.Id == ServiceLevelListId).FirstOrDefault();
                if (ServiceLevel != null)
                    Percent = ServiceLevel.PercentServiceLevel;
                //-----------------
                List<PrivateTraining_View_ServiceLocationWorkUnit> ListServiceWorkUnits;

                if (Percent == 0)
                    ListServiceWorkUnits = list.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
                    {
                        WorkUnitId = c.WorkUnitId,
                        WorkUnitTitle = c.WorkUnits.Title,
                        PriceWorkUnit = c.PriceWorkUnit,
                    }).ToList();
                else
                {
                    float dd = Percent / 100; // اضافه شدن مبلغ سطح خدمت 
                    ListServiceWorkUnits = list.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
                    {
                        WorkUnitId = c.WorkUnitId,
                        WorkUnitTitle = c.WorkUnits.Title,
                        PriceWorkUnit = c.PriceWorkUnit + (dd * c.PriceWorkUnit),
                    }).ToList();
                }

                return Json(new
                    {Resualt = "true", WorkUnits = ListServiceWorkUnits, ServiceLocationId = ServiceLocationId});
            }
            catch (Exception ex)
            {
                return Json(new {Resualt = "false", WorkUnits = "", ServiceLocationId = ""});
            }
        }

        #endregion
    }
}