using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OfficeOpenXml;
using Unity.VisualScripting;
using UnityEngine;

namespace ZLuaFramework
{
    /// <summary>
    /// When change the excel asset in <see cref="PathSetting.ExcelsPath"/> folder
    /// auto generate lua table relative to the excel
    /// </summary>
    public class ExcelToLuaFile
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
            ConvertExcelToLua(add);
        }

        static string[] luaConfigPaths;
        static void DelExcel(string del)
        {
            if (luaConfigPaths == null)
            {
                luaConfigPaths = Directory.GetFiles(LuaConnectConfig.Instance.OutputLuaFolder.assetPath);
            }
            string configName = Path.GetFileName(del).Replace(".xlsx", "");
            foreach (var config in luaConfigPaths)
            {
                if (Path.GetFileName(config).StartsWith(configName))
                {
                    File.Delete(config);
                }
            }
        }

        static bool CheckIsExcelPath(string path)
        {
            return path.StartsWith(LuaConnectConfig.Instance.ExcelFolder.assetPath);
        }
        static void ConvertExcelToLua(string excelFilePath)
        {
            File.SetAttributes(excelFilePath, FileAttributes.Normal);
            FileInfo excelFile = new FileInfo(excelFilePath);
            string excelName = Path.GetFileName(excelFilePath).Replace(".xlsx", "");

            string realSheetName = "";
            int rowr = 0;
            int liner = 0;
            string valued = "";
            try
            {
                ExcelPackage package = new ExcelPackage(excelFile);
                foreach (var worksheet in package.Workbook.Worksheets)
                {
                    realSheetName = worksheet.Name;
                    if (worksheet.Name.StartsWith("Sheet") && worksheet.Name != "Sheet1")
                    {
                        Debug.LogError($"[ExcelToLuaFile] sheet in {excelName}.xlsx cant startwith 'Sheet' except Sheet1");
                        continue;
                    }
                    string sheetName = (worksheet.Name == "Sheet1" || package.Workbook.Worksheets.Count == 1) ? "" : ("_" + worksheet.Name);
                    string tableName = excelName + sheetName;
                    string sheetPath = Path.Combine(LuaConnectConfig.Instance.OutputLuaFolder.assetPath, tableName + ".lua");

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
                        rowr = row;
                        string id = worksheet.Cells[row, 1].Value.ToSafeString();
                        if (id == "(null)")
                            continue;
                        tableString.AppendLine(Space() + "[" + id + "] =");
                        tableString.AppendLine(Space() + "{");

                        for (int col = 1; col <= colCount; col++)
                        {
                            liner = col;
                            string value = worksheet.Cells[row, col].Value.ToSafeString();
                            bool valueIsNull = value.Equals("(null)");
                            valued = value;
                            switch (types[col])
                            {
                                case "number":
                                    value = valueIsNull ? "0" : value;
                                    break;
                                case "string":
                                    value = valueIsNull ? "\"\"" : "\"" + value + "\"";
                                    break;
                                case "bool":
                                    if (valueIsNull)
                                        value = "false";
                                    else
                                    {
                                        value = (value.Equals("1")
                                                || value.ToLower().Equals("true"))
                                                ? "true" : "false";
                                    }

                                    break;
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
                Debug.LogError($"Error when convert xlsx excelName:{excelName},sheetName:{realSheetName},row:{rowr},line:{liner},value:{valued}\n{e.Message}\n{e.StackTrace}");
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

