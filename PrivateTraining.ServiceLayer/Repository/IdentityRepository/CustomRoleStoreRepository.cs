using Microsoft.AspNet.Identity;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface;

namespace PrivateTraining.ServiceLayer.Repository.IdentityRepository
{
    public class CustomRoleStoreRepository : ICustomRoleStore
    {
        private readonly IRoleStore<CustomRole, int> _roleStore;

        public CustomRoleStoreRepository(IRoleStore<CustomRole, int> roleStore)
        {
            _roleStore = roleStore;
        }
    }
}
