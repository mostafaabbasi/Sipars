﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PrivateTraining.DomainClasses.Security
{
    public class CustomUserRole : IdentityUserRole<int>
    {

    }
}
