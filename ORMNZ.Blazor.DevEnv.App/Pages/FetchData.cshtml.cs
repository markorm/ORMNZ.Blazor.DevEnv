using Microsoft.AspNetCore.Blazor.Components;
using ORMNZ.Blazor.Authorization;
using ORMNZ.Blazor.DevEnv.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORMNZ.Blazor.DevEnv.App.Pages
{

    [Authorize(Policy = "Users", Roles = "GetWeatherForecasts")]
    public class FetchDataViewModel : AuthorizationComponent
    {

        public WeatherForecast[] forecasts;

        [Inject]
        public WeatherForecastService ForecastService { get; set; }

        public override async Task OnAuthorizationSuccess()
        {
            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
        }

    }

}
