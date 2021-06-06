using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using System.Data.Entity;
using PrivateTraining.DomainClasses.EntitiesView;
using Microsoft.AspNet.Identity;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ServiceLocationController : BaseController
    {
        private readonly ILocation _Location;
        private readonly IServiceLocation _ServiceLocation;
        private readonly IUnitOfWork _uow;
        private readonly IServiceProperties _ServiceProperties;
        private IDbSet<State> _State;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private readonly IWorkUnit _WorkUnit;
        List<int> services = new List<int>();
        private readonly IService _Service;
        List<int> ListServiceChild = new List<int>();

        public ServiceLocationController(IUnitOfWork uow, ILocation location, IServiceLocation serviceLocation, IServiceProperties serviceProperties, IWorkUnit workUnit, IService Service)
        {
            _uow = uow;
            _Location = location;
            _ServiceLocation = serviceLocation;
            _ServiceProperties = serviceProperties;
            _State = _uow.Set<State>();
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _WorkUnit = workUnit;
            _Service = Service;

        }
        // GET: PrivateTrain/ServiceLocation
        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult Location()
        {
            ViewBag.ListServices = _ServiceProperties.GetAllServiceProperties();
            ViewBag.ListStates = _State.ToList();
            return View();
        }

        /// <summary>
        /// افزودن محل
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> AddServiceLocation(DomainClasses.EntitiesView.PrivateTrainig.View_ServiceLocations paramServiceLocation, int checkboxPercentOfShares, int checkboxPriceRegisterServiceProvider, int checkboxPriceWorkUnitService)
        {
            try
            {
                int Status = 0;
                Location templocation = new Location();
                templocation.Name = paramServiceLocation.LocationName;
                templocation.LocationCode = paramServiceLocation.LocationCode;
                templocation.CityId = paramServiceLocation.CityId;
                templocation.PercentOfShares = checkboxPercentOfShares * paramServiceLocation.PercentOfShares;
                templocation.PercentPriceWorkUnitServiceLocation = checkboxPriceWorkUnitService * paramServiceLocation.PercentPriceWorkUnitServiceLocation;
                templocation.PercentPriceRegisterServiceProvider = checkboxPriceRegisterServiceProvider * paramServiceLocation.PercentPriceRegisterServiceProvider;

                await _Location.AddLocation(templocation);
                await _uow.SaveAllChangesAsync();
                var locationId = templocation.Id;

                var AllServices = await _ServiceProperties.GetAllServiceProperties();
                var ListServicePropertieslevel0 = AllServices.ToList().Where(c => c.ParentId == 0 && c.IsEnable == true);
                foreach (var Query in ListServicePropertieslevel0)
                {
                    subservice(AllServices, Query.Id);
                }

                foreach (var item in services)
                {
                    //---------------------- ثبت خصوصیات خدمت برای محل
                    ServiceProperties tempServiceProperties = await _ServiceProperties.GetServiceProperties(item);
                    DomainClasses.Entities.PrivateTraining.View_ServiceLocations tempServiceLocation = new DomainClasses.Entities.PrivateTraining.View_ServiceLocations();
                    tempServiceLocation.ServiceId = tempServiceProperties.Id;
                    tempServiceLocation.ServiceCode = tempServiceProperties.ServiceCode;
                    tempServiceLocation.LocationId = locationId;
                    tempServiceLocation.LocationCode = paramServiceLocation.LocationCode;
                    tempServiceLocation.ServiceLocationCode = tempServiceLocation.LocationCode + "" + tempServiceLocation.ServiceCode;
                    tempServiceLocation.MaxCapacityService = tempServiceProperties.MaxCapacity;
                    tempServiceLocation.MinCapacityService = tempServiceProperties.MinCapacity;
                    tempServiceLocation.PercentCountReservationService = tempServiceProperties.PercentCountReservation;
                    tempServiceLocation.CityId = templocation.CityId;

                    tempServiceLocation.PercentPriceWorkUnitServiceLocation = checkboxPriceWorkUnitService * (paramServiceLocation.PercentPriceWorkUnitServiceLocation);
                    tempServiceLocation.PercentPriceRegisterServiceProvider = checkboxPriceRegisterServiceProvider * (paramServiceLocation.PercentPriceRegisterServiceProvider);
                    //tempServiceLocation.CapacityServiceProvider = tempServiceProperties.CapacityServiceProvider;
                    tempServiceLocation.PercentOfShares = checkboxPercentOfShares * (paramServiceLocation.PercentOfShares) + (tempServiceProperties.PercentOfShares);

                    tempServiceLocation.CalculationPriceRegisterServiceProvider = (tempServiceProperties.PriceRegisterServiceProvider) + ((tempServiceLocation.PercentPriceRegisterServiceProvider / 100) * (tempServiceProperties.PriceRegisterServiceProvider));

                    await _ServiceLocation.AddServiceLocation(tempServiceLocation);
                    Status = await _uow.SaveAllChangesAsync();

                    //---------------------- ثبت قیمت های واحد کار تعریف شده خدمت ؛ برای محل
                    var listServiceWorkUnit = _ServiceWorkUnit.Where(c => c.ServicePropertiesId == tempServiceLocation.ServiceId).ToList();
                    foreach (var item2 in listServiceWorkUnit)
                    {
                        ServiceLocationWorkUnit tempServiceLocationWorkUnit = new ServiceLocationWorkUnit();
                        tempServiceLocationWorkUnit.ServiceLocationId = tempServiceLocation.Id;
                        tempServiceLocationWorkUnit.WorkUnitId = item2.WorkUnitId;
                        tempServiceLocationWorkUnit.PriceWorkUnit = item2.PriceWorkUnit;
                        tempServiceLocationWorkUnit.CalculationPriceServiceLocationWorkUnit = item2.PriceWorkUnit + (tempServiceLocation.PercentPriceWorkUnitServiceLocation / 100) * item2.PriceWorkUnit;
                        _ServiceLocationWorkUnit.Add(tempServiceLocationWorkUnit);
                        Status = await _uow.SaveAllChangesAsync();
                    }

                }

                return Json(new { Result = true, Messages = Status });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = "" });
            }

        }

        public void subservice(IEnumerable<ServiceProperties> ServiceProperties, int GId)
        {
            var TempServices = ServiceProperties.Where(c => c.ParentId == GId).ToList();

            if (TempServices.Count() == 0)
            {
                services.Add(GId);
            }

            foreach (var item in TempServices)
            {
                subservice(ServiceProperties, item.Id);
            }

        }

        [HttpPost]
        public virtual async Task<JsonResult> ListLocation()
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _Location.GetAllLocation();
                list = list.Where(c => c.IsEnable == true);

                if (!string.IsNullOrEmpty(datatable.searchValue))
                {
                    // list = list.Where(c => c.Title.Contains(datatable.searchValue));
                }
                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                 "<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" ng-checked=all class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
                /*+rec.Id*/,
                rec.LocationCode.ToString(),
                rec.Name,
                rec.Cities.Name,
                rec.Cities.States.Name,
                StatusName(rec.IsEnable,rec.Id),

                }).ToList();

                datatable.draw = datatable.draw;
                datatable.recordsFiltered = datatable.recordsTotal;
                datatable.recordsTotal = datatable.recordsTotal;

                return Json(datatable);
            }
            catch (Exception ex)
            {

                return Json(null);
            }

        }

        public string StatusName(bool IsEnable, int Id)
        {
            string Status = "";
            if (IsEnable)
                Status = "<a class=\"col-lg-3 btn btn-success shiny btn-circle btn-xs\" title=\"غیرفعال کردن\" ng-click=\"InactiveUser(" + Id + ")\" id=\"" + Id + "\">" +
                         "<i class=\"fa fa-check\" style=\"color:#fff;\"></i></a> ";
            else
                Status = "<a class=\"col-lg-3 btn btn-warning shiny btn-circle btn-xs\" title=\"فعال کردن\" ng-click=\"InactiveUser(" + Id + ")\" id=\"" + Id + "\">" +
                         "<i class=\"fa fa-ban\" style=\"color:#fff;\"></i></a>";

            return Status;
        }

        public virtual async Task<ActionResult> GetListServiceLocation()
        {
            ViewBag.ListStates = _State.ToList();
            ViewBag.Services = await _ServiceProperties.GetAllServiceProperties();
            return View();
        }

        /// <summary>
        /// صفحه لیست خدمت - محل ها
        /// </summary>
        /// 
        [HttpPost]
        public virtual async Task<JsonResult> ListServiceLocation(int StateId = 0, int CityId = 0, int LocationId = 0, int ServiceId = 0)
        {
            try
            {
                datatable datatable = new datatable();
                datatable = datatableclasses.getdatatalbe(Request);

                var list = await _ServiceLocation.GetAllServiceLocation();


                //if (StateId != 0 && CityId == 0 && LocationId == 0)
                //{
                //    list = list.Where(x => x.Cities.StateId == StateId);
                //}
                //else if (StateId != 0 && CityId != 0 && LocationId == 0)
                //{
                //    list = list.Where(x => x.CityId == CityId);
                //}
                //else if (StateId != 0 && CityId != 0 && LocationId != 0)
                //{
                //    list = list.Where(x => x.UserServiceLocations.Any(y => y.LocationId == LocationId));
                //}

                if (StateId != 0)
                    list = list.Where(x => x.Cities.StateId == StateId);
                if (CityId != 0)
                    list = list.Where(x => x.CityId == CityId);
                if (LocationId != 0)
                    list = list.Where(x => x.LocationId == LocationId);
                if (ServiceId != 0)
                {
                    var services = await _Service.GetAllService();
                    RetrunListChild(ServiceId, services.ToList());
                    ListServiceChild.Add(ServiceId);

                    list = list.Where(x => ListServiceChild.Contains(x.ServiceId));
                    //  list = list.Where(x => x.ServiceId == ServiceId);
                }

                if (!string.IsNullOrEmpty(datatable.searchValue))
                    list = list.Where(c => c.ServiceLocationCode.Contains(datatable.searchValue));
                datatable.recordsTotal = list.Count();
                list = list.OrderBy(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);

                // Select Feild
                datatable.data = list.ToList().Select(rec => new string[]
                {
                rec.Id.ToString(),
                rec.Cities.States.Name,
                rec.Cities.Name,
                rec.Locations.Name,
                rec.Services.Title,
                rec.LocationCode.ToString(),
                rec.ServiceCode.ToString(),
                rec.ServiceLocationCode,
                }).ToList();

                datatable.draw = datatable.draw;
                datatable.recordsFiltered = datatable.recordsTotal;
                datatable.recordsTotal = datatable.recordsTotal;

                return Json(datatable);
            }
            catch (Exception ex)
            {

                return Json(null);
            }

        }

        //[HttpPost]
        //public virtual async Task<JsonResult> ListServiceLocation2()
        //{
        //    try
        //    {
        //        var ListServiceLocation = await _ServiceLocation.GetAllServiceLocation();
        //        var Listresult = ListServiceLocation.Where(c => c.IsEnable == true).ToList().Select(a => new ServiceLocation()
        //        {
        //            Id = a.Id,
        //            LocationCode = a.LocationCode,
        //            ServiceCode = a.ServiceCode,
        //            ServiceLocationCode = a.ServiceLocationCode,
        //            MaxCapacityService = a.MaxCapacityService,
        //            MinCapacityService = a.MinCapacityService,
        //            PercentCountReservationService = a.PercentCountReservationService,
        //            PriceWorkUnitService = a.PriceWorkUnitService,
        //            PriceRegisterServiceProvider = a.PriceRegisterServiceProvider,
        //            CapacityServiceProvider = a.CapacityServiceProvider,
        //            PercentOfShares = a.PercentOfShares,
        //        }).ToList();
        //        return Json(new { Result = true, List = Listresult });
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(null);
        //    }

        //}

        // [HttpPost]
        /// <summary>
        /// صفحه اختصاصی خدمت - محل
        /// </summary>
        public virtual ActionResult GetServiceLocationPrivate(int Id = 0)
        {
            ViewBag.Id = Id;
            ViewBag.ListStates = _State.ToList();
            return View();
        }

        /// <summary>
        /// لود اطلاعات خدمت محل برای ویرایش
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditServiceLocationPrivate(int Id)
        {
            try
            {
                var ListServiceLocation = await _ServiceLocation.GetAllServiceLocation();
                var tempServiceLocation = ListServiceLocation.Where(c => c.Id == Id).ToList().Select(a => new DomainClasses.EntitiesView.PrivateTrainig.View_ServiceLocations
                {
                    ServiceName = a.Services.Title,
                    LocationName = a.Locations.Name,
                    Id = a.Id,
                    LocationCode = a.LocationCode,
                    ServiceCode = a.ServiceCode,
                    ServiceLocationCode = a.ServiceLocationCode,
                    MaxCapacityService = a.MaxCapacityService,
                    MinCapacityService = a.MinCapacityService,
                    PercentCountReservationService = a.PercentCountReservationService,
                    PercentPriceWorkUnitServiceLocation = a.PercentPriceWorkUnitServiceLocation,// PriceWorkUnitService,
                    PercentPriceRegisterServiceProvider = a.PercentPriceRegisterServiceProvider,// PriceRegisterServiceProvider,
                    //CapacityServiceProvider = a.CapacityServiceProvider,
                    PercentOfShares = a.PercentOfShares,
                    // CalculationPricePercentOfShares = a.CalculationPricePercentOfShares,
                    CalculationPriceRegisterServiceProvider = a.CalculationPriceRegisterServiceProvider,
                    //CalculationPriceServiceLocationWorkUnit=a.CalculationPriceServiceLocationWorkUnit,
                    CityId = a.CityId,
                    StateId = a.Cities.StateId,
                    LocationId = a.LocationId,

                }).FirstOrDefault();


                var listService = await _ServiceProperties.GetAllServiceProperties();
                var ListServiceChild = listService.ToList().Select(c => new Service
                {
                    Id = c.Id,
                    Title = c.Title,
                }).ToList();


                var listWorkUnit = await _WorkUnit.GetAllWorkUnit();
                var ListWorkUnits = listWorkUnit.ToList().Select(c => new PrivateTraining_View_ServiceLocationWorkUnit
                {
                    WorkUnitId = c.Id,
                    WorkUnitTitle = c.Title,
                    selected = c.ServiceLocationWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServiceLocationId == Id),
                    PriceWorkUnit = c.ServiceLocationWorkUnits.Any(b => b.WorkUnitId == c.Id && b.ServiceLocationId == Id) ? c.ServiceLocationWorkUnits.FirstOrDefault(b => b.WorkUnitId == c.Id && b.ServiceLocationId == Id).PriceWorkUnit : 0

                }).ToList();

                return Json(new { Result = true, tempServiceLocation = tempServiceLocation, listServicePeroperties = ListServiceChild, listWorkUnits = ListWorkUnits });
            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }

        /// <summary>
        /// ویرایش خدمت محل
        /// </summary>

        [HttpPost]
        public virtual async Task<JsonResult> EditServiceLocationPrivate(DomainClasses.Entities.PrivateTraining.View_ServiceLocations paramServiceLocation, List<int> Workunits, List<int> PriceWorkUnits)
        {
            try
            {

                DomainClasses.Entities.PrivateTraining.View_ServiceLocations tempServiceLocation = await _ServiceLocation.GetServiceLocation(paramServiceLocation.Id);
                tempServiceLocation.MaxCapacityService = paramServiceLocation.MaxCapacityService;
                tempServiceLocation.MinCapacityService = paramServiceLocation.MinCapacityService;
                tempServiceLocation.PercentOfShares = paramServiceLocation.PercentOfShares;
                tempServiceLocation.PercentCountReservationService = paramServiceLocation.PercentCountReservationService;
                //tempServiceLocation.CapacityServiceProvider = paramServiceLocation.CapacityServiceProvider;
                tempServiceLocation.CalculationPriceRegisterServiceProvider = paramServiceLocation.CalculationPriceRegisterServiceProvider;
                var listworkold = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == paramServiceLocation.Id).ToList();

                int WorkUnitId1 = 0;
                int PriceWorkUnitId = 0;
                if (Workunits != null)
                {
                    for (int i = 0; i < Workunits.Count(); i++)
                    {

                        WorkUnitId1 = Convert.ToInt32(Workunits[i]);
                        PriceWorkUnitId = PriceWorkUnits[i];
                        var y = listworkold.Where(c => c.WorkUnitId == Workunits[i]).FirstOrDefault();

                        if (Workunits[i] != 0 && PriceWorkUnitId != 0 && y == null)
                        {
                            ServiceLocationWorkUnit tempServiceLocationWorkUnit = new ServiceLocationWorkUnit();
                            tempServiceLocationWorkUnit.ServiceLocationId = paramServiceLocation.Id;
                            tempServiceLocationWorkUnit.WorkUnitId = Workunits[i];
                            tempServiceLocationWorkUnit.PriceWorkUnit = PriceWorkUnitId;
                            _ServiceLocationWorkUnit.Add(tempServiceLocationWorkUnit);
                        }
                        else if (Workunits[i] != 0 && PriceWorkUnitId != 0 && y != null)
                        {
                            y.PriceWorkUnit = PriceWorkUnitId;
                        }
                    }

                    var listDelt = listworkold.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                    foreach (var item in listDelt)
                    {
                        _ServiceLocationWorkUnit.Remove(item);
                        await _uow.SaveAllChangesAsync();
                    }
                }
                else
                {
                    var listDelt = listworkold.Where(c => c.ServiceLocationId == paramServiceLocation.Id).ToList();
                    foreach (var item in listDelt)
                    {
                        _ServiceLocationWorkUnit.Remove(item);
                        await _uow.SaveAllChangesAsync();
                    }
                }
                //---------------------------------------------- ویرایش زیر مجموعه
                var services2 = await _Service.GetAllService();

                RetrunListChild(tempServiceLocation.ServiceId, services2.ToList());
                foreach (var item2 in ListServiceChild)
                {
                    var tempServiceLocation2 = _ServiceLocation.GetAllServiceLocationWirhServiceIds(item2, tempServiceLocation.LocationId);
                    tempServiceLocation2.MaxCapacityService = paramServiceLocation.MaxCapacityService;
                    tempServiceLocation2.MinCapacityService = paramServiceLocation.MinCapacityService;
                    tempServiceLocation2.PercentOfShares = paramServiceLocation.PercentOfShares;
                    tempServiceLocation2.PercentCountReservationService = paramServiceLocation.PercentCountReservationService;
                    tempServiceLocation2.CalculationPriceRegisterServiceProvider = paramServiceLocation.CalculationPriceRegisterServiceProvider;

                    var h1 = _uow.SaveAllChanges();

                    var listOldWorkUnit2 = _ServiceLocationWorkUnit.Where(c => c.ServiceLocationId == tempServiceLocation2.Id).ToList();

                    int WorkUnitId3 = 0;
                    int PriceWorkUnitId3 = 0;
                    for (int i = 0; i < Workunits.Count(); i++)
                    {

                        WorkUnitId3 = Workunits[i];
                        PriceWorkUnitId3 = PriceWorkUnits[i];
                        var y = listOldWorkUnit2.Where(c => c.WorkUnitId == WorkUnitId3).ToList();

                        //   if (WorkUnitId2 != 0 && PriceWorkUnitId2 != 0 && y.Count() <= 0)
                        if (WorkUnitId3 != 0 && PriceWorkUnitId3 != 0 && y.Count() > 0)
                        {

                            var d = listOldWorkUnit2.Where(c => c.WorkUnitId == WorkUnitId3).FirstOrDefault();
                            if (d != null)
                            {
                                d.ServiceLocationId = tempServiceLocation2.Id;
                                d.WorkUnitId = WorkUnitId3;
                                d.PriceWorkUnit = PriceWorkUnitId3;
                                d.CalculationPriceServiceLocationWorkUnit = PriceWorkUnitId3 + (tempServiceLocation2.PercentPriceWorkUnitServiceLocation / 100) * PriceWorkUnitId3;
                            }
                        }
                        else
                        {
                            ServiceLocationWorkUnit tempServiceWorkUnit = new ServiceLocationWorkUnit();
                            tempServiceWorkUnit.ServiceLocationId = tempServiceLocation2.Id;
                            tempServiceWorkUnit.WorkUnitId = WorkUnitId3;
                            tempServiceWorkUnit.PriceWorkUnit = PriceWorkUnitId3;
                            tempServiceWorkUnit.CalculationPriceServiceLocationWorkUnit = PriceWorkUnitId3 + (tempServiceLocation2.PercentPriceWorkUnitServiceLocation / 100) * PriceWorkUnitId3;
                            _ServiceLocationWorkUnit.Add(tempServiceWorkUnit);
                        }
                        var g = _uow.SaveAllChanges();
                    }

                    var listDelt2 = listOldWorkUnit2.Where(c => !Workunits.Contains(c.WorkUnitId)).ToList();
                    foreach (var item3 in listDelt2)
                    {
                        _ServiceLocationWorkUnit.Remove(item3);
                        var s = _uow.SaveAllChanges();
                    }
                    //  SetServiceLOcationWorkUnit(Workunits, PriceWorkUnits, tempServiceLocation2);


                }
                //----------------------------------------------
                await _uow.SaveAllChangesAsync();

                return Json(new { Result = true, Messages = "با موفقیت ویرایش شد." });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Messages = "خطا" });
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
        /// لود اطلاعات  محل برای ویرایش
        /// </summary>
        [HttpPost]
        public virtual async Task<JsonResult> LoadEditLocation(int Id)
        {
            try
            {
                var Alllocation = await _Location.GetAllLocation();
                //  var h = Alllocation.Where(c => c.Id == Id).ToList();
                var templocation = Alllocation.Where(c => c.Id == Id).ToList().Select(a => new DomainClasses.EntitiesView.PrivateTrainig.View_ServiceLocations
                {
                    LocationId = a.Id,
                    LocationName = a.Name,
                    LocationCode = a.LocationCode,
                    //PercentPriceWorkUnitServiceLocation = a.ServiceLocations.Where(c => c.LocationId == a.Id).FirstOrDefault().PercentPriceWorkUnitServiceLocation,
                    //PercentPriceRegisterServiceProvider = a.ServiceLocations.Where(c => c.LocationId == a.Id).FirstOrDefault().PercentPriceRegisterServiceProvider,
                    //PercentOfShares = a.ServiceLocations.Where(c => c.LocationId == a.Id).FirstOrDefault().PercentOfShares,
                    PercentPriceWorkUnitServiceLocation = a.PercentPriceWorkUnitServiceLocation,
                    PercentPriceRegisterServiceProvider = a.PercentPriceRegisterServiceProvider,
                    PercentOfShares = a.PercentOfShares,
                    CityId = a.CityId,
                    StateId = a.Cities.StateId,
                }).FirstOrDefault();

                return Json(new { Result = true, Message = " با موفقیت انجام شد", templocation = templocation });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "خطا در برقراری ارتباط" });
            }

        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteLocation(string[] LocationId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= LocationId.Length - 1; i++)
                {
                    IdS = LocationId[i].ToString();
                    var g = await _Location.GetLocation(Convert.ToInt32(LocationId[i]));
                    g.IsEnable = false;
                    var d = await _uow.SaveAllChangesAsync();
                    //  await _Location.DeleteLocation(Convert.ToInt32(LocationId[i]));
                }
                return Json(new { Result = true, Message = "حذف با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> DeleteServiceLocation(string[] ServiceLocationId)
        {
            string IdS = "0";
            try
            {
                for (int i = 0; i <= ServiceLocationId.Length - 1; i++)
                {
                    IdS = ServiceLocationId[i].ToString();
                    await _ServiceLocation.DeleteServiceLocation(Convert.ToInt32(ServiceLocationId[i]));
                }
                return Json(new { Result = true, Message = "حذف با موفقیت انجام شد" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "امکان حذف برای کد " + IdS + " وجود ندارد" });
            }
        }

        [HttpPost]
        public virtual async Task<JsonResult> EditLocationAndServiceLocations(DomainClasses.EntitiesView.PrivateTrainig.View_ServiceLocations param, int id, int checkboxPercentOfShares, int checkboxPriceRegisterServiceProvider, int checkboxPriceWorkUnitService)
        {
            try
            {
                var list = await _Location.GetLocation(id);
                if (list != null)
                {
                    list.CityId = param.CityId;
                    list.Name = param.LocationName;
                    list.LocationCode = param.LocationCode;

                    list.PercentOfShares = checkboxPercentOfShares * param.PercentOfShares;
                    list.PercentPriceWorkUnitServiceLocation = checkboxPriceWorkUnitService * param.PercentPriceWorkUnitServiceLocation;
                    list.PercentPriceRegisterServiceProvider = checkboxPriceRegisterServiceProvider * param.PercentPriceRegisterServiceProvider;
                }

                var AllSL = await _ServiceLocation.GetAllServiceLocation();
                var SL = AllSL.ToList().Where(c => c.LocationId == id);
                foreach (var item in SL)
                {
                    var tempServiceProperties = await _ServiceProperties.GetServiceProperties(item.ServiceId);

                    item.LocationCode = param.LocationCode;
                    item.ServiceLocationCode = item.ServiceCode + "" + param.LocationCode;

                    item.PercentPriceWorkUnitServiceLocation = checkboxPriceWorkUnitService * (param.PercentPriceWorkUnitServiceLocation);
                    item.PercentPriceRegisterServiceProvider = checkboxPriceRegisterServiceProvider * (param.PercentPriceRegisterServiceProvider);
                    //  item.PercentOfShares = checkboxPercentOfShares * (param.PercentOfShares) + (tempServiceProperties.PercentOfShares);
                    item.PercentOfShares = checkboxPercentOfShares * (param.PercentOfShares);
                    item.CalculationPriceRegisterServiceProvider = Math.Round(((tempServiceProperties.PriceRegisterServiceProvider) + ((item.PercentPriceRegisterServiceProvider / 100) * (tempServiceProperties.PriceRegisterServiceProvider))) / 100) * 100;
                    var listServiceWorkUnit = _ServiceWorkUnit.Where(c => c.ServicePropertiesId == item.ServiceId).ToList();
                    foreach (var item2 in listServiceWorkUnit)
                    {
                        var tempSLW = _ServiceLocationWorkUnit.Where(c => c.Id == item2.Id).FirstOrDefault();
                        if (tempSLW != null)
                            tempSLW.CalculationPriceServiceLocationWorkUnit = item2.PriceWorkUnit + (item.PercentPriceWorkUnitServiceLocation / 100) * item2.PriceWorkUnit;
                    }
                }

                await _uow.SaveAllChangesAsync();
                return Json(new { Result = true, Messages = "" });
            }

            catch (Exception ex)
            {
                return Json(new { Result = false });
            }
        }

        /// <summary>
        /// نمایش مناطق شهرها
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult ListLocationGetByCityId(int CityId, int DefaultLocationId = 0,int UserId=0)
        {
            try
            {

                var CurentUserId = 0;
                if (UserId == 0)
                    CurentUserId = Convert.ToInt32(User.Identity.GetUserId());
                else
                    CurentUserId = UserId;

                var ListLocation = _Location.GetAllLocation().Result.Select(a => new PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig.PrivateTraining_View_LocationUsers
                {
                    Id = a.Id,
                    Name = a.Name,
                    selected = a.UserLocations.Any(b => b.LocationId == a.Id && b.UserId == CurentUserId && b.IsEnable == true),
                    CityId = a.CityId,
                }).ToList();

                if (CityId != 0)
                    ListLocation = ListLocation.Where(c => c.CityId == CityId).ToList();

                if (ListLocation.Count() > 0)
                {
                    return Json(new { list = ListLocation, Resualt = true });
                }
                else
                {
                    return Json(new { Resualt = false });
                }

            }
            catch (Exception)
            {
                return Json(new { Resualt = false, JsonRequestBehavior.AllowGet });
            }
        }

        /// <summary>
        /// نمایش محل - خدمت
        /// </summary>
        [HttpPost]
        public virtual JsonResult ListServiceLocationGetByCityId(int CityId)
        {
            try
            {
                var ListLocation = _Location.GetAllLocation().Result.Where(c => c.CityId == CityId).ToList().Select(a => new DomainClasses.EntitiesView.PrivateTrainig.View_ServiceLocations
                {
                    LocationId = a.Id,
                    LocationName = a.Name,
                    Services = a.ServiceLocations.Where(c => c.LocationId == a.Id).Select(d => new View_ServiceInLocation
                    {
                        ServiceLocationid = d.Id,
                        ServiceId = d.ServiceId,
                        Servicename = d.Services.Title,
                        Selected = a.UserServiceLocations.Any(t => t.UserId.ToString() == User.Identity.GetUserId() && t.ServiceLocationId == d.Id && t.IsEnable == true)
                    }).ToList(),

                }).ToList();

                if (ListLocation.Count() > 0)
                {
                    return Json(new { list = ListLocation, Resualt = true });
                }
                else
                {
                    return Json(new { Resualt = false });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Resualt = false, Messages = ex.Message });
            }
        }

    }
}