using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SqlServerHelper.Core.Extension
{
    public static class ExcelExtensions
    {
        //加入擴充方法: SetQuickStyle，指定前景色/背景色/水平對齊
        public static void SetQuickStyle(this ExcelRange range,
            Color foreColor,
            Color bgColor = default(Color),
            ExcelHorizontalAlignment hAlign = ExcelHorizontalAlignment.Left)
        {
            range.Style.Font.Color.SetColor(foreColor);
            if (bgColor != default(Color))
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(bgColor);
            }
            range.Style.HorizontalAlignment = hAlign;
        }

        public static void SetHyperlink(this ExcelRange range, Uri uri)
        {
            range.Hyperlink = uri;
            range.Style.Font.UnderLine = true;
            range.Style.Font.Color.SetColor(Color.Blue);
        }
    }
}
