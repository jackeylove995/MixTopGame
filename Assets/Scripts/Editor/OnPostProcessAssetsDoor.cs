using UnityEditor;

namespace MTG
{
    public class OnPostProcessAssetsDoor : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            ExcelToLuaConfig.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            AutoMarkAddress.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            ChangeRoleFrameSpritesName.OnPostprocessAllAssets(importedAssets, deletedAssets, movedAssets, movedFromAssetPaths);
            // 刷新Unity编辑器资源
            AssetDatabase.Refresh();
        }
    }
}

