using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Isite
{

    [Table("SettingTranslations", Schema = "isite")]
    public class SettingTranslation : EntityBase
    {
        [Column(TypeName = "VARCHAR(191)")]
        public string locale { get; set; } = null!;
        [Column(TypeName = "TEXT")]
        [ObjectToString]
        public string? value { get; set; } = null!;

        [Column(TypeName = "TEXT")]
        public string? description { get; set; } = null!;

    }
}
