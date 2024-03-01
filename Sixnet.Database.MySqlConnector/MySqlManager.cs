using System.Data;
using MySqlConnector;
using Sixnet.Development.Data;
using Sixnet.Development.Data.Database;

namespace Sixnet.Database.MySqlConnector
{
    /// <summary>
    /// Defines mysql manager
    /// </summary>
    internal static class MySqlManager
    {
        #region Fields

        /// <summary>
        /// Gets current database server type
        /// </summary>
        internal const DatabaseServerType CurrentDatabaseServerType = DatabaseServerType.MySQL;

        /// <summary>
        /// Key word prefix
        /// </summary>
        internal const string KeywordPrefix = "`";

        /// <summary>
        /// Key word suffix
        /// </summary>
        internal const string KeywordSuffix = "`";

        /// <summary>
        /// Default query translator
        /// </summary>
        static readonly MySqlDataCommandResolver DefaultResolver = new MySqlDataCommandResolver();

        #endregion

        #region Get database connection

        /// <summary>
        /// Get a database connection
        /// </summary>
        /// <param name="server">Database server</param>
        /// <returns>Return database connection</returns>
        public static IDbConnection GetConnection(SixnetDatabaseServer server)
        {
            return SixnetDataManager.GetDatabaseConnection(server) ?? new MySqlConnection(server.ConnectionString);
        }

        #endregion

        #region Get command resolver

        /// <summary>
        /// Get command resolver
        /// </summary>
        /// <returns>Return a command resolver</returns>
        internal static MySqlDataCommandResolver GetCommandResolver()
        {
            return DefaultResolver;
        }

        #endregion

        #region Wrap keyword

        /// <summary>
        /// Wrap keyword by the KeywordPrefix and the KeywordSuffix
        /// </summary>
        /// <param name="originalValue">Original value</param>
        /// <returns></returns>
        internal static string WrapKeyword(string originalValue)
        {
            return $"{KeywordPrefix}{originalValue}{KeywordSuffix}";
        }

        #endregion
    }
}
