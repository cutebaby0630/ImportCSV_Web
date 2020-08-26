using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;

namespace SqlServerHelper.Core.Extension
{
    public static class CollectionsExtension
    {
        public static string ToHTML(this DataTable dt)
        {
            try
            {
                string html = "<table>";
                //add header row
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<td>" + dt.Columns[i].ColumnName + "</td>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table>";

                return html;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }

    public static class ObjectsExtension
    {

    }
}
