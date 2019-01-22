using Microsoft.AspNetCore.Blazor.Browser.Services;
using ORMNZ.Blazor.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORMNZ.Blazor.DevEnv.App.Services
{
    /// <summary>
    /// Handle user authorization
    /// </summary>
    public class UserAuthorizationService: AuthorizationServiceBase
    {

        /// <summary>
        /// The uri helper we will use to navigate on policy fail.
        /// </summary>
        private readonly BrowserUriHelper _browserUriHelper;

        /// <summary>
        /// The current user.
        /// </summary>
        public User CurrentUser { get; private set; } = new User
        {
            FirstName = "Bob",
            LastName = "Dole",
            Email = "bob.dole@illuminati.biz",
            Active = true,
            Type = UserType.User,
            Roles = new string[] { "GetWeatherForecasts" }
        };

        /// <summary>
        /// Construct an instance of the UserAuthorizationService with dependencies.
        /// </summary>
        public UserAuthorizationService(BrowserUriHelper browserUriHelper)
        {
            _browserUriHelper = browserUriHelper;
        }

        /// <summary>
        /// Authorize access agains the current user.
        /// </summary>
        public override async Task<bool> AuthorizeAsync(AuthorizationContext context)
        {
            // If the current user is null redirect to login.
            if (CurrentUser == null)
            {
                _browserUriHelper.NavigateTo("/login");
                return false;
            }

            // If the user is not active they should not authorized,
            // redirect the use to the locked page.
            if (!CurrentUser.Active)
            {
                _browserUriHelper.NavigateTo("/locked");
                return false;
            }

            // If the current user is an admin pass them irrespective of the required roles,
            // otherwise determine if the user has all required roles.
            if (CurrentUser.Type == UserType.Admin || context.Roles.Intersect(CurrentUser.Roles).Count() == context.Roles.Count())
            {
                return true;
            }

            // The default state is failing authorization,
            // redirect the user to the forbidden page.
            _browserUriHelper.NavigateTo("/forbidden");
            return false;
        }
    }


    public class User
    {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; } = false;

        public UserType Type { get; set; }

        public string[] Roles { get; set; } = new string[] { };

    }

    public enum UserType: int
    {
        Admin, User
    }

}
