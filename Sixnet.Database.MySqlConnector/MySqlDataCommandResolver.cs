using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Sixnet.Development.Data;
using Sixnet.Development.Data.Command;
using Sixnet.Development.Data.Database;
using Sixnet.Development.Data.Field;
using Sixnet.Development.Entity;
using Sixnet.Development.Queryable;
using Sixnet.Exceptions;

namespace Sixnet.Database.MySqlConnector
{
    /// <summary>
    /// Defines data command resolver for mysql
    /// </summary>
    internal class MySqlDataCommandResolver : BaseDataCommandResolver
    {
        #region Constructor

        public MySqlDataCommandResolver()
        {
            DatabaseServerType = DatabaseServerType.MySQL;
            DefaultFieldFormatter = new MySqlDefaultFieldFormatter();
            ParameterPrefix = "@";
            WrapKeywordFunc = MySqlManager.WrapKeyword;
            RecursiveKeyword = "WITH RECURSIVE";
            DbTypeDefaultValues = new Dictionary<DbType, string>()
            {
                { DbType.Byte, "0" },
                { DbType.SByte, "0" },
                { DbType.Int16, "0" },
                { DbType.UInt16, "0" },
                { DbType.Int32, "0" },
                { DbType.UInt32, "0" },
                { DbType.Int64, "0" },
                { DbType.UInt64, "0" },
                { DbType.Single, "0" },
                { DbType.Double, "0" },
                { DbType.Decimal, "0" },
                { DbType.Boolean, "0" },
                { DbType.String, "''" },
                { DbType.StringFixedLength, "''" },
                { DbType.Guid, "UUID()" },
                { DbType.DateTime, "NOW()" },
                { DbType.DateTime2, "NOW()" },
                { DbType.DateTimeOffset, "NOW()" },
                { DbType.Time, "CURTIME()" }
            };
        }

        #endregion

        #region Get query statement

        /// <summary>
        /// Get query statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <param name="translationResult">Queryable translation result</param>
        /// <param name="location">Queryable location</param>
        /// <returns></returns>
        protected override DatabaseQueryStatement GenerateQueryStatementCore(DataCommandResolveContext context, QueryableTranslationResult translationResult, QueryableLocation location)
        {
            var queryable = translationResult.GetOriginalQueryable();
            string sqlStatement;
            IEnumerable<IDataField> outputFields = null;
            switch (queryable.ExecutionMode)
            {
                case QueryableExecutionMode.Script:
                    sqlStatement = translationResult.GetCondition();
                    break;
                case QueryableExecutionMode.Regular:
                default:
                    // table pet name
                    var tablePetName = context.GetTablePetName(queryable, queryable.GetModelType());
                    //sort
                    var sort = translationResult.GetSort();
                    var hasSort = !string.IsNullOrWhiteSpace(sort);
                    //limit
                    var limit = GetLimitString(queryable.SkipCount, queryable.TakeCount, hasSort);
                    //combine
                    var combine = translationResult.GetCombine();
                    var hasCombine = !string.IsNullOrWhiteSpace(combine);
                    //group
                    var group = translationResult.GetGroup();
                    //having
                    var having = translationResult.GetHavingCondition();
                    //pre script output
                    var targetScript = translationResult.GetPreOutputStatement();

                    if (string.IsNullOrWhiteSpace(targetScript))
                    {
                        //target
                        var targetStatement = GetFromTargetStatement(context, queryable, location, tablePetName);
                        outputFields = targetStatement.OutputFields;
                        //condition
                        var condition = translationResult.GetCondition(ConditionStartKeyword);
                        //join
                        var join = translationResult.GetJoin();
                        //target statement
                        targetScript = $"{targetStatement.Script}{join}{condition}{group}{having}";
                    }
                    else
                    {
                        targetScript = $"{targetScript}{group}{having}";
                        outputFields = translationResult.GetPreOutputFields();
                    }

                    // output fields
                    if (outputFields.IsNullOrEmpty() || !queryable.SelectedFields.IsNullOrEmpty())
                    {
                        outputFields = DataManager.GetQueryableFields(DatabaseServerType, queryable.GetModelType(), queryable, context.IsRootQueryable(queryable));
                    }
                    var outputFieldString = FormatFieldsString(context, queryable, location, FieldLocation.Output, outputFields);
                    //pre script
                    var preScript = GetPreScript(context, location);
                    //statement
                    sqlStatement = $"SELECT{GetDistinctString(queryable)} {outputFieldString} FROM {targetScript}{sort}{limit}";
                    switch (queryable.OutputType)
                    {
                        case QueryableOutputType.Count:
                            sqlStatement = hasCombine
                                ? $"{preScript}SELECT COUNT(1) FROM (({sqlStatement}){combine}){TablePetNameKeyword}{tablePetName}"
                                : $"{preScript}SELECT COUNT(1) FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}";
                            break;
                        case QueryableOutputType.Predicate:
                            sqlStatement = hasCombine
                                ? $"{preScript}SELECT EXISTS(({sqlStatement}){combine})"
                                : $"{preScript}SELECT EXISTS({sqlStatement})";
                            break;
                        default:
                            sqlStatement = hasCombine
                            ? $"{preScript}({sqlStatement}){combine}"
                            : $"{preScript}{sqlStatement}";
                            break;
                    }
                    break;
            }

            //parameters
            var parameters = context.GetParameters();

            //log script
            if (location == QueryableLocation.Top)
            {
                LogScript(sqlStatement, parameters);
            }

            return DatabaseQueryStatement.Create(sqlStatement, parameters, outputFields);
        }

