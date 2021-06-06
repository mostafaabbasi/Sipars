using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface;
using PrivateTraining.ServiceLayer.Extention;
using PrivateTraining.DomainClasses.Entities.Security;
using PrivateTraining.ServiceLayer.BLL;

namespace PrivateTraining.ServiceLayer.Repository.IdentityRepository
{
    public class ApplicationUserManagerRepository
        : UserManager<ApplicationUser, int>, IApplicationUserManager
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IUserStore<ApplicationUser, int> _store;
        private readonly IUnitOfWork _uow;
        private readonly IDbSet<ApplicationUser> _users;
        private readonly IIdentity _identity;
        private ApplicationUser _user;
        byte NotDel = (byte)DeleteUserRecord.Show;

        public ApplicationUserManagerRepository(IUserStore<ApplicationUser, int> store,
            IUnitOfWork uow,
            IIdentity identity,
            IApplicationRoleManager roleManager,
            IDataProtectionProvider dataProtectionProvider,
            IIdentityMessageService smsService,
            IIdentityMessageService emailService)
            : base(store)
        {
            _store = store;
            _uow = uow;
            _identity = identity;
            _users = _uow.Set<ApplicationUser>();
            _roleManager = roleManager;
            _dataProtectionProvider = dataProtectionProvider;
            this.SmsService = smsService;
            this.EmailService = emailService;

            createApplicationUserManager();
        }


        public ApplicationUser FindById(int userId)
        {
            return _users.Find(userId);
        }

        public ApplicationUser FindByUserName(string UserName)
        {
            return _users.Where(c => c.UserName == UserName).FirstOrDefault();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUser applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return this.Users.ToListAsync();
        }

        public ApplicationUser GetCurrentUser()
        {
            return _user ?? (_user = this.FindById(GetCurrentUserId()));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _user ?? (_user = await this.FindByIdAsync(GetCurrentUserId()));
        }

        public int GetCurrentUserId()
        {
            return _identity.GetUserId<int>();
        }

        public async Task<bool> HasPassword(int userId)
        {
            var user = await FindByIdAsync(userId);
            return user != null && user.PasswordHash != null;
        }

        public async Task<bool> HasPhoneNumber(int userId)
        {
            var user = await FindByIdAsync(userId);
            return user != null && user.PhoneNumber != null;
        }

        public Func<CookieValidateIdentityContext, Task> OnValidateIdentity()
        {
            return SecurityStampValidator.OnValidateIdentity<ApplicationUserManagerRepository, ApplicationUser, int>(
                         validateInterval: TimeSpan.FromSeconds(0),
                         regenerateIdentityCallback: (manager, user) => generateUserIdentityAsync(manager, user),
                         getUserIdCallback: id => id.GetUserId<int>());
        }

        public void SeedDatabase()
        {

            int CountAdmin = GetCountAdmins();

            if (CountAdmin == 0)
            {
                const string Email = "iman.sadeghi2009@gmail.com";
                const string password = "Admin@123456";
                //  const string NationalCode = "0";
                PersianDate pd = new PersianDate();
                const string UserName = "admin";
                
                const int PersonnelId = 0;
                const string Name = "iman";
                const string Family = "sadeghi";
                const string Mobile = "09123456789";
                const int State = 1;
                const int City = 1;
                
                const bool Sex = false;
                const string YearBrithDay = "0";
                const string MonthBrithDay = "0";
                const string DayBrithDay = "0";
                
                const bool Status = true;
                string RegisterDate = pd.PersianDateLow();

                
                const int StateId = 1;
                const int CityId = 1;

                byte Usertype = Convert.ToByte(UserType.NotUser);


                List<string> Roles = new List<string>();
                Roles.Add("Administrator");
                Roles.Add("Admin");
                Roles.Add("Modrator");
                Roles.Add("User");

                foreach (var item in Roles)
                {
                    var role = _roleManager.FindRoleByName(item);
                    if (role == null)
                    {
                        role = new CustomRole(item);
                        var roleresult = _roleManager.CreateRole(role);
                    }
                }
                //Create Role Admin if it does not exist


                var user = this.FindByUserName(UserName);
                if (user == null)
                {
                    try { 
                    user = new ApplicationUser { UserName = UserName, EmailConfirmed = true, PersonnelId = PersonnelId, Name = Name, Family = Family, Sex = Sex, UserType = Usertype, Mobile = Mobile, StateId = StateId, CityId = CityId, Status = Status, RegisterDate = RegisterDate ,
                        YearBrithDay = YearBrithDay, MonthBrithDay= MonthBrithDay, DayBrithDay= DayBrithDay
                    ,
                    };
                    var result = this.Create(user, password);
                    result = this.SetLockoutEnabled(user.Id, false);
                    }
                    catch(Exception ex)
                    {
                    }
                }

                // Add user admin to Role Admin if not already added
                var rolesForUser = this.GetRoles(user.Id);
                if (!rolesForUser.Contains("Admin"))
                {
                    var result = this.AddToRole(user.Id, "Admin");
                }
            }

        }

