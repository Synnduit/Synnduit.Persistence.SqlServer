using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents a record that specifies a source system that a given source system
    /// shares identifiers for a given entity type with.
    /// </summary>
    [Table("[dbo].[SharedIdentifierSourceSystem]")]
    internal class SharedIdentifierSourceSystem : ISharedIdentifierSourceSystem
    {
        /// <summary>
        /// Gets or sets the ID of the source (external) system.
        /// </summary>
        [Key, Column(Order = 0)]
        public Guid SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entity type.
        /// </summary>
        [Key, Column(Order = 1)]
        public Guid EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source (external) system that the source system
        /// represented by the current instance shares identifiers for the entity type
        /// represented by the current instance with.
        /// </summary>
        [Key, Column(Order = 2)]
        public Guid SharedIdentifierSourceSystemId { get; set; }
    }
}
