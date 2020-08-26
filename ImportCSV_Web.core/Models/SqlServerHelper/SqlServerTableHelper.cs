using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SqlServerHelper.Core
{
    public class SqlServerTableHelper
    {
        /*
 SqlServerTableHelper sqlHelper = new SqlServerTableHelper(string.Format(Properties.Settings.Default.CompareConnectString, "HISDB"));

 List<SqlServerDBTableInfo> _tableList = sqlHelper.FillTableList("ODRTOrderIndex").FillColumnList().FillIndexList().FillConstraintList().GetTableList();

 return;
 */

        private SqlServerDBHelper _dbHelper { get; set; }

        private List<SqlServerDBTableInfo> _tableList { get; set; }

        public SqlServerTableHelper(String connectString)
        {
            _dbHelper = new SqlServerDBHelper(connectString);
        }

        public SqlServerTableHelper(SqlServerDBHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public SqlServerTableHelper FillTableList(params string[] tableList)
        {
            _tableList = _dbHelper.GetTableListAsync()?.Result?.Where(p => tableList.Contains(p.TableName)).ToList();

            return this;
        }

        public SqlServerTableHelper FillAllTableList()
        {
            _tableList = _dbHelper.GetTableListAsync()?.Result.ToList();

            return this;
        }


        public SqlServerTableHelper FillColumnList()
        {
            List<SqlServerDBColumnInfo> sqlServerDBColumnList = _dbHelper.GetColumnListAsync(_tableList.Select(p => p.TableName).ToArray())?.Result.ToList();

            foreach (SqlServerDBTableInfo tableInfo in _tableList)
            {
                tableInfo.SqlServerDBColumnList =
                    (sqlServerDBColumnList.FindAll(p => p.TableName == tableInfo.TableName) == null)
                        ? new List<SqlServerDBColumnInfo>()
                        : sqlServerDBColumnList.FindAll(p => p.TableName == tableInfo.TableName).ToList();
            }

            return this;
        }

        public SqlServerTableHelper FillConstraintList()
        {
            List<SqlServerDBConstraintsInfo> constraints = _dbHelper.FillConstraintsListAsync()?.Result.ToList();

            foreach (SqlServerDBConstraintsInfo info in constraints)
            {
                SqlServerDBColumnInfo column = _tableList?.Find(p => p.TableName == info.ConstraintsTableName)?.SqlServerDBColumnList?.Find(p => p.ColumnName == info.ConstraintsColumn);

                if (column == null) { continue; }

                if (column.ColumnConstraints == null)
                {
                    column.ColumnConstraints = new List<SqlServerDBConstraintsInfo>();
                }

                column.ColumnConstraints.Add(info);
            }

            return this;
        }

        public SqlServerTableHelper FillIndexList()
        {
            List<SqlServerIndexInfo> indexList = _dbHelper.FillIndexListAsync()?.Result.ToList();

            foreach (SqlServerDBTableInfo table in _tableList)
            {
                table.SqlServerDBIndexList = indexList?.Where(p => p.TableName == table.TableName)?.ToList();
            }

            return this;
        }

        public List<SqlServerDBTableInfo> GetTableList()
        {
            return _tableList;
        }
    }
}
