// Generated from ARTIFICIAL_INTELLIGENCE_README.md of com.tauntastic.scriptableenums.
// Keeps the paragraph below in sync with Assets/ARTIFICIAL_INTELLIGENCE_README.md so AI agents can read how to use this package.
namespace Tauntastic.AIReadme
{
    [UnityEditor.InitializeOnLoad]
    internal static class ScriptableEnumsAIReadmeInjector
    {
        private const string FileName = "ARTIFICIAL_INTELLIGENCE_README.md";
        private const string Header = "# ARTIFICIAL INTELLIGENCE README";
        private const string PackageKey = "Package \"com.tauntastic.scriptableenums\"";

        private const string Paragraph = @"Package ""com.tauntastic.scriptableenums"" (assemblies Tauntastic.ScriptableEnums and Tauntastic.ScriptableEnums.Editor, types in namespace Tauntastic, no dependencies): enums implemented as ScriptableObject assets. Create an enum type by deriving from the abstract class ScriptableEnum (there is no generic ScriptableEnum<T> and no DisplayText property) with a [CreateAssetMenu], then create one asset per value; the asset name is the identifier and values may hold extra fields and methods. Declare a field of your enum type to get a searchable dropdown of all its assets, use [ScriptableEnum] to get the same drawer on a ScriptableObject field of a type that is not itself drawn that way, [ScriptableEnumsDisable] to grey a field out, and ScriptableEnum.Flags<T> (members TypedValues, Values, Type, implicit conversions to List<T>, List<ScriptableEnum> and Type) for a serialized multi-select. Query values with ScriptableEnum.GetAll<T>(), GetAll(Type), GetByName<T>(string) (trimmed and case-insensitive, returns null and logs a warning when zero or several assets match), GetByName(Type, string), GetAllInstances(Type), SetByName(ref T, string) and the TrySetIfNullOrWrongName overloads (ref T only, ref T plus name, ICollection<T> plus name, Dictionary<string, T> plus name, and the object extension version in ScriptableEnumUtils), and note the implicit conversion from string searches Resources and throws on zero or several matches. In the Editor lookups use AssetDatabase so assets may live anywhere, but in player builds they use Resources.LoadAll with a per-type cache, so asset files that must be found at runtime have to be in a Resources folder; call GetAll once and cache the list instead of per frame. An editor AssetPostprocessor (PreventProjectWideDuplicateScriptableEnumPostProcessor) renames imported assets that duplicate another asset's name for the same enum type, so choose unique names up front, and never rename or delete value assets that scenes or prefabs reference without checking references first. Drawers, ScriptableEnumField, ScriptableEnumFlagsField and ScriptableEnumEditorUtils are editor-only and must not be used from runtime code.";

        static ScriptableEnumsAIReadmeInjector()
        {
            UnityEditor.EditorApplication.delayCall += Inject;
        }

        private static void Inject()
        {
            if (UnityEditor.EditorApplication.isCompiling || UnityEditor.EditorApplication.isUpdating)
            {
                UnityEditor.EditorApplication.delayCall += Inject;
                return;
            }

            try
            {
                string path = System.IO.Path.Combine(UnityEngine.Application.dataPath, FileName);
                bool fileExisted = System.IO.File.Exists(path);
                string existingText = fileExisted ? Normalize(System.IO.File.ReadAllText(path)) : string.Empty;

                var paragraphs = new System.Collections.Generic.List<string>();
                if (fileExisted)
                {
                    foreach (string block in existingText.Split(new[] { "\n\n" }, System.StringSplitOptions.RemoveEmptyEntries))
                    {
                        string trimmed = block.Trim();
                        if (trimmed.Length > 0)
                            paragraphs.Add(trimmed);
                    }
                }
                else
                {
                    paragraphs.Add(Header);
                }

                // Identical paragraph already present: add nothing.
                bool alreadyPresent = paragraphs.Contains(Paragraph);

                // Drop every other paragraph that refers to this same package (outdated versions).
                paragraphs.RemoveAll(p => p.StartsWith(PackageKey, System.StringComparison.Ordinal) && !string.Equals(p, Paragraph, System.StringComparison.Ordinal));

                if (!alreadyPresent)
                    paragraphs.Add(Paragraph);

                string newText = string.Join("\n\n", paragraphs) + "\n";
                if (fileExisted && string.Equals(newText, existingText, System.StringComparison.Ordinal))
                    return;

                System.IO.File.WriteAllText(path, newText, new System.Text.UTF8Encoding(false));
                UnityEditor.AssetDatabase.ImportAsset("Assets/" + FileName);
            }
            catch (System.Exception exception)
            {
                UnityEngine.Debug.LogWarning("[ScriptableEnumsAIReadmeInjector] Could not update Assets/" + FileName + ": " + exception.Message);
            }
        }

        private static string Normalize(string text)
        {
            return text.Replace("\r\n", "\n").Replace("\r", "\n");
        }
    }
}