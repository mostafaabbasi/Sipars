using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.StaticMethods;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.IrSms;


#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// </summary>
    public class TestingController : BaseApiController
    {
        #region DI

        public TestingController(IUnitOfWork uow, ILocation location)
        {
        }

        #endregion

        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CityList()
        {
            var suc = await Telegram.SendLogFromGoogleMessage("salam");

            return Ok(new {result = suc});
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "";
        }

        /// <summary>
        /// نمایش شهر ها
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ip()
        {
            string str = "";
//            using (var sqlCmd = new SqlCommand("select distinct local_net_address, local_tcp_port from sys.dm_exec_connections where local_net_address is not null"))
//            using (var sqlCmd = new SqlCommand("RESTORE DATABASE testDB FROM DISK = 'C:\\siparsir.bak'"))
            
//            using (var sqlCmd = new SqlCommand(@"RESTORE DATABASE testDB
//            FROM DISK = 'C:\\siparsir.bak'
//            WITH MOVE 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\siparsir_PrivateTraning_afd8eb5de7db45f7876b8d2e902ad189.mdf' TO 'C:\\testDB.mdf',
//            MOVE 'C:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\siparsir_PrivateTraning_afd8eb5de7db45f7876b8d2e902ad189.ldf' TO 'C:\\testDB.ldf'"))
//
//                
//            using (var sqlCmd = new SqlCommand(@"RESTORE FILELISTONLY
//                                                    FROM DISK = 'C:\\siparsir.bak'"))
//            {
//                using (var sqlConnection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;User Instance=True"))
//                {
//                    sqlCmd.Connection = sqlConnection;
//                    sqlCmd.CommandType = CommandType.Text;
//                    await sqlConnection.OpenAsync();
//                    
//                    SqlDataReader reader = sqlCmd.ExecuteReader();
//                    
//                    reader.Read();
//                    try
//                    {
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetValue(2) + "\n";  
//                         str += reader.GetValue(3) + "\n";  
//                         str += reader.GetValue(4) + "\n";  
//                         str += reader.GetValue(5) + "\n";  
//                         str += reader.GetValue(6) + "\n";  
//                         str += reader.GetValue(7) + "\n";  
//                         str += reader.GetValue(8) + "\n";  
//                         str += reader.GetValue(9) + "\n";  
//                         reader.Read();
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetValue(2) + "\n";  
//                         str += reader.GetValue(3) + "\n";  
//                         str += reader.GetValue(4) + "\n";  
//                         str += reader.GetValue(5) + "\n";  
//                         str += reader.GetValue(6) + "\n";  
//                         str += reader.GetValue(7) + "\n";  
//                         str += reader.GetValue(8) + "\n";  
//                         str += reader.GetValue(9) + "\n"; 
//                         reader.Read();
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetValue(2) + "\n";  
//                         str += reader.GetValue(3) + "\n";  
//                         str += reader.GetValue(4) + "\n";  
//                         str += reader.GetValue(5) + "\n";  
//                         str += reader.GetValue(6) + "\n";  
//                         str += reader.GetValue(7) + "\n";  
//                         str += reader.GetValue(8) + "\n";  
//                         str += reader.GetValue(9) + "\n"; 
//                         reader.Read();
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetValue(2) + "\n";  
//                         str += reader.GetValue(3) + "\n";  
//                         str += reader.GetValue(4) + "\n";  
//                         str += reader.GetValue(5) + "\n";  
//                         str += reader.GetValue(6) + "\n";  
//                         str += reader.GetValue(7) + "\n";  
//                         str += reader.GetValue(8) + "\n";  
//                         str += reader.GetValue(9) + "\n"; 
//                         reader.Read();
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetValue(2) + "\n";  
//                         str += reader.GetValue(3) + "\n";  
//                         str += reader.GetValue(4) + "\n";  
//                         str += reader.GetValue(5) + "\n";  
//                         str += reader.GetValue(6) + "\n";  
//                         str += reader.GetValue(7) + "\n";  
//                         str += reader.GetValue(8) + "\n";  
//                         str += reader.GetValue(9) + "\n"; 
//                         
//                    }
//                    catch (Exception e)
//                    {
//                    }
//                   
//                    if (sqlConnection.State != ConnectionState.Closed)
//                        sqlConnection.Close();
//                }
//            }  
            
//            using (var sqlCmd = new SqlCommand(@"RESTORE DATABASE testDB
//            FROM DISK = 'C:\\siparsir.bak'
//            WITH MOVE 'PrivateTraining' TO 'C:\database\testDB.mdf',
//            MOVE 'PrivateTraining_log' TO 'C:\database\testDB.ldf'"))
//            {
//                using (var sqlConnection = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=test;Integrated Security=True;User Instance=True"))
//                {
//                    sqlCmd.Connection = sqlConnection;
//                    sqlCmd.CommandType = CommandType.Text;
//                    await sqlConnection.OpenAsync();
//                    
//                    SqlDataReader reader = sqlCmd.ExecuteReader();
//                    
//                    reader.Read();
//                    try
//                    {
//                         str += reader.GetValue(0) + "\n";  
//                         str += reader.GetValue(1) + "\n";  
//                         str += reader.GetString(0) + "\n";  
//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine(e);
//                        throw;
//                    }
//                   
//                    if (sqlConnection.State != ConnectionState.Closed)
//                        sqlConnection.Close();
//                }
//            }
////            
            //
            return Ok(new {result = GetLocalIPAddress(), str});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> s()
        {
            PrivateTraining.ServiceLayer.IrSms.API_SMSServer sms = new API_SMSServer();
            sms.sendsms("salam", "salam", "salam", "salam", "salam", "salam");
            sms.sendsmsAsync("salam", "salam", "salam", "salam", "salam", "salam");

            return Ok(new {result = "done"});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IHttpActionResult> t1()
        {
            var startDateTime = DateTime.Now;
            var req = HttpContext.Current?.Request;
            var res = HttpContext.Current?.Response;
            var items = HttpContext.Current?.Items;
            var msg = new PrivateTraining.Utils.Logger().Test(this.Request, req, null, res, startDateTime, "", "",
                items);


            return Ok(new {result = "done", msg});
        }
    }
}