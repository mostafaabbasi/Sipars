using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Entities.Security;

namespace PrivateTraining.Areas.PrivateTrain.Controllers
{
    public partial class ScoreController : Controller
    {
        private IUnitOfWork _uow;
        private IDbSet<State> _State;
        private IDbSet<ServiceProperties> _ServiceProperties;
        private Task _userManager;

        public ScoreController(IUnitOfWork uow)
        {
            _uow = uow;
            _State = _uow.Set<State>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
        }
        // GET: PrivateTrain/Score
        public virtual ActionResult GetScoreServiceProvider()
        {
            ViewBag.ListStates = _State.Where(c => c.IsEnable == true).ToList();
            ViewBag.Services = _ServiceProperties.ToList();
            return View();
        }

        //[HttpPost]
        //public virtual async Task<JsonResult> ListScoreServiceProvider(int StateId = 0, int CityId = 0, int LocationId = 0, int ServiceId = 0)
        //{

        //    try
        //    {
        //      //  datatable datatable = new datatable();
        //      //  datatable = datatableclasses.getdatatalbe(Request);

        //      //  var list = await _userManager.GetAllUsers();

        //      //  list = list.OfType<ServiceProviderInfo>();

        //      //  if (StateId != 0 && CityId == 0 && LocationId == 0)
        //      //  {
        //      //      list = list.Where(x => x.StateId == StateId);
        //      //  }
        //      //  else if (StateId != 0 && CityId != 0 && LocationId == 0)
        //      //  {
        //      //      list = list.Where(x => x.CityId == CityId);
        //      //  }
        //      //  else if (StateId != 0 && CityId != 0 && LocationId != 0)
        //      //  {
        //      //      list = list.Where(x => x.UserServiceLocations.Any(y => y.LocationId == LocationId));
        //      //  }
        //      //  if (ServiceId != 0)
        //      //  {
        //      //      list = list.Where(x => x.UserServiceLocations.Any(y => y.ServiceId == ServiceId));
        //      //  }
        //      //  if (StatusUserServiceLocationId == 6) // خدمتیار های جدید
        //      //  {
        //      //      list = list.Where(c => c.UserServices.Any(b => b.ActiveServiceForUser == 0 && b.IsEnable == true));
        //      //  }
        //      //  else if (StatusUserServiceLocationId != 5)
        //      //  {
        //      //      list = list.Where(c => c.UserServiceLocations.Any(b => b.StatusServiceLocationUser == StatusUserServiceLocationId));
        //      //  }

        //      //  if (!string.IsNullOrEmpty(datatable.searchValue))
        //      //  {
        //      //      list = list.Where(c => c.Name.Contains(datatable.searchValue) ||
        //      //                             c.Family.Contains(datatable.searchValue) ||
        //      //                             //   c.FatherName.Contains(datatable.searchValue) ||
        //      //                             c.PersonnelId.ToString().Contains(datatable.searchValue)
        //      //      //  ||  c.NationalCode.Contains(datatable.searchValue) ||
        //      //      //   c.ShId.Contains(datatable.searchValue)
        //      //      );
        //      //  }

        //      //  datatable.recordsTotal = list.Count();
        //      //  list = list.OrderByDescending(c => c.Id).Skip(datatable.skip).Take(datatable.pageSize);


        //      //  // Select Feild
        //      //  datatable.data = list.ToList().Select(rec => new string[]
        //      //  {
        //      //   //"<div class=\"checkbox\" style=\"margin-right:-20px;padding-left:5px;\"><label><input type=\"checkbox\" ng-model=\"checkbox["+rec.Id+"]\" class=\"case\" value=\""+rec.Id +"\" ><span class=\"text\"></span></lable></div>"
        //      //   ""
        //      //  + rec.Id.ToString(),
        //      //  rec.Name +" "+ rec.Family,
        //      //  rec.Email,
        //      //  rec.Mobile,
        //      //  rec.States.Name,
        //      //  rec.Cities.Name,
        //      //  ExitServiceUser(rec.Id).ToString(),
        //      //  ExitLocationUser(rec.Id).ToString(),
        //      //  ServiceProviderCode(rec.Id),
        //      ////  "Status"



        //      //  }).ToList();

        //      //  datatable.draw = datatable.draw;
        //      //  datatable.recordsFiltered = datatable.recordsTotal;
        //      //  datatable.recordsTotal = datatable.recordsTotal;

        //      //  return Json(datatable);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(null);
        //    }

        //}

        [HttpPost]
        public virtual async Task<JsonResult> ss()
        {
            try
            {
                return Json(new { Result = true, Messages = "" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = true, Messages = "" });
            }
        }
    }
}