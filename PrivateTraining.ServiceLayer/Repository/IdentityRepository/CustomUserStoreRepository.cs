using Microsoft.AspNet.Identity.EntityFramework;
using PrivateTraining.DataLayer.Context;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.Repository.IdentityRepository
{
    public class CustomUserStoreRepository :
        UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>,
        ICustomUserStore
    {
        //private readonly IDbSet<ApplicationUser> _myUserStore;
        public CustomUserStoreRepository(ApplicationDbContext context)
            : base(context)
        {
            //_myUserStore = context.Set<ApplicationUser>();
        }

        //public override Task<ApplicationUser> FindByIdAsync(int userId)
        //{
        //   return Task.FromResult(_myUserStore.Find(userId));
        //}
    }
}
