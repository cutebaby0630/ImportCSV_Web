using System;
using System.Data;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SqlServerHelper.Core;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImportCSV_Web.core.Models
{
    public class ImportService
    {
        public void BackupFile(string filepath, string filename)
        {
            var tablename = filename;
            DirectoryInfo readlocalfile = new DirectoryInfo(filepath);

            //連線SSMS 取得連線字串
            IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsetting.json", optional: true, reloadOnChange: true).Build();
            string connString = config.GetConnectionString("DefaultConnection");
            SqlServerDBHelper sqlHelper = new SqlServerDBHelper(string.Format(connString, "HISDB", "msdba", "1qaz@wsx"));
            SqlServerTableHelper sqltablehelper = new SqlServerTableHelper(string.Format(connString, "HISDB", "msdba", "1qaz@wsx"));
            List<SqlServerDBColumnInfo> tableList = sqltablehelper.FillTableList($"{tablename}").FillColumnList().GetTableList().First().SqlServerDBColumnList;
            for (int x = 0; x <= tableList.Count - 1; x++)
            {
                if (tableList[x].DataTypeName == "BIT()")
                {
                    tableList[x].DataTypeName = "BIT";
                }
            }
            //Step1.2. 檔案讀取→\\10.1.225.17\d$\csv  \\10.1.225.17\d$\CSV_20200721
            var host = @"10.1.225.17";
            var RDPfile = "CSV";
            var username = @"TW-VYIN-207\Administrator";
            var password = "p@ssw0rd";
            string old_path = "";
            string new_path = $@"\\{host}\d$\{RDPfile}\" + tablename + ".csv";
            using (new RDPCredentials(host, username, password))
            {

                //Step1.3. 找到相對應File
                DirectoryInfo readfile = new DirectoryInfo($@"\\{host}\d$\{RDPfile}\{tablename}.csv");
                //Step1.4. 將File中的資料存入var
                string LastWriteTime = File.GetLastWriteTime(readfile.ToString()).ToString("yyyyMMdd");
                old_path = $@"\\{host}\d$\{RDPfile}\" + tablename + "_" + LastWriteTime + ".csv";

                //Step1.5. 修改名稱(原File_修改日期yyyyMMdd)--備份
                readfile.MoveTo(old_path);

                //Step1.6. 將下載的File 複製到 mstv
                File.Copy(readlocalfile.ToString(), new_path);

            }
        }
    }
    #region -- connect RDP --
    class RDPCredentials : IDisposable
    {
        private string Host { get; }

        public RDPCredentials(string Host, string UserName, string Password)
        {
            var cmdkey = new Process
            {
                StartInfo =
            {
                FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe"),
                Arguments = $@"/list",
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true
            }
            };
            cmdkey.Start();
            cmdkey.WaitForExit();
            if (!cmdkey.StandardOutput.ReadToEnd().Contains($@"TERMSRV/{Host}"))
            {
                this.Host = Host;
                cmdkey = new Process
                {
                    StartInfo =
            {
                FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe"),
                Arguments = $@"/generic:DOMAIN/{Host} /user:{UserName} /pass:{Password}",
                WindowStyle = ProcessWindowStyle.Hidden
            }
                };
                cmdkey.Start();
            }
        }

        public void Dispose()
        {
            if (Host != null)
            {
                var cmdkey = new Process
                {
                    StartInfo =
            {
                FileName = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\cmdkey.exe"),
                Arguments = $@"/delete:TERMSRV/{Host}",
                WindowStyle = ProcessWindowStyle.Hidden
            }
                };
                cmdkey.Start();
            }
        }
    }
    #endregion
}
