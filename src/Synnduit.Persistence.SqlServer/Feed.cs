using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents an entity feed.
    /// </summary>
    [Table("Feed", Schema = "dbo")]
    internal class Feed
    {
        /// <summary>
        /// Gets or sets the ID of the entity type.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source (external) system.
        /// </summary>
        public Guid SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type that represents the feed.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(256)]
        public string FeedTypeFullName { get; set; }
    }
}
