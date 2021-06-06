using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.BussinessLayer.Generic;
using PrivateTraining.ServiceLayer.Interface.Security;

namespace PrivateTraining.Areas.Test.Controllers
{
   
    public  class TestController : ApiController
    {
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public  async Task<JsonResult> ListCity(int StateId, int DefaultCityId = 0)
        {
            return null;
        }
    }
}