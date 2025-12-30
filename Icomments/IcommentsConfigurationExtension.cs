using Idata.Data;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
//appendUsing
using Icomments.Repositories.Caching;
using Icomments.Services.Interfaces;
using Icomments.Services;
using Icomments.Repositories.Interfaces;
using Icomments.Repositories;

namespace Icomments
{
    public class IcommentsServiceProvider
    {

        public static WebApplicationBuilder? Boot(WebApplicationBuilder? builder)
        {
            //TODO Implement controllerBase to avoid basic crud redundant code
            builder.Services.AddControllers().ConfigureApplicationPartManager(o =>
            {
                o.ApplicationParts.Add(new AssemblyPart(typeof(IcommentsServiceProvider).Assembly));
            });
            //appendRepositories
            builder.Services.AddScoped(typeof(IIcommentRepository), typeof(IcommentRepository));

            //appendServices
            builder.Services.AddScoped<IIcommentService, IcommentService>();
                        
            //appendDecorators
            //builder.Services.Decorate<IIcommentRepository, CachedIcommentRepository>();

            return builder;


        }

    }
}
