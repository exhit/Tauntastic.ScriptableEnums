using UnityEditor;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
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