        #endregion

        #region Get insert statement

        /// <summary>
        /// Get insert statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <returns></returns>
        protected override List<DatabaseExecutionStatement> GenerateInsertStatements(DataCommandResolveContext context)
        {
            var command = context.DataCommandExecutionContext.Command;
            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var entityType = dataCommandExecutionContext.Command.GetEntityType();
            var fields = DataManager.GetInsertableFields(DatabaseServerType, entityType);
            var fieldCount = fields.GetCount();
            var insertFields = new List<string>(fieldCount);
            var insertValues = new List<string>(fieldCount);
            EntityField autoIncrementField = null;
            EntityField splitField = null;
            dynamic splitValue = default;

            foreach (var field in fields)
            {
                if (field.InRole(FieldRole.Increment))
                {
                    autoIncrementField ??= field;
                    if (!autoIncrementField.InRole(FieldRole.PrimaryKey) && field.InRole(FieldRole.PrimaryKey)) // get first primary key field
                    {
                        autoIncrementField = field;
                    }
                    continue;
                }
                // fields
                insertFields.Add(WrapKeywordFunc(field.FieldName));
                // values
                var insertValue = command.FieldsAssignment.GetNewValue(field.PropertyName);
                insertValues.Add(FormatInsertValueField(context, command.Queryable, insertValue));

                // split value
                if (field.InRole(FieldRole.SplitValue))
                {
                    splitValue = insertValue;
                    splitField = field;
                }
            }

            ThrowHelper.ThrowNotSupportIf(autoIncrementField != null && splitField != null, $"Not support auto increment field for split table:{entityType.Name}");

            if (splitField != null)
            {
                dataCommandExecutionContext.SetSplitValues(new List<dynamic>(1) { splitValue });
            }
            var tableNames = dataCommandExecutionContext.GetTableNames();
            ThrowHelper.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");
            var statementBuilder = new StringBuilder();
            var incrScripts = new List<string>();
            foreach (var tableName in tableNames)
            {
                statementBuilder.AppendLine($"INSERT INTO {WrapKeywordFunc(tableName)} ({string.Join(",", insertFields)}) VALUES ({string.Join(",", insertValues)});");
            }
            if (autoIncrementField != null)
            {
                var incrField = $"{command.Id}";
                var incrParameter = FormatParameterName(incrField);
                statementBuilder.AppendLine($"SET {incrParameter} = LAST_INSERT_ID();");
                incrScripts.Add($"{incrParameter} {ColumnPetNameKeyword} {incrField}");
            }
            return new List<DatabaseExecutionStatement>()
            {
                new DatabaseExecutionStatement()
                {
                    Script = statementBuilder.ToString(),
                    ScriptType = GetCommandType(command),
                    MustAffectData = command.Options?.MustAffectData ?? false,
                    Parameters = context.GetParameters(),
                    IncrScript = string.Join(",", incrScripts)
                }
            };
        }

        #endregion

        #region Get update statement

