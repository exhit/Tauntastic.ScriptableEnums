using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Tauntastic
{
    using ScriptableEnums;

    [Icon(_PATH_PREFIX + "/com.tauntastic.scriptableenums/Images/d_ScriptableEnum Icon.png")]
    abstract public partial class ScriptableEnum : ScriptableObject
    {
#if UNITY_EDITOR
        public bool PreventProjectWideDuplicates => true;
#endif

        
        private const string _PATH_PREFIX =
#if TAUNTASTIC_ASSETS_PACKAGE
            "Assets/Tauntastic";
#else
            "Packages";
#endif

        [SerializeField]
        [ScriptableEnumsDisable]
        private string _displayText;

        public string DisplayText
        {
            get => _displayText;
            protected set => _displayText = value;
        }


        protected virtual void Awake()
        {
            DisplayText = name;
        }

        protected virtual void OnValidate()
        {
            DisplayText = name;
        }

        #region STATIC

        private static readonly Dictionary<Type, ScriptableEnum[]> _allOptionsCache = new();

        public static implicit operator ScriptableEnum(string textIdentifier)
        {
            var allScriptableEnums = Resources.LoadAll<ScriptableEnum>("");
            var matchingEnums = allScriptableEnums.Where(x => x.DisplayText.Contains(textIdentifier)).ToArray();

            return matchingEnums.Length switch
            {
                0 => throw new Exception($"No scriptable enum found for {textIdentifier}"),
                > 1 => throw new Exception(
                    $"Multiple scriptable enums found for {textIdentifier}, please specify a more specific name"),
                _ => matchingEnums.FirstOrDefault()
            };
        }

        public static T[] GetAll<T>() where T : ScriptableEnum
        {
            var type = typeof(T);
            T[] assets;
#if UNITY_EDITOR
            assets = AssetDatabase
                .FindAssets($"t:{type}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(obj => obj != null)
                .ToArray();
#else
            if (_allOptionsCache.TryGetValue(type, out var ses))
                return ses.Cast<T>().ToArray();
            assets = Resources.LoadAll("", type).Cast<T>().ToArray();
#endif
            _allOptionsCache[type] = assets;
            return assets;
        }

        public static ScriptableEnum[] GetAll(Type type)
        {
            ScriptableEnum[] assets;
#if UNITY_EDITOR
            assets = AssetDatabase
                .FindAssets($"t:{type}")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ScriptableEnum>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(obj => obj != null)
                .ToArray();
#else
            if (_allOptionsCache.TryGetValue(type, out assets))
                return assets;
            assets = Resources.LoadAll("", type).Cast<ScriptableEnum>().ToArray();
#endif
            _allOptionsCache[type] = assets;
            return assets;
        }

        public static T GetByName<T>(string textIdentifier) where T : ScriptableEnum
        {
            return GetByName(typeof(T), textIdentifier) as T;
        }

        public static ScriptableEnum GetByName(Type type, string textIdentifier)
        {
            textIdentifier = textIdentifier.Trim().ToLower();
            var allScriptableEnums = GetAll(type);
            var matchingEnums = allScriptableEnums.Where(x => x.DisplayText.Trim().ToLower() == textIdentifier)
                .ToArray();

            switch (matchingEnums.Length)
            {
                case 0:
                    Debug.LogWarning($"No scriptable enum found for {textIdentifier}");
                    return null;
                case > 1:
                    Debug.LogWarning($"Multiple scriptable enums found for {textIdentifier}, please specify a more specific name");
                    return null;
                default:
                    return matchingEnums.FirstOrDefault();
            }
        }

        public static IEnumerable<ScriptableEnum> GetAllInstances(Type type)
        {
            return GetAll(type);
        }

        public static void SetByName<T>(ref T se, string name) where T : ScriptableEnum
        {
            var getByName = GetByName(typeof(T), name);
            se = getByName as T;
        }

        public static bool TrySetIfNullOrWrongName<T>(ref T se, string name) where T : ScriptableEnum
        {
            if (se != null && se.name == name) return false;
            SetByName(ref se, name);
            return se != null;
        }

        public static bool TrySetIfNullOrWrongName<T>(ref T se) where T : ScriptableEnum
        {
            if (se != null) return false;
            se = ScriptableEnum.GetAll<T>().FirstOrDefault();
            return se != null;
        }
        
        public static bool TrySetIfNullOrWrongName<T>(ICollection<T> collection, string name) where T : ScriptableEnum
        {
            if (collection.Any(x => x.name == name)) return false;
            T se = null;
            SetByName(ref se, name);
            if (se == null) return false;
            collection.Add(se);
            return true;
        }

        public static bool TrySetIfNullOrWrongName<T>(Dictionary<string, T> dictionary, string name) where T : ScriptableEnum
        {
            if (dictionary == null || dictionary.TryGetValue(name, out T se)) return false;
            SetByName(ref se, name);
            if (se == null) return false;
            if (dictionary.TryAdd(name, se)) return true;
            dictionary[name] = se;
            return true;
        }

        #endregion
    }
}