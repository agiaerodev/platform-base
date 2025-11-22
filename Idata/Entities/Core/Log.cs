using Idata.Data.Entities;
using Idata.Data.Entities.Iprofile;
using Ihelpers.DataAnotations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Security.Policy;

namespace Idata.Entities.Core
{
    /// <summary>
    /// Don't edit this class
    /// </summary>

    [Index(nameof(message))]
    [Index(nameof(source_app))]

    /// <summary>
    /// Represents a log entry in the system.
    /// </summary>
    [Table("Logs", Schema = "core")]
    public class Log
    {

        /// <summary>
        /// The unique identifier for the log entry.
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// The message associated with the log entry.
        /// </summary>
        public string? message { get; set; }

        /// <summary>
        /// The type of log entry, such as Information, Warning, or Exception.
        /// </summary>
        public string? type { get; set; }

        /// <summary>
        /// The stack trace associated with the log entry, if available.
        /// </summary>
        public string? stackTrace { get; set; }

        /// <summary>
        /// The identifier of the user associated with the log entry, if available.
        /// </summary>
        public long? user_id { get; set; } = null;

        /// <summary>
        /// The date and time when the log entry was created.
        /// </summary>
        public DateTime? dateCreated { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public virtual string? default_include { get; set; } = "";

        /// <summary>
        /// The application or module that writes the log
        /// </summary>
        public string? source_app { get; set; }

        /// <summary>
        /// A list of fields that can be searched when querying the logs.
        /// </summary>
        [Ignore]
        [NotMapped]
        public string? searchable_fields { get; set; } = "type,user_id,id,dateCreated";

        [RelationalField]
        [ForeignKey("user_id")]
        public User? user { get; set; }


        /// <summary>
        /// Returns all properties for the `Log` class.
        /// </summary>
        /// <returns>A list of `PropertyInfo` objects representing the properties of the `Log` class.</returns>

        public List<PropertyInfo> getProperties()
        {
            return this.GetType().GetProperties().ToList();
        }

    }
}
