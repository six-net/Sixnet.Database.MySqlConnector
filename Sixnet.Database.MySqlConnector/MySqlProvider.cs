using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Sixnet.App;
using Sixnet.Code;
using Sixnet.Development.Data.Command;
using Sixnet.Development.Data.Dapper;
using Sixnet.Development.Data.Database;
using Sixnet.Exceptions;

namespace Sixnet.Database.MySqlConnector
{
    /// <summary>
    /// Defines database provider implementation for mysql database(8.0+)
    /// </summary>
    public class MySqlProvider : BaseDatabaseProvider
    {
        #region Constructor

        public MySqlProvider()
        {
            queryDatabaseTablesScript = "SELECT TABLE_NAME AS TableName FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{0}' AND TABLE_TYPE='BASE TABLE';";
        }

        #endregion

        #region Connection

        /// <summary>
        /// Get database connection
        /// </summary>
        /// <param name="server">Database server</param>
        /// <returns></returns>
        public override IDbConnection GetDbConnection(DatabaseServer server)
        {
            return MySqlManager.GetConnection(server);
        }

        #endregion

        #region Data command resolver

        /// <summary>
        /// Get data command resolver
        /// </summary>
        /// <returns></returns>
        protected override ISixnetDataCommandResolver GetDataCommandResolver()
        {
            return MySqlManager.GetCommandResolver();
        }

        #endregion

        #region Parameter

        /// <summary>
        /// Convert data command parametes
        /// </summary>
        /// <param name="parameters">Data command parameters</param>
        /// <returns></returns>
        protected override DynamicParameters ConvertDataCommandParameters(DataCommandParameters parameters)
        {
            return parameters?.ConvertToDynamicParameters(MySqlManager.CurrentDatabaseServerType);
        }

        #endregion

        #region Bulk

