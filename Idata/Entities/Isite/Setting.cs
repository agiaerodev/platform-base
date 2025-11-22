using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Data.Entities.Isite
{
    [Table("Settings", Schema = "isite")]
    [Index(nameof(name), IsUnique = true)]
    public class Setting : EntityBase
    {
        public string name { get; set; } = null!;
        [Column(TypeName = "TEXT")]

        [NotMapped]
        public string? value { get; set; } = null!;
        [ObjectToString]
        
        public string? plain_value { get; set; } = null!;
        public bool? is_translatable { get; set; } = null!;

        [RelationalField]
        public List<SettingTranslation> translations { get; set; }

        public override void Initialize()
        {
            this.value = this.plain_value;
        }
    }
}
