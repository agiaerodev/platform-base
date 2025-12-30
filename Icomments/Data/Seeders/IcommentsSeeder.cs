using Idata.Data;

namespace Icomments.Data.Seeders
{
    public class IcommentsSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<IdataContext>();

                context.Database.EnsureCreated();

                IcommentsModuleSeeder.Seed(applicationBuilder);
            }
        }
    }
}
