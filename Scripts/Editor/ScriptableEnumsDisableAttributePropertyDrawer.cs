using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
    /// <summary>Property drawer that disables the field once it is laid out.</summary>
    [CustomPropertyDrawer(typeof(ScriptableEnumsDisableAttribute), true)]
    public class ScriptableEnumsDisableAttributePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var propertyField = new PropertyField(property);
            propertyField.RegisterCallbackOnce<GeometryChangedEvent>(_ => propertyField.SetEnabled(false));
            return propertyField;
        }
    }
}