using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Iprofile
{

    [Table("Departments", Schema = "iprofile")]
    public partial class Department : EntityBase
    {
        public string title { get; set; } = null!;

        [NotMapped]
        public dynamic? parent { get; set; } = null;
        public long? parent_id { get; set; }
        public long? is_internal { get; set; }
        public long? lft { get; set; }
        public long? rgt { get; set; }
        public long? depth { get; set; }

        public long? organization_id { get; set; }

        [RelationalField]
        public virtual List<User> users { get; set; } = new();


    }
}
