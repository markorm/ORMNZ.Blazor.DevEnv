using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using ORMNZ.Blazor.Authorization;
using ORMNZ.Blazor.DevEnv.App.Services;

namespace ORMNZ.Blazor.DevEnv.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            // Add the authorization services
            services
                // EXMAPLE: Specify the IAuthorizationManager instance to use.
                // In this case adding the default AuthorizationManager is redundant,
                // this default will be used if no custom IAuthorizationManager is
                // used here.
                .UseAuthorizationManager<AuthorizationManager>()
                // Add an authorization service to handle the "Users" policy.
                // This class' Authorize method will be invoked any time there is
                // an Authorize attribute with this policy set
                .AddAuthorization<UserAuthorizationService>(options =>
                {
                    options.Policy = "Users";
                })
                // Add an authorization service to handle the "Admins" policy.
                .AddAuthorization<AdminAuthorizationService>(options =>
                {
                    options.Policy = "Admins";
                });

            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();

        }

        public void Configure(IBlazorApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
