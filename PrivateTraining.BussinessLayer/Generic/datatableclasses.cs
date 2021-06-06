using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using PrivateTraining.DomainClasses.Entities;

namespace PrivateTraining.BussinessLayer.Generic
{
    public static class datatableclasses
    {
        public static datatable getdatatalbe(HttpRequestBase Request)
        {
            datatable datatable=new datatable();

            datatable.draw = Request.Form.GetValues("draw").FirstOrDefault();

            datatable.start = Request.Form.GetValues("start").FirstOrDefault();
            datatable.length = Request.Form.GetValues("length").FirstOrDefault();

            datatable.sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            datatable.sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            datatable.searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            datatable.pageSize = datatable.length != null ? Convert.ToInt32(datatable.length) : 0;
            datatable.skip = datatable.start != null ? Convert.ToInt32(datatable.start) : 0;
            datatable.recordsTotal = 0;
            return datatable;

        }
    }
}
