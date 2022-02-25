using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents an entity source/destination system identifier mapping along with
    /// associated information.
    /// </summary>
    [Table("[dbo].[EntityMapping]")]
    internal class EntityMapping : IEntityMapping
    {
        /// <summary>
        /// Gets or sets the ID of the mapping.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entity type.
        /// </summary>
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source (external) system.
        /// </summary>
        public Guid SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID that uniquely identifies the entity in the source system.
        /// </summary>
        [Column(TypeName = "NVARCHAR")]
        [Required]
        [MaxLength(256)]
        public string SourceSystemEntityId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the destination (external) system.
        /// </summary>
        public Guid DestinationSystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID that uniquely identifies the entity in the destination
        /// system.
        /// </summary>
        [Column(TypeName = "NVARCHAR")]
        [Required]
        [MaxLength(256)]
        public string DestinationSystemEntityId { get; set; }

        /// <summary>
        /// Gets or sets the mapping's origin (code).
        /// </summary>
        public int Origin { get; set; }

        /// <summary>
        /// Gets or sets the mapping's state (code).
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the serialized entity hash.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(24)]
        public string SerializedEntityHash { get; set; }
    }
}
