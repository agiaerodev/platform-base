using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace Idata.Entities.Core
{
    /// <summary>
    /// EntityBase is the base class that holds common properties and methods of the entities.
    /// </summary>
    public class EntityBase
    {
        /// <summary>
        /// A unique identifier for the entity
        /// </summary>
        [Key]
        public long? id { get; set; } = null;
        /// <summary>
        /// The date and time when the entity was created.
        /// </summary>
        public DateTime? created_at { get; set; }
        /// <summary>
        /// The date and time when the entity was updated.
        /// </summary>
        public DateTime? updated_at { get; set; }
        /// <summary>
        /// The date and time when the entity was deleted.
        /// </summary>
        public DateTime? deleted_at { get; set; }

        /// <summary>
        /// The date and time when the entity was restored.
        /// </summary>
        public DateTime? restored_at { get; set; }

        /// <summary>
        /// The user who created the entity.
        /// </summary>
        public long? created_by { get; set; } = null;
        /// <summary>
        /// The user who updated the entity.
        /// </summary>
        public long? updated_by { get; set; } = null;
        /// <summary>
        /// The user who deleted the entity.
        /// </summary>
        public long? deleted_by { get; set; } = null;

        /// <summary>
        /// The user who restored the entity.
        /// </summary>
        public long? restored_by { get; set; } = null;
        /// <summary>
        /// An external identifier for the entity, used on each entity if needs to store an external_id (users azure id for example).
        /// </summary>
        public string? external_id { get; set; }

        /// <summary>
        /// An special identifier for the entity, multipurpose.
        /// </summary>
        public string? offline_id { get; set; }

        /// <summary>
        /// A string representation of options for the entity, in json format.
        /// </summary>
        [Column(TypeName = "TEXT")]
        [SimpleObjectToString]
        public string? options { get; set; }
        /// <summary>
        /// This attribute activates the revision saver in the RepositoryBase
        /// </summary>
        [NotMapped]
        public virtual bool? is_revisionable { get; set; } = true;
        /// <summary>
        /// Specifies if the entity is reportable or not.
        /// </summary>
        [NotMapped]
        public virtual bool? is_reportable { get; set; } = false;

        /// <summary>
        /// Specifies if the entity can be deleted (not soft deleted) inside repository base
        /// </summary>
        [NotMapped]
        public virtual bool? force_delete { get; set; } = false;

        /// <summary>
        /// A dictionary of dynamic parameters used by the transformer to store and track relations given in front, them use it inside SyncRelations function.
        /// </summary>
        [Ignore]
        [NotMapped]
        public Dictionary<string, dynamic?> dynamic_parameters { get; set; } = new();

        /// <summary>
        /// Specifies the default included fields when the entity is loaded, when performing query these default includes will be added to the query.
        /// </summary>
        [NotMapped]
        public virtual string? default_include { get; set; } = "";
        /// <summary>
        /// Specifies the searchable fields of the entity, when a criteria is send in a get request without the field, we search for the searchable fields to apply query.
        /// </summary>
        [NotMapped]
        public virtual string? searchable_fields { get; set; } = "";
        /// <summary>
        /// A string array of file formats for the entity, for reporting purposes.
        /// </summary>
        [NotMapped]
        public virtual string[]? file_formats { get; set; } = null;

        /// <summary>
        /// returns the properties of the entuty
        /// </summary>
        /// <returns></returns>
        public List<PropertyInfo> getProperties()
        {
            return GetType().GetProperties().ToList();
        }
        /// <summary>
        /// Returns the relational fields names of entity inside a string list
        /// </summary>
        /// <returns>List of property names with relation of class with navigation</returns>
        public List<string> getRelations()
        {
            List<string> relations = new List<string>();

            var propList = GetType().GetProperties().ToList();

            foreach (PropertyInfo prop in propList)
            {
                var relational = prop.GetCustomAttributes(true).Where(att => att is IDataAnnotationBase && att is RelationalField).FirstOrDefault();

                if (relational != null)
                {
                    //check if is list to get the type of the list (objetive is get the default relation of the class)
                    Type relationType = prop.PropertyType;

                    dynamic? obj;

                    relations.Add(prop.Name);

                    if (prop.PropertyType.Name.Contains("List"))
                    {
                        var internalType = Activator.CreateInstance(relationType);

                        relationType = internalType.GetType().GetGenericArguments().Single();

                        obj = Activator.CreateInstance(relationType);
                    }
                    else
                    {
                        obj = Activator.CreateInstance(relationType);
                    }

                    string defaultIncludes = obj.default_include;

                    if (defaultIncludes != "")
                    {

                        if (defaultIncludes.Contains(','))
                        {
                            string[] splitInclude = defaultIncludes.Split(',');

                            foreach (string fieldToInclude in splitInclude)
                            {
                                relations.Add(prop.Name + "." + fieldToInclude);
                            }

                        }
                        else
                        {
                            relations.Add(prop.Name + "." + defaultIncludes);
                        }
                    }

                }

            }
            return relations;
        }
        /// <summary>
        /// Method that returns child class name
        /// </summary>
        /// <returns></returns>
        public string getClasssName()
        {
            return GetType().Name;
        }

        /// <summary>
        /// method that initializes the entity, meant to be override and perform operations on the entity
        /// </summary>
        public virtual void Initialize()
        {
            return;
        }

        [Obsolete("InitializeForCSV is deprecated, please use entity repository export instead.")]
        public virtual void InitializeForCSV()
        {
            default_include = null;
            options = null;
            dynamic_parameters = null;
            return;
        }


    }
}
