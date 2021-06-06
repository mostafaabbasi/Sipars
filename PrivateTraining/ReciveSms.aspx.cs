using PrivateTraining.DataLayer.Context;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PrivateTraining
{
    public partial class ReciveSms : System.Web.UI.Page
    {
        private IUnitOfWork _uow { get; set; }
        private IApplicationUserManager _userManager { get; set; }

        //public ReciveSms(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //}

        public void Page_Load(object sender, EventArgs e)
        {
            var from = "";
            var text = "";
            var to = "";

            if (Request["from"] != null)
                from = Request["from"];

            if (Request["text"] != null)
                text += Request["text"];

            if (Request["to"] != null)
                to += Request["to"];

            //   lblcheck.Text = "from="+ from+ "  ___  text="+ text+ "  ___  to="+ to;

            //  text = "0_" + from + " _ " + text + " _ " + to;
            //PersianDate PD = new PersianDate();

            //using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            //using (var command = new SqlCommand(" insert into [Framework].[SMSsReceived] (UserId,SenderNumber,ReceiverNumber,[Content],Status,StatusType,Date,Time,IsEnable) "
            //    + "values (1,'" + from + "','" + to + "',N'" + "0_" + from + " _ " + text + " _ " + to + "','hhhh',1,'" + PD.PersianDateLow() + "','" + PD.CurrentTime() + "',0)", connection))
            //{
            //    connection.Open();
            //    command.ExecuteNonQuery();
            //    connection.Close();
            //}

            Response.Redirect("/Framework/Sms/ReceiveSms?from=" + from + "&text=" + text + "&to=" + to);

        }
    }
}