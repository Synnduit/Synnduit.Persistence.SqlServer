using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents the serialized source system entity associated with a source/destination
    /// system identifier mapping.
    /// </summary>
    [Table("[dbo].[MappingEntity]")]
    internal class MappingEntity
    {
        /// <summary>
        /// Gets or sets the ID of the mapping.
        /// </summary>
        [Key]
        public Guid MappingId { get; set; }

        /// <summary>
        /// Gets or sets the serialized entity data.
        /// </summary>
        [Required]
        public byte[] EntityData { get; set; }
    }
}
