using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlServerHelper.Core.Extension;

namespace SqlServerHelper.Core
{
    #region -- Public Class SqlServerDBTableInfo --

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerDBTableInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDBTableInfo" /> class.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public SqlServerDBTableInfo(DataRow dr)
        {
            int i;
            DateTime dt;

            TableName = dr["TableName"].ToString();
            SchemaName = dr["SchemaName"].ToString();
            ColumnCount = int.TryParse(dr["ColumnCount"].ToString(), out i) ? i : 0;
            MaxLength = int.TryParse(dr["MaxLength"].ToString(), out i) ? i : 0;
            TableDescription = dr["TableDescription"].ToString();
            PrimaryKeyName = dr["PrimaryKeyName"].ToString();
            CreateDate = DateTime.TryParse(dr["CreateDate"].ToString(), out dt) ? dt : DateTime.MinValue;
            ModifyDate = DateTime.TryParse(dr["ModifyDate"].ToString(), out dt) ? dt : DateTime.MinValue;
            RowCount = int.TryParse(dr["RowCount"].ToString(), out i) ? i : 0;
        }

        public SqlServerDBTableInfo() { }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String TableName { set; get; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String TableVeriableName
        {
            get { return TableName.SplitbyCase().ToCamelCase(); }
        }
        //public String TableVeriableName { get { return TableName; } }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String TableClassName
        {
            get { return TableName.SplitbyCase().ToPascalCase(); }
        }
        //public String TableClassName { get { return TableName; } }


        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        /// <value>
        /// The name of the schema.
        /// </value>
        public String SchemaName { set; get; }

        /// <summary>
        /// Gets or sets the column count.
        /// </summary>
        /// <value>
        /// The column count.
        /// </value>
        public int ColumnCount { set; get; }

        /// <summary>
        /// Gets or sets the maximum length.
        /// </summary>
        /// <value>
        /// The maximum length.
        /// </value>
        public int MaxLength { set; get; }

        /// <summary>
        /// Gets or sets the table description.
        /// </summary>
        /// <value>
        /// The table description.
        /// </value>
        public String TableDescription { set; get; }

        /// <summary>
        /// Gets or sets the name of the primary key.
        /// </summary>
        /// <value>
        /// The name of the primary key.
        /// </value>
        public String PrimaryKeyName { set; get; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>
        /// The create date.
        /// </value>
        public DateTime CreateDate { set; get; }

        /// <summary>
        /// Gets or sets the modify date.
        /// </summary>
        /// <value>
        /// The modify date.
        /// </value>
        public DateTime ModifyDate { set; get; }

        public int RowCount { set; get; }

        /// <summary>
        /// Gets or sets the SQL server database column list.
        /// </summary>
        /// <value>
        /// The SQL server database column list.
        /// </value>
        public List<SqlServerDBColumnInfo> SqlServerDBColumnList { set; get; }


        /// <summary>
        /// Gets the SQL server database column dictionary.
        /// </summary>
        /// <value>
        /// The SQL server database column dictionary.
        /// </value>
        public Dictionary<string, SqlServerDBColumnInfo> SqlServerDBColumnDictionary => SqlServerDBColumnList?.ToDictionary(x => x.ColumnName);

        /// <summary>
        /// Gets or sets the SQL server database column list.
        /// </summary>
        /// <value>
        /// The SQL server database column list.
        /// </value>
        public List<SqlServerIndexInfo> SqlServerDBIndexList { set; get; }

        public Dictionary<string, SqlServerIndexInfo> SqlServerDBIndexDictionary => SqlServerDBIndexList?.ToDictionary(x => x.IndexName);

        /// <summary>
        /// Gets the declare.
        /// </summary>
        /// <returns></returns>
        public String GetDeclare()
        {
            List<SqlServerDBColumnInfo> schemes = SqlServerDBColumnList;

            string columns = string.Join("\r\n",
                schemes.Cast<SqlServerDBColumnInfo>().Select(c => $"Declare @{c.ColumnName} {c.DataTypeName};")
                    .ToArray());

            return columns;
        }

        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns></returns>
        public String GetDefaultValue()
        {
            List<SqlServerDBColumnInfo> schemes = SqlServerDBColumnList;

            string columns = string.Join("\r\n",
                schemes.Cast<SqlServerDBColumnInfo>().Select(c => $"SET @{c.ColumnName} = ''; ").ToArray());

            return columns;
        }

        /// <summary>
        /// Gets the select columns.
        /// </summary>
        /// <value>
        /// The select columns.
        /// </value>
        public String SelectColumns
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Cast<SqlServerDBColumnInfo>().Select(c => c.ColumnName).ToArray());
            }
        }

        /// <summary>
        /// Gets the insert values.
        /// </summary>
        /// <value>
        /// The insert values.
        /// </value>
        private String InsertValues
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Cast<SqlServerDBColumnInfo>().Select(c => String.Format("@{0}", c.ColumnName))
                        .ToArray());
            }
        }

        /// <summary>
        /// Gets the update values.
        /// </summary>
        /// <value>
        /// The update values.
        /// </value>
        public String UpdateValues
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("{0} = @{0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the merge columns.
        /// </summary>
        /// <value>
        /// The merge columns.
        /// </value>
        private String MergeColumns
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("@{0} as {0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the merge values.
        /// </summary>
        /// <value>
        /// The merge values.
        /// </value>
        public String MergeUpdateValues
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Where(s => !s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("{0} = @{0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the merge values.
        /// </summary>
        /// <value>
        /// The merge values.
        /// </value>
        public String MergeUpdateValuesFromSource
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Where(s => !s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("{0} = s.{0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the merge keys.
        /// </summary>
        /// <value>
        /// The merge keys.
        /// </value>
        public String MergeKeys
        {
            get
            {
                return String.Join(" AND ",
                    SqlServerDBColumnList.Where(s => s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("t.{0} = s.{0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>
        /// The keys.
        /// </value>
        private String Keys
        {
            get
            {
                return String.Join(" AND ",
                    SqlServerDBColumnList.Where(s => s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => String.Format("{0} = @{0}", c.ColumnName)).ToArray());
            }
        }

        /// <summary>
        /// Gets the where condition.
        /// </summary>
        /// <value>
        /// The where condition.
        /// </value>
        private String WhereCondition
        {
            get { return String.IsNullOrEmpty(Keys) ? String.Empty : $"WHERE {Keys}"; }
        }

        /// <summary>
        /// Gets the type of the method parameters with.
        /// </summary>
        /// <value>
        /// The type of the method parameters with.
        /// </value>
        public String MethodParametersWithType
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Where(s => s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => c.ParameterStringWithType).ToArray());
            }
        }

        /// <summary>
        /// Gets the method parameters.
        /// </summary>
        /// <value>
        /// The method parameters.
        /// </value>
        public String MethodParameters
        {
            get
            {
                return String.Join(",",
                    SqlServerDBColumnList.Where(s => s.IsPrimaryKey).Cast<SqlServerDBColumnInfo>()
                        .Select(c => c.ColumnVeriableName).ToArray());
            }
        }

        /// <summary>
        /// Gets the select SQL.
        /// </summary>
        /// <value>
        /// The select SQL.
        /// </value>
        public String SelectSql => $"SELECT {SelectColumns} \r\nFROM dbo.{TableName} with(nolock) \r\n{WhereCondition}";

        /// <summary>
        /// Gets the select all SQL.
        /// </summary>
        /// <value>
        /// The select all SQL.
        /// </value>
        public String SelectAllSql => $"SELECT {SelectColumns} \r\nFROM {TableName} with(nolock) ";

        /// <summary>
        /// Gets the insert SQL.
        /// </summary>
        /// <value>
        /// The insert SQL.
        /// </value>
        public String InsertSql
        {
            get { return $"INSERT INTO dbo.{TableName}\r\n({SelectColumns}) \r\nVALUES \r\n({InsertValues})"; }
        }

        /// <summary>
        /// Gets the update SQL.
        /// </summary>
        /// <value>
        /// The update SQL.
        /// </value>
        public String UpdateSql
        {
            get { return $"UPDATE dbo.{TableName} \r\nSET {UpdateValues} \r\n{WhereCondition}"; }
        }

        /// <summary>
        /// Gets the merge SQL.
        /// </summary>
        /// <value>
        /// The merge SQL.
        /// </value>
        public String MergeSql
        {
            get
            {
                return
                    $"MERGE dbo.{TableName} AS T \r\nUSING (SELECT {MergeColumns}) AS S \r\nON ({MergeKeys})\r\nWHEN NOT MATCHED BY TARGET\r\n    THEN INSERT({SelectColumns}) VALUES({InsertValues})\r\nWHEN MATCHED \r\n    THEN UPDATE SET {MergeUpdateValues};";
            }
        }
    }

    #endregion

    #region -- Public Class SqlServerDBColumnInfo --

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerDBColumnInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDBColumnInfo" /> class.
        /// </summary>
        /// <param name="dr">The dr.</param>
        public SqlServerDBColumnInfo(DataRow dr)
        {
            int i;

            TableName = dr["TableName"].ToString();
            ColumnName = dr["ColumnName"].ToString();
            OrdinalPosition = int.TryParse(dr["OrdinalPosition"].ToString(), out i) ? i : 0;
            IsPrimaryKey = !String.IsNullOrEmpty(dr["IsPrimaryKey"].ToString()) && dr["IsPrimaryKey"].ToString() == "Y";
            DataType = dr["DataType"].ToString();
            DataLen = dr["DataLen"].ToString();
            DataTypeName = dr["DataTypeName"].ToString().Replace("()", string.Empty);
            ColumnDescription = dr["ColumnDescription"].ToString();
            ColumnDefault = dr["ColumnDefault"].ToString();
            IsNullable = !String.IsNullOrEmpty(dr["IsNullable"].ToString()) && dr["IsNullable"].ToString() == "Y";
            IsIdentity = !String.IsNullOrEmpty(dr["IsIdentity"].ToString()) && dr["IsIdentity"].ToString() == "Y";
            CollationName = dr["CollationName"].ToString();
        }

        public SqlServerDBColumnInfo() { }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String TableName { set; get; }

        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        /// <value>
        /// The name of the column.
        /// </value>
        public String ColumnName { set; get; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String ColumnVeriableName
        {
            get { return ColumnName.SplitbyCase().ToCamelCase(); }
        }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public String ColumnPropertyName
        {
            get { return ColumnName.SplitbyCase().ToPascalCase(); }
        }

        /// <summary>
        /// Gets or sets the ordinal position.
        /// </summary>
        /// <value>
        /// The ordinal position.
        /// </value>
        public int OrdinalPosition { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary key.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is primary key; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimaryKey { set; get; }

        /// <summary>
        /// Gets or sets the type of the data.
        /// </summary>
        /// <value>
        /// The type of the data.
        /// </value>
        public String DataType { set; get; }

        /// <summary>
        /// Gets or sets the length of the data.
        /// </summary>
        /// <value>
        /// The length of the data.
        /// </value>
        public string DataLen { set; get; }

        /// <summary>
        /// Gets or sets the name of the data type.
        /// </summary>
        /// <value>
        /// The name of the data type.
        /// </value>
        public String DataTypeName { set; get; }

        /// <summary>
        /// Gets or sets the column description.
        /// </summary>
        /// <value>
        /// The column description.
        /// </value>
        public String ColumnDescription { set; get; }

        /// <summary>
        /// Gets or sets the column default.
        /// </summary>
        /// <value>
        /// The column default.
        /// </value>
        public String ColumnDefault { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is nullable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { set; get; }

        public bool IsIdentity { set; get; }

        /// <summary>
        /// CollationName
        /// </summary>
        public String CollationName { set; get; }

        public List<SqlServerGroupbyValueInfo> GroupbyValueList { set; get; }

        public List<SqlServerDBConstraintsInfo> ColumnConstraints { set; get; }

        /// <summary>
        /// Gets the column type string.
        /// </summary>
        /// <value>
        /// The column type string.
        /// </value>
        public SqlDbType ColumnSqlDbType
        {
            get { return GetColumnTypeString(DataType); }
        }

        /// <summary>
        /// Gets the column default value string.
        /// </summary>
        /// <value>
        /// The column default value string.
        /// </value>
        public String ColumnDefaultValueString
        {
            get { return GetColumnDefaultValueString(ColumnSqlDbType); }
        }

        /// <summary>
        /// Gets the property type string.
        /// </summary>
        /// <value>
        /// The property type string.
        /// </value>
        public String PropertyTypeString
        {
            get { return GetPropertyTypeString(ColumnSqlDbType); }
        }

        ///// <summary>
        ///// Gets the property string.
        ///// </summary>
        ///// <value>
        ///// The property string.
        ///// </value>
        //public String PropertyString { get { return $"public {PropertyTypeString}{PropertyNullableType} {ColumnName}"; } }

        /// <summary>
        /// Gets the type of the property nullable.
        /// </summary>
        /// <value>
        /// The type of the property nullable.
        /// </value>
        public String PropertyNullableType
        {
            get
            {
                return (IsNullable &&
                        (ColumnSqlDbType == SqlDbType.Decimal
                         || ColumnSqlDbType == SqlDbType.Float
                         || ColumnSqlDbType == SqlDbType.Int
                         || ColumnSqlDbType == SqlDbType.Bit
                         || ColumnSqlDbType == SqlDbType.BigInt
                         || ColumnSqlDbType == SqlDbType.DateTime
                        )
                    ? "?"
                    : String.Empty);
            }
        }

        /// <summary>
        /// Gets the default validator string.
        /// </summary>
        /// <value>
        /// The default validator string.
        /// </value>
        public String DefaultValidatorString
        {
            get { return GetColumnDefaultValidatorString(ColumnSqlDbType); }
        }

        /// <summary>
        /// Gets the type of the parameter string with.
        /// </summary>
        /// <value>
        /// The type of the parameter string with.
        /// </value>
        public String ParameterStringWithType
        {
            get { return $"{PropertyTypeString} {ColumnVeriableName}"; }
        }

        /// <summary>
        /// Gets the type script observable string.
        /// </summary>
        /// <value>
        /// The type script observable string.
        /// </value>
        public String TypeScriptObservableString
        {
            get { return GetTypeScriptObservableString(ColumnSqlDbType); }
        }

        public string GetJSONValueString(string s)
        {
            if (string.IsNullOrEmpty(s) && IsNullable)
            {
                return "\"\"";
            }

            switch (ColumnSqlDbType)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return $"\"{s.Trim()}\"";
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                    return $"\"{(string.IsNullOrEmpty(s) ? "0" : s)}\"";

                case SqlDbType.Bit:
                    return $"\"{s?.ToLower() ?? "false"}\"";

                case SqlDbType.DateTime:

                    DateTime d = !string.IsNullOrEmpty(s) && DateTime.TryParse(s, out DateTime dt) ? dt : DateTime.MinValue;

                    return d == DateTime.MinValue ? "\"\"" :
                        (d == d.Date ? $"\"{d:yyyy/MM/dd}\""
                                     : $"\"{d:yyyy/MM/dd HH:mm:ss}\"");
            }
        }

        public string GetClassValueString(string s)
        {
            if (string.IsNullOrEmpty(s) && IsNullable)
            {
                return $"null";
            }

            switch (ColumnSqlDbType)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return $"\"{s.Trim()}\"";
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                    return $"{(string.IsNullOrEmpty(s) ? "0" : s)}";

                case SqlDbType.Bit:
                    return $"{s?.ToLower() ?? "false"}";

                case SqlDbType.DateTime:

                    DateTime d = !string.IsNullOrEmpty(s) && DateTime.TryParse(s, out DateTime dt) ? dt : DateTime.MinValue;

                    return d == DateTime.MinValue ?
                        (IsNullable ? "null" : "DateTime.MinValue") :
                        (d == d.Date ? $"new DateTime({d.Year},{d.Month},{d.Day})"
                                     : $"new DateTime({d.Year},{d.Month},{d.Day},{d.Hour},{d.Minute},{d.Second})");
            }
        }

        public string GetColumnValueString(string s)
        {
            if (string.IsNullOrEmpty(s) && IsNullable)
            {
                return $"null";
            }

            switch (ColumnSqlDbType)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return $"'{s}'";
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                    return $"{(string.IsNullOrEmpty(s) ? "0" : s)}";

                case SqlDbType.Bit:
                    return $"{s?.ToLower() ?? "false"}";

                case SqlDbType.DateTime:
                    return $"'{s}'";
            }
        }

        public string GetSqlValueString(string s)
        {
            if (string.IsNullOrEmpty(s) && IsNullable)
            {
                return $"null";
            }

            switch (ColumnSqlDbType)
            {
                default:
                case SqlDbType.VarChar:
                    return $"'{s.Replace("\'", "\'\'")}'";
                case SqlDbType.NVarChar:
                    return $"N'{s.Replace("\'", "\'\'")}'";
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                    return $"{(string.IsNullOrEmpty(s) ? "0" : s)}";

                case SqlDbType.Bit:
                    return $"{((s?.ToLower() ?? "false") == "true" ? "1" : "0")}";

                case SqlDbType.DateTime:
                    DateTime d = !string.IsNullOrEmpty(s) && DateTime.TryParse(s, out DateTime dt) ? dt : DateTime.MinValue;

                    return d == DateTime.MinValue ?
                        (IsNullable ? "null" : "GETDATE()") :
                        (d == d.Date && d.Date == DateTime.Today ? $"CONVERT(DATE,GETDATE(),120)" : $"'{s}'");
            }
        }

        public string GetCSVValueString(string s)
        {
            if (string.IsNullOrEmpty(s) && IsNullable)
            {
                return string.Empty;
            }

            switch (ColumnSqlDbType)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return $"{s.Trim()}";
                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.BigInt:
                case SqlDbType.Int:
                    return $"{(string.IsNullOrEmpty(s) ? "0" : s)}";

                case SqlDbType.Bit:
                    return $"{((s?.ToLower() ?? "false") == "true" ? "1" : "0")}";

                case SqlDbType.DateTime:

                    DateTime d = !string.IsNullOrEmpty(s) && DateTime.TryParse(s, out DateTime dt) ? dt : DateTime.MinValue;

                    return d == DateTime.MinValue ? "" :
                        (d == d.Date ? $"{d:yyyy/MM/dd}"
                                     : $"{d:yyyy/MM/dd HH:mm:ss}");
            }
        }

        /// <summary>
        /// Gets the type script observable string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private String GetTypeScriptObservableString(SqlDbType type)
        {
            switch (type)
            {
                default:
                    return "ko.observable()";
                case SqlDbType.VarChar:
                    return "ko.observable<string>()";
                case SqlDbType.Bit:
                    return "ko.observable<boolean>()";
                case SqlDbType.Int:
                    return "ko.observable<number>()";
            }
        }

        /// <summary>
        /// Gets the column default value string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private String GetColumnDefaultValidatorString(SqlDbType type)
        {
            switch (type)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return $"[StringLength({DataLen})]";

                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    return
                        $"[RegularExpression(@\"^[+-]?\\d*[.]?\\d*$\", ErrorMessage = \"{(String.IsNullOrEmpty(ColumnDescription) ? ColumnName : ColumnDescription)} 輸入格式錯誤，只允許數字\")]";
                case SqlDbType.Int:
                    //case SqlDbType.BigInt:
                    return
                        $"[RegularExpression(@\"\\d+\", ErrorMessage = \"{(String.IsNullOrEmpty(ColumnDescription) ? ColumnName : ColumnDescription)} 輸入格式錯誤，只允許數字\")]";

                case SqlDbType.Bit:
                    return String.Empty;

                case SqlDbType.DateTime:
                    return String.Empty;



            }
        }

        /// <summary>
        /// Gets the property type string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private String GetPropertyTypeString(SqlDbType type)
        {

            switch (type)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return "string";

                case SqlDbType.Decimal:
                case SqlDbType.Float:
                    return "decimal";

                case SqlDbType.BigInt:
                    return "long";

                case SqlDbType.Int:
                    return "int";

                case SqlDbType.Bit:
                    return "bool";

                case SqlDbType.DateTime:
                    return "DateTime";


            }
        }

        /// <summary>
        /// Gets the column default value string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private String GetColumnDefaultValueString(SqlDbType type)
        {
            switch (type)
            {
                default:
                case SqlDbType.VarChar:
                case SqlDbType.NVarChar:
                    return "string.Empty";

                case SqlDbType.Decimal:
                case SqlDbType.Float:
                case SqlDbType.Int:
                case SqlDbType.BigInt:
                    return "0";

                case SqlDbType.Bit:
                    return "false";

                case SqlDbType.DateTime:
                    return "DateTime.MinValue";


            }
        }

        /// <summary>
        /// Gets the column type string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private SqlDbType GetColumnTypeString(String type)
        {
            switch (type)
            {
                default:

                case "NTEXT":
                case "CHAR":
                case "VARCHAR":
                case "TEXT":
                    return SqlDbType.VarChar;
                case "NCHAR":
                case "NVARCHAR":
                    return SqlDbType.NVarChar;
                case "NUMERIC":
                case "DECIMAL":
                    return SqlDbType.Decimal;

                case "FLOAT":
                case "REAL":
                    return SqlDbType.Float;

                case "BIGINT":
                    return SqlDbType.BigInt;

                case "INT":
                case "SMALLINT":
                case "TINYINT":
                    return SqlDbType.Int;

                case "BIT":
                    return SqlDbType.Bit;

                case "DATE":
                case "TIME":
                case "DATETIME":
                case "DATETIME2":
                case "SMALLDATETIME":
                    return SqlDbType.DateTime;
            }
        }
    }

    #endregion

    #region -- Public Class SqlServerDBConstraintsInfo --

    /// <summary>
    /// 
    /// </summary>
    public class SqlServerDBConstraintsInfo
    {
        public SqlServerDBConstraintsInfo(DataRow dr)
        {
            ConstraintsTableName = dr["ConstraintsTableName"].ToString();
            ConstraintsColumn = dr["ConstraintsColumn"].ToString();
            ConstraintsName = dr["ConstraintsName"].ToString();

        }

        public SqlServerDBConstraintsInfo() { }

        /// <summary>
        /// Gets or sets the name of the foreign key table.
        /// </summary>
        /// <value>
        /// The name of the foreign key table.
        /// </value>
        public String ConstraintsTableName { set; get; }

        /// <summary>
        /// Gets or sets the foreign key column.
        /// </summary>
        /// <value>
        /// The foreign key column.
        /// </value>
        public String ConstraintsColumn { set; get; }

        /// <summary>
        /// Gets or sets the name of the foreign key.
        /// </summary>
        /// <value>
        /// The name of the foreign key.
        /// </value>
        public String ConstraintsName { set; get; }
    }

    #endregion

    #region -- Public Class PROMCodes --

    public class PROMCodes
    {
        /// <summary>
        /// CodeType [代碼類別] INT
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// CodeNo [代碼] VARCHAR(16)
        /// </summary>
        public string CodeNo { get; set; }

        /// <summary>
        /// ParentType [父代碼類別] INT
        /// </summary>
        public int? ParentType { get; set; }

        /// <summary>
        /// ParentNo [父代碼] VARCHAR(16)
        /// </summary>
        public string ParentNo { get; set; }

        /// <summary>
        /// CodeName [代碼名稱] NVARCHAR(256)
        /// </summary>
        public string CodeName { get; set; }

        /// <summary>
        /// CodeDescription [代碼描述] NVARCHAR(1024)
        /// </summary>
        public string CodeDescription { get; set; }

        /// <summary>
        /// Status [狀態] BIT()
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// SortNo [排序] INT
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// ModifyTime [更新時間] DATETIME
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }

    #endregion

    #region -- Public Class PROMCodeTypes --

    /// <summary>
    /// 代碼類別檔
    /// </summary>
    public class PROMCodeTypes
    {
        /// <summary>
        /// CodeType [代碼類別] INT
        /// </summary>
        public int CodeType { get; set; }

        /// <summary>
        /// CodeTypeColumn [類別欄位名稱] VARCHAR(64)
        /// </summary>
        public string CodeTypeColumn { get; set; }

        /// <summary>
        /// CodeNoColumn [類別代碼欄位名稱] VARCHAR(64)
        /// </summary>
        public string CodeNoColumn { get; set; }

        /// <summary>
        /// TypeDescription [類別描述] NVARCHAR(1024)
        /// </summary>
        public string TypeDescription { get; set; }

        /// <summary>
        /// ModifyTime [更新時間] DATETIME
        /// </summary>
        public DateTime? ModifyTime { get; set; }
    }

    #endregion

    #region -- Public Class SqlServerIndexInfo --

    public class SqlServerIndexInfo
    {
        public SqlServerIndexInfo(DataRow dr)
        {
            IndexName = dr["IndexName"].ToString();
            IndexColumns = dr["IndexColumns"].ToString();
            IndexType = dr["IndexType"].ToString();
            Unique = dr["Unique"].ToString();
            TableView = dr["TableView"].ToString();
            TableName = dr["TableName"].ToString();
            ObjectType = dr["ObjectType"].ToString();
            CreateIndexScript = dr["CreateIndexScript"].ToString();
            DropIndexScript = dr["DropIndexScript"].ToString();
        }

        public SqlServerIndexInfo() { }

        public string IndexName { get; set; }
        public string IndexColumns { get; set; }
        public string IndexType { get; set; }
        public string Unique { get; set; }
        public string TableView { get; set; }
        public string TableName { get; set; }
        public string ObjectType { get; set; }
        public string CreateIndexScript { get; set; }
        public string DropIndexScript { get; set; }
    }

    #endregion

    #region -- Public Class SqlServerGroupbyValueInfo --

    public class SqlServerGroupbyValueInfo
    {
        public SqlServerGroupbyValueInfo(DataRow dr)
        {
            GroupbyColumn = (dr.IsNull("GroupbyColumn")) ? "null" : dr["GroupbyColumn"].ToString();
            GroupbyCount = int.TryParse(dr["GroupbyCount"].ToString(), out int i) ? i : 0;
        }

        public string GroupbyColumn { set; get; }
        public int GroupbyCount { set; get; }
    }

    #endregion
}
