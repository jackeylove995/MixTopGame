using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MTG
{
    public class ExcelToLuaConfig
    {
        public static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            List<string> addOrChange = new List<string>();
            List<string> deleteOrRemoved = new List<string>();
            foreach (string path in importedAssets)
            {
                addOrChange.Add(path);
            }

            foreach (string path in deletedAssets)
            {
                deleteOrRemoved.Add(path);
            }

            foreach (string path in movedAssets)
            {
                addOrChange.Add(path);
            }

            foreach (string path in movedFromAssetPaths)
            {
                deleteOrRemoved.Add(path);
            }

            AddExcels(addOrChange);
            DelExcels(deleteOrRemoved);
        }

        static void AddExcels(List<string> adds)
        {
            foreach (var add in adds)
            {
                AddExcel(add);
            }
        }

        static void DelExcels(List<string> dels)
        {
            foreach (var del in dels)
            {
                DelExcel(del);
            }
        }

        static void AddExcel(string add)
        {

            if (add.Contains(PathSetting.ExcelsPath))
            {
                PathSetting.SafeCreateDirectory(PathSetting.ExcelsToLuaOutputPath);
                string luaFilePath = add.Replace("DevelopAssets", "HotFixAssets/Lua").Replace(".xlsx", ".lua");
                ConvertExcelToLua(add, luaFilePath);
            }

        }

        static void DelExcel(string del)
        {

        }

        static void ConvertExcelToLua(string excelFilePath, string txtFilePath)
        {
            File.SetAttributes(excelFilePath, FileAttributes.Normal);
            FileInfo excelFile = new FileInfo(excelFilePath);
            Debug.Log("excel path:" + excelFile);
            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["sheet1"]; // 第一个工作表

                StreamWriter writer = new StreamWriter(txtFilePath);

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;

                for (int row = 1; row <= rowCount; row++)
                {
                    string rowContent = "";
                    for (int col = 1; col <= colCount; col++)
                    {
                        rowContent += worksheet.Cells[row, col].Value.ToString() + "\t"; // 使用制表符分隔每个单元格的值
                    }
                    writer.WriteLine(rowContent);
                }

                writer.Close();
            }

            Debug.Log("Excel data has been successfully converted to " + txtFilePath);
        }
    }
}

