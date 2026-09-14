using System.Collections.Generic;
using System;
using Sixnet.Development.Data.Field.Formatting;
using Sixnet.Exceptions;

namespace Sixnet.Database.MySqlConnector
{
    /// <summary>
    /// Defines default field converter for mysql
    /// </summary>
    internal class SixnetMySqlDefaultFieldFormatter : ISixnetFieldFormatter
    {
        public string Format(SixnetFormatFieldContext context)
        {
            var formatOption = context.FormatSetting;
            var formatedFieldName = context.FieldName;
            var parameterString = formatOption.Parameter?.ToString();

            formatedFieldName = formatOption.Name switch
            {
                SixnetFieldFormatterNames.TO_STRING => $"CAST({formatedFieldName} AS CHAR({(string.IsNullOrWhiteSpace(parameterString) ? "1000" : parameterString)}))",
                SixnetFieldFormatterNames.DISTINCT => $"DISTINCT {formatedFieldName}",
                SixnetFieldFormatterNames.IS_NULL => $"{formatedFieldName} IS NULL",
                SixnetFieldFormatterNames.NOT_NULL => $"{formatedFieldName} IS NOT NULL",
                SixnetFieldFormatterNames.CHARLENGTH => $"CHAR_LENGTH({formatedFieldName})",
                SixnetFieldFormatterNames.COUNT => $"COUNT({formatedFieldName})",
                SixnetFieldFormatterNames.SUM => $"SUM({formatedFieldName})",
                SixnetFieldFormatterNames.MAX => $"MAX({formatedFieldName})",
                SixnetFieldFormatterNames.MIN => $"MIN({formatedFieldName})",
                SixnetFieldFormatterNames.AVG => $"AVG({formatedFieldName})",
                SixnetFieldFormatterNames.JSON_VALUE => $"JSON_UNQUOTE(JSON_EXTRACT({formatedFieldName},{parameterString}))",
                SixnetFieldFormatterNames.JSON_OBJECT => $"JSON_EXTRACT({formatedFieldName},{parameterString})",
                SixnetFieldFormatterNames.AND => $"({formatedFieldName} & {parameterString})",
                SixnetFieldFormatterNames.OR => $"({formatedFieldName} | {parameterString})",
                SixnetFieldFormatterNames.XOR => $"({formatedFieldName} ^ {parameterString})",
                SixnetFieldFormatterNames.NOT => $"(~{formatedFieldName})",
                SixnetFieldFormatterNames.ADD => $"({formatedFieldName}+{parameterString})",
                SixnetFieldFormatterNames.SUBTRACT => $"({formatedFieldName}-{parameterString})",
                SixnetFieldFormatterNames.MULTIPLY => $"({formatedFieldName}*{parameterString})",
                SixnetFieldFormatterNames.DIVIDE => $"({formatedFieldName}/{parameterString})",
                SixnetFieldFormatterNames.MODULO => $"MOD({formatedFieldName},{parameterString})",
                SixnetFieldFormatterNames.STRING_CONCAT => $"CONCAT({formatedFieldName},{parameterString})",
                SixnetFieldFormatterNames.DATE_TIME_DATE => $"DATE({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_YEAR => $"YEAR({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_MONTH => $"MONTH({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_DAY => $"DAY({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_HOUR => $"HOUR({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_MINUTE => $"MINUTE({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_SECOND => $"SECOND({formatedFieldName})",
                SixnetFieldFormatterNames.DATE_TIME_FORMAT_STRING => $"DATE_FORMAT({formatedFieldName}, '{parameterString}')",
                SixnetFieldFormatterNames.DATE_STRING => $"DATE_FORMAT({formatedFieldName}, '%Y-%m-%d')",
                SixnetFieldFormatterNames.US_DATE_STRING => $"DATE_FORMAT({formatedFieldName}, '%m/%d/%Y')",
                SixnetFieldFormatterNames.JAPAN_DATE_STRING => $"DATE_FORMAT({formatedFieldName}, '%Y/%m/%d')",
                SixnetFieldFormatterNames.DATE_TIME_ADD_DAY => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} DAY)",
                SixnetFieldFormatterNames.DATE_TIME_ADD_MONTH => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} MONTH)",
                SixnetFieldFormatterNames.DATE_TIME_ADD_YEAR => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} YEAR)",
                SixnetFieldFormatterNames.DATE_TIME_ADD_HOUR => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} HOUR)",
                SixnetFieldFormatterNames.DATE_TIME_ADD_MINUTE => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} MINUTE)",
                SixnetFieldFormatterNames.DATE_TIME_ADD_SECOND => $"DATE_ADD({formatedFieldName}, INTERVAL {parameterString} SECOND)",
                SixnetFieldFormatterNames.TO_LOWER => $"LOWER({formatedFieldName})",
                SixnetFieldFormatterNames.TO_UPPER => $"UPPER({formatedFieldName})",
                SixnetFieldFormatterNames.SUB_STRING => Substring(formatedFieldName, formatOption.Parameter),
                SixnetFieldFormatterNames.STRING_REPLACE => ReplaceString(formatedFieldName, formatOption.Parameter),
                SixnetFieldFormatterNames.MATH_ROUND => $"ROUND({formatedFieldName}, {parameterString})",
                SixnetFieldFormatterNames.MATH_ABS => $"ABS({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_CEILING => $"CEILING({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_FLOOR => $"FLOOR({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_TRUNCATE => $"TRUNCATE({formatedFieldName}, 0)",
                SixnetFieldFormatterNames.MATH_SIGN => $"SIGN({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_POW => $"POW({formatedFieldName}, {parameterString})",
                SixnetFieldFormatterNames.MATH_SQRT => $"SQRT({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_EXP => $"EXP({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_LOG => $"LOG({parameterString}, {formatedFieldName})",
                SixnetFieldFormatterNames.MATH_COS => $"COS({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_SIN => $"SIN({formatedFieldName})",
                SixnetFieldFormatterNames.MATH_TAN => $"TAN({formatedFieldName})",
                SixnetFieldFormatterNames.STRING_INDEX_OF => StringIndexOf(formatedFieldName, formatOption.Parameter),
                SixnetFieldFormatterNames.STRING_LAST_INDEX_OF => StringLastIndexOf(formatedFieldName, formatOption.Parameter),
                SixnetFieldFormatterNames.EXISTS => $"EXISTS{formatedFieldName}",
                SixnetFieldFormatterNames.NOT_EXISTS => $"NOT EXISTS{formatedFieldName}",
                _ => throw new SixnetException($"{context.Server.DatabaseType} does not support field formatter: {formatOption.Name}"),
            };

            return formatedFieldName;
        }

        #region Substring
        string Substring(string formatedFieldName, dynamic parameter)
        {
            if (parameter is Tuple<dynamic, dynamic> tupeParameter)
            {
                return $"SUBSTRING({formatedFieldName}, {tupeParameter.Item1 + 1}, {tupeParameter.Item2})";
            }
            else
            {
                return $"SUBSTRING({formatedFieldName}, {parameter + 1})";
            }
        }
        #endregion

        #region Replace
        string ReplaceString(string formatedFieldName, object parameter)
        {
            if (parameter is Tuple<dynamic, dynamic> tupeTwoParameter)
            {
                return $"REPLACE({formatedFieldName}, '{tupeTwoParameter.Item1}', '{tupeTwoParameter.Item2}')";
            }
            SixnetDirectThrower.ThrowAppException(true, $"Error field formatter: {formatedFieldName}");
            return string.Empty;
        }
        #endregion

        #region String IndexOf / LastIndexOf
        string StringIndexOf(string formatedFieldName, object parameter)
        {
            if (parameter is Tuple<dynamic, dynamic> tupe)
            {
                var charValue = tupe.Item1;
                var startIndex = tupe.Item2;
                return $"(LOCATE('{charValue}', {formatedFieldName}, {startIndex + 1}) - 1)";
            }
            else
            {
                return $"(LOCATE('{parameter}', {formatedFieldName}) - 1)";
            }
        }

        string StringLastIndexOf(string formatedFieldName, object parameter)
        {
            if (parameter is Tuple<dynamic, dynamic> tupe)
            {
                var charValue = tupe.Item1;
                return $"(LENGTH({formatedFieldName}) - LOCATE(REVERSE('{charValue}'), REVERSE({formatedFieldName})))";
            }
            else
            {
                return $"(LENGTH({formatedFieldName}) - LOCATE(REVERSE('{parameter}'), REVERSE({formatedFieldName})))";
            }
        }
        #endregion
    }
}
