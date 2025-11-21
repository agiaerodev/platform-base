using Idata.Data.Entities.Iprofile;
using Idata.Entities.Core;
using Ihelpers.DataAnotations;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace Idata.Entities.Iprofile
{
    [Table("AuthClients", Schema = "iprofile")]

    public class AuthClient : EntityBase
    {

        public string hash { get; set; }
        public long? user_id { get; set; }
        public long? impersonated_by { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        [NoUserTimezone]
        public DateTime expires_at { get; set; }
        [NoUserTimezone]
        public DateTime? refreshed_at { get; set; } = null;
        
        [NoUserTimezone]
        public DateTime? last_request { get; set; } = DateTime.UtcNow;
        
        public string? device { get; set; }

        public bool revoked { get; set; }

        #region Relations
        [ForeignKey("user_id")]
        [RelationalField]
        public User user { get; set; }

        [ForeignKey("impersonated_by")]
        [RelationalField]
        public User? impersonator { get; set; }

        #endregion
    }
}
