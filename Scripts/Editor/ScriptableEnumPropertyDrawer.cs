using UnityEditor;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>Property drawer that draws ScriptableEnum fields with a <see cref="ScriptableEnumField"/>.</summary>
    [CustomPropertyDrawer(typeof(ScriptableEnum), true)]
    public class ScriptableEnumPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.isArray)
            {
                return new UnityEditor.UIElements.PropertyField(property);
            }
            
            return new ScriptableEnumField(property, fieldInfo);
        }
    }
}