        /// <summary>
        /// Bulk insert datas
        /// </summary>
        /// <param name="command">Database bulk insert command</param>
        public override async Task BulkInsertAsync(BulkInsertDatabaseCommand command)
        {
            var dataTable = command.DataTable;
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            var bulkInsertOptions = command.BulkInsertionOptions;
            var dbConnection = command.Connection.DbConnection as MySqlConnection;
            var dataFilePath = WriteTableToFile(dataTable, Path.Combine(Directory.GetCurrentDirectory(), "temp"), ignoreTitle: true);
            if (string.IsNullOrWhiteSpace(dataFilePath))
            {
                throw new SixnetException("Failed to generate temporary data file");
            }
            dataFilePath = Path.Combine(SixnetApplication.RootPath, dataFilePath);
            var loader = new MySqlBulkLoader(dbConnection)
            {
                Local = true,
                TableName = dataTable.TableName,
                FieldTerminator = CultureInfo.CurrentCulture.TextInfo.ListSeparator,
                LineTerminator = Environment.NewLine,
                FileName = dataFilePath,
                NumberOfLinesToSkip = 0
            };
            if (bulkInsertOptions is MySqlBulkInsertionOptions mySqlBulkInsertOptions)
            {
                loader.Priority = mySqlBulkInsertOptions.Priority;
                loader.ConflictOption = mySqlBulkInsertOptions.ConflictOption;
                loader.EscapeCharacter = mySqlBulkInsertOptions.EscapeCharacter;
                loader.FieldQuotationOptional = mySqlBulkInsertOptions.FieldQuotationOptional;
                loader.FieldQuotationCharacter = mySqlBulkInsertOptions.FieldQuotationCharacter;
                loader.LineTerminator = mySqlBulkInsertOptions.LineTerminator;
                loader.FieldTerminator = mySqlBulkInsertOptions.FieldTerminator;
                if (!string.IsNullOrWhiteSpace(mySqlBulkInsertOptions.LinePrefix))
                {
                    loader.LinePrefix = mySqlBulkInsertOptions.LinePrefix;
                }
                if (mySqlBulkInsertOptions.NumberOfLinesToSkip >= 0)
                {
                    loader.NumberOfLinesToSkip = mySqlBulkInsertOptions.NumberOfLinesToSkip;
                }
                if (mySqlBulkInsertOptions.Columns.IsNullOrEmpty())
                {
                    loader.Columns.AddRange(mySqlBulkInsertOptions.Columns);
                }
                if (mySqlBulkInsertOptions.Timeout > 0)
                {
                    loader.Timeout = mySqlBulkInsertOptions.Timeout;
                }
                if (!string.IsNullOrWhiteSpace(mySqlBulkInsertOptions.CharacterSet))
                {
                    loader.CharacterSet = mySqlBulkInsertOptions.CharacterSet;
                }
            }
            if (loader.Columns.IsNullOrEmpty())
            {
                loader.Columns.AddRange(dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            }
            await loader.LoadAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Bulk insert datas
        /// </summary>
        /// <param name="command">Database bulk insert command</param>
        public override void BulkInsert(BulkInsertDatabaseCommand command)
        {
            var dataTable = command.DataTable;
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            var bulkInsertOptions = command.BulkInsertionOptions;
            var dbConnection = command.Connection.DbConnection as MySqlConnection;
            var dataFilePath = WriteTableToFile(dataTable, Path.Combine(Directory.GetCurrentDirectory(), "temp"), ignoreTitle: true);
            if (string.IsNullOrWhiteSpace(dataFilePath))
            {
                throw new SixnetException("Failed to generate temporary data file");
            }
            dataFilePath = Path.Combine(SixnetApplication.RootPath, dataFilePath);
            var loader = new MySqlBulkLoader(dbConnection)
            {
                Local = true,
                TableName = dataTable.TableName,
                FieldTerminator = CultureInfo.CurrentCulture.TextInfo.ListSeparator,
                LineTerminator = Environment.NewLine,
                FileName = dataFilePath,
                NumberOfLinesToSkip = 0
            };
            if (bulkInsertOptions is MySqlBulkInsertionOptions mySqlBulkInsertOptions)
            {
                loader.Priority = mySqlBulkInsertOptions.Priority;
                loader.ConflictOption = mySqlBulkInsertOptions.ConflictOption;
                loader.EscapeCharacter = mySqlBulkInsertOptions.EscapeCharacter;
                loader.FieldQuotationOptional = mySqlBulkInsertOptions.FieldQuotationOptional;
                loader.FieldQuotationCharacter = mySqlBulkInsertOptions.FieldQuotationCharacter;
                loader.LineTerminator = mySqlBulkInsertOptions.LineTerminator;
                loader.FieldTerminator = mySqlBulkInsertOptions.FieldTerminator;
                if (!string.IsNullOrWhiteSpace(mySqlBulkInsertOptions.LinePrefix))
                {
                    loader.LinePrefix = mySqlBulkInsertOptions.LinePrefix;
                }
                if (mySqlBulkInsertOptions.NumberOfLinesToSkip >= 0)
                {
                    loader.NumberOfLinesToSkip = mySqlBulkInsertOptions.NumberOfLinesToSkip;
                }
                if (mySqlBulkInsertOptions.Columns.IsNullOrEmpty())
                {
                    loader.Columns.AddRange(mySqlBulkInsertOptions.Columns);
                }
                if (mySqlBulkInsertOptions.Timeout > 0)
                {
                    loader.Timeout = mySqlBulkInsertOptions.Timeout;
                }
                if (!string.IsNullOrWhiteSpace(mySqlBulkInsertOptions.CharacterSet))
                {
                    loader.CharacterSet = mySqlBulkInsertOptions.CharacterSet;
                }
            }
            if (loader.Columns.IsNullOrEmpty())
            {
                loader.Columns.AddRange(dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName));
            }
            loader.Load();
        }

        /// <summary>
        /// Write datatable to a csv file
        /// </summary>
        /// <param name="dataTable">Data table</param>
        /// <param name="savePath">Save path,Will save file to the application root directory when the parameter value is null or empty</param>
        /// <param name="fileName">Without extension file name,Will use a random file name when the parameter value is null or empty</param>
        /// <param name="ignoreTitle">Whether ignore column name,Default is false</param>
        /// <returns>Return the file full path</returns>
        string WriteTableToFile(DataTable dataTable, string savePath = "", string fileName = "", bool ignoreTitle = false)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = GuidHelper.GetGuidUniqueCode().ToLower();
            }
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            var filePath = Path.Combine(savePath, $"{fileName}.csv");
            using (var sw = new StreamWriter(filePath, false, new UTF8Encoding(false)))
            {
                var lineDatas = new List<string>(dataTable.Columns.Count);
                if (!ignoreTitle)
                {
                    foreach (DataColumn col in dataTable.Columns)
                    {
                        lineDatas.Add(col.ColumnName);
                    }
                    var titles = string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, lineDatas);
                    sw.WriteLine(titles);
                }
                var columnCount = dataTable.Columns.Count;
                foreach (DataRow row in dataTable.Rows)
                {
                    lineDatas.Clear();
                    for (int i = 0; i < columnCount; i++)
                    {
                        var rowValue = Convert.IsDBNull(row[i]) ? string.Empty : row[i].ToString();
                        if (dataTable.Columns[i].DataType == typeof(bool))
                        {
                            rowValue = string.Equals(rowValue, bool.TrueString, StringComparison.OrdinalIgnoreCase) ? "1" : string.Empty;
                        }
                        if (dataTable.Columns[i].DataType == typeof(DateTimeOffset))
                        {
                            rowValue = ((DateTimeOffset)row[i]).LocalDateTime.ToString();
                        }
                        lineDatas.Add(rowValue);
                    }
                    sw.WriteLine(string.Join(CultureInfo.CurrentCulture.TextInfo.ListSeparator, lineDatas));
                }
                sw.Close();
                sw.Dispose();
            }
            return filePath;
        }

        #endregion

        #region Get tables

        /// <summary>
        /// Get tables
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        public override List<SixnetDataTable> GetTables(DatabaseCommand command)
        {
            return command.Connection.DbConnection.Query<SixnetDataTable>(string.Format(queryDatabaseTablesScript, command.Connection.DbConnection.Database)).ToList();
        }

        /// <summary>
        /// Get tables
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns></returns>
        public override async Task<List<SixnetDataTable>> GetTablesAsync(DatabaseCommand command)
        {
            return (await command.Connection.DbConnection.QueryAsync<SixnetDataTable>(string.Format(queryDatabaseTablesScript, command.Connection.DbConnection.Database)).ConfigureAwait(false)).ToList();
        }

        #endregion
    }
}
