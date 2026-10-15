using UnityEditor;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>Property drawer that draws fields marked with ScriptableEnumAttribute as an enum-style dropdown.</summary>
    [CustomPropertyDrawer(typeof(ScriptableEnumAttribute), true)]
    public class ScriptableEnumAttributePropertyDrawer : PropertyDrawer
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