        private void createApplicationUserManager()
        {
            // Configure validation logic for usernames
            this.UserValidator = new UserValidator<ApplicationUser, int>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,

            };

            // Configure user lockout defaults
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            this.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser, int>
            {
                MessageFormat = "Your security code is: {0}"
            });
            this.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser, int>
            {
                Subject = "SecurityCode",
                BodyFormat = "Your security code is {0}"
            });

            if (_dataProtectionProvider != null)
            {
                var dataProtector = _dataProtectionProvider.Create("ASP.NET Identity");
                this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtector);
            }
        }

        private async Task<ClaimsIdentity> generateUserIdentityAsync(ApplicationUserManagerRepository manager, ApplicationUser applicationUser)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(applicationUser, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<bool> CheckPersonnelId(int PersonnelId)
        {
            var SW = false;
            var user = await _users.Where(c => c.PersonnelId == PersonnelId).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;

        }

        public async Task<bool> CheckMobileNumber(string Mobile)
        {
            var SW = false;
            var user = await _users.Where(c => c.Mobile == Mobile).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;

        }

        public async Task<bool> CheckNationalCode(string NationalCode, int Id)
        {
            var SW = false;
            var user = await _users.Where(c => c.NationalCode == NationalCode && c.Id != Id && c.Deleted == NotDel).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;
        }

        public async Task<bool> CheckEmails(string Email, int Id)
        {
            var SW = false;
            var user = await _users.Where(c => c.Email == Email && c.Id != Id && c.Deleted == NotDel).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;
        }

        public async Task<bool> CheckUserNames(string UserName, int Id)
        {
            var SW = false;
            var user = await _users.Where(c => (c.UserName == UserName || c.Email == UserName) && c.Id != Id && c.Deleted == NotDel).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;
        }

        public async Task<bool> CheckUserNameAndEmails(string UserName, int Id)
        {
            var SW = false;
            var user = await _users.Where(c => (c.UserName == UserName || c.Email == UserName) && c.Id != Id && c.Deleted == NotDel).FirstOrDefaultAsync();
            if (user == null)
            {
                SW = true;
            }
            return SW;
        }


        public async Task<int> GetUserIdWithersonnelId(int PersonnelId)
        {
            var user = await _users.Where(c => c.PersonnelId == PersonnelId).FirstOrDefaultAsync();
            if (user != null)
                return user.Id;
            else
                return 0;

        }

        public async Task<IQueryable<ApplicationUser>> GetAllUsers()
        {
            var y = _users;
            //return _users.Where(x => x.Roles.Select(y => y.RoleId).Contains(4));
            return _users.Where(x => x.Roles.Any(a => a.RoleId == (int)Roles.User || a.RoleId == (int)Roles.ServiceProvider ) && x.Deleted == (byte)DeleteUserRecord.Show).OrderByDescending(c => c.Id);
        }

        public async Task<IQueryable<ApplicationUser>> GetAllModarators()
        {
            //return _users.Where(x => x.Roles.Select(y => y.RoleId).Contains(4));
            return _users.Where(x => x.Roles.Any(a => a.RoleId == (int)Roles.Modrator) && x.Deleted == (byte)DeleteUserRecord.Show);
        }

        public async Task<IQueryable<ApplicationUser>> GetAllAdmins()
        {
            return _users.Where(x => (x.Roles.Any(a => a.RoleId == (int)Roles.Admin) || x.Roles.Any(a => a.RoleId == (int)Roles.Administrator)) && x.Deleted == (byte)DeleteUserRecord.Show);
        }

        public IQueryable<ApplicationUser> GetAllAdmin()
        {
            return _users.Where(x => (x.Roles.Any(a => a.RoleId == (int)Roles.Admin) || x.Roles.Any(a => a.RoleId == (int)Roles.Administrator)) && x.Deleted == (byte)DeleteUserRecord.Show);
        }

        public int GetCountAdmins()
        {
            return _users.Where(x => (x.Roles.Any(a => a.RoleId == (int)Roles.Admin) || x.Roles.Any(a => a.RoleId == (int)Roles.Administrator)) && x.Deleted == (byte)DeleteUserRecord.Show).Count();
        }

        public async Task<IQueryable<ApplicationUser>> GetAlTypelUsers()
        {
            //return _users.Where(x => x.Roles.Select(y => y.RoleId).Contains(4));
            return _users.Where(x => x.Deleted == (byte)DeleteUserRecord.Show).OrderByDescending(c => c.Id);
        }

        public IQueryable<ApplicationUser> GetSelectedUsers(string[] UserId)
        {
            return _users.Where(x => x.Deleted == (byte)DeleteUserRecord.Show && UserId.Contains(x.Id.ToString())).OrderByDescending(c => c.Id);
        }

        public async Task<IdentityResult> InactiveUser(int UserId)
        {
            ApplicationUser User = FindById(UserId);

            if (User.Status)
                User.Status = false;
            else
                User.Status = true;

           // if (User.LockoutEnabled)
            if (User.Status)
            {
                User.LockoutEnabled = false;
                User.LockoutEndDateUtc = null;
            }
            else
            {
                User.LockoutEnabled = true;
                User.LockoutEndDateUtc = DateTime.Now;
            }

            var Result = await this.UpdateAsync(User);
            return Result;
        }

        public async Task<IdentityResult> DeleteUser(int UserId)
        {
            ApplicationUser User = FindById(UserId);
            User.Deleted = (byte)DeleteUserRecord.Delete;
            User.LockoutEnabled = true;
            User.LockoutEndDateUtc = DateTime.Now;
            var Result = await this.UpdateAsync(User);
            return Result;

            //_users.Remove(User);
            //return await _uow.SaveAllChangesAsync();
        }


        //public async Task<IdentityResult> SuspensionUser(int UserId)
        //{
        //    ApplicationUser User = FindById(UserId);

        //    if (User.Suspension)
        //        User.Suspension = false;
        //    else
        //        User.Suspension = true;

        //    var Result = await this.UpdateAsync(User);
        //    return Result;
        //}


        //public async Task<bool> checkSuspensionUser(int UserId)
        //{
        //   // return _users.Where(x => x.Id == UserId).FirstOrDefault().Suspension;
        //}

        public async Task<ApplicationUser> FindByUserNameAndEmail(string UserName, string Email)
        {
            return await _users.Where(c => c.UserName == UserName && c.Email == Email).FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser> FindByUserNameAndMobile(string UserName, string Mobile)
        {
            return await _users.Where(c => c.UserName == UserName && c.Mobile == Mobile).FirstOrDefaultAsync();
        }

        public string GetUserName(int UserId)
        {
            return _users.Where(c => c.Id == UserId).FirstOrDefault().UserName;
        }

        public Task<IQueryable<ApplicationUser>> GetAlTypelUsers(int Id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<ApplicationUser> GetAllUsersWithId(int UserId)
        {
            //  if (UserId != 0)
            return _users.Include(u => u.Roles).Where(x => x.Id == UserId);
            //   else
            //     return _users;
        }

        public async Task<IQueryable<ApplicationUser>> GetAlTypelUsersWithOutAdmin()
        {
            return _users.Where(x => (x.Roles.Any(a => a.RoleId != (int)Roles.Admin) || x.Roles.Any(a => a.RoleId != (int)Roles.Administrator)) && x.Deleted == (byte)DeleteUserRecord.Show);

        }

        public IQueryable<ApplicationUser> GetAlTypelUsersWithOutAdmins()
        {
            return _users.Where(x => (x.Roles.Any(a => a.RoleId != (int)Roles.Admin) || x.Roles.Any(a => a.RoleId != (int)Roles.Administrator)) && x.Deleted == (byte)DeleteUserRecord.Show);
        }

        public int GetCountUserNewRegister()
        {
            return _users.Where(x => x.Status == false && x.Deleted == (byte)DeleteUserRecord.Show).Count();
        }

        public bool ActiveUser(string ActiveCode)
        {
            var User = _users.Where(c => c.ActiveCode == ActiveCode).FirstOrDefault();

            if (User != null)
            {
                User.Status = true;
                User.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
                _uow.SaveAllChanges();
                return true;
            }
            else
                return false;

        }

        public async Task<ApplicationUser> FindByEmailAndNationalCode(string Email, string NationalCode)
        {
            return await _users.Where(c => (c.UserName == Email || c.Email == Email) && c.NationalCode == NationalCode).FirstOrDefaultAsync();
        }
    }
}

