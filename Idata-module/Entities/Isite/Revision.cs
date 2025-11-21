using Idata.Data.Entities;
using Idata.Data.Entities.Iprofile;
using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Entities.Isite
{
    [Table("Revisions", Schema = "isite")]
    public class Revision : EntityBase
    {

        public Revision()
        {
            searchable_fields = "revisionable_type, revisionable_id";
        }

        #region relations

        [ForeignKey("user_id")]
        [RelationalField]
        public virtual User? userProfile { get; set; }

        [ForeignKey("created_by")]
        [RelationalField]
        public virtual User? creator { get; set; }

        [ForeignKey("updated_by")]
        [RelationalField]
        public virtual User? editor { get; set; }

        [ForeignKey("deleted_by")]
        [RelationalField]
        public virtual User? destroyer { get; set; }


        #endregion
        public long? user_id { get; set; }

        public string? revisionable_type { get; set; }
        public long? revisionable_id { get; set; }
        public string? key { get; set; }

        [Column(TypeName = "text")]
        
        public string? old_value { get; set; }

        [Column(TypeName = "text")]
        
        public string? new_value { get; set; }

    }
}
