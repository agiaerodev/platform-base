using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Isite
{
    [Table("Modules", Schema = "isite")]
    public class Module : EntityBase
    {
        [NotMapped]
        public string title { get; set; } = null!;
        public string name { get; set; } = null!;
        public string? alias { get; set; } = null!;
        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? permissions { get; set; } = null!;

        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? settings { get; set; } = null!;
        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? cms_pages { get; set; } = null!;
        public string? cms_sidebar { get; set; } = null!;
        public bool? enabled { get; set; } = null!;
        public int? priority { get; set; } = null!;

        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? configs { get; set; }

        [RelationalField]
        public List<ModuleTranslation> translations { get; set; }

        public override void Initialize()
        {
            this.title = this.name;
        }
    }
}
