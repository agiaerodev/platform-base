using Idata.Entities.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Isite
{
    [Table("ModuleTranslations", Schema = "isite")]
    public class ModuleTranslation : EntityBase
    {
        [Column(TypeName = "VARCHAR(191)")]
        public string locale { get; set; } = null!;
        [Column(TypeName = "VARCHAR(191)")]
        public string? title { get; set; } = null!;

    }
}
