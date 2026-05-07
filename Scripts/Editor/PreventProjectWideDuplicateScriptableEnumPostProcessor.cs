using UnityEditor;

namespace Tauntastic.ScriptableEnums.Editor
{
    public class PreventProjectWideDuplicateScriptableEnumPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            ScriptableEnumEditorUtils.RenameProjectWideDuplicates(importedAssets);
        }
    }
}