using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace Synnduit.Persistence.SqlServer
{
    /// <summary>
    /// The context for the Synnduit database.
    /// </summary>
    internal class DatabaseContext : DbContext
    {
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="connectionString">The connection string to use.</param>
        public DatabaseContext(string connectionString)
        {
            this.connectionString = connectionString;
            this.Database.SetCommandTimeout(720);
        }

        /// <summary>
        /// Overriden to set up the SQL Server database provider.
        /// </summary>
        /// <param name="optionsBuilder">The options builder instance.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(this.connectionString);
        }

        /// <summary>
        /// Overriden to define those aspects of the data model that cannot be defined via
        /// attributes.
        /// </summary>
        /// <param name="modelBuilder">The model builder instance.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Feed>()
                .HasKey(nameof(Feed.EntityTypeId), nameof(Feed.SourceSystemId));
            modelBuilder
                .Entity<SharedIdentifierSourceSystem>()
                .HasKey(
                    nameof(SharedIdentifierSourceSystem.SourceSystemId),
                    nameof(SharedIdentifierSourceSystem.EntityTypeId),
                    nameof(SharedIdentifierSourceSystem.SharedIdentifierSourceSystemId));
        }

        /// <summary>
        /// Gets or sets the <see cref="EntityMapping" /> data set.
        /// </summary>
        public virtual DbSet<EntityMapping> EntityMappings { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EntityType" /> data set.
        /// </summary>
        public virtual DbSet<EntityType> EntityTypes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ExternalSystem" /> data set.
        /// </summary>
        public virtual DbSet<ExternalSystem> ExternalSystems { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Feed" /> data set.
        /// </summary>
        public virtual DbSet<Feed> Feeds { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MappedEntityIdentifier" /> data set.
        /// </summary>
        public virtual DbSet<MappedEntityIdentifier> MappedEntityIdentifiers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MappingEntity" /> data set.
        /// </summary>
        public virtual DbSet<MappingEntity> MappingEntities { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Parameter" /> data set.
        /// </summary>
        public virtual DbSet<Parameter> Parameters { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SharedIdentifierSourceSystem" /> data set.
        /// </summary>
        public virtual DbSet<SharedIdentifierSourceSystem>
            SharedIdentifierSourceSystems { get; set; }

        /// <summary>
        /// Clears all shared source system identifier (group) records for the specified
        /// entity type.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        public void ClearSharedSourceSystemIdentifiers(Guid entityTypeId)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[ClearSharedSourceSystemIdentifiers]",
                this.CreateParameter(
                    "@entityTypeId", SqlDbType.UniqueIdentifier, entityTypeId));
        }

        /// <summary>
        /// Creates a new entity deletion.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="destinationSystemEntityId">
        /// The ID that uniquely identifies the entity in the destination system.
        /// </param>
        /// <param name="outcome">The outcome of the deletion transaction (code).</param>
        public void CreateEntityDeletion(
            Guid operationId,
            DateTimeOffset timeStamp,
            Guid entityTypeId,
            string destinationSystemEntityId,
            int outcome)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateEntityDeletion]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateDestinationSystemEntityId(destinationSystemEntityId),
                this.CreateOutcomeParameter(outcome));
        }

        /// <summary>
        /// Creates a new identity entity transaction.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="outcome">The outcome of the transaction (code).</param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="sourceSystemEntityId">
        /// The ID that uniquely identifies the entity in the source system.
        /// </param>
        public void CreateIdentityEntityTransaction(
            Guid operationId,
            DateTimeOffset timeStamp,
            int outcome,
            Guid entityTypeId,
            Guid sourceSystemId,
            string sourceSystemEntityId)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateIdentityEntityTransaction]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateOutcomeParameter(outcome),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateSourceSystemEntityIdParameter(sourceSystemEntityId));
        }

        /// <summary>
        /// Creates a new message associated with the specified source system entity
        /// identity (always) and operation (if it exists).
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="sourceSystemEntityId">
        /// The ID that uniquely identifies the entity in the source system.
        /// </param>
        /// <param name="type">The message type (code).</param>
        /// <param name="textHash">The hash of the text of the message.</param>
        /// <param name="text">The text of the message.</param>
        public void CreateIdentityOperationMessage(
            Guid operationId,
            DateTimeOffset timeStamp,
            Guid entityTypeId,
            Guid sourceSystemId,
            string sourceSystemEntityId,
            int type,
            string textHash,
            string text)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateIdentityOperationMessage]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateSourceSystemEntityIdParameter(sourceSystemEntityId),
                this.CreateTypeParameter(type),
                this.CreateTextHashParameter(textHash),
                this.CreateTextParameter(text));
        }

        /// <summary>
        /// Creates a new serialized source system entity associated with the specified
        /// source system entity identity (always) and operation (if it exists).
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="sourceSystemEntityId">
        /// The ID that uniquely identifies the entity in the source system.
        /// </param>
        /// <param name="entityDataHash">The (serialized) entity data hash.</param>
        /// <param name="entityData">
        /// The (serialized) entity data (i.e., the serialized entity).
        /// </param>
        /// <param name="entityLabel">
        /// The (serialized) entity label (i.e., name or short description).
        /// </param>
        public void CreateIdentityOperationSourceSystemEntity(
            Guid operationId,
            DateTimeOffset timeStamp,
            Guid entityTypeId,
            Guid sourceSystemId,
            string sourceSystemEntityId,
            string entityDataHash,
            byte[] entityData,
            string entityLabel)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateIdentityOperationSourceSystemEntity]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateSourceSystemEntityIdParameter(sourceSystemEntityId),
                this.CreateEntityDataHashParameter(entityDataHash),
                this.CreateEntityDataParameter(entityData),
                this.CreateEntityLabelParameter(entityLabel));
        }

        /// <summary>
        /// Creates a new entity source/destination system identifier mapping.
        /// </summary>
        /// <param name="id">The ID of the mapping.</param>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityDataHash">
        /// The (serialized) source system entity data hash.
        /// </param>
        /// <param name="entityData">
        /// The (serialized) source system entity data (i.e., the serialized entity).
        /// </param>
        /// <param name="entityLabel">
        /// The (serialized) source system entity label (i.e., name or short description).
        /// </param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="sourceSystemEntityId">
        /// The ID that uniquely identifies the entity in the source system.
        /// </param>
        /// <param name="destinationSystemEntityId">
        /// The ID that uniquely identifies the entity in the destination system.
        /// </param>
        /// <param name="origin">The mapping's origin (code).</param>
        /// <param name="state">The mapping's state (code).</param>
        public void CreateMapping(
            Guid id,
            Guid operationId,
            DateTimeOffset timeStamp,
            string entityDataHash,
            byte[] entityData,
            string entityLabel,
            Guid entityTypeId,
            Guid sourceSystemId,
            string sourceSystemEntityId,
            string destinationSystemEntityId,
            int origin,
            int state)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateMapping]",
                this.CreateIdParameter(id),
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityDataHashParameter(entityDataHash),
                this.CreateEntityDataParameter(entityData),
                this.CreateEntityLabelParameter(entityLabel),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateSourceSystemEntityIdParameter(sourceSystemEntityId),
                this.CreateDestinationSystemEntityId(destinationSystemEntityId),
                this.CreateOriginParameter(origin),
                this.CreateStateParameter(state));
        }

        /// <summary>
        /// Creates a new mapping entity transaction.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="outcome">The outcome of the transaction (code).</param>
        /// <param name="mappingId">
        /// The ID of the entity source/destination system identifier mapping that the
        /// transaction is associated with.
        /// </param>
        public void CreateMappingEntityTransaction(
            Guid operationId,
            DateTimeOffset timeStamp,
            int outcome,
            Guid mappingId)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateMappingEntityTransaction]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateOutcomeParameter(outcome),
                this.CreateMappingIdParameter(mappingId));
        }

        /// <summary>
        /// Creates a new serialized destination system entity associated with an
        /// operation.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityDataHash">The (serialized) entity data hash.</param>
        /// <param name="entityData">
        /// The (serialized) entity data (i.e., the serialized entity).
        /// </param>
        /// <param name="entityLabel">
        /// The (serialized) entity label (i.e., name or short description).
        /// </param>
        public void CreateOperationDestinationSystemEntity(
            Guid operationId,
            DateTimeOffset timeStamp,
            string entityDataHash,
            byte[] entityData,
            string entityLabel)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOperationDestinationSystemEntity]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityDataHashParameter(entityDataHash),
                this.CreateEntityDataParameter(entityData),
                this.CreateEntityLabelParameter(entityLabel));
        }

        /// <summary>
        /// Creates a new operation message.
        /// </summary>
        /// <param name="operationId">The ID of the operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="type">The message type (code).</param>
        /// <param name="textHash">The hash of the text of the message.</param>
        /// <param name="text">The text of the message.</param>
        public void CreateOperationMessage(
            Guid operationId,
            DateTimeOffset timeStamp,
            int type,
            string textHash,
            string text)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOperationMessage]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateTypeParameter(type),
                this.CreateTextHashParameter(textHash),
                this.CreateTextParameter(text));
        }

        /// <summary>
        /// Creates a new serialized source system entity associated with an operation.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityDataHash">The (serialized) entity data hash.</param>
        /// <param name="entityData">
        /// The (serialized) entity data (i.e., the serialized entity).
        /// </param>
        /// <param name="entityLabel">
        /// The (serialized) entity label (i.e., name or short description).
        /// </param>
        public void CreateOperationSourceSystemEntity(
            Guid operationId,
            DateTimeOffset timeStamp,
            string entityDataHash,
            byte[] entityData,
            string entityLabel)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOperationSourceSystemEntity]",
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityDataHashParameter(entityDataHash),
                this.CreateEntityDataParameter(entityData),
                this.CreateEntityLabelParameter(entityLabel));
        }

        /// <summary>
        /// Creates or updates the specified entity type.
        /// </summary>
        /// <param name="id">The ID of the entity type.</param>
        /// <param name="destinationSystemId">
        /// The ID of the entity type's parent destination (external) system.
        /// </param>
        /// <param name="name">The name of the entity type.</param>
        /// <param name="typeName">
        /// The assembly qualified name of the type that represents the entity.
        /// </param>
        /// <param name="sinkTypeFullName">
        /// The full name of the type that represents the entity type's sink.
        /// </param>
        /// <param name="cacheFeedTypeFullName">
        /// The full name of the type that represents the entity type's cache feed;
        /// optional, may be null.
        /// </param>
        /// <param name="isMutable">
        /// A value indicating whether instances of the entity type are mutable; i.e.,
        /// whether or not they may change between runs.
        /// </param>
        /// <param name="isDuplicable">
        /// a value indicating whether source system instances of the entity type may be
        /// duplicates (i.e., represent the same destination system entity); in other
        /// words, this value indicates whether or not source system entity instances
        /// should be deduplicated.
        /// </param>
        public void CreateOrUpdateEntityType(
            Guid id,
            Guid destinationSystemId,
            string name,
            string typeName,
            string sinkTypeFullName,
            string cacheFeedTypeFullName,
            bool isMutable,
            bool isDuplicable)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOrUpdateEntityType]",
                this.CreateIdParameter(id),
                this.CreateDestinationSystemIdParameter(destinationSystemId),
                this.CreateNameParameter(name),
                this.CreateTypeNameParameter(typeName),
                this.CreateSinkTypeFullNameParameter(sinkTypeFullName),
                this.CreateCacheFeedTypeFullName(cacheFeedTypeFullName),
                this.CreateIsMutableParameter(isMutable),
                this.CreateIsDuplicableParameter(isDuplicable));
        }

        /// <summary>
        /// Creates or updates the specified external system.
        /// </summary>
        /// <param name="id">The ID of the external system.</param>
        /// <param name="name">The name of the external system.</param>
        public void CreateOrUpdateExternalSystem(Guid id, string name)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOrUpdateExternalSystem]",
                this.CreateIdParameter(id),
                this.CreateNameParameter(name));
        }

        /// <summary>
        /// Creates or updates the specified feed.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="feedTypeFullName">
        /// The full name of the type that represents the feed.
        /// </param>
        public void CreateOrUpdateFeed(
            Guid entityTypeId, Guid sourceSystemId, string feedTypeFullName)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateOrUpdateFeed]",
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateFeedTypeFullNameParameter(feedTypeFullName));
        }

        /// <summary>
        /// Creates the specified application parameter.
        /// </summary>
        /// <param name="id">The ID of the parameter.</param>
        /// <param name="destinationSystemId">
        /// The ID of the destination (external) system that the parameter is associated
        /// with.
        /// </param>
        /// <param name="entityTypeId">
        /// The ID of the entity type that the parameter is associated with.
        /// </param>
        /// <param name="sourceSystemId">
        /// The ID of the source (external) system that the parameter is associated with.
        /// </param>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The parameter value.</param>
        public void CreateParameter(
            Guid id,
            Guid? destinationSystemId,
            Guid? entityTypeId,
            Guid? sourceSystemId,
            string name,
            string value)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateParameter]",
                this.CreateIdParameter(id),
                this.CreateDestinationSystemIdParameter(destinationSystemId),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateNameParameter(name),
                this.CreateValueParameter(value));
        }

        /// <summary>
        /// Creates a new shared source system identifier record.
        /// </summary>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="groupNumber">
        /// The group number; source systems belonging to the same group (number) share
        /// identifiers for the entity type.
        /// </param>
        public void CreateSharedSourceSystemIdentifier(
            Guid entityTypeId,
            Guid sourceSystemId,
            int groupNumber)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateSharedSourceSystemIdentifier]",
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateGroupNumberParameter(groupNumber));
        }

        /// <summary>
        /// Creates a new value change record.
        /// </summary>
        /// <param name="id">The ID of the value change record.</param>
        /// <param name="mappingEntityTransactionId">
        /// The ID of the mapping entity transaction that the value change is associated
        /// with.
        /// </param>
        /// <param name="valueName">The name of the value that has changed.</param>
        /// <param name="previousValue">The previous value.</param>
        /// <param name="newValue">The new value.</param>
        public void CreateValueChange(
            Guid id,
            Guid mappingEntityTransactionId,
            string valueName,
            string previousValue,
            string newValue)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[CreateValueChange]",
                this.CreateIdParameter(id),
                this.CreateMappingEntityTransactionIdParameter(mappingEntityTransactionId),
                this.CreateValueNameParameter(valueName),
                this.CreatePreviousValueParameter(previousValue),
                this.CreateNewValueParameter(newValue));
        }

        /// <summary>
        /// Deletes the specified application parameter.
        /// </summary>
        /// <param name="id">The ID of the parameter.</param>
        public void DeleteParameter(Guid id)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[DeleteParameter]",
                this.CreateIdParameter(id));
        }

        /// <summary>
        /// Sets the state of the specified mapping.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping.</param>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The curren time stamp.</param>
        /// <param name="state">The new state (code) of the mapping.</param>
        public void SetMappingState(
            Guid mappingId, Guid operationId, DateTimeOffset timeStamp, int state)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[SetMappingState]",
                this.CreateMappingIdParameter(mappingId),
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateStateParameter(state));
        }

        /// <summary>
        /// Sets the value of the specified application parameter.
        /// </summary>
        /// <param name="id">The ID of the parameter.</param>
        /// <param name="value">The new parameter value.</param>
        public void SetParameterValue(Guid id, string value)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[SetParameterValue]",
                this.CreateIdParameter(id),
                this.CreateValueParameter(value));
        }

        /// <summary>
        /// Updates the last access correlation ID of the specified source system entity
        /// identity.
        /// </summary>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="entityTypeId">The ID of the entity type.</param>
        /// <param name="sourceSystemId">The ID of the source (external) system.</param>
        /// <param name="sourceSystemEntityId">
        /// The ID that uniquely identifies the entity in the source system.
        /// </param>
        public void UpdateIdentityCorrelationId(
            Guid operationId,
            Guid entityTypeId,
            Guid sourceSystemId,
            string sourceSystemEntityId)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[UpdateIdentityCorrelationId]",
                this.CreateOperationIdParameter(operationId),
                this.CreateEntityTypeIdParameter(entityTypeId),
                this.CreateSourceSystemIdParameter(sourceSystemId),
                this.CreateSourceSystemEntityIdParameter(sourceSystemEntityId));
        }

        /// <summary>
        /// Updates the serialized source system entity for the specified mapping.
        /// </summary>
        /// <param name="mappingId">The ID of the mapping.</param>
        /// <param name="operationId">The ID of the current operation.</param>
        /// <param name="timeStamp">The current time stamp.</param>
        /// <param name="entityDataHash">
        /// The new (serialized) source system entity data hash.
        /// </param>
        /// <param name="entityData">
        /// The new (serialized) source system entity data (i.e., the serialized entity).
        /// </param>
        /// <param name="entityLabel">
        /// The new (serialized) source system entity label (i.e., name or short
        /// description).
        /// </param>
        public void UpdateMappingEntity(
            Guid mappingId,
            Guid operationId,
            DateTimeOffset timeStamp,
            string entityDataHash,
            byte[] entityData,
            string entityLabel)
        {
            this.ExecuteStoredProcedure(
                "[dbo].[UpdateMappingEntity]",
                this.CreateMappingIdParameter(mappingId),
                this.CreateOperationIdParameter(operationId),
                this.CreateTimeStampParameter(timeStamp),
                this.CreateEntityDataHashParameter(entityDataHash),
                this.CreateEntityDataParameter(entityData),
                this.CreateEntityLabelParameter(entityLabel));
        }

        private void ExecuteStoredProcedure(
            string storedProcedureName,
            params SqlParameter[] parameters)
        {
            // generate the SQL statement
            var sql = new StringBuilder();
            sql.Append($"EXECUTE {storedProcedureName} ");
            for (int i = 0; i < parameters.Length; i++)
            {
                if (i != 0)
                {
                    sql.Append(", ");
                }
                sql.Append(parameters[i].ParameterName);
            }

            // execute the SQL statement
            this.Database.ExecuteSqlRaw(sql.ToString().Trim(), parameters);
        }

        private SqlParameter CreateCacheFeedTypeFullName(string cacheFeedTypeFullName)
        {
            return this.CreateParameter(
                "@cacheFeedTypeFullName", SqlDbType.VarChar, cacheFeedTypeFullName);
        }

        private SqlParameter CreateDestinationSystemEntityId(
            string destinationSystemEntityId)
        {
            return this.CreateParameter(
                "@destinationSystemEntityId",
                SqlDbType.NVarChar,
                destinationSystemEntityId);
        }

        private SqlParameter CreateDestinationSystemIdParameter(Guid? destinationSystemId)
        {
            return this.CreateParameter(
                "@destinationSystemId", SqlDbType.UniqueIdentifier, destinationSystemId);
        }

        private SqlParameter CreateEntityDataParameter(byte[] entityData)
        {
            return this.CreateParameter("@entityData", SqlDbType.VarBinary, entityData);
        }

        private SqlParameter CreateEntityDataHashParameter(string entityDataHash)
        {
            return this.CreateParameter(
                "@entityDataHash", SqlDbType.VarChar, entityDataHash);
        }

        private SqlParameter CreateEntityLabelParameter(string entityLabel)
        {
            return this.CreateParameter("@entityLabel", SqlDbType.NVarChar, entityLabel);
        }

        private SqlParameter CreateEntityTypeIdParameter(Guid? entityTypeId)
        {
            return this.CreateParameter(
                "@entityTypeId", SqlDbType.UniqueIdentifier, entityTypeId);
        }

        private SqlParameter CreateFeedTypeFullNameParameter(string feedTypeFullName)
        {
            return this.CreateParameter(
                "@feedTypeFullName", SqlDbType.VarChar, feedTypeFullName);
        }

        private SqlParameter CreateGroupNumberParameter(int groupNumber)
        {
            return this.CreateParameter("@groupNumber", SqlDbType.Int, groupNumber);
        }

        private SqlParameter CreateIdParameter(Guid id)
        {
            return this.CreateParameter("@id", SqlDbType.UniqueIdentifier, id);
        }

        private SqlParameter CreateIsDuplicableParameter(bool isDuplicable)
        {
            return this.CreateParameter("@isDuplicable", SqlDbType.Bit, isDuplicable);
        }

        private SqlParameter CreateIsMutableParameter(bool isMutable)
        {
            return this.CreateParameter("@isMutable", SqlDbType.Bit, isMutable);
        }

        private SqlParameter CreateMappingEntityTransactionIdParameter(
            Guid mappingEntityTransactionId)
        {
            return this.CreateParameter(
                "@mappingEntityTransactionId",
                SqlDbType.UniqueIdentifier,
                mappingEntityTransactionId);
        }

        private SqlParameter CreateMappingIdParameter(Guid mappingId)
        {
            return this.CreateParameter(
                "@mappingId", SqlDbType.UniqueIdentifier, mappingId);
        }

        private SqlParameter CreateNameParameter(string name)
        {
            return this.CreateParameter("@name", SqlDbType.VarChar, name);
        }

        private SqlParameter CreateNewValueParameter(string newValue)
        {
            return this.CreateParameter("@newValue", SqlDbType.NVarChar, newValue);
        }

        private SqlParameter CreateOperationIdParameter(Guid operationId)
        {
            return this.CreateParameter(
                "@operationId", SqlDbType.UniqueIdentifier, operationId);
        }

        private SqlParameter CreateOriginParameter(int origin)
        {
            return this.CreateParameter("@origin", SqlDbType.Int, origin);
        }

        private SqlParameter CreateOutcomeParameter(int outcome)
        {
            return this.CreateParameter("@outcome", SqlDbType.Int, outcome);
        }

        private SqlParameter CreatePreviousValueParameter(string previousValue)
        {
            return this.CreateParameter(
                "@previousValue", SqlDbType.NVarChar, previousValue);
        }

        private SqlParameter CreateSinkTypeFullNameParameter(string sinkTypeFullName)
        {
            return this.CreateParameter(
                "@sinkTypeFullName", SqlDbType.VarChar, sinkTypeFullName);
        }

        private SqlParameter
            CreateSourceSystemEntityIdParameter(string sourceSystemEntityId)
        {
            return this.CreateParameter(
                "@sourceSystemEntityId", SqlDbType.NVarChar, sourceSystemEntityId);
        }

        private SqlParameter CreateSourceSystemIdParameter(Guid? sourceSystemId)
        {
            return this.CreateParameter(
                "@sourceSystemId", SqlDbType.UniqueIdentifier, sourceSystemId);
        }

        private SqlParameter CreateStateParameter(int state)
        {
            return this.CreateParameter("@state", SqlDbType.Int, state);
        }

        private SqlParameter CreateTextParameter(string text)
        {
            return this.CreateParameter("@text", SqlDbType.NVarChar, text);
        }

        private SqlParameter CreateTextHashParameter(string textHash)
        {
            return this.CreateParameter("@textHash", SqlDbType.VarChar, textHash);
        }

        private SqlParameter CreateTimeStampParameter(DateTimeOffset timeStamp)
        {
            return this.CreateParameter("@timeStamp", SqlDbType.DateTimeOffset, timeStamp);
        }

        private SqlParameter CreateTypeNameParameter(string typeName)
        {
            return this.CreateParameter("@typeName", SqlDbType.VarChar, typeName);
        }

        private SqlParameter CreateTypeParameter(int type)
        {
            return this.CreateParameter("@type", SqlDbType.Int, type);
        }

        private SqlParameter CreateValueParameter(string value)
        {
            return this.CreateParameter("@value", SqlDbType.VarChar, value);
        }

        private SqlParameter CreateValueNameParameter(string valueName)
        {
            return this.CreateParameter("@valueName", SqlDbType.VarChar, valueName);
        }

        private SqlParameter CreateParameter(
            string parameterName, SqlDbType dbType, object value)
        {
            var parameter = new SqlParameter(parameterName, dbType)
            {
                Value = value ?? DBNull.Value
            };
            return parameter;
        }
    }
}
