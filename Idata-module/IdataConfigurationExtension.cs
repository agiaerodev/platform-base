using Idata.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Idata
{
    public static class IdataServiceProvider
    {//Test Comment Idata Private!
        public static WebApplicationBuilder? Boot(WebApplicationBuilder? builder)
        {
            builder.Services.AddDbContext<IdataContext>(options =>
            {
                options.UseSqlServer(Ihelpers.Helpers.ConfigurationHelper.GetConfig("ConnectionStrings:DefaultConnection"));
            }, ServiceLifetime.Transient, ServiceLifetime.Transient);
       
            return builder;

        }
    }
}
