using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.CSV;
using UnityEngine;

namespace ZLuaFramework
{
    public class CSVToTable
    {
        public static StringTableCollection CSVCollection;

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
                if(LocalizationConnectConfig.Instance.ExcelFile.asset == null)
                    return;

                if (path.Equals(LocalizationConnectConfig.Instance.ExcelFile.assetPath))
                    GenerateTableEntrys(path);
            }                    
        }

        static void GenerateTableEntrys(string path)
        {            
            //read xlsx
            ReadXlsx(path, out string content, out List<string> languageKeys);

            //write csv
            string csvPath = path.Replace("xlsx", "csv");
            WriteCsv(csvPath, content);

            //import collection
            ImportCollection(csvPath);

            //write to lua file
            WriteLua(languageKeys);
            Debug.Log("generate language table");
        }

        static void ReadXlsx(string path, out string content, out List<string> languageKeys)
        {
            StringBuilder contentSb = new StringBuilder();
            languageKeys = new List<string>();

            File.SetAttributes(path, FileAttributes.Normal);
            FileInfo excelFile = new FileInfo(path);
            ExcelPackage package = new ExcelPackage(excelFile);
            
            foreach(var worksheet in package.Workbook.Worksheets)
            {
                for (int i = 1; i <= worksheet.Dimension.Rows; i++)
                {
                    StringBuilder rowStr = new StringBuilder();
                    if ( i == 1 )
                    {
                        rowStr.Append("Id");
                        for (int j = 1; j <= worksheet.Dimension.Columns; j++)
                        {
                            rowStr.Append("," + worksheet.Cells[i, j].Value.ToString());
                        }
                    }
                    else
                    {
                        contentSb.Append("\n");
                        rowStr.Append(i.ToString());
                        for (int j = 1; j <= worksheet.Dimension.Columns; j++)
                        {
                            string cellStr = worksheet.Cells[i, j].Value.ToString();
                            if (j == 1)
                            {
                                languageKeys.Add(cellStr);
                            }
                            rowStr.Append($",\"{cellStr}\"");
                        }
                    }

                    contentSb.Append(rowStr.ToString());
                }
            }
            
            content = contentSb.ToString();
        }

        static void WriteCsv(string csvPath, string content)
        {
            // 写入CSV文件内容
            using (StreamWriter writer = new StreamWriter(csvPath))
            {
                writer.Write(content);               
            }
        }

        static void ImportCollection(string csvPath)
        {
            using (var fs = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                var stream = new StreamReader(fs);

                if (CSVCollection == null)
                {
                    CSVCollection = AssetDatabase.LoadAssetAtPath<StringTableCollection>("Assets/ZLuaFrameWork/LocalizationConnect/LocaleTables/Common.asset");
                }

                Csv.ImportInto(stream, CSVCollection, true, null, true);
            }
        }

        static void WriteLua(List<string> languageKeys)
        {
            if (LocalizationConnectConfig.Instance.OutputLuaKeyFile.asset == null) return;

            using (StreamWriter writer = new StreamWriter(LocalizationConnectConfig.Instance.OutputLuaKeyFile.assetPath))
            {
                writer.WriteLine("--Auto Generate By " + typeof(CSVToTable).ToString());

                writer.WriteLine("LKey = {}");
                foreach (var key in languageKeys) 
                {
                    writer.WriteLine($"LKey.{key} = \"{key}\"");
                }
            }
        }
    }
}

