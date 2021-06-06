using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using PrivateTraining.DomainClasses.Entities;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.DomainClasses.Entities.BusDriving;
using PrivateTraining.DomainClasses.Entities.PrivateTraining;
using PrivateTraining.DomainClasses.Entities.FrameWork;
using PrivateTraining.DomainClasses.Entities.Climbings;
using PrivateTraining.DomainClasses.Entities.BaseTable;
using PrivateTraining.DomainClasses.Entities.Golbahar;
using PrivateTraining.DomainClasses.Entities.Log;
using PrivateTraining.DomainClasses.Entities.Payment;

namespace PrivateTraining.DataLayer.Context
{
    public class ApplicationDbContext :
        IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        IUnitOfWork
    {
        #region DBSet

        /// <summary>
        /// معرفي جداول به DBSET
        /// </summary>

        #region Framework

        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.Action> Actions { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.Menu> Menus { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.Setting> Settings { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.SaveEmail> SaveEmails { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.Freind> Freinds { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.PaymentOrder> PaymentOrders { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.PaymentOrderDetail> PaymentOrderDetails
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.Message> Messages { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.SMSReceived> SMSsReceived { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.SMSSended> SMSsSended { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.FrameWork.SettingSms> SettingSms { set; get; }

        #endregion

        #region Security

        public DbSet<PrivateTraining.DomainClasses.Entities.Security.GroupPolicy> GroupPolicies { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Security.GroupPolicyUser> GroupPolicyUsers { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Security.HistoryLogin> HistoryLogins { set; get; }

        #endregion

        #region BaseTable

        public DbSet<PrivateTraining.DomainClasses.Entities.BaseTable.City> Cities { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BaseTable.State> States { set; get; }

        #endregion

        #region BusDriving

        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.Line> Lines { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.Shift> Shifts { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.MaximumLeave> MaximumLeaves { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.MaximumLeaveLine> MaximumLeaveLines { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.InvalidDay> InvalidDays { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.LeaveRequest> LeaveRequests { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.PeriodLeave> PeriodLeaves { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.Salary> Salaries { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.Year> Years { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.ReferenceLeave> ReferenceLeaves { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.SubjectRequest> SubjectRequests { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.BusDriving.UserRequest> UserRequests { set; get; }

        #endregion

        #region  PrivateTraining

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.Service> Services { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceProperties> ServicesProperties
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceWorkUnit> serviceWorkUnits
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.UserService> UserServices { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.WorkUnit> WorkUnits { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.Location> Locations { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.View_ServiceLocations> ServiceLocations
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceLocationWorkUnit>
            ServiceLocationWorkUnits { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceReceiverRequest>
            ServiceReceiverRequests { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.Debt> Debts { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.DebtServiceProvider> DebtServiceProviders
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.DebtServiceReceiverServiceLocation>
            DebtServiceReceiverServiceLocations { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.payment> Payments { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.paymentDetail> PaymentDetails { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.UserFile> UserFiles { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.Form> Form { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.FormAnswer> FormAnswers { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.FormQuestion> FormQuestions { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.CommentPrivate> CommentPrivates
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.PrivateSetting> PrivateSettings
        {
            set;
            get;
        }

        public DbSet<PrivateTraining.DomainClasses.Entities.PrivateTraining.ServiceLevel> ServiceLevels { set; get; }

        #endregion PrivateTraining

        #region Climbing

        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.Equipment> Equipments { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.Club> Clubs { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.ClubManager> ClubManagers { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.ClimbingFile> ClimbingFiles { set; get; }

        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.Program> Programs { set; get; }

        // public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.ProgramRegister> ProgramRegisters { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.FinePercent> FinePercents { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.Comment> CommentsClimbing { set; get; }
        public DbSet<PrivateTraining.DomainClasses.Entities.Climbings.ProgramModrator> ProgramModrators { set; get; }

        #endregion

        #region Golbahar

        public DbSet<CompanyType> CompanyTypes { set; get; }
        public DbSet<DelayFinePercent> DelayFinePercents { set; get; }
        public DbSet<Company> Companies { set; get; }
        public DbSet<Project> Projects { set; get; }
        public DbSet<Bloc> Blocs { set; get; }
        public DbSet<Unit> Units { set; get; }
        public DbSet<PercentagIncreaseAnnually> PercentagIncreaseAnnuallys { set; get; }
        public DbSet<CalRent> CalRents { set; get; }
        public DbSet<CalRentLog> CalRentLogs { set; get; }
        public DbSet<SettingGolbahar> SettingGolbahars { set; get; }
        public DbSet<AccountPercent> AccountPercents { set; get; }
        public DbSet<ReceiptHistory> ReceiptHistorys { set; get; }
        public DbSet<ReceiptHistoryYear> ReceiptHistoryYears { set; get; }

        #endregion

        public DbSet<Api> Api { get; set; }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<BuyServiceCostTime> BuyServiceCostTime { get; set; }
        public DbSet<BuyService> BuyService { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<BankPayment> BankPayment { get; set; }

        #endregion

        /// <summary>
        /// It looks for a connection string named connectionString1 in the web.config file.
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            //this.Database.Log = data => System.Diagnostics.Debug.WriteLine(data);
        }

        #region Configration

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("Users", "Security");
            builder.Entity<CustomRole>().ToTable("Roles", "Security");
            builder.Entity<CustomUserClaim>().ToTable("UserClaims", "Security");
            builder.Entity<CustomUserRole>().ToTable("UserRoles", "Security");
            builder.Entity<CustomUserLogin>().ToTable("UserLogins", "Security");

            #region Bus

            builder.Entity<ReferenceLeave>().HasRequired(c => c.SenderUser).WithMany().WillCascadeOnDelete(false);
            builder.Entity<LeaveRequest>().HasRequired(c => c.UserRequests).WithMany().WillCascadeOnDelete(false);
            builder.Entity<ReferenceLeave>().HasRequired(c => c.LeaveRequests).WithMany().WillCascadeOnDelete(false);
            builder.Entity<UserRequest>().HasRequired(c => c.Users).WithMany().WillCascadeOnDelete(false);

            #endregion

            #region PrivateTraining

            builder.Entity<UserServiceLocation>().HasRequired(c => c.Locations).WithMany().WillCascadeOnDelete(false);
            builder.Entity<UserServiceLocation>().HasRequired(c => c.Services).WithMany().WillCascadeOnDelete(false);
            builder.Entity<UserServiceLocation>().HasRequired(c => c.Users).WithMany().WillCascadeOnDelete(false);
            builder.Entity<View_ServiceLocations>().HasRequired(c => c.Cities).WithMany().WillCascadeOnDelete(false);
            builder.Entity<City>().HasRequired(c => c.States).WithMany().WillCascadeOnDelete(false);
            builder.Entity<ApplicationUser>().HasRequired(c => c.Cities).WithMany().WillCascadeOnDelete(false);
            builder.Entity<ServiceReceiverServiceLocation>().HasRequired(c => c.ServiceLocations).WithMany()
                .WillCascadeOnDelete(false);
            builder.Entity<Location>().HasRequired(c => c.Cities).WithMany().WillCascadeOnDelete(false);
            builder.Entity<ServiceReceiverServiceLocation>().HasRequired(c => c.ApplicationProviderUsers).WithMany()
                .WillCascadeOnDelete(false);
            //builder.Entity<ServiceReceiverServiceLocation>().HasRequired(c => c.ChangeStatusUsers).WithMany().WillCascadeOnDelete(false);
            builder.Entity<paymentDetail>().HasRequired(c => c.Modrators).WithMany().WillCascadeOnDelete(false);
            builder.Entity<paymentDetail>().HasRequired(c => c.Members).WithMany().WillCascadeOnDelete(false);
            builder.Entity<payment>().HasRequired(c => c.Modrators).WithMany().WillCascadeOnDelete(false);
            builder.Entity<paymentDetail>().HasRequired(c => c.Payments).WithMany().WillCascadeOnDelete(false);
            builder.Entity<FormAnswer>().HasRequired(c => c.ApplicationProviderUsers).WithMany()
                .WillCascadeOnDelete(false);
            builder.Entity<DomainClasses.Entities.PrivateTraining.CommentPrivate>().HasRequired(c => c.ReciverUsers)
                .WithMany().WillCascadeOnDelete(false);
            builder.Entity<ServiceReceiverServiceLocation>().Property(x => x.Id).HasPrecision(18, 0);

            #endregion

            #region Climing

            builder.Entity<ClubManager>().HasRequired(c => c.UserClub).WithMany().WillCascadeOnDelete(false);
            builder.Entity<Program>().HasRequired(c => c.Users).WithMany().WillCascadeOnDelete(false);
            builder.Entity<Program>().HasRequired(c => c.Clubs).WithMany().WillCascadeOnDelete(false);

            #endregion

            #region Framwork

            builder.Entity<Message>().HasRequired(c => c.ReciverUsers).WithMany().WillCascadeOnDelete(false);
            builder.Entity<Freind>().HasRequired(c => c.AcceptorUsers).WithMany().WillCascadeOnDelete(false);
            builder.Entity<PaymentOrder>().Property(c => c.AllPrice).HasPrecision(18, 0);
            builder.Entity<PaymentOrderDetail>().Property(c => c.price).HasPrecision(18, 0);
            builder.Entity<PaymentOrderDetail>().Property(c => c.Fine).HasPrecision(18, 0);

            #endregion

            #region Log

            builder.Entity<Api>().ToTable("Api", "Log");

            #endregion
            
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public int SaveAllChanges()
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveAllChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        public IEnumerable<TEntity> AddThisRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            return ((DbSet<TEntity>) this.Set<TEntity>()).AddRange(entities);
        }

        public void MarkAsChanged<TEntity>(TEntity entity) where TEntity : class
        {
            Entry(entity).State = EntityState.Modified;
        }

        public IList<T> GetRows<T>(string sql, params object[] parameters) where T : class
        {
            return Database.SqlQuery<T>(sql, parameters).ToList();
        }

        public IList<T> GetRowsWithoutParam<T>(string sql) where T : class
        {
            return Database.SqlQuery<T>(sql).ToList();
        }

        public void ForceDatabaseInitialize()
        {
            //this.Database.Initialize(force: true);
        }

        #endregion
    }
}