        /// <summary>
        /// Get update statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <returns></returns>
        protected override List<DatabaseExecutionStatement> GenerateUpdateStatements(DataCommandResolveContext context)
        {
            var command = context.DataCommandExecutionContext.Command;
            SixnetException.ThrowIf(command?.FieldsAssignment?.NewValues.IsNullOrEmpty() ?? true, "No set update field");

            #region translate

            var translationResult = Translate(context);
            var preScripts = context.GetPreScripts();

            #endregion

            #region script 

            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var entityType = dataCommandExecutionContext.Command.GetEntityType();

            var tableNames = dataCommandExecutionContext.GetTableNames(command);
            ThrowHelper.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");

            var tablePetName = command.Queryable == null ? context.GetNewTablePetName() : context.GetDefaultTablePetName(command.Queryable);
            var newValues = command.FieldsAssignment.NewValues;
            var updateSetArray = new List<string>();
            foreach (var newValueItem in newValues)
            {
                var newValue = newValueItem.Value;
                var propertyName = newValueItem.Key;
                var updateField = DataManager.GetField(dataCommandExecutionContext.Server.ServerType, command.GetEntityType(), PropertyField.Create(propertyName)) as PropertyField;

                ThrowHelper.ThrowFrameworkErrorIf(updateField == null, $"Not found field:{propertyName}");

                var fieldFormattedName = WrapKeywordFunc(updateField.FieldName);
                var newValueExpression = FormatUpdateValueField(context, command, newValue);
                updateSetArray.Add($"{tablePetName}.{fieldFormattedName}={newValueExpression}");
            }

            // parameters
            var parameters = ConvertParameter(command.ScriptParameters) ?? new DataCommandParameters();
            parameters.Union(context.GetParameters());

            // statement
            var scriptType = GetCommandType(command);
            string scriptTemplate;
            if (preScripts.IsNullOrEmpty())
            {
                var condition = translationResult?.GetCondition(ConditionStartKeyword);
                var join = translationResult?.GetJoin();
                scriptTemplate = $"UPDATE {{0}}{TablePetNameKeyword}{tablePetName}{join} SET {string.Join(",", updateSetArray)}{condition};";
                var statementBuilder = new StringBuilder();
                foreach (var tableName in tableNames)
                {
                    statementBuilder.AppendLine(string.Format(scriptTemplate, WrapKeywordFunc(tableName)));
                }
                return new List<DatabaseExecutionStatement>(1)
                {
                    new DatabaseExecutionStatement()
                    {
                        Script = statementBuilder.ToString(),
                        ScriptType = scriptType,
                        MustAffectData = command.Options?.MustAffectData ?? false,
                        Parameters = parameters,
                        HasPreScript = false
                    }
                };
            }
            else
            {
                var queryStatement = GenerateQueryStatementCore(context, translationResult, QueryableLocation.JoinTarget);
                var updateTablePetName = "UTB";
                var joinItems = FormatWrapJoinPrimaryKeys(context, command.Queryable, command.GetEntityType(), tablePetName, tablePetName, updateTablePetName);
                scriptTemplate = $"{FormatPreScript(context)}UPDATE {{0}}{TablePetNameKeyword}{tablePetName} INNER JOIN ({queryStatement.Script}){TablePetNameKeyword}{updateTablePetName} ON {string.Join(" AND ", joinItems)} SET {string.Join(",", updateSetArray)};";
                var statements = new List<DatabaseExecutionStatement>(tableNames.Count);
                foreach (var tableName in tableNames)
                {
                    statements.Add(new DatabaseExecutionStatement()
                    {
                        Script = string.Format(scriptTemplate, WrapKeywordFunc(tableName)),
                        ScriptType = scriptType,
                        MustAffectData = command.Options?.MustAffectData ?? false,
                        Parameters = parameters,
                        HasPreScript = true
                    });
                }
                return statements;
            }

            #endregion
        }

        #endregion

        #region Get delete statement

