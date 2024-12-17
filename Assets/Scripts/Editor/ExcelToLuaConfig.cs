using System.Collections.Generic;
using System.IO;
using System.Text;
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

            Dels(deleteOrRemoved);
            Adds(addOrChange);
        }

        static void Adds(List<string> adds)
        {
            foreach (var add in adds)
            {
                if (CheckIsExcelPath(add))
                    AddExcel(add);
            }
        }

        static void Dels(List<string> dels)
        {
            foreach (var del in dels)
            {
                if (CheckIsExcelPath(del))
                    DelExcel(del);
            }
        }

        static void AddExcel(string add)
        {
            PathSetting.SafeCreateDirectory(PathSetting.ExcelsToLuaOutputPath);
            ConvertExcelToLua(add, ConvertExcelToLuaPath(add));
        }

        static void DelExcel(string del)
        {
            File.Delete(ConvertExcelToLuaPath(del));
        }

        static string ConvertExcelToLuaPath(string excelPath)
        {
            return excelPath.Replace("DevelopAssets", "HotFixAssets/Lua").Replace(".xlsx", ".lua");
        }


        static bool CheckIsExcelPath(string path)
        {
            return path.Contains("DevelopAssets") && path.Contains("Excels");
        }
        static void ConvertExcelToLua(string excelFilePath, string txtFilePath)
        {
            File.SetAttributes(excelFilePath, FileAttributes.Normal);
            FileInfo excelFile = new FileInfo(excelFilePath);
            Debug.Log("excel path:" + excelFile);
            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["sheet1"]; // 第一个工作表
                string tableName = Path.GetFileName(excelFilePath).Replace(".xlsx", "") + "_" + "sheet1";

                //第一行名称
                //第二行类型
                //第三行描述
                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                if (rowCount < 4)
                {
                    return;
                }

                //第一行名称
                string[] names = new string[colCount + 1];
                for (int i = 1; i <= colCount; i++)
                {
                    names[i] = worksheet.Cells[1, i].Value.ToString();
                }

                //第二行类型
                string[] types = new string[colCount + 1];
                for (int i = 1; i <= colCount; i++)
                {
                    types[i] = worksheet.Cells[2, i].Value.ToString();
                }

                StringBuilder tableString = new StringBuilder();
                tableString.AppendLine("local " + tableName + " =");
                tableString.AppendLine("{");
                for (int row = 4; row <= rowCount; row++)
                {
                    tableString.AppendLine(Space() + "[" + worksheet.Cells[row, 1].Value.ToString() + "] =");
                    tableString.AppendLine(Space() + "{");
                    for (int col = 2; col <= colCount; col++)
                    {
                        string value = worksheet.Cells[row, col].Value.ToSafeString();
                        switch (types[col])
                        {
                            case "int":
                                value = value;
                                break;
                            case "string":
                                value = "\"" + value + "\"";
                                break;
                            case "bool":
                                value = value == "1" ? "true" : "false";
                                break;
                        }
                        tableString.AppendLine(Space(2) + names[col] + " = " + value + ",");
                    }
                    tableString.AppendLine(Space() + "},");
                }
                tableString.AppendLine("}");
                tableString.AppendLine("return " + tableName);

                File.WriteAllText(txtFilePath, tableString.ToString());

            }

            Debug.Log("Excel data has been successfully converted to " + txtFilePath);
        }


        static Dictionary<int, string> spaceCache;
        static string Space(int count = 1)
        {
            if (spaceCache == null)
            {
                spaceCache = new Dictionary<int, string>();
            }
            if (spaceCache.ContainsKey(count))
            {
                return spaceCache[count];
            }

            string newSpace = "";
            for (int i = 0; i < count; i++)
            {
                newSpace += "\t";
            }
            spaceCache[count] = newSpace;
            return spaceCache[count];
        }
    }
}

