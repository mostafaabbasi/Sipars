using Microsoft.AspNet.Identity.Owin;
using PrivateTraining.DomainClasses.Security;
using PrivateTraining.ServiceLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace PrivateTraining.ServiceLayer.Repository.IdentityRepository
{
    public class ApplicationSignInManagerRepository :
        SignInManager<ApplicationUser, int>, IApplicationSignInManager
    {
        private readonly ApplicationUserManagerRepository _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public ApplicationSignInManagerRepository(ApplicationUserManagerRepository userManager,
                                        IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        {
            _userManager = userManager;
            _authenticationManager = authenticationManager;
        }
    }
}
