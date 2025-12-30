using Idata.Data.Entities.Iprofile;
using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using Ihelpers.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;


namespace Idata.Entities.Icomments
{

    [Table("Icomments", Schema = "icomments")]
    public class Icomment: EntityBase
    {

        public Icomment()
        {

        }

        #region relations      


        #endregion


        [Column(TypeName = "text")]
        public string? comment { get; set; }

        #region relations

        [ForeignKey("user_id")]
        [RelationalField]
        public virtual User? userProfile { get; set; }
        #endregion


        public bool? approved { get; set; }
        public bool? is_internal { get; set; }
        public string? commentable_type { get; set; }
        public long? commentable_id { get; set; }
        public string? guest_name { get; set; }
        public string? guest_email { get; set; }
        public long? user_id { get; set; }

        public override void Initialize()
        {

            //Due to be a new fix, prevent previous strings to stop working
            try
            {
                this.comment = this.comment?.Base64Decode();
            }
            catch
            {


            }

        }


    }
}

