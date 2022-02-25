using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents the identifier that uniquely identifies a known entity in the
    /// destination system along with associated mapping information.
    /// </summary>
    [Table("[dbo].[MappedEntityIdentifier]")]
    internal class MappedEntityIdentifier : IMappedEntityIdentifier
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
        [MaxLength(512)]
        public string SourceSystemEntityId { get; set; }

        /// <summary>
        /// Gets or sets the ID that uniquely identifies the entity in the destination
        /// system.
        /// </summary>
        [Column(TypeName = "NVARCHAR")]
        [Required]
        [MaxLength(512)]
        public string DestinationSystemEntityId { get; set; }

        /// <summary>
        /// Gets the mapping's origin (code).
        /// </summary>
        public int Origin { get; set; }

        /// <summary>
        /// Gets the mapping's state (code).
        /// </summary>
        public int State { get; set; }
    }
}
