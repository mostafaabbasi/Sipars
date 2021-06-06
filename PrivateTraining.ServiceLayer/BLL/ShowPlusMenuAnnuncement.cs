using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using PrivateTraining.DomainClasses.Entities;

namespace PrivateTraining.ServiceLayer.BLL
{
    public class ShowPlusMenuAnnuncement
    {
        private readonly IServiceProperties _ServiceProperties;
        public IDbSet<DomainClasses.Entities.PrivateTraining.View_ServiceLocations> _ServiceLocation;

        //private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        //private readonly IUnitOfWork _uow;
        //public ShowPlusMenuAnnuncement(IUnitOfWork uow)
        //{
        //    _uow = uow;
        //    _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
        //}

        /// <summary>
        /// نمایش منو مدیر
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string ShowPlusMenu(int Id, List<ServiceProperties> ServiceProperties)
        {

            string List = "";
            //var Model =await _ServiceProperties.GetAllServiceProperties();
            var ListServiceProperties = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true).OrderBy(c => c.Id);

            foreach (var Query in ListServiceProperties)
            {
                List += " <div class=\"tree-folder\" style=\"display: none;\"> " +
                           " <div class=\"tree-folder-header\">";
                var Exitchild = ServiceProperties.Where(c => c.ParentId == Query.Id).ToList();
                if (Exitchild.Count() == 0)
                {
                    List += " <div class=\"tree-folder-name\" style=\"font-weight:bold;padding-right:15px;\">" +
                                    Query.Title +
                                   " <div class=\"tree-actions\">" +
                                      "<i class=\"fa fa-plus green\" ng-click=\"ShowAddServicePropertiesModal(" + Query.Id + ",0,0)\"></i> " +
                                      "<i class=\"fa fa-edit blue\" ng-click=\"ShowEditModal(" + Query.Id + ",1," + "'" + Query.Title + "'" + ")\"></i>" +
                                         "  <i class=\"fa fa-trash-o danger\" ng-click=\"showDeleteModal(" + Query.Id + ")\"></i>" +
                                         "  <i class=\"fa fa-rotate-right blizzard\"></i>" +
                                    " <i class=\"fa fa-pencil blue\" ng-click=\"$root.showEditServiceDialog(" + Query.Id + "," + "'" + Query.Title + "'" + ")\"></i>"+
                                      " </div>" +
                                  " </div>";
                }
                else
                {
                    List += "<i class=\"fa fa-folder\" ng-click=\"changeOpenMinus($event) \" ></i>" +
                             " <div class=\"tree-folder-name\" ng-click=\"changeOpenMinus($event) \" >" +
                             "<span class=\"text\" id=\"txtspan_" + Query.Id + "\" style=\"padding-right:5px; \">" + Query.Title + " </span>"+
                            " <div class=\"tree-actions\">" +
                            "<i class=\"fa fa-plus green\" ng-click=\"ShowAddServicePropertiesModal(" + Query.Id + ",0,0)\"></i> " +
                            "<i class=\"fa fa-edit blue\" ng-click=\"ShowEditModal(" + Query.Id + ",1," + "'" + Query.Title + "'" + ")\"></i>" +
                                "  <i class=\"fa fa-trash-o danger\" ng-click=\"showDeleteModal(" + Query.Id + ")\"></i>" +
                                "  <i class=\"fa fa-rotate-right blizzard\"></i>" +
                             " <i class=\"fa fa-pencil blue\" ng-click=\"$root.showEditServiceDialog(" + Query.Id + "," + "'" + Query.Title + "'" + ")\"></i>"+
                            " </div>" +
                             "</div>";
                }
                List += "  </div>" +
                                 "  <div class=\"tree-folder-content\" style=\"display: block; \">";
                if (ListServiceProperties.Where(c => c.ParentId == Query.Id) != null)
                {
                    List += ShowPlusMenu(Query.Id, ServiceProperties);
                }
                List += " </div>" +
                      "  </div>";

            }

            return List;
        }

        /// <summary>
        /// نمایش زیر منو صفحه اصلی
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string ShowSubMenu(int Id, int Level, IEnumerable<ServiceProperties> ServiceProperties)
        {
            var Model = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true).OrderBy(c => c.Title);
            string List = "", Style = "", Clas = "";

