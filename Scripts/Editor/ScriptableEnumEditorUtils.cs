using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tauntastic.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Tauntastic.ScriptableEnums.Editor
{
    public static class ScriptableEnumEditorUtils
    {
        private const int _PREDEFINED_COUNT = 25;

        public static VisualElement GetCountCappedScriptableEnumElement(SerializedProperty property,
            FieldInfo fieldInfo, int countThreshold = _PREDEFINED_COUNT)
        {
            if (property.isArray)
            {
                return new PropertyField(property);
            }

            if (fieldInfo.FieldType.IsExistingScriptableObjectCountAbove(countThreshold))
            {
                return new PropertyField(property);
            }

            return new ScriptableEnumField(property, fieldInfo);
        }

        public static bool IsExistingScriptableObjectCountAbove(this Type type, int threshold = _PREDEFINED_COUNT)
        {
            var assets = GetAssetsOfType(type);
            return assets.Count > threshold;
        }

        public static List<ScriptableObject> GetAssetsOfType(Type type)
        {
            return GetAssetsOfType(type.FullName);
        }

        public static List<ScriptableObject> GetAssetsOfType(string type)
        {
            var assets =
                AssetDatabase
                    .FindAssets($"t:{type}")
                    .Select(guid =>
                        AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid)))
                    .Where(obj => obj != null)
                    .ToList();

            return assets;
        }

        public static void RenameProjectWideDuplicates(string[] importedAssets, bool logSuccessfulRenames = false)
        {
            foreach (string importedAssetPath in importedAssets)
            {
                // Load the asset
                Object importedObject = AssetDatabase.LoadMainAssetAtPath(importedAssetPath);

                if (importedObject is not ScriptableEnum { PreventProjectWideDuplicates: true } importedSE)
                    continue;
                
                importedSE.RenameProjectWideDuplicate(importedAssetPath, logSuccessfulRenames);
            }
        }
    }
}