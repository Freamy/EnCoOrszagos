using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using EnCoOrszag.Models.DataAccess.Entities;

namespace EnCoOrszag.Models.DataAccess
{
    public class ApplicationUser : IdentityUser
    {

        public virtual Country Country { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here -- Original comment
            return userIdentity;
        }
    }
}