            if (Model.Count() > 0)
            {
                if (Level == 1)
                {
                    List += "<div class=\"cat-left-drop-menu-single  \" >";
                    Clas = "class=\"mega-ServiceProperties-header mega-ServiceProperties pull-right\"";
                }
                else List += "<div>";

                if (Level == 2)
                {
                    Style = "padding-right:0px;\"";
                }
                else if (Level == 3)
                {
                    Style = "style=\"padding-right:20px;\"";
                }
                else if (Level == 4)
                {
                    Style = "style=\"padding-right:40px;\"";
                }

                List += "<ul>";
                foreach (var Query in Model)
                {

                    List += "<li  class=\"menu-item\">" +
                        "<a href=\"?Gid=" + Query.Id + "\" " + Clas + Style + " ><span>" +
                             Query.Title +
                         "</a>" + ShowSubMenu(Query.Id, Query.Level, ServiceProperties) +
                      "</li>";
                }
                List += "</ul></div>";
                // List += "</div></div>";




            }
            return List;
        }


        /// <summary>
        /// نمایش نقشه نام گروه محصولات
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        //public string FunServicePropertiesName(int GId, IEnumerable<ServiceProperties> ServiceProperties, int Type = (int)AnnouncementType.Free)
        //{
        //    string ListServicePropertiesName = "";
        //    int Level = ServiceProperties.Where(c => c.Id == GId).FirstOrDefault().Level;
        //    if (Level > 0)
        //    {
        //        for (int i = 1; i <= Level; i++)
        //        {
        //            ListServicePropertiesName += ServiceProperties.Where(c => c.Id == GId).FirstOrDefault().Title + "|" + GId + ">";
        //            int ParentId = ServiceProperties.Where(c => c.Id == GId).FirstOrDefault().ParentId;
        //            GId = ParentId;
        //        }
        //    }

        //    string[] ListArray = ListServicePropertiesName.Substring(0, ListServicePropertiesName.Length - 1).Split('>');
        //    ListServicePropertiesName = "";
        //    for (int i = ListArray.Length - 1; i >= 0; i--)
        //    {
        //        string[] Name = ListArray[i].Split('|');
        //        if (Type == (int)AnnouncementType.Auction)
        //            ListServicePropertiesName += "<a style=\"color:#FF6F73;\" href=\"/Announcement/AuctionAnnoucements?T=0&G=" + Name[1] + "&B=0\">" + Name[0] + "</a> >>";
        //        else
        //            ListServicePropertiesName += "<a style=\"color:#FF6F73;\" href=\"/?Gid=" + Name[1] + "\">" + Name[0] + "</a> >>";

        //    }
        //    ListServicePropertiesName = ListServicePropertiesName.Substring(0, ListServicePropertiesName.Length - 2).Replace(">>", "<span style=\"color:#ddd;font-weight:bold;font-size:12px;\" > &nbsp; > &nbsp;</span>");
        //    return ListServicePropertiesName;
        //}

        /// <summary>
        /// بدست آوردن  ریشه Id
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        public int GetOneLevelServicePropertiesId(IEnumerable<ServiceProperties> ServiceProperties, int GId = 0)
        {
            if (GId != 0)
            {
                int Level = ServiceProperties.FirstOrDefault(c => c.Id == GId).Level;
                if (Level > 0)
                {
                    for (int i = 1; i < Level; i++)
                    {
                        int ParentId = ServiceProperties.FirstOrDefault(c => c.Id == GId).ParentId;
                        GId = ParentId;
                    }
                }
            }
            return GId;
        }

        /// <summary>
        /// بدست آوردن  مرحله قبل Id
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        public int GetPreviousLevelServicePropertiesId(IEnumerable<ServiceProperties> ServiceProperties, int GId)
        {
            int Level = ServiceProperties.FirstOrDefault(c => c.Id == GId).Level;
            if (Level > 1)
            {
                GId = ServiceProperties.FirstOrDefault(c => c.Id == GId).ParentId;
            }
            return GId;
        }

        /// <summary>
        ///  ها مرحله به مرحله Id
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        public string GetListSubServicePropertiesId(IEnumerable<ServiceProperties> ServiceProperties, int GId)
        {
            string list = GId + ",";
            int Level = ServiceProperties.FirstOrDefault(c => c.Id == GId).Level;
            if (Level > 0)
            {
                for (int i = 2; i <= Level; i++)
                {
                    int ParentId = ServiceProperties.FirstOrDefault(c => c.Id == GId).ParentId;
                    list += ParentId + ",";
                    GId = ParentId;
                }
            }
            return list.Substring(0, list.Length - 1);
        }

        /// <summary>
        /// بدست آوردن نام ریشه
        /// </summary>
        /// <param name="GId"></param>
        /// <returns></returns>
        public string GetOneLevelName(IEnumerable<ServiceProperties> ServiceProperties, int GId = 0)
        {
            return ServiceProperties.FirstOrDefault(c => c.Id == GetOneLevelServicePropertiesId(ServiceProperties, GId)).Title;
        }

        public void ImportFile()
        {
            string sourcePath = @"D:\1";
            string targetPath = @"D:\1\New folder";
            string fileName = "Zip-Code.zip";

            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            System.IO.File.Copy(sourceFile, destFile, true);

            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                foreach (string s in files)
                {
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }

        }

        /// <summary>
        /// نمایش زیر منو خدمات زمان ثبت نام خدمت دهنده
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ServiceProperties"></param>
        /// <param name="ListServiceWorkUnits"></param>
        /// <param name="ListUserServices"></param>
        /// <param name="UserServiceLocation"></param>
        /// <returns></returns>
        public string ShowListPlusMenu(int Id, List<ServiceProperties> ServiceProperties, List<ServiceWorkUnit> ListServiceWorkUnits,
            List<UserService> ListUserServices, List<UserServiceLocation> UserServiceLocation)
        {

            string List = "";
            //var Model =await _ServiceProperties.GetAllServiceProperties();
            var ListServiceProperties = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true).OrderBy(c => c.Id);

            foreach (var Query in ListServiceProperties)
            {
                List += " <div class=\"tree-folder\" style=\"display: none;\"> " +
                           " <div class=\"tree-folder-header\">";
                //if (ListServiceProperties.Where(c => c.ParentId == Query.Id).Count() == 0)
                if (ServiceProperties.Where(c => c.ParentId == Query.Id).Count() == 0)
                {
                    List += " <div class=\"tree-folder-name\" >" +
                        "<div class=\"radio\">" +
                             " <label id=\"txtspan_" + Query.Id + "\" >" + Query.Title +
                                  "<span class=\"text-success\" id=\"DivCountCapacity_"+ Query.Id + "\"> " + CheckCapacity(Query.Id, ListUserServices, Query.MaxCapacity, Query.PercentCountReservation, UserServiceLocation) + " نفر  </span>" +
                                  "<input type = \"radio\" class=\" RadioService\" ng-model=\"Services_" + Query.Id + "\" ng-value=" + Query.Id + " ng-click=CheckHowPerformService(" + Query.Id + ") name=\"Services\"   > " +
                                  "<span class=\"text\"   style=\"padding-right: 7px\"  > </span>" +
                              "</label>" +
                          "</div>";
                }
                else
                {
                    List += "<i class=\"fa fa-folder\" ng-click=\"changeOpenMinus($event) \"></i>" +
                        " <div class=\"tree-folder-name\" ng-click=\"changeOpenMinus($event) \" >" +
                        "<span class=\"text\" id=\"txtspan_" + Query.Id + "\" style=\"padding-right:5px; \">" + Query.Title + " </span>";
                }
                List += " </div>" +
                "  </div>" +
                "  <div class=\"tree-folder-content\" style=\"display: block; \">";
                if (ListServiceProperties.Where(c => c.ParentId == Query.Id) != null)
                {
                    List += ShowListPlusMenu(Query.Id, ServiceProperties, ListServiceWorkUnits, ListUserServices, UserServiceLocation);
                }
                List += " </div>" +
                      "  </div>";

            }

            return List;
        }

        /// <summary>
        /// نمایش ظرفیت اصلی و ظرفیت رزرو بریا خدمت دهنده ها زمان ثبت نام
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="ListUserServices"></param>
        /// <param name="MaxCapacity"></param>
        /// <param name="PercentCountReservation"></param>
        /// <param name="UserServiceLocation"></param>
        /// <returns></returns>
        public string CheckCapacity(int ServiceId, List<UserService> ListUserServices, int MaxCapacity, int PercentCountReservation, List<UserServiceLocation> UserServiceLocation)
        {
            var P = (byte)StatusServiceLocationUser.PendingApproval;
            var A = (byte)StatusServiceLocationUser.Active;
            var R = (byte)StatusServiceLocationUser.Reservation;
            var U = (byte)StatusServiceLocationUser.SubmitInformation;

            //   MaxCapacity = _ServiceLocation.Where(c=>c.ServiceId==ServiceId && c.LocationId);
            // ----------------  تعداد دفعاتی که این خدمت توسط خدمت دهنده ها انتخاب شده
            //var CountUserService = ListUserServices.Where(c => c.ServiceId == ServiceId).Count();
            var CountUserService = UserServiceLocation.Where(c => c.ServiceId == ServiceId &&
           (c.StatusServiceLocationUser == P || c.StatusServiceLocationUser == A || c.StatusServiceLocationUser == U)).Count();

            //------------- اختلاف حداکثر ظرفیت از ثبت نام شده ها
            string s = "ظرفیت باقیمانده : " + (MaxCapacity - CountUserService).ToString() + " &nbsp; - &nbsp; ";

            //-------------- تعداد رزرو مجاز
            var CountReserves = Math.Floor((PercentCountReservation / 100.0) * MaxCapacity);
            //-------------- رزرو شده ها
            var Reserves = UserServiceLocation.Where(c => c.ServiceId == ServiceId && c.StatusServiceLocationUser == R).Count();
            //--------------  باقیمانده رزرو
            s += "ظرفیت رزرو : " + (CountReserves - Reserves).ToString();

            //  return s;
            return "ظرفیت باقیمانده : 0   -   ظرفیت رزرو : 0";
        }

        public string CheckCapacityWithLocation(int ServiceId, List<UserService> ListUserServices, int MaxCapacity, double PercentCountReservation, List<UserServiceLocation> UserServiceLocation,int LocationId=0)
        {
            var P = (byte)StatusServiceLocationUser.PendingApproval;
            var A = (byte)StatusServiceLocationUser.Active;
            var R = (byte)StatusServiceLocationUser.Reservation;
            var U = (byte)StatusServiceLocationUser.SubmitInformation;

            var CountUserService = UserServiceLocation.Where(c => c.ServiceId == ServiceId && c.LocationId== LocationId &&
           (c.StatusServiceLocationUser == P || c.StatusServiceLocationUser == A || c.StatusServiceLocationUser == U )).Count();

            //------------- اختلاف حداکثر ظرفیت از ثبت نام شده ها
            string s = "ظرفیت باقیمانده : " + (MaxCapacity - CountUserService).ToString() + " &nbsp; - &nbsp; ";

            //-------------- تعداد رزرو مجاز
            var CountReserves = Math.Floor((PercentCountReservation / 100.0) * MaxCapacity);
            //-------------- رزرو شده ها
            var Reserves = UserServiceLocation.Where(c => c.ServiceId == ServiceId && c.StatusServiceLocationUser == R).Count();
            //--------------  باقیمانده رزرو
            s += "ظرفیت رزرو : " + (CountReserves - Reserves).ToString();

            return s;
        }

        /// <summary>
        /// نمایش زیر منو خدمات زمان ثبت درخواست
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ServiceProperties"></param>
        /// <param name="ListServiceWorkUnits"></param>
        /// <returns></returns>
        public string ShowListPlusMenuByCheckbox(int Id, List<PrivateTraining_View_ServiceUsers> ServiceProperties, List<ServiceWorkUnit> ListServiceWorkUnits)
        {
            string List = "";
            //var Model =await _ServiceProperties.GetAllServiceProperties();
            var ListServiceProperties = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true).OrderBy(c => c.Id);

            foreach (var Query in ListServiceProperties)
            {
                List += " <div class=\"tree-folder\" style=\"display: none;\"> " +
                            " <div class=\"tree-folder-header\">";
                var Exitchild = ServiceProperties.Where(c => c.ParentId == Query.Id).ToList();
                if (Exitchild.Count() == 0)
                {
                    List += " <div class=\"tree-folder-name\" >" +
                                   "<div class=\"checkbox\" >" +
                                        " <label id=\"txtspan_" + Query.Id + "\">" + Query.Title;
                    // "<span class=\"text-success\"> ظرفیت : " + Query.MaxCapacity + " نفر  </span>";
                    //List += "<input type = \"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"@item.selected\"  name=\"Services\" ng-click=\"ServiceSelected(" + Query.Id + ")\"  ng-value=\"" + Query.Id + "\" ng-model=\"Services_" + Query.Id + "\"  >";
                    if (Query.selected == true)
                    {
                        List += "<input type =\"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"true\" ng-model=\"Services.Service[" + Query.Id + "]\"  name=\"Services\"  ng-value=\"" + Query.Id + "\"  ng-click='ServiceSelected(" + Query.Id + ")'>";
                    }
                    else if (Query.selected == false)
                    {
                        List += "<input type =\"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"false\" ng-model=\"Services.Service[" + Query.Id + "]\"  name=\"Services\"  ng-value=\"" + Query.Id + "\"  ng-click='ServiceSelected(" + Query.Id + ")'>";
                    }
                    List += "<span class=\"text\"   style=\"padding-right: 7px\"  > </span>" +
                                                        "</label>" +

                "<button class=\"btn btn-magenta btn-sm btnDesc\" style=\"display: inline-block\"  ng-click=\"ShowModalSpecialConditionsOfWork(" + Query.Id + ") \">توضیحات</button>" +
                "<div class=\"tree-actions2\"><button class=\"btn btn-info btn-sm\" style=\"margin-left:3px; \" ng-click=\"ShowServiceProvider(" + Query.Id + ")\" >لیست خدمتیاران</button><button class=\"btn btn-warning btn-sm\" style=\"margin-left:3px; \" ng-click=\"ShowWorkUnits(" + Query.Id + ")\">انتخاب با سیستم </button>" +
        "<button class=\"btn btn-success btn-sm \"  ng-click=\"ListPrice(" + Query.Id + ") \">تعرفه ها</button>" +
        "<span class=\"NotExit_" + Query.Id + " \" style=\"display: none\">خدمت دهنده ای وجود ندارد</span></div>" +
           "</div></div>";
                }
                else
                {
                    List += "<i class=\"fa fa-folder\" ng-click=\"changeOpenMinus($event) \" ></i>" +
                             " <div class=\"tree-folder-name\" ng-click=\"changeOpenMinus($event) \" >" +
                             "<span class=\"text\" id=\"txtspan_"+ Query.Id + "\" style=\"padding-right:5px; \">" + Query.Title + " </span></div>";
                }

                List += "  </div>" +
                "  <div class=\"tree-folder-content\" style=\"display: block; \">";
                if (Exitchild.Count() != 0)
                {
                    List += ShowListPlusMenuByCheckbox(Query.Id, ServiceProperties, ListServiceWorkUnits);
                }
                List += " </div>" +
                     "  </div> ";

            }

            return List;
        }

        /// <summary>
        /// نمایش زیر منو خدمات زمان ثبت درخواست
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="ServiceProperties"></param>
        /// <param name="ListServiceWorkUnits"></param>
        /// <returns></returns>
        public string ShowListPlusMenuByCheckboxArray(int Id, List<PrivateTraining_View_ServiceUsers> ServiceProperties, List<ServiceWorkUnit> ListServiceWorkUnits)
        {
            string List = "";
            //var Model =await _ServiceProperties.GetAllServiceProperties();
            var ListServiceProperties = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true).OrderBy(c => c.Id);

            foreach (var Query in ListServiceProperties)
            {
                List += " <div class=\"tree-folder\" style=\"display: none;\"> " +
                            " <div class=\"tree-folder-header\">";
                var Exitchild = ServiceProperties.Where(c => c.ParentId == Query.Id).ToList();
                if (Exitchild.Count() == 0)
                {
                    List += " <div class=\"tree-folder-name\" >" +
                                   "<div class=\"checkbox\" >" +
                                        " <label id=\"txtspan_" + Query.Id + "\" >" + Query.Title;
                    //   "<span class=\"text-success\"> ظرفیت : " + Query.MaxCapacity + " نفر  </span>";

                    //List += "<input type = \"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"@item.selected\"  name=\"Services\" ng-click=\"ServiceSelected(" + Query.Id + ")\"  ng-value=\"" + Query.Id + "\" ng-model=\"Services_" + Query.Id + "\"  >";
                    if (Query.selected == true)
                    {
                        List += "<input type =\"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"true\" ng-model=\"Services.Service[" + Query.Id + "]\"  name=\"Services\"  ng-value=\"" + Query.Id + "\"  ng-click='ServiceSelected(" + Query.Id + ")'>";
                    }
                    else if (Query.selected == false)
                    {
                        List += "<input type =\"checkbox\" class=\"CheckboxTreeService\" ng-checked=\"false\" ng-model=\"Services.Service[" + Query.Id + "]\"  name=\"Services\"  ng-value=\"" + Query.Id + "\"  ng-click='ServiceSelected(" + Query.Id + ")'>";
                    }
                    List += "<span class=\"text\"   style=\"padding-right: 7px\"  > </span>" +
                                                        "</label>" +

                  "<div class=\"tree-actions2\"><button class=\"btn btn-info btn-sm\" style=\"margin-left:3px; \" ng-click=\"ShowServiceProvider(" + Query.Id + ")\" >لیست خدمتیاران</button>  <button class=\"btn btn-warning btn-sm\" style=\"margin-left:3px; \"  ng-click=\"ShowWorkUnits(" + Query.Id + ")\">انتخاب با سیستم</button><button class=\"btn btn-success btn-sm \"  ng-click=\"ListPrice(" + Query.Id + ") \">تعرفه ها</button><span class=\"NotExit_" + Query.Id + " \" style=\"display: none\">خدمت دهنده ای وجود ندارد</span></div>" +


                "</div></div>";
                }
                else
                {
                    List += "<i class=\"fa fa-folder\" ng-click=\"changeOpenMinus($event) \" ></i>" +
                             " <div class=\"tree-folder-name\" ng-click=\"changeOpenMinus($event) \" >" +
                             "<span class=\"text\" id=\"txtspan_" + Query.Id + "\" style=\"padding-right:5px; \">" + Query.Title + " </span></div>";
                }

                List += "  </div>" +
                "  <div class=\"tree-folder-content\" style=\"display: block; \">";
                if (Exitchild.Count() != 0)
                {
                    List += ShowListPlusMenuByCheckbox(Query.Id, ServiceProperties, ListServiceWorkUnits);
                }
                List += " </div>" +
                     "  </div> ";

            }

            return List;
        }

        public string ListSubMenuServices(int Id, List<PrivateTraining_View_ServiceUsers> ServiceProperties)
        {
            string List = "";
            var ListServiceProperties = ServiceProperties.Where(c => c.ParentId == Id && c.IsEnable == true);
            List += "<ul class='DivSub ' >";
            foreach (var Query in ListServiceProperties)
            {
                var Exitchild = ServiceProperties.Where(c => c.ParentId == Query.Id).ToList();

                // List += "<li ><a  class=\"Level_" + Query.Level + "  \" href=\"javascript:void(0)\"  title=\" " + Query.Title +" \" class=\"sub" + Query.Level+ " \">" + Query.Title + "</a>";
                if (Exitchild.Count() == 0)
                {
                    List += "<li ><a  class=\"Level_" + Query.Level + "  \" href=\"/privateTraining#/\"  title=\" " + Query.Title + " \" class=\"sub" + Query.Level + " \">" + Query.Title + "</a></li>";
                }
                else if (Exitchild.Count() != 0)
                {
                    List += "<li ><a  class=\"Level_" + Query.Level + "  \" href=\"javascript:void(0)\"  title=\" " + Query.Title + " \" class=\"sub" + Query.Level + " \">" + Query.Title + "<span class=\"glyphicon glyphicon-chevron-down\"></span></a>";
                    List += ListSubMenuServices(Query.Id, ServiceProperties);
                    List += "</li>";
                }
            }
            List += "</ul>";
            return List;
        }
    }
}
