using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace MTG
{
    /// <summary>
    /// When change the excel asset in <see cref="PathSetting.ExcelsPath"/> folder
    /// auto generate lua table relative to the excel
    /// </summary>
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
            ConvertExcelToLua(add);
        }

        static string[] luaConfigPaths;
        static void DelExcel(string del)
        {
            if (luaConfigPaths == null)
            {
                luaConfigPaths = Directory.GetFiles(PathSetting.ExcelsToLuaOutputPath);              
            }
            string configName = Path.GetFileName(del).Replace(".xlsx", "");
            foreach(var config in luaConfigPaths)
            {
                if(Path.GetFileName(config).StartsWith(configName))
                {
                    File.Delete(config);
                }
            }
        }

        static bool CheckIsExcelPath(string path)
        {
            return path.StartsWith(PathSetting.ExcelsPath);
        }
        static void ConvertExcelToLua(string excelFilePath)
        {
            File.SetAttributes(excelFilePath, FileAttributes.Normal);
            FileInfo excelFile = new FileInfo(excelFilePath);

            string excelName = Path.GetFileName(excelFilePath).Replace(".xlsx", "");
            try
            {
                ExcelPackage package = new ExcelPackage(excelFile);
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    string sheetName = (worksheet.Name == "Sheet1" || package.Workbook.Worksheets.Count == 1) ? "" : ("_" + worksheet.Name);
                    string tableName = excelName + sheetName;
                    string sheetPath = Path.Combine(PathSetting.ExcelsToLuaOutputPath, tableName + ".lua");

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
                    tableString.AppendLine("-- Auto Generate By " + excelFilePath + "\n");
                    tableString.AppendLine("local " + tableName + " =");
                    tableString.AppendLine("{");
                    for (int row = 4; row <= rowCount; row++)
                    {
                        tableString.AppendLine(Space() + "[" + worksheet.Cells[row, 1].Value.ToString() + "] =");
                        tableString.AppendLine(Space() + "{");
                        for (int col = 1; col <= colCount; col++)
                        {
                            string value = worksheet.Cells[row, col].Value.ToSafeString();
                            if (value.Equals("(null)"))
                            {
                                value = "nil";
                            }
                            else
                            {
                                switch (types[col])
                                {
                                    /*case "int":
                                        value = value;
                                        break;*/
                                    case "string":
                                        value = "\"" + value + "\"";
                                        break;
                                    case "bool":
                                        value =
                                            (value.Equals("1") ||
                                             value.ToLower().Equals("true"))
                                            ? "true" : "false";
                                        break;
                                }
                            }

                            tableString.AppendLine(Space(2) + names[col] + " = " + value + ",");
                        }
                        tableString.AppendLine(Space() + "},");
                    }
                    tableString.AppendLine("}\n");
                    tableString.AppendLine("return " + tableName);

                    File.WriteAllText(sheetPath, tableString.ToString());
                    Debug.Log("Excel data has been successfully converted to " + sheetPath);
                }
                package.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogWarning("Error when convert xlsx " + "\n" + e.Message + "\n" + e.StackTrace);
            }
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