        /// <summary>
        /// Get delete statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <returns></returns>
        protected override List<DatabaseExecutionStatement> GenerateDeleteStatements(DataCommandResolveContext context)
        {
            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var command = dataCommandExecutionContext.Command;

            #region translate

            var translationResult = Translate(context);
            var preScripts = context.GetPreScripts();

            #endregion

            #region script

            var tablePetName = command.Queryable == null ? context.GetNewTablePetName() : context.GetDefaultTablePetName(command.Queryable);
            var entityType = dataCommandExecutionContext.Command.GetEntityType();
            var tableNames = dataCommandExecutionContext.GetTableNames(command);
            ThrowHelper.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");

            // parameters
            var parameters = ConvertParameter(command.ScriptParameters) ?? new DataCommandParameters();
            parameters.Union(context.GetParameters());

            // statement
            var scriptType = GetCommandType(command);
            string scriptTemplate;
            if (preScripts.IsNullOrEmpty())
            {
                var condition = translationResult?.GetCondition(ConditionStartKeyword);
                var join = translationResult?.GetJoin();
                scriptTemplate = $"DELETE {tablePetName} FROM {{0}}{TablePetNameKeyword}{tablePetName}{join}{condition};";
                var statementBuilder = new StringBuilder();
                foreach (var tableName in tableNames)
                {
                    statementBuilder.AppendLine(string.Format(scriptTemplate, WrapKeywordFunc(tableName)));
                }
                return new List<DatabaseExecutionStatement>(1)
                {
                    new DatabaseExecutionStatement()
                    {
                        Script = statementBuilder.ToString(),
                        ScriptType = scriptType,
                        MustAffectData = command.Options?.MustAffectData ?? false,
                        Parameters = parameters,
                        HasPreScript = false
                    }
                };
            }
            else
            {
                var queryStatement = GenerateQueryStatementCore(context, translationResult, QueryableLocation.JoinTarget);
                var updateTablePetName = "DTB";
                var joinItems = FormatWrapJoinPrimaryKeys(context, command.Queryable, command.GetEntityType(), tablePetName, tablePetName, updateTablePetName);
                scriptTemplate = $"{FormatPreScript(context)}DELETE {tablePetName} FROM {{0}}{TablePetNameKeyword}{tablePetName} INNER JOIN ({queryStatement.Script}){TablePetNameKeyword}{updateTablePetName} ON {string.Join(" AND ", joinItems)};";
                var statements = new List<DatabaseExecutionStatement>(tableNames.Count);
                foreach (var tableName in tableNames)
                {
                    statements.Add(new DatabaseExecutionStatement()
                    {
                        Script = string.Format(scriptTemplate, WrapKeywordFunc(tableName)),
                        ScriptType = scriptType,
                        MustAffectData = command.Options?.MustAffectData ?? false,
                        Parameters = parameters,
                        HasPreScript = true
                    });
                }
                return statements;
            }

            #endregion
        }

        #endregion

        #region Get create table statements

