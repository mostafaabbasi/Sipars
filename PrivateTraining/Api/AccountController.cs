using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.DomainClasses.EntitiesView.PrivateTrainig;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.LocationLayer.Interface.PrivateTraining;
using PrivateTraining.Models;
using PrivateTraining.ServiceLayer.BLL;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Interface.PrivateTraining;
using PrivateTraining.ServiceLayer.Interface.Security;
using PrivateTraining.ServiceLocationLayer.Interface.PrivateTraining;
using ApplicationUser = PrivateTraining.DomainClasses.Security.ApplicationUser;

#pragma warning disable 1591

namespace PrivateTraining.Api
{
    /// <summary>
    /// for auth users...
    /// </summary>
    public class AccountController : BaseApiController
    {
        #region DI

        private readonly IAuthenticationManager _authenticationManager;
        private readonly IApplicationSignInManager _signInManager;
        private readonly IApplicationUserManager _userManager;
        private IUnitOfWork _uow;
        private IDbSet<SupplementaryInfoUser> _SupplementaryInfoUser;
        private IDbSet<SuspensionUser> _SuspensionUser;
        private IDbSet<CustomRole> _CustomRole;

        private IDbSet<ApplicationUser> _User;

        //  private readonly ILeaveRequest _LeaveRequest;
        private readonly IServiceProviderInfo _ServiceProviderInfo;
        private readonly IServiceReceiverInfo _ServiceReceiverInfo;
        private readonly IServiceLocation _servicelocation;
        private IDbSet<City> _City;
        private IDbSet<State> _State;
        private IDbSet<Location> _Location;

        private IDbSet<ServiceProperties> _ServiceProperties;

        //private IDbSet<ServiceLocation> _ServiceLocation;
        private IDbSet<UserServiceLocation> _UserServiceLocations;
        private IDbSet<ServiceWorkUnit> _ServiceWorkUnit;
        private IDbSet<UserService> _UserService;
        private IDbSet<UserLocation> _UserLocation;
        private IDbSet<ServiceLocationWorkUnit> _ServiceLocationWorkUnit;
        private IDbSet<DebtServiceProvider> _DebtServiceProvider;
        private IDbSet<Debt> _Debt;
        private IDbSet<UserFile> _UserFile;
        private IDbSet<UserServiceScore> _UserServiceScore;
        private readonly ILocation _RepLocation;
        private IDbSet<ServiceReceiverServiceLocation> _ServiceReceiverServiceLocations;
        private readonly IGroupPolicy _group;
        private IDbSet<PrivateSetting> _privatesetting;
        List<string> ListServiceChild = new List<string>();
        private readonly IService _Service;
        private IDbSet<ServiceLevelList> _ServiceLevelList;
        private readonly IServiceLevel _ServiceLevel;


        private IServiceReceiverInfo _serviceReceiver;
        private IServiceReceiverServiceLocation _servicereceiveservicelocation;

        public AccountController(IApplicationUserManager userManager,
            IApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager,
            IUnitOfWork uow,
            // ILeaveRequest leaveRequest,
            IServiceProviderInfo serviceProviderInfo,
            IServiceReceiverInfo serviceReceiverInfo,
            IServiceLocation servicelocation,
            IGroupPolicy group, IService Service, IServiceLevel servicelevel,
            ILocation location,
            IServiceReceiverInfo serviceReceiver,
            IServiceReceiverServiceLocation servicereceiveservicelocation
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
            _uow = uow;
            _SupplementaryInfoUser = _uow.Set<SupplementaryInfoUser>();
            _SuspensionUser = _uow.Set<SuspensionUser>();
            _CustomRole = _uow.Set<CustomRole>();
            _User = _uow.Set<ApplicationUser>();
            //   _LeaveRequest = leaveRequest;
            _ServiceProviderInfo = serviceProviderInfo;
            _ServiceReceiverInfo = serviceReceiverInfo;
            _City = _uow.Set<City>();
            _State = _uow.Set<State>();
            _Location = _uow.Set<Location>();
            _ServiceProperties = _uow.Set<ServiceProperties>();
            _UserServiceLocations = _uow.Set<UserServiceLocation>();
            _ServiceWorkUnit = _uow.Set<ServiceWorkUnit>();
            _UserService = _uow.Set<UserService>();
            _UserLocation = _uow.Set<UserLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _RepLocation = location;
            _UserServiceScore = _uow.Set<UserServiceScore>();
            _servicelocation = servicelocation;
            _ServiceReceiverServiceLocations = _uow.Set<ServiceReceiverServiceLocation>();
            _ServiceLocationWorkUnit = _uow.Set<ServiceLocationWorkUnit>();
            _DebtServiceProvider = _uow.Set<DebtServiceProvider>();
            _Debt = _uow.Set<Debt>();
            _UserFile = _uow.Set<UserFile>();
            _group = group;
            _privatesetting = _uow.Set<PrivateSetting>();
            _Service = Service;
            _ServiceLevel = servicelevel;
            _ServiceLevelList = _uow.Set<ServiceLevelList>();


            _serviceReceiver = serviceReceiver;
            _servicereceiveservicelocation = servicereceiveservicelocation;
        }

