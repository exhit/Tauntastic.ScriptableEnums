using UnityEditor;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>Asset postprocessor that renames imported ScriptableEnum assets whose name duplicates another asset of the same enum type.</summary>
    public class PreventProjectWideDuplicateScriptableEnumPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            ScriptableEnumEditorUtils.RenameProjectWideDuplicates(importedAssets);
        }
    }
}