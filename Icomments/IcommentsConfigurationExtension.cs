using Idata.Data;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
//appendUsing

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

            //appendServices
                        
            //appendDecorators

            return builder;


        }

    }
}
