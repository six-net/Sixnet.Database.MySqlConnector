using System;
using System.Collections.Generic;
using System.Text;
using Sixnet.Development.Data.Command;
using Sixnet.Development.Data.Database;
using Sixnet.Development.Data.Field;
using Sixnet.Development.Data;
using Sixnet.Development.Entity;
using Sixnet.Development.Queryable;
using Sixnet.Exceptions;
using System.Threading.Tasks;

namespace Sixnet.Database.MySqlConnector
{
    internal partial class SixnetMySqlDataCommandResolver
    {
        #region Get query statement

        /// <summary>
        /// Get query statement
        /// </summary>
        /// <param name="context">Command resolve context</param>
        /// <param name="translationResult">Queryable translation result</param>
        /// <param name="location">Queryable location</param>
        /// <returns></returns>
        protected override async Task<SixnetQueryDatabaseStatement> GenerateQueryStatementCoreAsync(SixnetDataCommandResolveContext context, SixnetQueryableTranslationResult translationResult, SixnetQueryableLocation location)
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
                        var targetStatement = await GetFromTargetStatementAsync(context, queryable, location, tablePetName).ConfigureAwait(false);
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
                    var outputFieldString = await FormatFieldsStringAsync(context, queryable, location, SixnetFieldLocation.Output, outputFields).ConfigureAwait(false);
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
        protected override async Task<List<SixnetExecutionDatabaseStatement>> GenerateInsertStatementsAsync(SixnetDataCommandResolveContext context)
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
                insertFields.Add(FormatAndWrapObjectName(SixnetDatabaseObjectName.Create(field.GetFieldName(DatabaseType), SixnetDatabaseObjectType.Column)));
                // values
                var insertValue = command.FieldsAssignment.GetNewValue(field.PropertyName);
                insertValues.Add(await FormatInsertValueFieldAsync(context, command.Queryable, insertValue).ConfigureAwait(false));

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
            var tableNames = await dataCommandExecutionContext.GetTableNamesAsync().ConfigureAwait(false);
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
                SixnetExecutionDatabaseStatement.Create(DatabaseType, data=>
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
        protected override async Task<List<SixnetExecutionDatabaseStatement>> GenerateUpdateStatementsAsync(SixnetDataCommandResolveContext context)
        {
            var command = context.DataCommandExecutionContext.Command;
            SixnetException.ThrowIf(command?.FieldsAssignment?.NewValues.IsNullOrEmpty() ?? true, "No set update field");

            #region translate

            var translationResult = await TranslateAsync(context).ConfigureAwait(false);
            var preScripts = context.GetPreScripts();

            #endregion

            #region script 

            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var entityType = dataCommandExecutionContext.Command.GetEntityType();

            var tableNames = await dataCommandExecutionContext.GetTableNamesAsync(command).ConfigureAwait(false);
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
                var newValueExpression = await FormatUpdateValueFieldAsync(context, command, newValue).ConfigureAwait(false);
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
                var queryStatement = await GenerateQueryStatementCoreAsync(context, translationResult, SixnetQueryableLocation.JoinTarget).ConfigureAwait(false);
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
        protected override async Task<List<SixnetExecutionDatabaseStatement>> GenerateDeleteStatementsAsync(SixnetDataCommandResolveContext context)
        {
            var dataCommandExecutionContext = context.DataCommandExecutionContext;
            var command = dataCommandExecutionContext.Command;

            #region translate

            var translationResult = await TranslateAsync(context).ConfigureAwait(false);
            var preScripts = context.GetPreScripts();

            #endregion

            #region script

            var tablePetName = command.Queryable == null ? context.GetNewTablePetName() : context.GetDefaultTablePetName(command.Queryable);
            var entityType = dataCommandExecutionContext.Command.GetEntityType();
            var tableNames = await dataCommandExecutionContext.GetTableNamesAsync(command).ConfigureAwait(false);
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
                var queryStatement = await GenerateQueryStatementCoreAsync(context, translationResult, SixnetQueryableLocation.JoinTarget).ConfigureAwait(false);
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
    }
}
