
using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Iprofile
{
    [Table("Roles", Schema = "iprofile")]
    public partial class Role : EntityBase
    {


        public Role()
        {
            searchable_fields = "id,slug,name,external_guid";
        }
        [Column(TypeName = "VARCHAR(191)")]
        public string slug { get; set; } = null!;
        [Column(TypeName = "VARCHAR(191)")]
        public string name { get; set; } = null!;
        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? permissions { get; set; } = null!;
        [NotMapped]
        [ObjectToString]
        public object? settings { get; set; } = new { };

        public Guid? external_guid { get; set; }
        [RelationalField]
        public virtual List<User> users { get; set; } = new();

	}
}
