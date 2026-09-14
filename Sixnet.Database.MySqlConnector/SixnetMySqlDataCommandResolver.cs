using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Sixnet.Development.Data;
using Sixnet.Development.Data.Command;
using Sixnet.Development.Data.Dapper;
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
    internal partial class SixnetMySqlDataCommandResolver : SixnetBaseDataCommandResolver
    {
        #region Constructor

        public SixnetMySqlDataCommandResolver()
        {
            DatabaseType = SixnetDatabaseType.MySQL;
            DefaultFieldFormatter = new SixnetMySqlDefaultFieldFormatter();
            ParameterPrefix = "@";
            KeywordPrefix = "`";
            KeywordSuffix = "`";
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

        #region Data access

        #region Get query statement

        /// <summary>
        /// Get query statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <param name="translationResult">Queryable translation result</param>
        /// <param name="location">Queryable location</param>
        /// <returns></returns>
        protected override SixnetQueryDatabaseStatement GenerateQueryStatementCore(SixnetDataCommandResolveContext context, SixnetQueryableTranslationResult translationResult, SixnetQueryableLocation location)
        {
            var queryable = translationResult.GetOriginalQueryable();
            string sqlStatement;
            IEnumerable<ISixnetField> outputFields = null;
            switch (queryable.Info.ExecutionMode)
            {
                case SixnetQueryableExecutionMode.Script:
                    sqlStatement = translationResult.GetCondition();
                    break;
                case SixnetQueryableExecutionMode.Regular:
                default:
                    // table pet name
                    var tablePetName = context.GetTablePetName(queryable, queryable.GetModelType());
                    //sort
                    var sort = translationResult.GetSort();
                    var hasSort = !string.IsNullOrWhiteSpace(sort);
                    //limit
                    var limit = GetLimitString(queryable.Info.SkipCount, queryable.Info.TakeCount, hasSort);
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
                    if (outputFields.IsNullOrEmpty() || !queryable.Info.SelectedFields.IsNullOrEmpty())
                    {
                        outputFields = SixnetDataManager.GetQueryableFields(DatabaseType, queryable.GetModelType(), queryable, context.IsRootQueryable(queryable));
                    }
                    var outputFieldString = FormatFieldsString(context, queryable, location, SixnetFieldLocation.Output, outputFields);
                    //pre script
                    var preScript = GetPreScript(context, location);
                    //statement
                    sqlStatement = $"SELECT{GetDistinctString(queryable)} {outputFieldString} FROM {targetScript}{sort}{limit}";
                    switch (queryable.Info.OutputType)
                    {
                        case SixnetQueryableOutputType.Count:
                            sqlStatement = hasCombine
                                ? hasSort
                                    ? $"{preScript}SELECT COUNT(1) FROM ((SELECT {tablePetName}.* FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}){combine}){TablePetNameKeyword}{tablePetName}"
                                    : $"{preScript}SELECT COUNT(1) FROM (({sqlStatement}){combine}){TablePetNameKeyword}{tablePetName}"
                                : $"{preScript}SELECT COUNT(1) FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}";
                            break;
                        case SixnetQueryableOutputType.Predicate:
                            sqlStatement = hasCombine
                                ? hasSort
                                    ? $"{preScript}SELECT 1 WHERE EXISTS((SELECT {tablePetName}.* FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}){combine})"
                                    : $"{preScript}SELECT 1 WHERE EXISTS(({sqlStatement}){combine})"
                                : $"{preScript}SELECT 1 WHERE EXISTS({sqlStatement})";
                            break;
                        case SixnetQueryableOutputType.TempTable:
                            sqlStatement = hasCombine
                            ? hasSort
                                ? $"(SELECT {tablePetName}.* FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}){combine}"
                                : $"({sqlStatement}){combine}"
                            : $"{sqlStatement}";
                            sqlStatement = $"{preScript}(CREATE TEMPORARY TABLE {queryable.Info.TempTableName} AS SELECT * FROM ({sqlStatement}))";
                            break;
                        default:
                            sqlStatement = hasCombine
                            ? hasSort
                                ? $"{preScript}(SELECT {tablePetName}.* FROM ({sqlStatement}){TablePetNameKeyword}{tablePetName}){combine}"
                                : $"{preScript}({sqlStatement}){combine}"
                            : $"{preScript}{sqlStatement}";
                            break;
                    }
                    break;
            }

            //parameters
            var parameters = context.GetParameters();

            return SixnetQueryDatabaseStatement.Create(DatabaseType, location, sqlStatement, parameters, outputFields);
        }

        #endregion

        #region Get insert statement

        /// <summary>
        /// Get insert statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <returns></returns>
        protected override List<SixnetExecutionDatabaseStatement> GenerateInsertStatements(SixnetDataCommandResolveContext context)
        {
            var command = context.DataCommandExecutionContext.Command;
            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var entityType = dataCommandExecutionContext.Command.GetEntityType();
            var fields = SixnetDataManager.GetInsertableFields(DatabaseType, entityType);
            var fieldCount = fields.GetCount();
            var insertFields = new List<string>(fieldCount);
            var insertValues = new List<string>(fieldCount);
            SixnetDataField autoIncrementField = null;
            SixnetDataField splitField = null;
            dynamic splitValue = default;

            foreach (var field in fields)
            {
                if (field.InRole(SixnetFieldRole.Increment))
                {
                    autoIncrementField ??= field;
                    if (!autoIncrementField.InRole(SixnetFieldRole.PrimaryKey) && field.InRole(SixnetFieldRole.PrimaryKey)) // get first primary key field
                    {
                        autoIncrementField = field;
                    }
                    if (!SixnetDataManager.AllowInsertIncrementField(context.DataCommandExecutionContext))
                    {
                        continue;
                    }
                }
                // fields
                insertFields.Add(FormatAndWrapObjectName(field.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column));
                // values
                var insertValue = command.FieldsAssignment.GetNewValue(field.PropertyName);
                insertValues.Add(FormatInsertValueField(context, command.Queryable, insertValue));

                // split value
                if (field.InRole(SixnetFieldRole.SplitValue))
                {
                    splitValue = insertValue;
                    splitField = field;
                }
            }

            SixnetDirectThrower.ThrowNotSupportIf(autoIncrementField != null && splitField != null, $"Not support auto increment field for split table:{entityType.Name}");

            if (splitField != null)
            {
                dataCommandExecutionContext.SetSplitValues(new List<dynamic>(1) { splitValue });
            }
            var tableNames = dataCommandExecutionContext.GetTableNames();
            SixnetDirectThrower.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");
            var statementBuilder = new StringBuilder();
            var incrScripts = new List<string>();
            foreach (var tableName in tableNames)
            {
                statementBuilder.AppendLine($"INSERT INTO {FormatAndWrapObjectName(tableName)} ({string.Join(",", insertFields)}) VALUES ({string.Join(",", insertValues)});");
            }
            if (autoIncrementField != null)
            {
                var incrField = $"{command.Id}";
                var incrParameter = FormatParameterName(incrField);
                statementBuilder.AppendLine($"SET {incrParameter} = LAST_INSERT_ID();");
                incrScripts.Add($"{incrParameter} {ColumnPetNameKeyword} {incrField}");
            }
            return new List<SixnetExecutionDatabaseStatement>()
            {
                SixnetExecutionDatabaseStatement.Create(DatabaseType, data =>
                {
                    data.Script = statementBuilder.ToString();
                    data.ScriptType = GetCommandType(command);
                    data.MustAffectData = command.Options?.MustAffectData ?? false;
                    data.Parameters = context.GetParameters();
                    data.IncrScript = string.Join(",", incrScripts);
                })
            };
        }

        #endregion

        #region Get update statement

        /// <summary>
        /// Get update statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <returns></returns>
        protected override List<SixnetExecutionDatabaseStatement> GenerateUpdateStatements(SixnetDataCommandResolveContext context)
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
            SixnetDirectThrower.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");

            var tablePetName = command.Queryable == null ? context.GetNewTablePetName() : context.GetDefaultTablePetName(command.Queryable);
            var newValues = command.FieldsAssignment.NewValues;
            var updateSetArray = new List<string>();
            foreach (var newValueItem in newValues)
            {
                var newValue = newValueItem.Value;
                var propertyName = newValueItem.Key;
                var updateField = SixnetDataManager.GetField(dataCommandExecutionContext.Server.DatabaseType, command.GetEntityType(), SixnetDataField.Create(propertyName)) as SixnetDataField;

                SixnetDirectThrower.ThrowSixnetExceptionIf(updateField == null, $"Not found field:{propertyName}");

                var fieldFormattedName = FormatAndWrapObjectName(updateField.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column);
                var newValueExpression = FormatUpdateValueField(context, command, newValue);
                updateSetArray.Add($"{tablePetName}.{fieldFormattedName}={newValueExpression}");
            }

            // parameters
            var parameters = ConvertParameter(command.ScriptParameters) ?? new SixnetDataCommandParameters();
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
                    statementBuilder.AppendLine(string.Format(scriptTemplate, FormatAndWrapObjectName(tableName)));
                }
                return new List<SixnetExecutionDatabaseStatement>(1)
                {
                    SixnetExecutionDatabaseStatement.Create(DatabaseType, data =>
                    {
                        data.Script = statementBuilder.ToString();
                        data.ScriptType = scriptType;
                        data.MustAffectData = command.Options?.MustAffectData ?? false;
                        data.Parameters = parameters;
                        data.HasPreScript = false;
                    })
                };
            }
            else
            {
                var queryStatement = GenerateQueryStatementCore(context, translationResult, SixnetQueryableLocation.JoinTarget);
                var updateTablePetName = "UTB";
                var joinItems = FormatWrapJoinPrimaryKeys(context, command.Queryable, command.GetEntityType(), tablePetName, tablePetName, updateTablePetName);
                scriptTemplate = $"{FormatPreScript(context)}UPDATE {{0}}{TablePetNameKeyword}{tablePetName} INNER JOIN ({queryStatement.Script}){TablePetNameKeyword}{updateTablePetName} ON {string.Join(" AND ", joinItems)} SET {string.Join(",", updateSetArray)};";
                var statements = new List<SixnetExecutionDatabaseStatement>(tableNames.Count);
                foreach (var tableName in tableNames)
                {
                    statements.Add(SixnetExecutionDatabaseStatement.Create(DatabaseType, data =>
                    {
                        data.Script = string.Format(scriptTemplate, FormatAndWrapObjectName(tableName));
                        data.ScriptType = scriptType;
                        data.MustAffectData = command.Options?.MustAffectData ?? false;
                        data.Parameters = parameters;
                        data.HasPreScript = true;
                    }));
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
        protected override List<SixnetExecutionDatabaseStatement> GenerateDeleteStatements(SixnetDataCommandResolveContext context)
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
            SixnetDirectThrower.ThrowInvalidOperationIf(tableNames.IsNullOrEmpty(), $"Get table name failed for {entityType.Name}");

            // parameters
            var parameters = ConvertParameter(command.ScriptParameters) ?? new SixnetDataCommandParameters();
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
                    statementBuilder.AppendLine(string.Format(scriptTemplate, FormatAndWrapObjectName(tableName)));
                }
                return new List<SixnetExecutionDatabaseStatement>(1)
                {
                    SixnetExecutionDatabaseStatement.Create(DatabaseType, data =>
                    {
                        data.Script = statementBuilder.ToString();
                        data.ScriptType = scriptType;
                        data.MustAffectData = command.Options?.MustAffectData ?? false;
                        data.Parameters = parameters;
                        data.HasPreScript = false;
                    })
                };
            }
            else
            {
                var queryStatement = GenerateQueryStatementCore(context, translationResult, SixnetQueryableLocation.JoinTarget);
                var updateTablePetName = "DTB";
                var joinItems = FormatWrapJoinPrimaryKeys(context, command.Queryable, command.GetEntityType(), tablePetName, tablePetName, updateTablePetName);
                scriptTemplate = $"{FormatPreScript(context)}DELETE {tablePetName} FROM {{0}}{TablePetNameKeyword}{tablePetName} INNER JOIN ({queryStatement.Script}){TablePetNameKeyword}{updateTablePetName} ON {string.Join(" AND ", joinItems)};";
                var statements = new List<SixnetExecutionDatabaseStatement>(tableNames.Count);
                foreach (var tableName in tableNames)
                {
                    statements.Add(SixnetExecutionDatabaseStatement.Create(DatabaseType, data =>
                    {
                        data.Script = string.Format(scriptTemplate, FormatAndWrapObjectName(tableName));
                        data.ScriptType = scriptType;
                        data.MustAffectData = command.Options?.MustAffectData ?? false;
                        data.Parameters = parameters;
                        data.HasPreScript = true;
                    }));
                }
                return statements;
            }

            #endregion
        }

        #endregion

        #endregion

        #region Migration

        #region Get create table statements

        protected override SixnetDatabaseScriptInfo GetCreateTableScripts(SixnetGetCreateTableDefineScriptParameter parameter)
        {
            var tableName = FormatAndWrapObjectName(parameter.Table);
            var columnInfo = parameter.ColumnDefineInfo;
            var fields = columnInfo.ColumnScripts;
            var parimaryKeys = columnInfo.PrimaryKeys;
            var migrationInfo = parameter.MigrationInfo;
            var script = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(",", fields)}{(parimaryKeys.IsNullOrEmpty() ? "" : $", PRIMARY KEY ({string.Join(",", parimaryKeys)}) USING BTREE")});";
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>() { script }
            };
        }

        #endregion

        #region Get delete all table statements

        protected override SixnetDatabaseScriptInfo GetDeleteAllTableScripts(SixnetDeleteAllTableParameter parameter)
        {
            var schema = parameter.Schema;
            var command = parameter.Command;
            var sql = $@"
SELECT CONCAT(
    'DROP TABLE ',
    '`', TABLE_SCHEMA, '`.`', TABLE_NAME, '`;'
)
FROM information_schema.TABLES
WHERE TABLE_TYPE = 'BASE TABLE' AND TABLE_SCHEMA = '{parameter.Schema}';
";
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = command.Connection.DbConnection.Query<string>(sql, transaction: command.Connection.Transaction.DbTransaction)?.ToList() ?? new List<string>(0)
            };
        }

        #endregion

        #region Get rename table statements

        /// <summary>
        /// Get rename table statements
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetRenameTableScripts(SixnetRenameTableParameter parameter)
        {
            var oldFormattedTableName = FormatObjectName(parameter.CurrentTableName);
            var newFormattedTableName = FormatObjectName(parameter.NewTableName);
            var script = $@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.tables
        WHERE table_schema = '{oldFormattedTableName.SchemaName}'
          AND table_name = '{oldFormattedTableName.Name}'
          AND table_type = 'BASE TABLE'
    )
    , 'RENAME TABLE {WrapObjectName(oldFormattedTableName)} TO {WrapObjectName(newFormattedTableName).Name}'
    , 'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;";
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>()
                {
                    script
                }
            };
        }

        #endregion


        #region Get add filed statements

        /// <summary>
        /// Get add field scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetAddFieldScripts(SixnetAddFieldParameter parameter)
        {
            var table = parameter.Table;
            var fields = parameter.Fields;
            var command = parameter.Command;
            var formattedTableName = FormatObjectName(table);
            var scripts = new List<string>();
            foreach (var field in fields)
            {
                var dataFieldName = FormatObjectName(SixnetDatabaseObjectName.Create(field.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column));
                scripts.Add($@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = '{formattedTableName.SchemaName}'
          AND TABLE_NAME = '{formattedTableName.Name}'
          AND COLUMN_NAME = '{dataFieldName.Name}'
    ),
    'SELECT 1',
    'ALTER TABLE {WrapObjectName(formattedTableName)} ADD COLUMN {WrapObjectName(dataFieldName).Name} {GetFieldDefinition(field, command.MigrationInfo)}'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;");
            }
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = scripts
            };
        }

        #endregion

        #region Get delete filed statements

        protected override SixnetDatabaseScriptInfo GetDeleteFieldScripts(SixnetDeleteFieldParameter parameter)
        {
            var scripts = new List<string>();
            var table = parameter.Table;
            var fields = parameter.Fields;
            var formattedTableName = FormatObjectName(table);
            foreach (var field in fields)
            {
                var dataFieldName = FormatObjectName(SixnetDatabaseObjectName.Create(field.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column));
                scripts.Add($@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = '{formattedTableName.SchemaName}'
          AND TABLE_NAME = '{formattedTableName.Name}'
          AND COLUMN_NAME = '{dataFieldName.Name}'
    ),
    'ALTER TABLE {WrapObjectName(formattedTableName)} DROP COLUMN {WrapObjectName(dataFieldName).Name}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;");
            }
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = scripts
            };
        }

        #endregion

        #region Get update field statements 

        /// <summary>
        /// Get update field scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetUpdateFieldScripts(SixnetUpdateFieldParameter parameter)
        {
            var scripts = new List<string>();
            var table = parameter.Table;
            var fields = parameter.Fields;
            var command = parameter.Command;
            var formattedTableName = FormatObjectName(table);
            foreach (var fieldItem in fields)
            {
                var field = fieldItem.Value;
                var nowFieldName = fieldItem.Key;
                var nowFormatedFieldName = FormatObjectName(SixnetDatabaseObjectName.Create(nowFieldName, SixnetDatabaseObjectType.Column));

                var newFieldName = FormatObjectName(SixnetDatabaseObjectName.Create(field.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column));
                scripts.Add($@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = '{formattedTableName.SchemaName}'
          AND TABLE_NAME = '{formattedTableName.Name}'
          AND COLUMN_NAME = '{nowFormatedFieldName.Name}'
    ),
    'ALTER TABLE {WrapObjectName(formattedTableName)} MODIFY COLUMN {WrapObjectName(nowFormatedFieldName).Name} {GetFieldDefinition(field, command.MigrationInfo)}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;");
                if (!string.Equals(nowFormatedFieldName.Name, newFieldName.Name, StringComparison.OrdinalIgnoreCase))
                {
                    scripts.Add($@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.COLUMNS
        WHERE TABLE_SCHEMA = '{formattedTableName.SchemaName}'
          AND TABLE_NAME = '{formattedTableName.Name}'
          AND COLUMN_NAME = '{nowFormatedFieldName.Name}'
    ),
    'ALTER TABLE {WrapObjectName(formattedTableName)} RENAME COLUMN {WrapObjectName(nowFormatedFieldName).Name} TO {WrapObjectName(newFieldName)}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;");
                }
            }
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = scripts
            };
        }

        #endregion

        #region Add foreign key

        /// <summary>
        /// Get add foreign key scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetAddForeignKeyScripts(SixnetAddForeignKeyParameter parameter)
        {
            var foreignKeyInfo = parameter.ForeignKeyInfo;

            var sourceFieldName = FormatObjectName(foreignKeyInfo.SourceField);
            var formattedSourceTableName = FormatObjectName(foreignKeyInfo.SourceTable);
            var wrapedSourceTableName = FormatAndWrapObjectName(foreignKeyInfo.SourceTable);

            var referenceFieldName = FormatAndWrapObjectName(foreignKeyInfo.ReferenceField);
            var referenceTableName = FormatAndWrapObjectName(foreignKeyInfo.ReferenceTable);

            var constraintName = GetForeignKeyName(formattedSourceTableName, sourceFieldName);
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>()
                {
                    $@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.TABLE_CONSTRAINTS
        WHERE CONSTRAINT_SCHEMA = '{formattedSourceTableName.SchemaName}'
          AND TABLE_NAME = '{formattedSourceTableName.Name}'
          AND CONSTRAINT_NAME = '{constraintName.Name}'
          AND CONSTRAINT_TYPE = 'FOREIGN KEY'
    ),
    'SELECT 1',
    'ALTER TABLE {wrapedSourceTableName} ADD CONSTRAINT {WrapObjectName(constraintName).Name} FOREIGN KEY ({WrapObjectName(sourceFieldName).Name}) REFERENCES {referenceTableName} ({referenceFieldName})'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;"
                }
            };
        }

        #endregion

        #region Delete foreign key

        /// <summary>
        /// Get delete foreign key scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override SixnetDatabaseScriptInfo GetDeleteForeignKeyScripts(SixnetDeleteForeignKeyParameter parameter)
        {
            var foreignKeyInfo = parameter.ForeignKeyInfo;
            var sourceFieldName = FormatObjectName(foreignKeyInfo.SourceField);
            var formattedSourceTableName = FormatObjectName(foreignKeyInfo.SourceTable);
            var wrapedSourceTableName = FormatAndWrapObjectName(foreignKeyInfo.SourceTable);
            var constraintName = WrapObjectName(GetForeignKeyName(formattedSourceTableName, sourceFieldName));
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>()
                {
                    $@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.TABLE_CONSTRAINTS
        WHERE CONSTRAINT_SCHEMA = '{formattedSourceTableName.SchemaName}'
          AND TABLE_NAME = '{formattedSourceTableName.Name}'
          AND CONSTRAINT_NAME = '{constraintName.Name}'
          AND CONSTRAINT_TYPE = 'FOREIGN KEY'
    ),
    'ALTER TABLE {wrapedSourceTableName} DROP FOREIGN KEY {WrapObjectName(constraintName).Name}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;",

                    $@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.statistics
        WHERE TABLE_SCHEMA = '{formattedSourceTableName.SchemaName}'
          AND TABLE_NAME = '{formattedSourceTableName.Name}'
          AND INDEX_NAME = '{constraintName.Name}'
    ),
    'DROP INDEX {WrapObjectName(constraintName).Name} ON {wrapedSourceTableName}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;"
                }
            };
        }

        /// <summary>
        /// Get delete all foreign key scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetDeleteAllForeignKeyScripts(SixnetDeleteAllForeignKeyParameter parameter)
        {
            var sql = $@"
SELECT CONCAT(
    'ALTER TABLE `',
    TABLE_SCHEMA,
    '`.`',
    TABLE_NAME,
    '` DROP FOREIGN KEY `',
    CONSTRAINT_NAME,
    '`;'
) AS DropForeignKeySql
FROM information_schema.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'FOREIGN KEY'
  AND TABLE_SCHEMA = '{parameter.Schema}';
";
            var deleteScripts = parameter.Command.Connection.DbConnection.Query<string>(sql, transaction: parameter.Command.Connection.Transaction.DbTransaction);
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = deleteScripts.ToList()
            };
        }

        #endregion


        #region Add index

        /// <summary>
        /// Get add index scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetAddIndexScripts(SixnetAddIndexParameter parameter)
        {
            var indexInfo = parameter.IndexInfo;
            var formattedTableName = FormatObjectName(indexInfo.Table);
            var formattedAndWrapedTableName = FormatAndWrapObjectName(indexInfo.Table);
            var indexDefine = GetIndexDefine(indexInfo);
            var indexName = indexDefine.Item1;
            var indexFieldStrings = indexDefine.Item2;

            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>()
                {
                    $@"
SET @sql = (
    SELECT IF(
        EXISTS (
            SELECT 1
            FROM information_schema.statistics
            WHERE table_schema = '{formattedTableName.SchemaName}'
              AND table_name = '{formattedTableName.Name}'
              AND index_name = '{indexName}'
        ),
        'SELECT 1',
        'CREATE {(indexInfo.Unique ? "UNIQUE " : "")}INDEX {WrapObjectName(indexName).Name} ON {formattedAndWrapedTableName} ({string.Join(",", indexFieldStrings)})'
    )
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;"
                }
            };
        }

        #endregion

        #region Delete index

        /// <summary>
        /// Get delete index scripts
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetDeleteIndexScripts(SixnetDeleteIndexParameter parameter)
        {
            var indexInfo = parameter.IndexInfo;
            var indexName = GetIndexDefine(indexInfo).Item1;
            var formattedTableName = FormatObjectName(indexInfo.Table);
            var formattedAndWrapedTableName = FormatAndWrapObjectName(indexInfo.Table);

            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>()
                {
                    $@"
SET @sql = IF(
    EXISTS (
        SELECT 1
        FROM information_schema.statistics
        WHERE TABLE_SCHEMA = '{formattedTableName.SchemaName}'
          AND TABLE_NAME = '{formattedTableName.Name}'
          AND INDEX_NAME = '{indexName}'
    ),
    'DROP INDEX {WrapObjectName(indexName).Name} ON {formattedAndWrapedTableName}',
    'SELECT 1'
);

PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;"
                }
            };
        }

        #endregion


        #region Get delete all view statements

        protected override SixnetDatabaseScriptInfo GetDeleteAllViewScripts(SixnetDeleteAllViewParameter parameter)
        {
            var sql = $@"
SELECT CONCAT(
    'DROP VIEW `',
    TABLE_SCHEMA,
    '`.`',
    TABLE_NAME,
    '`;'
) AS DropViewSql
FROM information_schema.VIEWS
WHERE TABLE_SCHEMA = '{parameter.Schema}';
";
            var deleteScripts = parameter.Command.Connection.DbConnection.Query<string>(sql, transaction: parameter.Command.Connection.Transaction.DbTransaction);
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = deleteScripts?.ToList() ?? new List<string>(0)
            };
        }

        #endregion

        #region Get delete all function statements

        /// <summary>
        /// Get delete all function statements
        /// </summary>
        /// <param name="migrationCommand"></param>
        /// <returns></returns>
        protected override SixnetDatabaseScriptInfo GetDeleteAllFunctionScripts(SixnetDeleteAllFunctionParameter parameter)
        {
            var sql = $@"
SELECT CONCAT(
    'DROP FUNCTION `',
    ROUTINE_SCHEMA,
    '`.`',
    ROUTINE_NAME,
    '`;'
) AS DropFunctionSql
FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA = '{parameter.Schema}' AND ROUTINE_TYPE = 'FUNCTION';
";
            var deleteScripts = parameter.Command.Connection.DbConnection.Query<string>(sql, transaction: parameter.Command.Connection.Transaction.DbTransaction);
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = deleteScripts?.ToList() ?? new List<string>(0)
            };
        }

        #endregion

        #region Get delete all custom type statements

        protected override SixnetDatabaseScriptInfo GetDeleteAllCustomTypeScripts(SixnetDeleteAllCustomerTypeParameter parameter)
        {
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = new List<string>(0)
            };
        }


        #endregion

        #region Get delete all procedure statements

        protected override SixnetDatabaseScriptInfo GetDeleteAllProcedureScripts(SixnetDeleteAllProcedureParameter parameter)
        {
            var sql = $@"
SELECT CONCAT(
    'DROP PROCEDURE IF EXISTS `',
    ROUTINE_SCHEMA,
    '`.`',
    ROUTINE_NAME,
    '`;'
) AS DropProcedureSql
FROM information_schema.ROUTINES
WHERE ROUTINE_SCHEMA = '{parameter.Schema}'
AND ROUTINE_TYPE = 'PROCEDURE';
";
            var deleteScripts = parameter.Command.Connection.DbConnection.Query<string>(sql, transaction: parameter.Command.Connection.Transaction.DbTransaction);
            return new SixnetDatabaseScriptInfo()
            {
                Scripts = deleteScripts?.ToList() ?? new List<string>(0)
            };
        }

        #endregion

        #endregion

        #region Util

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
        protected override string GetSqlDataType(SixnetDataField field, SixnetMigrationInfo options)
        {
            SixnetDirectThrower.ThrowArgNullIf(field == null, nameof(field));
            var dbTypeName = "";
            if (!string.IsNullOrWhiteSpace(field.DbType))
            {
                dbTypeName = field.DbType;
            }
            else
            {
                var dbType = field.GetDataType().GetDbType();
                var length = field.Length;
                var precision = field.Precision;
                var notFixedLength = options.NotFixedLength || field.HasDbFeature(SixnetFieldDbFeature.NotFixedLength);
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
                        if (length >= 16000)
                        {
                            dbTypeName = "LONGTEXT";
                        }
                        else if (length >= 4000)
                        {
                            dbTypeName = "TEXT";
                        }
                        else if (notFixedLength || length > 200)
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

        #region Get field identity

        /// <summary>
        /// Get field identity
        /// </summary>
        /// <param name="field">Field</param>
        /// <param name="options">Options</param>
        /// <returns></returns>
        protected override string GetFieldIdentity(SixnetDataField field, SixnetMigrationInfo options)
        {
            SixnetDirectThrower.ThrowArgNullIf(field == null, nameof(field));
            if (!field.InRole(SixnetFieldRole.Increment))
            {
                return string.Empty;
            }
            var startValue = field.StartValue;
            if (startValue == 0)
            {
                startValue = 1;
            }
            var incrementValue = field.IncrementValue;
            if (incrementValue == 0)
            {
                incrementValue = 1;
            }

            return $" AUTO_INCREMENT";
        }

        #endregion

        #endregion
    }
}
