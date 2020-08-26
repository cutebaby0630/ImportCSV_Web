using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ImportCSV_Web.core.Models
{
    public class FileData
    {
        public string FileName { get; set; }

        public string FileResult { get; set; }
        public string ServerName { get; set; }
        public DataTable GetCompareTable { get; set; }
    }
}
