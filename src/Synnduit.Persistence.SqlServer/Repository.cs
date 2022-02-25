using Microsoft.EntityFrameworkCore;
using System.Composition;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// Exposes methods that allow the application access to the underlying persistence
    /// mechanism.
    /// </summary>
    [Export(typeof(IRepository))]
    public class Repository : IRepository
    {
        private readonly DatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        [ImportingConstructor]
        public Repository()
        {
            this.context = new DatabaseContext();
        }

        /// <summary>
        /// Disposes the underlying data context.
        /// </summary>
        public void Dispose()
        {
            this.context.Dispose();
        }

        /// <summary>
        /// Clears all shared source system identifier (group) records for the specified
        /// entity type.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        public void ClearSharedSourceSystemIdentifiers(Guid entityTypeId)
        {
            this.context.ClearSharedSourceSystemIdentifiers(entityTypeId);
        }

        /// <summary>
        /// Creates a new entity deletion.
        /// </summary>
        /// <param name="deletion">The transaction to create.</param>
        public void CreateEntityDeletion(IEntityDeletion deletion)
        {
            this.context.CreateEntityDeletion(
                deletion.Id,
                deletion.TimeStamp,
                deletion.EntityTypeId,
                deletion.DestinationSystemEntityId,
                deletion.Outcome);
        }

        /// <summary>
        /// Creates a new identity entity transaction.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        public void CreateIdentityEntityTransaction(IIdentityEntityTransaction transaction)
        {
            this.context.CreateIdentityEntityTransaction(
                transaction.Id,
                transaction.TimeStamp,
                transaction.Outcome,
                transaction.Identity.EntityTypeId,
                transaction.Identity.SourceSystemId,
                transaction.Identity.SourceSystemEntityId);
        }

        /// <summary>
        /// Creates a new message associated with the specified source system entity
        /// identity (always) and operation (if it exists).
        /// </summary>
        /// <param name="message">The message to create.</param>
        public void CreateIdentityOperationMessage(IIdentityOperationMessage message)
        {
            this.context.CreateIdentityOperationMessage(
                message.Operation.Id,
                message.Operation.TimeStamp,
                message.Identity.EntityTypeId,
                message.Identity.SourceSystemId,
                message.Identity.SourceSystemEntityId,
                message.Message.Type,
                message.Message.TextHash,
                message.Message.Text);
        }

        /// <summary>
        /// Creates a new serialized source system entity associated with the specified
        /// source system entity identity (always) and operation (if it exists).
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public void CreateIdentityOperationSourceSystemEntity(
            IIdentityOperationSourceSystemEntity entity)
        {
            this.context.CreateIdentityOperationSourceSystemEntity(
                entity.Operation.Id,
                entity.Operation.TimeStamp,
                entity.Identity.EntityTypeId,
                entity.Identity.SourceSystemId,
                entity.Identity.SourceSystemEntityId,
                entity.Entity.DataHash,
                entity.Entity.Data,
                entity.Entity.Label);
        }

        /// <summary>
        /// Creates a new entity source/destination system identifier mapping.
        /// </summary>
        /// <param name="mapping">The mapping to create.</param>
        public void CreateMapping(IMapping mapping)
        {
            this.context.CreateMapping(
                mapping.Id,
                mapping.Operation.Id,
                mapping.Operation.TimeStamp,
                mapping.SourceSystemEntity.DataHash,
                mapping.SourceSystemEntity.Data,
                mapping.SourceSystemEntity.Label,
                mapping.EntityTypeId,
                mapping.SourceSystemId,
                mapping.SourceSystemEntityId,
                mapping.DestinationSystemEntityId,
                mapping.Origin,
                mapping.State);
        }

        /// <summary>
        /// Creates a new mapping entity transaction.
        /// </summary>
        /// <param name="transaction">The transaction to create.</param>
        public void CreateMappingEntityTransaction(IMappingEntityTransaction transaction)
        {
            this.context.CreateMappingEntityTransaction(
                transaction.Id,
                transaction.TimeStamp,
                transaction.Outcome,
                transaction.MappingId);
        }

        /// <summary>
        /// Creates a new serialized destination system entity associated with an
        /// operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public void CreateOperationDestinationSystemEntity(
            IOperationSerializedEntity entity)
        {
            this.context.CreateOperationDestinationSystemEntity(
                entity.Operation.Id,
                entity.Operation.TimeStamp,
                entity.Entity.DataHash,
                entity.Entity.Data,
                entity.Entity.Label);
        }

        /// <summary>
        /// Creates a new operation message.
        /// </summary>
        /// <param name="message">The message to create.</param>
        public void CreateOperationMessage(IOperationMessage message)
        {
            this.context.CreateOperationMessage(
                message.Operation.Id,
                message.Operation.TimeStamp,
                message.Message.Type,
                message.Message.TextHash,
                message.Message.Text);
        }

        /// <summary>
        /// Creates a new serialized source system entity associated with an operation.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        public void CreateOperationSourceSystemEntity(IOperationSerializedEntity entity)
        {
            this.context.CreateOperationSourceSystemEntity(
                entity.Operation.Id,
                entity.Operation.TimeStamp,
                entity.Entity.DataHash,
                entity.Entity.Data,
                entity.Entity.Label);
        }

        /// <summary>
        /// Creates or updates the specified entity type.
        /// </summary>
        /// <param name="entityType">The entity type to create or update.</param>
        public void CreateOrUpdateEntityType(IEntityType entityType)
        {
            this.context.CreateOrUpdateEntityType(
                entityType.Id,
                entityType.DestinationSystemId,
                entityType.Name,
                entityType.TypeName,
                entityType.SinkTypeFullName,
                entityType.CacheFeedTypeFullName,
                entityType.IsMutable,
                entityType.IsDuplicable);
        }

        /// <summary>
        /// Creates or updates the specified external system.
        /// </summary>
        /// <param name="externalSystem">The external system to create or update.</param>
        public void CreateOrUpdateExternalSystem(IExternalSystem externalSystem)
        {
            this.context.CreateOrUpdateExternalSystem(
                externalSystem.Id, externalSystem.Name);
        }

        /// <summary>
        /// Creates or updates the specified feed.
        /// </summary>
        /// <param name="feed">The feed to create or update.</param>
        public void CreateOrUpdateFeed(IFeed feed)
        {
            this.context.CreateOrUpdateFeed(
                feed.EntityTypeId, feed.SourceSystemId, feed.FeedTypeFullName);
        }

        /// <summary>
        /// Creates the specified application parameter.
        /// </summary>
        /// <param name="parameter">The parameter to create.</param>
        public void CreateParameter(IParameter parameter)
        {
            this.context.CreateParameter(
                parameter.Id,
                parameter.DestinationSystemId,
                parameter.EntityTypeId,
                parameter.SourceSystemId,
                parameter.Name,
                parameter.Value);
        }

        /// <summary>
        /// Creates a new shared source system identifier record.
        /// </summary>
        /// <param name="sharedSourceSystemIdentifier">
        /// The shared source system identifier record to create.
        /// </param>
        public void CreateSharedSourceSystemIdentifier(
            ISharedSourceSystemIdentifier sharedSourceSystemIdentifier)
        {
            this.context.CreateSharedSourceSystemIdentifier(
                sharedSourceSystemIdentifier.EntityTypeId,
                sharedSourceSystemIdentifier.SourceSystemId,
                sharedSourceSystemIdentifier.GroupNumber);
        }

        /// <summary>
        /// Creates a new value change record.
        /// </summary>
        /// <param name="valueChange">The value change record to create.</param>
        public void CreateValueChange(IValueChange valueChange)
        {
            this.context.CreateValueChange(
                valueChange.Id,
                valueChange.MappingEntityTransactionId,
                valueChange.ValueName,
                valueChange.PreviousValue,
                valueChange.NewValue);
        }

        /// <summary>
        /// Deletes the specified application parameter.
        /// </summary>
        /// <param name="id">The ID of the parameter.</param>
        public void DeleteParameter(Guid id)
        {
            this.context.DeleteParameter(id);
        }

        /// <summary>
        /// Gets the dictionary of application parameters that apply to the specified
        /// destination system.
        /// </summary>
        /// <param name="destinationSystemId">
        /// The ID of the destination (external) system.
        /// </param>
        /// <returns>
        /// The dictionary (name/value) of application parameters that apply to the
        /// specified destination system.
        /// </returns>
        public IDictionary<string, string>
            GetDestinationSystemParameters(Guid destinationSystemId)
        {
            return
                this
                .context
                .Parameters
                .AsNoTracking()
                .Where(p =>
                    p.DestinationSystemId == destinationSystemId &&
                    p.EntityTypeId == null &&
                    p.SourceSystemId == null)
                .ToDictionary(p => p.Name, p => p.Value);
        }

        /// <summary>
        /// Gets all entity mappings for the specified destination system that are in one
        /// of the specified states.
        /// </summary>
        /// <param name="destinationSystemId">
        /// The ID of the destination (external) system.
        /// </param>
        /// <param name="states">
        /// The collection of requested mapping states (codes).
        /// </param>
        /// <returns>The collection of entity mappings.</returns>
        public IEnumerable<IEntityMapping> GetEntityMappings(
            Guid destinationSystemId, params int[] states)
        {
            return
                this
                .context
                .EntityMappings
                .AsNoTracking()
                .Where(em =>
                    em.DestinationSystemId == destinationSystemId &&
                    states.Contains(em.State))
                .ToArray();
        }

        /// <summary>
        /// Gets the specified entity type.
        /// </summary>
        /// <param name="id">The ID of the requested entity type.</param>
        /// <returns>The requested entity type.</returns>
        public IEntityType GetEntityType(Guid id)
        {
            return
                this
                .context
                .EntityTypes
                .AsNoTracking()
                .Single(et => et.Id == id);
        }

        /// <summary>
        /// Gets the dictionary of application parameters that apply to the specified
        /// entity type.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <returns>
        /// The dictionary of application parameters that apply to the specified entity
        /// type.
        /// </returns>
        public IDictionary<string, string> GetEntityTypeParameters(Guid entityTypeId)
        {
            return
                this
                .context
                .Parameters
                .AsNoTracking()
                .Where(p =>
                    p.DestinationSystemId == null &&
                    p.EntityTypeId == entityTypeId &&
                    p.SourceSystemId == null)
                .ToDictionary(p => p.Name, p => p.Value);
        }

        /// <summary>
        /// Gets the collection of all entity types.
        /// </summary>
        /// <returns>The collection of all entity types.</returns>
        public IEnumerable<IEntityType> GetEntityTypes()
        {
            return
                this
                .context
                .EntityTypes
                .AsNoTracking()
                .ToArray();
        }

        /// <summary>
        /// Gets the dictionary of application parameters that apply to the specified
        /// entity type and source system.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <returns>
        /// The dictionary of application parameters that apply to the specified entity
        /// type and source system.
        /// </returns>
        public IDictionary<string, string> GetEntityTypeSourceSystemParameters(
            Guid entityTypeId, Guid sourceSystemId)
        {
            return
                this
                .context
                .Parameters
                .AsNoTracking()
                .Where(p =>
                    p.DestinationSystemId == null &&
                    p.EntityTypeId == entityTypeId &&
                    p.SourceSystemId == sourceSystemId)
                .ToDictionary(p => p.Name, p => p.Value);
        }

        /// <summary>
        /// Gets the collection of all external systems.
        /// </summary>
        /// <returns>The collection of all external systems.</returns>
        public IEnumerable<IExternalSystem> GetExternalSystems()
        {
            return
                this
                .context
                .ExternalSystems
                .AsNoTracking()
                .ToArray();
        }

        /// <summary>
        /// Gets the full name of the type that represents the feed for the specified
        /// entity type and source system.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <returns>
        /// The full name of the type that represents the feed for the specified entity
        /// type and source system; if no feed is registered for the specified combination,
        /// a null reference will be returned.
        /// </returns>
        public string GetFeedTypeFullName(Guid entityTypeId, Guid sourceSystemId)
        {
            return
                this
                .context
                .Feeds
                .AsNoTracking()
                .Where(f =>
                    f.EntityTypeId == entityTypeId &&
                    f.SourceSystemId == sourceSystemId)
                .Select(f => f.FeedTypeFullName)
                .SingleOrDefault();
        }

        /// <summary>
        /// Gets the collection of identifiers that uniquely identify entities of the
        /// specified type in the destination system along with associated mapping
        /// information.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <returns>The collection of destination system identifiers.</returns>
        public IEnumerable<IMappedEntityIdentifier>
            GetMappedEntityIdentifiers(Guid entityTypeId)
        {
            return
                this
                .context
                .MappedEntityIdentifiers
                .AsNoTracking()
                .Where(m => m.EntityTypeId == entityTypeId)
                .ToArray();
        }

        /// <summary>
        /// Gets the serialized entity saved for the specified mapping.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping.</param>
        /// <returns>The serialized entity saved for the specified mapping.</returns>
        public byte[] GetMappingEntity(Guid mappingId)
        {
            return
                this
                .context
                .MappingEntities
                .AsNoTracking()
                .Where(me => me.MappingId == mappingId)
                .Select(me => me.EntityData)
                .Single();
        }

        /// <summary>
        /// Gets the collection of all application parameters.
        /// </summary>
        /// <returns>The collection of all application parameters.</returns>
        public IEnumerable<IParameter> GetParameters()
        {
            return
                this
                .context
                .Parameters
                .AsNoTracking()
                .ToArray();
        }

        /// <summary>
        /// Gets the collection of records specifying which source (external) systems share
        /// identifiers for individual entity type.
        /// </summary>
        /// <returns>
        /// The collection of records specifying which source (external) systems share
        /// identifiers for individual entity type.
        /// </returns>
        public IEnumerable<ISharedIdentifierSourceSystem>
            GetSharedIdentifierSourceSystems()
        {
            return
                this
                .context
                .SharedIdentifierSourceSystems
                .AsNoTracking()
                .ToArray();
        }

        /// <summary>
        /// Gets the dictionary of application parameters that apply to the specified
        /// source system.
        /// </summary>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <returns>
        /// The dictionary of application parameters that apply to the specified source
        /// system.
        /// </returns>
        public IDictionary<string, string> GetSourceSystemParameters(Guid sourceSystemId)
        {
            return
                this
                .context
                .Parameters
                .AsNoTracking()
                .Where(p =>
                    p.DestinationSystemId == null &&
                    p.EntityTypeId == null &&
                    p.SourceSystemId == sourceSystemId)
                .ToDictionary(p => p.Name, p => p.Value);
        }

        /// <summary>
        /// Sets the state of the specified mapping.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping.</param>
        /// <param name="operation">The current operation.</param>
        /// <param name="state">The new state (code) of the mapping.</param>
        public void SetMappingState(Guid mappingId, IOperation operation, int state)
        {
            this.context.SetMappingState(
                mappingId, operation.Id, operation.TimeStamp, state);
        }

        /// <summary>
        /// Sets the value of the specified application parameter.
        /// </summary>
        /// <param name="id">The ID of the parameter.</param>
        /// <param name="value">The new parameter value.</param>
        public void SetParameterValue(Guid id, string value)
        {
            this.context.SetParameterValue(id, value);
        }

        /// <summary>
        /// Updates the last access correlation ID of the specified source system entity
        /// identity.
        /// </summary>
        /// <param name="operationIdIdentity">
        /// The current operation ID/source system entity identity combination.
        /// </param>
        public void UpdateIdentityCorrelationId(IOperationIdIdentity operationIdIdentity)
        {
            this.context.UpdateIdentityCorrelationId(
                operationIdIdentity.OperationId,
                operationIdIdentity.Identity.EntityTypeId,
                operationIdIdentity.Identity.SourceSystemId,
                operationIdIdentity.Identity.SourceSystemEntityId);
        }

        /// <summary>
        /// Updates the serialized source system entity for the specified mapping.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping.</param>
        /// <param name="operation">The current operation.</param>       
        /// <param name="sourceSystemEntity">
        /// The new serialized source system entity.
        /// </param>
        public void UpdateMappingEntity(
            Guid mappingId,
            IOperation operation,
            ISerializedEntity sourceSystemEntity)
        {
            this.context.UpdateMappingEntity(
                mappingId,
                operation.Id,
                operation.TimeStamp,
                sourceSystemEntity.DataHash,
                sourceSystemEntity.Data,
                sourceSystemEntity.Label);
        }
    }
}
