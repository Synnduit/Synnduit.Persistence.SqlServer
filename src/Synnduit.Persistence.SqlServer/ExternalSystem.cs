using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Represents an external system.
    /// </summary>
    [Table("[dbo].[ExternalSystem]")]
    internal class ExternalSystem : IExternalSystem
    {
        /// <summary>
        /// Gets or sets the ID of the external system.
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the external system.
        /// </summary>
        [Column(TypeName = "VARCHAR")]
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
    }
}