        #endregion

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Subscribe(JObject input)
        {
            if (!User.Identity.IsAuthenticated)
                return Ok(new {result = "error", message = "need login to Subscribe!"});
            
            var subscription = input["subscription"]?.Value<string>();
            var userId = input["userId"]?.Value<int>();
            var oldUserId = input["oldUserId"]?.Value<int>();

            if (string.IsNullOrEmpty(subscription) || userId == null)
            {
                return Ok(new {result = "error", message = "ورودی صحیح نیست !"});
            }

            var user = await _userManager.FindByIdAsync((int) userId);
            if (user == null) return Ok(new {result = "not found", user = userId});

            user.Subscription = subscription;
            
            var rowAffected = await _uow.SaveAllChangesAsync();
            if (rowAffected != 1)
            {
                //error
                return Ok(new {result = "error", message = "ذخیره سازی تغییرات در پایگاه داده به خطایی برخورد!"});
            }

            if (oldUserId != null)
            {
                var oldUser = await _userManager.FindByIdAsync((int) oldUserId);
                if (oldUser != null)
                {
                    oldUser.Subscription = null;
                }
                await _uow.SaveAllChangesAsync();
            }

            return Ok(new {result = "done"});
            
        }
        

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> Test(JObject input)
        {
            // var users = _User.Where(u => u.Id == 1087 || u.Id == 1 || u.Id == 2 || u.NationalCode == "admin").Select(
            //     user => new
            //     {
            //         name = user.Name,
            //         family = user.Family,
            //         sex = user.Sex,
            //         id = user.Id,
            //         email = user.Email,
            //         mobile = user.Mobile,
            //         credit = user.Credit,
            //         addressJson = user.AddressJson,
            //         path = user.Path,
            //         picture = user.Picture,
            //         nationalCode = user.NationalCode,
            //         homePhone = user.HomePhone,
            //         userType = user.UserType,
            //     });

            // foreach (var user in users)
            // {
            //     var code = await _userManager.GeneratePasswordResetTokenAsync(1087);
            //     var result = await _userManager.ResetPasswordAsync(1087, code, "123456");
            // // }
            //
            //  code = await _userManager.GeneratePasswordResetTokenAsync(1);
            //  result = await _userManager.ResetPasswordAsync(1, code, "123456");
            //   
              var code = await _userManager.GeneratePasswordResetTokenAsync(2314);
             var result = await _userManager.ResetPasswordAsync(2314, code, "123456");

            return Ok(new
            {
                result = "done"
            });

            // var list = _ServiceLevelList.Select(s => new
            // {
            //     ServiceId = s.ServiceLevels.ServiceId,
            //     s.ServiceLevelId,
            //     s.ServicePropertiesId,
            //     UserServicesList = s.ServiceProperties.UserServices.Select(u => new
            //     {
            //         u.UserId
            //     }),
            // });

            // return Ok(new {result = "done", list});
            //var send = await Telegram.SendTelegramLogMessage("asdasd");
            //return Ok(new {result = "done", send});

            //id: 1087
            // var user = _ServiceProviderInfo.GetAllServiceProviderInfoById(1087).FirstOrDefault();
            //
            // user.WorkPhone = "09393013397";
            // user.HomePhone = "09393013397";
            // user.Mobile = "09393013397";
            // user.Email = "seyedali.farjad@gmail.com";
            // user.Resume += "test";
            // _uow.SaveAllChanges();
            //
            // return Ok(new {result = "done", user.UserName, user.ActiveCode});
        }


        
        public async Task<IHttpActionResult> Detail(int userId)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return Ok(new {result = "not found", user = userId});
                return Ok(new
                {
                    result = "done", user = new
                    {
                        name = user.Name,
                        family = user.Family,
                        sex = user.Sex,
                        id = user.Id,
                        email = user.Email,
                        mobile = user.Mobile,
                        credit = user.Credit,
                        addressJson = user.AddressJson,
                        path = user.Path,
                        picture = user.Picture,
                        nationalCode = user.NationalCode,
                        homePhone = user.HomePhone,
                        userType = user.UserType,
                        subscription = user.Subscription,
                    }
                });
            }
            else
            {
                return Ok(new {result = "error"});
            }
        }
        
        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> IsLogin()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = Convert.ToInt32(User.Identity.GetUserId() ?? "0");
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return Ok(new {result = "not found", user = userId});
                return Ok(new
                {
                    result = "done", user = new
                    {
                        name = user.Name,
                        family = user.Family,
                        sex = user.Sex,
                        id = user.Id,
                        email = user.Email,
                        mobile = user.Mobile,
                        credit = user.Credit,
                        addressJson = user.AddressJson,
                        path = user.Path,
                        picture = user.Picture,
                        nationalCode = user.NationalCode,
                        homePhone = user.HomePhone,
                        userType = user.UserType,
                        subscription = user.Subscription,
                    }
                });
            }
            else
            {
                return Ok(new {result = "error"});
            }
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> UpdateUser(JObject input)
        {
            if (!User.Identity.IsAuthenticated)
                return Ok(new {result = "error", message = "need login to update error!"});

            var userId = Convert.ToInt32(User.Identity.GetUserId() ?? "0");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Ok(new {result = "error", message = "کاربری با این شماره یافت نشد!"});
            }

            var name = input["name"]?.Value<string>();
            var family = input["family"]?.Value<string>();
            var sex = input["sex"]?.Value<bool>();
            var credit = input["credit"]?.Value<long>();
            var addressJson = input["addressJson"]?.Value<JArray>();

            if (name != null)
            {
                user.Name = name;
            }

            if (family != null)
            {
                user.Family = family;
            }

            if (sex != null)
            {
                user.Sex = (bool) sex;
            }

            if (credit != null)
            {
                //user.Credit = (long) credit;
            }

            if (addressJson != null)
            {
                user.AddressJson = addressJson.ToString(Formatting.None);
            }

            _uow.SaveAllChanges();

            return Ok(new
            {
                result = "done", user = new
                {
                    name = user.Name,
                    family = user.Family,
                    sex = user.Sex,
                    id = user.Id,
                    email = user.Email,
                    mobile = user.Mobile,
                    credit = user.Credit,
                    addressJson = user.AddressJson,
                }
            });
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> CompleteCustomerAccount(JObject input)
        {
            var username = input["username"]?.Value<string>();
            var password = input["password"]?.Value<string>();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Ok(new {result = "error", message = "شماره یا رمز ورود وارد نشده است!"});
            }

            var user = await _User.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return Ok(new {result = "error", message = "کاربری با این شماره یافت نشد!"});
            }

            var result = await _userManager.ChangePasswordAsync(user.Id, user.ActiveCode, password);

            user.Name = input["name"]?.Value<string>() ?? "";
            user.Family = input["family"]?.Value<string>() ?? "";
            user.Sex = input["sex"]?.Value<bool>() ?? false;
            user.Email = input["email"]?.Value<string>() ?? "";
            user.Credit = 0;
            user.AddressJson = "[]";
            user.ActiveCode = "!" + user.ActiveCode;

            _uow.SaveAllChanges();

            await Login(new LoginViewModel
            {
                UserName = username,
                Password = password,
                RememberMe = true,
            });

            return Ok(new
            {
                result = "done", user = new
                {
                    name = user.Name,
                    family = user.Family,
                    sex = user.Sex,
                    id = user.Id,
                    email = user.Email,
                    mobile = user.Mobile,
                    credit = user.Credit,
                    addressJson = user.AddressJson,
                },
                result.Errors
            });
        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> ActiveCodeCustomer(JObject input)
        {
            var username = input["username"]?.Value<string>();
            var activationCode = input["activationCode"]?.Value<string>();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(activationCode))
            {
                return Ok(new {result = "error", message = "شماره یا کد وارد نشده است!"});
            }

            var user = await _User.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return Ok(new {result = "error", message = "کاربری با این شماره یافت نشد!"});
            }

            if (user.ActiveCode != activationCode)
            {
                return Ok(new {result = "error", message = "کدفعال سازی اشتباه است!"});
            }

