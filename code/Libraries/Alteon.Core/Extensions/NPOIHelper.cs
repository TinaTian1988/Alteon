using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

using NPOI.HSSF.UserModel;
using System.Reflection;

namespace Alteon.Core.Extensions
{
    /// <summary>
    /// 用npoi组件操作Excel
    /// </summary>
    public class NPOIHelper
    {
        /// <summary>
        /// 把DataTable的数据填充到Excel模板并保存
        /// </summary>
        /// <param name="templatePath">Excel模板的完整路径</param>
        /// <param name="targetPath">输出Excel的完整路径</param>
        /// <param name="dt">数据源</param>
        /// <returns>填充是否成功</returns>
        public static bool GenReport(string templatePath, string targetPath, DataTable dt)
        {
            return GenReport(templatePath, targetPath, dt, 0, 0);
        }

        /// <summary>
        /// 把DataTable的数据填充到Excel模板并保存
        /// </summary>
        /// <param name="templatePath">Excel模板的完整路径</param>
        /// <param name="targetPath">输出Excel的完整路径</param>
        /// <param name="dt">数据源</param>
        /// <param name="startRowIdx">开始填充的行号(适合模板有表头和列名的情况)</param>
        /// <returns>填充是否成功</returns>
        public static bool GenReport(string templatePath, string targetPath, DataTable dt, int startRowIdx)
        {
            return GenReport(templatePath, targetPath, dt, startRowIdx, 0);
        }

        /// <summary>
        /// 把DataTable的数据填充到Excel模板并保存
        /// </summary>
        /// <param name="templatePath">Excel模板的完整路径</param>
        /// <param name="targetPath">输出Excel的完整路径</param>
        /// <param name="dt">数据源</param>
        /// <param name="startRowIdx">开始填充的行号(适合模板有表头和列名的情况)</param>
        /// <param name="startColIdx">开始填充的列号(适合模板有行名的情况)</param>
        /// <returns>填充是否成功</returns>
        public static bool GenReport(string templatePath, string targetPath, DataTable dt, int startRowIdx, int startColIdx)
        {
            bool flag = false;
            if (false == string.IsNullOrEmpty(templatePath))
            {
                if (false == string.IsNullOrEmpty(targetPath))
                {
                    if (null != dt)
                    {
                        if (true == File.Exists(templatePath))
                        {

                            int rowLen = dt.Rows.Count;
                            int colLen = dt.Columns.Count;
                            FileStream inputFile = new FileStream(templatePath, FileMode.Open, FileAccess.Read);
                            HSSFWorkbook hssfworkbook = new HSSFWorkbook(inputFile);
                            if (hssfworkbook.NumberOfSheets == 0)
                            {
                                hssfworkbook.CreateSheet("Sheet1");
                            }
                            HSSFSheet sheet = (HSSFSheet)hssfworkbook.GetSheetAt(0);
                            HSSFRow row;
                            HSSFCell cell;
                            string val;
                            for (int rowIdx = 0; rowIdx < rowLen; rowIdx++)
                            {
                                row = (HSSFRow)sheet.CreateRow(startRowIdx + rowIdx);
                                for (int colIdx = 0; colIdx < colLen; colIdx++)
                                {
                                    cell = (HSSFCell)row.CreateCell(startColIdx + colIdx);
                                    val = "" + dt.Rows[rowIdx][colIdx];
                                    var column = dt.Columns[colIdx];

                                    #region  ====old====

                                    //if (val is bool)
                                    //{
                                    //    cell.SetCellValue(Convert.ToBoolean(val));
                                    //}
                                    //else if (val is DateTime)
                                    //{
                                    //    cell.SetCellValue(Convert.ToDateTime(val));
                                    //}
                                    //else if (val is double || val is int)
                                    //{
                                    //    cell.SetCellValue(Convert.ToDouble(val));
                                    //}
                                    //else
                                    //{
                                    //    cell.SetCellValue("" + val);
                                    //}

                                    #endregion

                                    #region  ====new====

                                    switch (column.DataType.ToString())
                                    {
                                        case "System.String"://字符串类型
                                            cell.SetCellValue(val);
                                            break;
                                        case "System.DateTime"://日期类型
                                            if (true == val.AsNullOrWhiteSpace())
                                            {
                                                cell.SetCellValue("");
                                            }
                                            else
                                            {
                                                DateTime dateV = DateTime.Now;
                                                DateTime.TryParse(val, out dateV);
                                                cell.SetCellValue(dateV);
                                            }
                                            //cell.CellStyle = dateStyle;//格式化显示
                                            break;
                                        case "System.Boolean"://布尔型
                                            bool boolV = false;
                                            bool.TryParse(val, out boolV);
                                            cell.SetCellValue(boolV);
                                            break;
                                        case "System.Int16"://整型
                                        case "System.Int32":
                                        case "System.Int64":
                                        case "System.Byte":
                                            int intV = 0;
                                            int.TryParse(val, out intV);
                                            cell.SetCellValue(intV);
                                            break;
                                        case "System.Decimal"://浮点型
                                        case "System.Double":
                                            double doubV = 0;
                                            double.TryParse(val, out doubV);
                                            cell.SetCellValue(doubV);
                                            break;
                                        case "System.DBNull"://空值处理
                                            cell.SetCellValue("");
                                            break;
                                        default:
                                            cell.SetCellValue("");
                                            break;
                                    }
                                    #endregion
                                }
                            }
                            sheet.ForceFormulaRecalculation = true;

                            if (false == Directory.Exists(Path.GetDirectoryName(targetPath)))
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                            }
                            FileStream outputFile = new FileStream(targetPath, FileMode.Create);
                            hssfworkbook.Write(outputFile);
                            outputFile.Close();
                            flag = true;

                        }
                        else
                        {
                            throw new ArgumentException("文件 " + templatePath + " 不存在");
                        }
                    }
                    else
                    {
                        throw new ArgumentNullException("数据源DataTable为空");
                    }
                }
                else
                {
                    throw new ArgumentNullException("目标文件路径targetPath为空");
                }
            }
            else
            {
                throw new ArgumentNullException("模板文件路径templatePath为空");
            }
            return flag;
        }
        /// <summary>
        /// 根据传入的LIST对象生成EXCEL文件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="targetPath">目标完全路径名</param>
        /// <param name="SheetName">表名</param>
        /// <param name="lst">待导出列表</param>
        /// <param name="lstTitle">列名集合</param>
        public static bool ListExcel<T>(string targetPath, List<T> lst, List<string> lstTitle, string SheetName = "1")
        {
            bool flag = false;
            HSSFWorkbook hssfworkbook = new HSSFWorkbook();
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.CreateSheet(SheetName);

            T _t = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] propertys = _t.GetType().GetProperties();

