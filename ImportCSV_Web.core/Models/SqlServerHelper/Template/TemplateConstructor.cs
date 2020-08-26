using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImportCSV_Web.core.Models.SqlServerHelper.Template
{
    public partial class TableInfoTemplate
    {
        private List<string> TableNameList { set; get; }

        public TableInfoTemplate(params string[] tableList)
        {
            TableNameList = tableList.ToList();
        }
    }

    public partial class ColumnInfoTemplate
    {
        private List<string> TableNameList { set; get; }

        public ColumnInfoTemplate(params string[] tableList)
        {
            TableNameList = tableList.ToList();
        }
    }
}
