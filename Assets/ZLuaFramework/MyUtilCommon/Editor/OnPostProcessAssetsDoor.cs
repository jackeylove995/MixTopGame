using UnityEditor;

namespace ZLuaFramework
{
    public class OnPostProcessAssetsDoor : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            ExcelToLuaFile.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            AutoMarkAddress.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            CSVToTable.OnPostprocessAllAssets(importedAssets, null, null, null);
            // 刷新Unity编辑器资源
            AssetDatabase.Refresh();
        }
    }
}