            //给定的标题为空,赋值T默认的列名
            if (lstTitle == null)
            {
                lstTitle = new List<string>();
                for (int ycount = 0; ycount < propertys.Length; ycount++)
                {
                    lstTitle.Add(((System.Reflection.MemberInfo)(propertys[ycount])).Name);//获取实体中列名称,去掉列类型
                }
            }

            HSSFRow hsTitleRow = (HSSFRow)sheet1.CreateRow(0);
            //标题赋值
            for (int yt = 0; yt < lstTitle.Count; yt++)
            {
                hsTitleRow.CreateCell(yt).SetCellValue(lstTitle[yt]);
            }

            //填充数据项
            for (int xcount = 1; xcount < lst.Count; xcount++)
            {
                HSSFRow hsBodyRow = (HSSFRow)sheet1.CreateRow(xcount);

                for (int ycBody = 0; ycBody < propertys.Length; ycBody++)
                {
                    PropertyInfo pi = propertys[ycBody];
                    object obj = pi.GetValue(lst[xcount], null);
                    HSSFCell cell = (HSSFCell)hsBodyRow.CreateCell(ycBody);
                    if (obj == null)
                    {
                        cell.SetCellValue("");
                    }
                    else
                    {
                        switch ((Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType).ToString())
                        {
                            case "System.String"://字符串类型
                                cell.SetCellValue((obj ?? "").ToString());
                                break;
                            case "System.DateTime"://日期类型
                                DateTime dateV = DateTime.Now;
                                DateTime.TryParse(obj.ToString(), out dateV);
                                cell.SetCellValue(dateV);
                                HSSFCellStyle cellStyle = (HSSFCellStyle)hssfworkbook.CreateCellStyle();
                                HSSFDataFormat format = (HSSFDataFormat)hssfworkbook.CreateDataFormat();
                                cellStyle.DataFormat = format.GetFormat("yyyy年m月d日");
                                cell.CellStyle = cellStyle;
                                break;
                            case "System.Boolean"://布尔型
                                bool boolV = false;
                                bool.TryParse(obj.ToString(), out boolV);
                                cell.SetCellValue(boolV);
                                break;
                            case "System.Int16"://整型
                            case "System.Int32":
                            case "System.Int64":
                            case "System.Byte":
                                int intV = 0;
                                int.TryParse(obj.ToString(), out intV);
                                cell.SetCellValue(intV);
                                break;
                            case "System.Decimal"://浮点型
                            case "System.Double":
                                double doubV = 0;
                                double.TryParse(obj.ToString(), out doubV);
                                cell.SetCellValue(doubV);
                                break;
                            case "System.DBNull"://空值处理
                                cell.SetCellValue("");
                                break;
                            default:
                                cell.SetCellValue("");
                                break;
                        }
                    }
                }
            }
            if (false == Directory.Exists(Path.GetDirectoryName(targetPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
            }
            FileStream outputFile = new FileStream(targetPath, FileMode.Create);
            hssfworkbook.Write(outputFile);
            outputFile.Close();
            flag = true;
            return flag;
        }

    }
}
