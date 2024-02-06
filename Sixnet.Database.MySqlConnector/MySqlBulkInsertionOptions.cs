using System;
using System.Collections.Generic;
using System.Globalization;
using MySqlConnector;
using Sixnet.Development.Data.Database;

namespace Sixnet.Database.MySqlConnector
{
    /// <summary>
    /// Defines bulk insertion options for mysql
    /// </summary>
    public class MySqlBulkInsertionOptions : IBulkInsertionOptions
    {
        /// <summary>
        /// Gets or sets the loader priority.
        /// </summary>
        public MySqlBulkLoaderPriority Priority { get; set; } = MySqlBulkLoaderPriority.None;

        /// <summary>
        /// Gets or sets the conflict option.
        /// </summary>
        public MySqlBulkLoaderConflictOption ConflictOption { get; set; } = MySqlBulkLoaderConflictOption.None;

        /// <summary>
        /// Gets or sets the escape character.
        /// </summary>
        public char EscapeCharacter { get; set; }

        /// <summary>
        /// Indicates whether [field quotation optional].
        /// </summary>
        public bool FieldQuotationOptional { get; set; }

        /// <summary>
        /// Gets or sets the field quotation character.
        /// </summary>
        public char FieldQuotationCharacter { get; set; }

        /// <summary>
        /// Gets or sets the line prefix.
        /// </summary>
        public string LinePrefix { get; set; }

        /// <summary>
        /// Gets or sets the number of lines to skip.
        /// </summary>
        public int NumberOfLinesToSkip { get; set; } = 0;

        /// <summary>
        /// Gets the columns.
        /// </summary>
        public List<string> Columns { get; }

        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Gets or sets the character set.
        /// </summary>
        public string CharacterSet { get; set; }

        /// <summary>
        /// Gets or sets the line terminator.
        /// Default values is Environment.NewLine.
        /// </summary>
        public string LineTerminator { get; set; } = Environment.NewLine;

        /// <summary>
        /// Gets or sets the field terminator.
        /// Default values is  CultureInfo.CurrentCulture.TextInfo.ListSeparator;
        /// </summary>
        public string FieldTerminator { get; set; } = CultureInfo.CurrentCulture.TextInfo.ListSeparator;
    }
}
