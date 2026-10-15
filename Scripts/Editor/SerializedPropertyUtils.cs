using UnityEditor;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>SerializedProperty helpers used by the scriptable enum drawers.</summary>
    public static class SerializedPropertyUtils
    {
        public static bool IsPropertyInUnityObject(this SerializedProperty property)
        {
            if (property.isArray)
                return false;
            if (property.serializedObject.FindProperty(property.name) != null)
                return true;
            return false;
        }
    }
}