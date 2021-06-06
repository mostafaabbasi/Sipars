using PrivateTraining.ServiceLayer.Extention;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class DeleteFiles
    {
        /// <summary>
        /// حذف فیزیکی عکس ها
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public string DeleteFile(string FileName,string Type)
        {
            string strMessage = "";
            try
            {
                var path = "";
                if (Type == "Club")
                    path = UploadFileEnum.Club.GetDescription();
                if (Type == "Program")
                    path = UploadFileEnum.Program.GetDescription();
                if (Type == "ProfilePicture")
                    path = UploadFileEnum.ProfilePicture.GetDescription();

                Deletes(System.Web.Hosting.HostingEnvironment.MapPath(path + "/" + FileName));
                Deletes(System.Web.Hosting.HostingEnvironment.MapPath(path + "/80_" + FileName));
                Deletes(System.Web.Hosting.HostingEnvironment.MapPath(path + "/300_" + FileName));
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            return strMessage;
        }

        public string Deletes(string fullPath)
        {
            string strMessage = "";
            try
            {
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                strMessage = ex.Message;
            }
            return strMessage;
        }


    }
}