//            user.ActiveCode = "!" + user.ActiveCode;
//            _uow.SaveAllChanges();

            return Ok(new {result = "done"});
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.HttpPost]
        public async Task<IHttpActionResult> RegisterCustomer(JObject input)
        {
            var username = input["username"]?.Value<string>();
            var stateId = input["stateId"]?.Value<int>();
            var cityId = input["cityId"]?.Value<int>();
            var locationId = input["locationId"]?.Value<int>();
            if (string.IsNullOrEmpty(username))
            {
                return Ok(new {result = "error", message = "شماره اشتباه است!"});
            }

            var allUsers = await _userManager.GetAllUsers();
            var exitUser = allUsers.FirstOrDefault(u => u.UserName == username);
            if (exitUser != null)
            {
                if (!string.IsNullOrEmpty(exitUser.ActiveCode) && exitUser.ActiveCode[0] != '!')
                {
                    PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);

                    var text = "به جمع مشتریان ارجمند سی پارس خوش آمدید.";
                    text += "\n" + "کد فعال سازی حساب شما:" + "\n" + exitUser.ActiveCode;
                    Sms.SendSmsClass(exitUser.Mobile, text, exitUser.Id);


                    return Ok(new
                    {
                        result = "needActive",
                        userId = exitUser.Id
                    });
                }


                return Ok(new
                {
                    result = "error",
                    message =
                        "مشتری گرامی شماره همراه وارد شده در سیستم وجود دارد، اگر قبلا ثبت نموده اید لاگین کنید و اگر رمز عبور را فراموش کرده اید ، بازیابی رمز عبور انجام دهید."
                });
            }


            var pass = new Random().Next(100000, 999999) + "";
            var user = new ServiceReceiverInfo();

            user.UserName = username;

            user.Mobile = username;
            user.ActiveCode = pass;
            user.RegisterDate = new PersianDate().PersianDateLow();

            user.CityId = cityId ?? 2; //mashhad
            user.StateId = stateId ?? 1; //khoarasan razavi

            var result = await _userManager.CreateAsync(user, pass);
            if (result.Succeeded)
            {
                await _userManager.SetLockoutEnabledAsync(user.Id, false);
                await _userManager.AddToRoleAsync(user.Id, "User");
                DefineGroupUser("مشتری", user.Id);
                //------------ تخصیص کد به مشتری
                if (locationId != null)
                    CreateUserCode((int) locationId, user);

                PrivateTraining.ServiceLayer.BLL.SendSms Sms = new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);

                var text = "به جمع مشتریان ارجمند سی پارس خوش آمدید.";
                text += "\n" + "کد فعال سازی حساب شما:" + "\n" + pass;
                Sms.SendSmsClass(user.Mobile, text, user.Id);
            }
            else
            {
                return Ok(new
                {
                    result = "error",
                    message =
                        "خطا در ساخت حساب",
                    massageList = result.Errors
                });
            }


            return Ok(new
            {
                result = "needActive",
                userId = user.Id
            });
        }


        /// <summary>
        /// فعال کردن تعلیق کاربر
        /// </summary>
        private async Task activateSuspension()
        {
            try
            {
                var PD = new PersianDate();
                // PD.ConvertPersianNember();
                var y = PD.PersianDateLow();
                var List = _SuspensionUser.Where(c =>
                    c.ToSuspensionDate.CompareTo(y) >= 0 && c.FromSuspensionDate.CompareTo(y) <= 0).ToList();
                if (List.Count() > 0)
                {
                    foreach (var item in List)
                    {
                        var User = _User.Where(c => c.Id == item.UserId).FirstOrDefault();
                        //       User.Suspension =true;
                    }
                }

                await _uow.SaveAllChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// غیرفعال کردن تعلیق کاربر
        /// </summary>
        private async Task DeactivateSuspension()
        {
            try
            {
                var PD = new PersianDate();
                var y = PD.PersianDateLow();
                var List = _SuspensionUser.Where(c => c.ToSuspensionDate.CompareTo(y) < 0).ToList();
                if (List.Count() > 0)
                {
                    //foreach (var item in List)
                    //{
                    //    var User = _User.Where(c => c.Id == item.UserId).FirstOrDefault();
                    //    User.Suspension = false;
                    //}
                }

                await _uow.SaveAllChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }

        //
        // POST: /Account/Login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IHttpActionResult> Login(LoginViewModel model)
        {
            var Pd = new PersianDate();

            model.UserName = Pd.ConvertFaToEnNumber(model.UserName);
            model.Password = Pd.ConvertFaToEnNumber(model.Password);

//            if (!ModelState.IsValid)
//            {
//                return Ok(1);
//            }

            //غیرفعال کردن تعلیق در صورتی که از بازه تعلیق خارج شده باشد 
//            await activateSuspension();
//            await DeactivateSuspension();

            // This doesn't count login failures towards lockout only two factor authentication
            // To enable password failures to trigger lockout, change to shouldLockout: true
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true,
                shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:
                    var user = _User.FirstOrDefault(c => c.UserName == model.UserName);
                    return Ok(new
                    {
                        result = "done", user = new
                        {
                            name = user.Name,
                            family = user.Family,
                            sex = user.Sex,
                            id = user.Id,
                            email = user.Email,
                            mobile = user.Mobile,
                            
                            credit = user.Credit,
                            addressJson = user.AddressJson,
                        }
                    });

                case SignInStatus.LockedOut:
                    return Ok(new {message = "حساب بسته شده است", result = "lockedOut"});
                case SignInStatus.RequiresVerification:
                    return Ok(new {message = "تایید نیاز است", result = "requiresVerification"});
                default:
                    return Ok(new {message = "نام کاربری یا رمز ورود اشتباه است", result = "validation"});
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public virtual async Task<IHttpActionResult> LogOff()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            await _userManager.UpdateSecurityStampAsync(user.Id);

            return Ok(new {result = "done"});
        }

        private async Task signInAsync(ApplicationUser user, bool isPersistent)
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
            _authenticationManager.SignIn(new AuthenticationProperties {IsPersistent = isPersistent},
                await _userManager.GenerateUserIdentityAsync(user));
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public virtual async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var result = await _userManager.ChangePasswordAsync(_userManager.GetCurrentUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return Ok(new {result = "validation", message = "رمز عبور جاری اشتباه است"});
            }

            var user = await _userManager.GetCurrentUserAsync();
            if (user != null)
            {
                await signInAsync(user, isPersistent: false);
            }

            return Ok(new {result = "done"});
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public virtual async Task<IHttpActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            var PD = new PersianDate();
            model.UserName = PD.ConvertFaToEnNumber(model.UserName);
            model.Mobile = PD.ConvertFaToEnNumber(model.Mobile);

            var user = await _userManager.FindByUserNameAndMobile(model.UserName, model.Mobile);


            if (user == null)
            {
                return Ok(new {result = "validation", message = "اطلاعات وارد شده معتبر نمی باشد."});
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);


            var pass = new Random().Next(100000, 999999) + "";
            var result = await _userManager.ResetPasswordAsync(user.Id, code, pass);

            if (!string.IsNullOrEmpty(user.Mobile))
            {
                var Sms = new SendSms(_uow);
                code = Sms.SendSmsClass(user.Mobile, "«سیپارس»\nرمز عبور جدید شما در در سامانه :\n " + pass, user.Id);
            }

            return Ok(new {result = "done", err = result, code});
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public virtual async Task<IHttpActionResult> Register(RegisterViewModel model, string Type = "Enter")
        {
            // IdentityResult identityResult = _userManager.CreateAsync(user, model.Password).Result;
            var SW = await _userManager.CheckPersonnelId(model.PersonnelId);
            IdentityResult result;
            IdentityResult result2;

            if (SW)
            {
                // ثبت اولیه در صورت یک شماره پرسنلی وجود نداشت

                var user = new SupplementaryInfoUser();
                SetUserInfo(user, model);

                result = await _userManager.CreateAsync(user, model.Password);
                var result3 = await _userManager.SetLockoutEnabledAsync(user.Id, false);
                if (Type == "Excel" || model.RoleId == (int) Roles.User)
                    result2 = await _userManager.AddToRoleAsync(user.Id, "User");
                else
                {
                    if (model.RoleId == (int) Roles.Admin)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "Admin");
                    else if (model.RoleId == (int) Roles.Modrator)
                        result2 = await _userManager.AddToRoleAsync(user.Id, "Modrator");
                }
            }
            else
            {
                // ویرایش
                var user = FindUserId(model.PersonnelId);
                SetUserInfo(user, model);
                result = await _userManager.UpdateAsync(user);
            }

            if (result.Succeeded)
            {
                return Ok(new {result = "done"});
            }

            string Messages = "", PM = "";
            foreach (var item in result.Errors)
            {
                Messages += item;
            }

            if (Messages.IndexOf("Passwords must be at least 6 characters") != -1)
                PM += "کلمه عبور باید حداقل 6 حرف باشد. " + "<br/>";

            if (Messages.IndexOf("Passwords must have at least one lowercase ('a'-'z').") != -1 ||
                Messages.IndexOf("Passwords must have at least one digit ('0'-'9').") != -1
                || Messages.IndexOf("Passwords must have at least one uppercase ('A'-'Z').") != -1)
                PM += "کلمه عبور باید شامل حروف بزرگ و کوچک و ترکیب اعداد باشد." + "<br/>";

            if (Messages.IndexOf("Email") != -1)
                PM += "ایمیل وارد شده نامعتبر است. " + "<br/>";

            if (Messages.IndexOf("Name") != -1)
                PM += "شماره پرسنلی وارد شده نامعتبر است. " + "<br/>";

            // Email 'm_sadeghi200n@yahoo.com' is already taken.
            return Ok(new {result = "error", message = PM});
        }

        private SupplementaryInfoUser FindUserId(int PersonnelId)
        {
            var UserId = _SupplementaryInfoUser.Where(c => c.PersonnelId == PersonnelId).FirstOrDefault();
            return _SupplementaryInfoUser.Find(UserId.Id);
        }

        /// <summary>
        /// پر کردن فیلدهای جدول اطلاعات کاربر از مدل فرستاده شده
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private SupplementaryInfoUser SetUserInfo(SupplementaryInfoUser user, RegisterViewModel model)
        {
            var pd = new PersianDate();
            user.UserName = model.PersonnelId.ToString();
            user.Email = model.Email;
            user.NationalCode = model.NationalCode;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Family = model.Family;
            user.PersonnelId = model.PersonnelId;

            user.Sex = model.Sex;
            user.HomeAddress = model.Address;
            user.HomePhone = model.Phone;
            user.RegisterDate = pd.PersianDateLow();

            user.CertificateId = model.CertificateId;
            user.CertificateType = model.CertificateType;
            user.CertificationDate = model.CertificationDate;
            user.CertificateCredit = model.CertificateCredit;
            user.Status = model.Status;
            user.BusId = model.BusId;
            user.YearEmployment = model.YearEmployment;
            user.EducationComers = model.EducationComers;
            user.OtherCourses = model.OtherCourses;
            user.NumberChildren = model.NumberChildren;
            user.Degree = model.Degree;
            user.FieldOfStudy = model.FieldOfStudy;
            user.IssuedOnHealthCards = model.IssuedOnHealthCards;
            user.ValidityDuration = model.ValidityDuration;
            user.TheValidityPeriodOfTheYear = model.TheValidityPeriodOfTheYear;
            user.ExpirationDate = model.ExpirationDate;
            user.LockoutEnabled = false;
            user.Deleted = (byte) DeleteUserRecord.Show;
            //  user.Suspension = false;
            return user;
        }


        /// <summary>
        /// پر کردن فیلدهای جدول اطلاعات کاربر از مدل فرستاده شده
        /// </summary>
        /// <param name="user"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        private ServiceReceiverInfo SetUserInformation(ServiceReceiverInfo user, View_ApproveService model)
        {
            user.UserName = model.Mobile;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            user.Family = model.Family;
            user.Sex = model.Sex;
            user.HomeAddress = model.HomeAddress;
            user.CityId = model.CityId;
            user.StateId = model.StateId;
            user.LockoutEnabled = false;
            user.Deleted = (byte) DeleteUserRecord.Show;
            user.UserType = (byte) UserType.ServiceReceiver;
            user.YearBrithDay = "0";
            user.MonthBrithDay = "0";
            user.DayBrithDay = "0";
            return user;
        }

        /// <summary>
        /// ایجاد کد مشتری
        /// </summary>
        /// <param name="LocationId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private async void CreateUserCode(int LocationId, ServiceReceiverInfo user)
        {
            //----------- کد محل
            var tempLocationCode = await _RepLocation.GetLocation(LocationId);
            var AllServiceProvider = await _serviceReceiver.GetAllServiceReceiverInfo();
            //-------------   آخرین شماره مشتری ثبت شده در این  محل
            var maxusercodeServiceReceiver = AllServiceProvider.ToList()
                .Where(c => c.LocationCode == tempLocationCode.LocationCode).Max(c => c.UserCode);

            if (maxusercodeServiceReceiver != null)
            {
                //--------------- ایجاد کد جدید  ---  
                var TempUserCodePlus1 = string.Format("{0:00000}", Convert.ToInt32(maxusercodeServiceReceiver) + 1);
                user.UserCode = TempUserCodePlus1;
                user.ServiceReceiverCode = tempLocationCode.LocationCode + TempUserCodePlus1;
            }
            else
            {
                //---------------  دفعه اول
                user.UserCode = "00001";
                user.ServiceReceiverCode = tempLocationCode.LocationCode + user.UserCode;
            }

            user.LocationCode = tempLocationCode.LocationCode;
        }

        public async void DefineGroupUser(string Name = "", int UserId = 0)
        {
            var GroupId = _group.GetAllIGroupPolicySearchName(Name);
            if (GroupId != 0)
            {
                PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser GroupPolicyUser =
                    new PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser();
                GroupPolicyUser.UserId = UserId;
                GroupPolicyUser.GroupPolicyId = GroupId;
                await _group.AddGroupPolicyUser(GroupPolicyUser);
                _uow.SaveAllChanges();
            }
        }

        public async Task<bool> SaveServiceForServiceReciever(ServiceReceiverServiceLocation service)
        {
            try
            {
                //service.WhoChangeStatus = 1;
                await _servicereceiveservicelocation.AddServiceReceiverServiceLocation(service);
                int Status = await _uow.SaveAllChangesAsync();
                if (Status == 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// ارسال ایمیل و پیامک به خدمت دهنده ها
        /// </summary>
        /// <param name="ServiceId"></param>
        /// <param name="service"></param>
        /// <param name="approve"></param>
        public async void SendSmsAndEmail(int ServiceId, ServiceReceiverServiceLocation service,
            View_ApproveService approve, decimal ServiceReceiverServiceLocationId,
            int ServiceLevelListId = 0)
        {
            try
            {
                // var Price = _ServiceWorkUnit.Where(c => c.ServicePropertiesId == ServiceId).FirstOrDefault().PriceWorkUnit;
                // var p = service.WorkUnits.ServiceLocationWorkUnits.FirstOrDefault();

                string Domain = HttpContext.Current.Request.Url.Host;
                string Title = " خدمات آنلاین ";
                var Query = _userManager.FindById(service.ServiceProviderId);
                if (Query != null)
                {
                    //if (Query.Email != null && Query.Email != "")
                    //{
                    //    IdentityMessage Message = new IdentityMessage();
                    //    PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
                    //    string Body = "<div style='Direction:rtl;'>" +
                    //"<br /><div>کاربر گرامی  " + Query.Name + " " + Query.Family + " عزیز درخواست جدید برای شما در سایت " + Title
                    //+ " ثبت شده است. </div>";
                    //    Body += " آدرس : " + approve.HomeAddress + " - کد خدمت : " + ServiceId + " - نام خدمت : " + approve.ServiceName + " - مبلغ : " + Price + " تومان " + " - کد درخواست : " + ServiceReceiverServiceLocationId;
                    //    Body += "<br /><div><a href='http://" + Domain + "' target='_blank'>" + Domain + "</a></div>" + "</div>";
                    //    Message.Body = Body;
                    //    Message.Subject = " درخواست جدید در سایت " + Domain;
                    //    Message.Destination = Query.Email;
                    //    await email.SendAsync(Message);
                    //}
                    if (Query.Mobile != null && Query.Mobile != "")
                    {
                        PrivateTraining.ServiceLayer.BLL.PrivateTraining PT =
                            new PrivateTraining.ServiceLayer.BLL.PrivateTraining(_uow, _userManager);
                        PrivateTraining.ServiceLayer.BLL.SendSms Sms =
                            new PrivateTraining.ServiceLayer.BLL.SendSms(_uow);
                        ////string Text = "کاربر گرامی " + Query.Name + " " + Query.Family + " عزیز درخواست جدید برای شما در سایت " + Title 
                        ////  + "ثبت شده است." + "\n" +
                        //// - کد خدمت : " + ServiceId + "
                        //string Text = "درخواست جدید از سایت خدمات آنلاین " + "\n" +
                        //" آدرس : " + approve.HomeAddress + "  - نام خدمت : " + approve.ServiceName + " - مبلغ : " + Price + " تومان " + " - کد درخواست : " + ServiceReceiverServiceLocationId + "." +
                        //" جهت موافقت با درخواست عدد 2 و عدم موافقت عدد 12 را با فرمت زیر ارسال نمایید. " + "." +
                        //" کد درخواست * عدد - مثال" + "2*147" +
                        //" - یا به پنل خود در سایت مراجعه نمایید  ";
                        ////  "\n" + " http://" + Domain;

                        string Text = PT.SendSmsForProviderNewRequest(ServiceReceiverServiceLocationId, service, Domain,
                            ServiceLevelListId);
                        //   Sms.SendSmsClass(Query.Mobile, Text, Convert.ToInt32(User.Identity.GetUserId()));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// ذخیره اطلاعات مشتری
        /// </summary>
        /// <param name="approve"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        [System.Web.Http.AllowAnonymous]
        public virtual async Task<IHttpActionResult> CustomerRegister(View_ApproveService approve)
        {
            var Username = "";
            var PD = new PersianDate();
            var ServiceReciverId = 0;
            var rand = new Random();
            var pass = rand.Next(100000, 999999);
            var CountMember = await _userManager.GetAllUsers();
            var sw = false;
            var Sms = new SendSms(_uow);
            approve.Mobile = PD.ConvertFaToEnNumber(approve.Mobile);

            var UserInfo = CountMember.OfType<ServiceReceiverInfo>()
                .FirstOrDefault(c => c.UserName == approve.Mobile);


            IdentityResult result;
            IdentityResult result2;

            if (UserInfo == null)
            {
                var user = new ServiceReceiverInfo();
                SetUserInformation(user, approve);
                //------------ تخصیص کد به مشتری
                CreateUserCode(approve.LocationId, user);

                //   user.UserName = user.ServiceReceiverCode;
                user.UserName = user.Mobile;
                Username = user.UserName;
                result = await _userManager.CreateAsync(user, Convert.ToString(pass));
                if (result.Succeeded)
                {
                    sw = true;
                    var result3 = await _userManager.SetLockoutEnabledAsync(user.Id, false);
                    result2 = await _userManager.AddToRoleAsync(user.Id, "User");

                    DefineGroupUser("مشتری", user.Id);
                }

                ServiceReciverId = user.Id;
            }
            else
            {
                return Ok(new
                {
                    result = "duplicate",
                    message =
                        "مشتری گرامی شماره همراه وارد شده در سیستم وجود دارد ، اگر قبلا ثبت نموده اید لاگین کنید و اگر رمز عبور را فراموش کرده اید ، بازیابی رمز عبور انجام دهید."
                });
            }


            if (ServiceReciverId != 0)
            {
                var ServiceLocation = await _servicelocation.GetAllServiceLocation();
                foreach (var item in approve.SelectServiceProviderForServices)
                {
                    var ServiceLocationId = ServiceLocation
                        .FirstOrDefault(c => c.ServiceId == item.ServiceId && c.LocationId == approve.LocationId).Id;
                    var service = new ServiceReceiverServiceLocation();
                    service.ServiceLocationId = ServiceLocationId;
                    service.ServiceProviderId = item.ServiceProviderId;
                    service.ServiceReceiverId = ServiceReciverId;
                    service.DateRegister = PD.PersianDateLow();
                    service.WorkUnitId = item.WorkUnitId;
                    //service.WhoChangeStatus = UserInfo.Id;////
                    bool Status = await SaveServiceForServiceReciever(service);

                    if (!Status)
                        return Ok(new {result = "error", message = "مشکلی در ثبت اطلاعات به وجود آمده است"});
                    //----------------------------- ارسال ایمیل و پیامک ثبت نام مشتری

                    try
                    {
                        string Domain = HttpContext.Current.Request.Url.Host;
                        var Title = " خدمات آنلاین ";
                        //if (user.Email != null && user.Email != "")
                        //{
                        //    PrivateTraining.ServiceLayer.EmailService email = new PrivateTraining.ServiceLayer.EmailService(_uow);
                        //    email.SendEmailRegister(Domain, user.Name, user.Family, user.Mobile, pass.ToString(), user.Email, Title);

                        //}
                        if (!string.IsNullOrEmpty(approve.Mobile))
                        {
                            Sms.SensSmsRegisterReciver(Domain, approve.Name, approve.Family,
                                approve.Mobile, pass.ToString(), approve.Mobile,
                                Convert.ToInt32(User.Identity.GetUserId()), Title, service.Id,
                                approve.Sex, sw ? "new" : "");
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    //-----------------------  ارسال ایمیل و پیامک به خدمت دهنده ها
                    SendSmsAndEmail(item.ServiceId, service, approve, service.Id, item.ServiceLevelListId);
                }
            }
            else
                return Ok(new
                    {result = "error", message = "مشکلی در ثبت و یا دریافت نام کاربری به وجود آمده است"});


            return Ok(new
            {
                result = "done",
                password = pass,
                userName = Username /*approve.Mobile*/
            });
        }


        //end controller
    }
}