        /// <summary>
        /// Get create table statements
        /// </summary>
        /// <param name="migrationCommand">Migration command</param>
        /// <returns></returns>
        protected override List<DatabaseExecutionStatement> GetCreateTableStatements(DatabaseMigrationCommand migrationCommand)
        {
            var migrationInfo = migrationCommand.MigrationInfo;
            if (migrationInfo?.NewTables.IsNullOrEmpty() ?? true)
            {
                return new List<DatabaseExecutionStatement>(0);
            }
            var newTables = migrationInfo.NewTables;
            var statements = new List<DatabaseExecutionStatement>();
            var options = migrationCommand.MigrationInfo;
            foreach (var newTableInfo in newTables)
            {
                if (newTableInfo?.EntityType == null || (newTableInfo?.TableNames.IsNullOrEmpty() ?? true))
                {
                    continue;
                }
                var entityType = newTableInfo.EntityType;
                var entityConfig = EntityManager.GetEntityConfiguration(entityType);
                ThrowHelper.ThrowFrameworkErrorIf(entityConfig == null, $"Get entity config failed for {entityType.Name}");

                var newFieldScripts = new List<string>();
                var primaryKeyNames = new List<string>();
                foreach (var field in entityConfig.AllFields)
                {
                    var dataField = DataManager.GetField(MySqlManager.CurrentDatabaseServerType, entityType, field.Value);
                    if (dataField is EntityField dataEntityField)
                    {
                        var dataFieldName = MySqlManager.WrapKeyword(dataEntityField.FieldName);
                        newFieldScripts.Add($"{dataFieldName}{GetSqlDataType(dataEntityField, options)}{GetFieldNullable(dataEntityField, options)}{GetSqlDefaultValue(dataEntityField, options)}");
                        if (dataEntityField.InRole(FieldRole.PrimaryKey))
                        {
                            primaryKeyNames.Add($"{dataFieldName}");
                        }
                    }
                }
                foreach (var tableName in newTableInfo.TableNames)
                {
                    var createTableStatement = new DatabaseExecutionStatement()
                    {
                        Script = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(",", newFieldScripts)}{(primaryKeyNames.IsNullOrEmpty() ? "" : $", PRIMARY KEY ({string.Join(",", primaryKeyNames)}) USING BTREE")});"
                    };
                    statements.Add(createTableStatement);

                    // Log script
                    LogExecutionStatement(createTableStatement);
                }
            }
            return statements;
        }

        #endregion

        #region Get limit string

        /// <summary>
        /// Get limit string
        /// </summary>
        /// <param name="offsetNum">Offset num</param>
        /// <param name="takeNum">Take num</param>
        /// <returns></returns>
        protected override string GetLimitString(int offsetNum, int takeNum, bool hasSort)
        {
            if (takeNum < 1)
            {
                return string.Empty;
            }
            if (offsetNum < 0)
            {
                offsetNum = 0;
            }
            return $" LIMIT {offsetNum},{takeNum}";

        }

        #endregion

        #region Get field sql data type

        /// <summary>
        /// Get sql data type
        /// </summary>
        /// <param name="field">Field</param>
        /// <returns></returns>
        protected override string GetSqlDataType(EntityField field, MigrationInfo options)
        {
            ThrowHelper.ThrowArgNullIf(field == null, nameof(field));
            var dbTypeName = "";
            if (!string.IsNullOrWhiteSpace(field.DbType))
            {
                dbTypeName = field.DbType;
            }
            else
            {
                var dbType = field.DataType.GetDbType();
                var length = field.Length;
                var precision = field.Precision;
                var notFixedLength = options.NotFixedLength || field.HasDbFeature(FieldDbFeature.NotFixedLength);
                static int getCharLength(int flength, int defLength) => flength < 1 ? defLength : flength;
                switch (dbType)
                {
                    case DbType.Binary:
                        dbTypeName = $"LONGBLOB";
                        break;
                    case DbType.Boolean:
                        dbTypeName = "BIT";
                        break;
                    case DbType.Byte:
                        dbTypeName = "TINYINT UNSIGNED";
                        break;
                    case DbType.SByte:
                        dbTypeName = "TINYINT";
                        break;
                    case DbType.Date:
                        dbTypeName = "DATE";
                        break;
                    case DbType.DateTime:
                    case DbType.DateTime2:
                    case DbType.DateTimeOffset:
                        dbTypeName = "DATETIME(6)";
                        break;
                    case DbType.Decimal:
                    case DbType.Currency:
                        dbTypeName = $"DECIMAL({(length < 1 ? DefaultDecimalLength : length)}, {(precision < 0 ? DefaultDecimalPrecision : precision)})";
                        break;
                    case DbType.Double:
                        dbTypeName = "DOUBLE";
                        break;
                    case DbType.Guid:
                        dbTypeName = "CHAR(36)";
                        break;
                    case DbType.Int16:
                        dbTypeName = "SMALLINT";
                        break;
                    case DbType.UInt16:
                        dbTypeName = "SMALLINT UNSIGNED";
                        break;
                    case DbType.Int32:
                        dbTypeName = "INT";
                        break;
                    case DbType.UInt32:
                        dbTypeName = "INT UNSIGNED";
                        break;
                    case DbType.Int64:
                        dbTypeName = "BIGINT";
                        break;
                    case DbType.UInt64:
                        dbTypeName = "BIGINT UNSIGNED";
                        break;
                    case DbType.Single:
                        dbTypeName = "FLOAT";
                        break;
                    case DbType.AnsiString:
                    case DbType.AnsiStringFixedLength:
                    case DbType.String:
                    case DbType.StringFixedLength:
                        length = getCharLength(length, DefaultCharLength);
                        if (length > 16000)
                        {
                            dbTypeName = "TEXT";
                        }
                        else if(notFixedLength || length >200)
                        {
                            dbTypeName = $"VARCHAR({length})";
                        }
                        else
                        {
                            dbTypeName = $"CHAR({length})";
                        }
                        break;
                    case DbType.Time:
                        dbTypeName = $"TIME({(length < 1 ? 6 : length)})";
                        break;
                    default:
                        throw new NotSupportedException(dbType.ToString());
                }
            }
            return $" {dbTypeName}";
        }

        #endregion
    }
}
