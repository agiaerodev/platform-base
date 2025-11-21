
using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace Idata.Data.Entities.Iprofile
{
    [Table("Users", Schema = "iprofile")]
    public partial class User : EntityBase
    {

        public User()
        {

            searchable_fields = "id,email,first_name,last_name";
            is_reportable = true;
        }

        [Column(TypeName = "VARCHAR(191)")]

        public string email { get; set; }
        [Column(TypeName = "VARCHAR(191)")]
        [Password]
        public string password { get; set; }


        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? permissions { get; set; }

        [NotMapped]
        [ObjectToString]
        public object? all_settings { get; set; } = new { };

        [NotMapped]
        public dynamic? social_data { get; set; } = new ExpandoObject();
        [NotMapped]
        public override string? default_include { get; set; } = "departments,roles";

        [NotMapped]
        public Dictionary<string, bool?>? AllPermissions { get; set; } = new();

        public DateTime? last_login { get; set; } = DateTime.UtcNow;
        [NoUserTimezone]
        public DateTime? last_request { get; set; } = DateTime.UtcNow;
        [NotMapped]

        public string? full_name
        {
            get
            {
                return string.Format("{0} {1}", this.first_name, this.last_name);
            }
            set => full_name = value;
        }

        [Column(TypeName = "VARCHAR(191)")]
        public string? first_name { get; set; } = null;
        [Column(TypeName = "VARCHAR(191)")]
        public string? last_name { get; set; } = null;

        public string? timezone { get; set; }

        [NotMapped]
        public string? timezone_offset { get; set; }


        [Column(TypeName = "VARCHAR(191)")]
        public string? language { get; set; } = null;

        public Guid? external_guid { get; set; }

        [RelationalField]
        public virtual List<Role> roles { get; set; } = new();
        [RelationalField]
        public virtual List<Department> departments { get; set; } = new();

	
		//[RelationalField]
		//public virtual List<Log>? logs { get; set; }
		public bool HasAccess(string permission)
        {
            bool? allowed = false;

            AllPermissions.TryGetValue(permission, out allowed);

            return allowed ?? false;
        }

        public override void Initialize()
        {


            if (this.AllPermissions.Count == 0)
            {
                JObject jallPermissions = new();

                foreach (var role in this.roles)
                {
                    JObject rolePermissions = JObject.Parse(role.permissions);
                    jallPermissions.Merge(rolePermissions, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Replace
                    });

                }

                //User permissions
                if (this.permissions != null)
                {
                    JObject userPermissions = JObject.Parse(this.permissions);
                    jallPermissions.Merge(userPermissions, new JsonMergeSettings
                    {
                        MergeArrayHandling = MergeArrayHandling.Replace
                    });

                }
                AllPermissions = jallPermissions.ToObject<Dictionary<string, bool?>>();
            }



        }

        public override void InitializeForCSV()
        {
            AllPermissions = null;
            permissions = null;
            roles = null;
            departments = null;
            password = null;

            base.InitializeForCSV();

        }

    }
}
