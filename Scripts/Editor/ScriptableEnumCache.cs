using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>
    /// Shared per-type cache of ScriptableObject assets used by the scriptable enum drawers.
    /// Invalidated only when assets are actually imported, moved or deleted.
    /// </summary>
    public static class ScriptableEnumCache
    {
        public const string NullChoice = "<null>";

        public class Entry
        {
            public List<ScriptableObject> Assets;
            public List<string> Choices;
            public Dictionary<string, ScriptableObject> NameToAsset;
            public Dictionary<ScriptableObject, string> AssetToName;

            public string GetDisplayName(ScriptableObject asset)
            {
                if (asset == null) return NullChoice;
                return AssetToName.TryGetValue(asset, out string name) ? name : asset.name;
            }
        }

        private static readonly Dictionary<Type, Entry> _cache = new();

        /// <summary>Incremented every time the cache is invalidated.</summary>
        public static int Version { get; private set; }

        public static event Action Invalidated;

        public static Entry Get(Type type)
        {
            if (_cache.TryGetValue(type, out Entry entry))
                return entry;

            entry = Build(type);
            _cache[type] = entry;
            return entry;
        }

        public static void Invalidate()
        {
            _cache.Clear();
            Version++;
            Invalidated?.Invoke();
        }

        private static Entry Build(Type type)
        {
            List<ScriptableObject> assets = ScriptableEnumEditorUtils.GetAssetsOfType(type);

            Dictionary<string, int> counts = new(assets.Count);
            foreach (ScriptableObject asset in assets)
            {
                string name = asset.name;
                counts[name] = counts.TryGetValue(name, out int c) ? c + 1 : 1;
            }

            Entry entry = new()
            {
                Assets = assets,
                Choices = new List<string>(assets.Count + 1) { NullChoice },
                NameToAsset = new Dictionary<string, ScriptableObject>(assets.Count),
                AssetToName = new Dictionary<ScriptableObject, string>(assets.Count)
            };

            foreach (ScriptableObject asset in assets)
            {
                string displayName = asset.name;
                if (counts[displayName] > 1)
                    displayName += $" ({counts[displayName]})";

                entry.NameToAsset[displayName] = asset;
                entry.AssetToName[asset] = displayName;
            }

            foreach (string name in entry.AssetToName.Values)
                entry.Choices.Add(name);

            return entry;
        }

        private class Postprocessor : AssetPostprocessor
        {
            private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved,
                string[] movedFrom)
            {
                if (imported.Length == 0 && deleted.Length == 0 && moved.Length == 0) return;
                Invalidate();
            }
        }
    }
}
