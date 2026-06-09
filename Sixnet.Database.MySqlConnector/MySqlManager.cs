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
            return SixnetDataManager.GetDatabaseConnection(server) ?? new MySqlConnection(SixnetDataManager.ResolveConnectionString(server));
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
    }
}
