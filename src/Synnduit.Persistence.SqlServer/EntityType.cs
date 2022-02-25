using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents an entity type.
    /// </summary>
    [Table("[dbo].[EntityType]")]
    internal class EntityType : IEntityType
    {
        /// <summary>
        /// Gets or sets the ID of the entity type.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entity type's parent destination (external) system.
        /// </summary>
        public Guid DestinationSystemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the entity type.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the assembly qualified name of the type that represents the
        /// entity.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(1024)]
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type that represents the entity type's sink.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(256)]
        public string SinkTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets the full name of the type that represents the entity type's cache
        /// feed; optional, may be null.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [MaxLength(256)]
        public string CacheFeedTypeFullName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether instances of the entity type are
        /// mutable; i.e., whether or not they may change between runs.
        /// </summary>
        public bool IsMutable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether source system instances of the entity
        /// type may be duplicates (i.e., represent the same destination system entity); in
        /// other words, this value indicates whether or not source system entity instances
        /// should be deduplicated.
        /// </summary>
        public bool IsDuplicable { get; set; }
    }
}
