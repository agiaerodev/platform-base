using System.Reflection;

namespace Idata.Helpers
{
    public class ReportableHelper
    {
        public static async Task<List<object>?> GetReportableClasses()
        {
            var Response = new List<object>();

            var allClasses = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsClass && a.BaseType.Name == "EntityBase" && a.Namespace != null && (a.Namespace.Contains(@"Idata.Data.Entities") || a.Namespace.Contains(@"Idata.Entities"))).ToList();

            foreach (var classType in allClasses)
            {

                dynamic instance = Activator.CreateInstance(classType);

                if (instance.is_reportable)
                {
                    Response.Add(new { label = classType.Name, value = classType.Namespace + "." + classType.Name });
                }
            }
            return Response;
        }
    }
}
