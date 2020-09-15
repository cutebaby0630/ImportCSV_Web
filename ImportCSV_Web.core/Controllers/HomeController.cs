using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImportCSV_Web.core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ImportCSV_Web.core.Controllers
{
  public class HomeController : Controller
  {
    private readonly ILogger<HomeController> _logger;
    private readonly Models.ImportService importService = new ImportService();

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    public IActionResult Index()
    {
      return View();
    }

    [HttpPost]
    public IActionResult Index([FromForm] Dictionary<string, string> fields, IFormFileCollection files)
    {
      // IFormFile.Name: 此檔案欄位在表單中的名稱
      // IFormFile.FileName: 檔案來源名稱 (無路徑)
      // IFormFile.Length: 檔案大小
      foreach (var file in files)
      {
        string filename = file.FileName;//取得檔名字
        var tablename = filename.Replace(".csv", "");

        if (file.Length > 0)
        {

          var stream = new FileStream(Path.GetFileName("TempFile"), FileMode.Open);
            file.CopyToAsync(stream);
          
        }
      }
      return View();
    }
      public IActionResult Privacy()
      {
        return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }
    }
  }
