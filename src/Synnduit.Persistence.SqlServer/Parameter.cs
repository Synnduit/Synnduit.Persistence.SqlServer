using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents an application parameter.
    /// </summary>
    [Table("[dbo].[Parameter]")]
    internal class Parameter : IParameter
    {
        /// <summary>
        /// Gets or sets the ID of the parameter.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the ID of the destination (external) system that the parameter is
        /// associated with.
        /// </summary>
        public Guid? DestinationSystemId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the entity type that the parameter is associated with.
        /// </summary>
        public Guid? EntityTypeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the source (external) system that the parameter is
        /// associated with.
        /// </summary>
        public Guid? SourceSystemId { get; set; }

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter value.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(1024)]
        public string Value { get; set; }
    }
}
