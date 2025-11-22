using System.Reflection;
using Ihelpers.DataAnotations;
using Ihelpers.Extensions;
using Ihelpers.Helpers;
namespace Idata.Helpers
{
    public class EntitiesHelper
    {

        public enum CrudActions
        {
            create,
            update, 
            delete,
            get,
            restore
        }

        /// <summary>
        /// Asynchronously stores the path of all classes inside Idata assembly that inherit from the EntityBase class and are located in the Idata.Data.Entities namespace, in the cache container located in Ihelper.
        /// </summary>
        /// <returns></returns>
        public static async Task StoreClassesPath()
        {

            // Retrieve all classes in the current executing assembly that inherit from the EntityBase class and are located in the Idata.Data.Entities namespace. and store inside cache
            var allClasses = Assembly.GetExecutingAssembly().GetTypes().Where(a => a.IsClass && a.BaseType.Name.Contains("EntityBase") && a.Namespace != null).ToList();

            await ConfigContainer.cache.CreateValue($"AllClasses", allClasses, 43.200);

            foreach(var specificClass in allClasses)
            {
                //Dictionary with the class and the class properties
                var classProperties = specificClass.GetProperties().ToList();
             
                await ConfigContainer.cache.CreateValue($"{specificClass.FullName}", classProperties, 43.200);

                foreach (var property in classProperties)
                {
                    // Store each property of the class in the cache
                    await ConfigContainer.cache.CreateValue($"{specificClass.FullName}.{property.Name}", property, 43.200);

                    // Check if the property has the Ignore attribute and is of type IDataAnnotationBase
                    var ignoreAttr = property.GetCustomAttributes(true).Where(att => att != null && att is Ignore && att is IDataAnnotationBase).FirstOrDefault();
                    if (ignoreAttr != null) continue;
                    //Ayudame a guardarlo en el cache:
                    var dataAnnotations = property.GetCustomAttributes(true).Where(att => att != null && att is not Ignore && att.ToString().Contains("Nullable") == false && att is IDataAnnotationBase).ToArray();
                    if (dataAnnotations.Length > 0)
                    {
                        // Store the data annotations in the cache
                        await ConfigContainer.cache.CreateValue($"{specificClass.FullName}.{property.Name}.DataAnnotations", dataAnnotations, 43.200);
                    }
                    else
                    {
                        // If no data annotations are found, store an empty array
                        await ConfigContainer.cache.CreateValue($"{specificClass.FullName}.{property.Name}.DataAnnotations", new object?[] { }, 43.200);
                    }

                    //also store the property name with the camelCase version of its name

                    var camelCasePropName = StringHelper.ToCamelCase(property.Name);
                    await ConfigContainer.cache.CreateValue($"{specificClass.FullName}.{property.Name}.CamelCase", camelCasePropName, 43.200);
                }
            }



        }


        /// <summary>
        /// Gets the first class that is a subclass of EntityBase in the Idata.Data.Entities namespace 
        /// and whose namespace contains one or more of the specified arguments.
        /// </summary>
        /// <param name="input">The input string containing one or more comma-separated arguments.</param>
        /// <returns>The first class that matches the specified criteria, or null if no class is found.</returns>
        public static async Task<Type?> GetClassBy(string input)
        {
            // Split the input string into individual arguments, if necessary
            List<string> arguments = new List<string>();

            if (input.Contains(','))
            {
                arguments = input.Split(',').ToList();
            }
            else
            {
                arguments.Add(input);
            }

            // Query for classes that meet the specified criteria
            var clasesQuery = Assembly.GetExecutingAssembly().GetTypes()
                .Where(a => a.IsClass && a.BaseType.Name == "EntityBase"
                    && a.Namespace != null && a.Namespace.Contains(@"Idata.Data.Entities"));

            // Narrow the query results by filtering by each argument in the list
            foreach (var argument in arguments)
            {
                clasesQuery = clasesQuery.Where(a => a.Namespace.ToLower().Contains(argument.ToLower()));
            }

            // Return the first matching class, or null if no match is found
            return clasesQuery.FirstOrDefault();
        }
    }
}
