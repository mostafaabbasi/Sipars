using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.Climbings;
using PrivateTraining.ServiceLayer.Extention;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class UploadFile
    {
        private IDbSet<ClimbingFile> _climbingfile;
        private readonly IUnitOfWork _uow;

        public UploadFile(IUnitOfWork uow)
        {
            _uow = uow;
            _climbingfile = _uow.Set<ClimbingFile>();
        }

        public string FunInsertFile(List<HttpPostedFileBase> file, int UserId, string PathImages)
        {
            return "";
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string InsertFile(List<HttpPostedFileBase> file, UploadFileEnum PathImages, int ClubId = 0, byte FileTypes = 0, int UserId = 0,
            Nullable<int> ProgramId = null, bool IsEnable = true)
        {
            var PathImagesTemp = PathImages;
            PersianDate PersianDate = new PersianDate();
            string Date = PersianDate.PersianDateLow();
            ClimbingFile UFile = new ClimbingFile();
            int Counter = 1;

            if (file != null)
            {
                foreach (var item in file)
                {
                    if (item != null && item.ContentLength > 0)
                    {
                        string FileTypee = item.FileName.Substring(item.FileName.LastIndexOf("."), item.FileName.Length - item.FileName.LastIndexOf("."));
                        var fileName = Counter.ToString() + "_" + RandomNumber(1, 1000).ToString() + Path.GetFileName(Date + FileTypee);
                        var path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.GetDescription()), fileName);
                        //  var path = Path.Combine(PathImagesTemp.GetDescription(), fileName);
                        //   var path = System.Web.Hosting.HostingEnvironment.MapPath("~/UserFiles/Club/") + fileName;
                        item.SaveAs(path);

                        var Logo = Convert.ToInt32(FileType.Logo);
                        var UserPic = Convert.ToInt32(FileType.UserPic);

                        var Query = _climbingfile.Where(c => c.ClubId == ClubId && c.FileType == FileTypes && FileTypes == Logo).FirstOrDefault();
                        var Query2 = _climbingfile.Where(c => c.ClubId == 0 && c.ProgramId == 0 && c.FileType == FileTypes &&
                                                        FileTypes == UserPic && c.UserId == UserId).FirstOrDefault();
                        DeleteFiles DF = new DeleteFiles();

                        if (Query2 != null)
                        {
                            DF.DeleteFile(Query2.FileName, "ProfilePicture");

                            Query2.FileType = FileTypes;
                            Query2.UserId = UserId;
                            Query2.FilePath = PathImagesTemp.GetDescription();
                            Query2.FileName = fileName;
                            Query2.ClubId = ClubId;
                            Query2.ProgramId = ProgramId;
                        }
                        else if (Query != null)
                        {
                            DF.DeleteFile(Query.FileName, "Club");

                            Query.FileType = FileTypes;
                            Query.UserId = UserId;
                            Query.FilePath = PathImagesTemp.GetDescription();
                            Query.FileName = fileName;
                            Query.ClubId = ClubId;
                            Query.ProgramId = ProgramId;
                        }
                        else
                        {
                            UFile.FileType = FileTypes;
                            UFile.UserId = UserId;
                            UFile.FilePath = PathImagesTemp.GetDescription();
                            UFile.FileName = fileName;
                            UFile.ClubId = ClubId;
                            UFile.ProgramId = ProgramId;
                            UFile.IsEnable = IsEnable;
                            _climbingfile.Add(UFile);
                        }
                        _uow.SaveAllChanges();
                        Counter++;

                        if (FileTypee != ".pdf")
                        {
                            // برش با ارتفاع 300 برای نمایش آگهی ها
                            WebImage img = new WebImage(item.InputStream);
                            if (img.Height > 300)
                            {
                                //    img = img.Resize(200, 200, true, true);
                                img.Resize(width: 300, height: 300, preserveAspectRatio: true, preventEnlarge: true);
                                img.Crop(1, 1, 0, 0);
                            }
                            fileName = "300_" + fileName;
                            path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.GetDescription()), fileName);
                            img.Save(path);

                            // برش با ارتفاع 100 برای نمایش عکس ویرایش و اسلایدر

                            img.Resize(width: 80, height: 80, preserveAspectRatio: true, preventEnlarge: true);
                            img.Crop(1, 1, 0, 0);
                            fileName = "80_" + fileName.Replace("300_", "");
                            path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(PathImagesTemp.GetDescription()), fileName);
                            img.Save(path);
                        }
                    }
                }
            }
            return "";
        }
    }
}
