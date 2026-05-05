using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
    
    [CustomPropertyDrawer(typeof(ScriptableEnum.Flags), true)]
    public class ScriptableEnumFlagsPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.isArray)
                return new PropertyField(property);
            return new ScriptableEnumFlagsField(property, fieldInfo);
        }
        
        // public override VisualElement CreatePropertyGUI(SerializedProperty property)
        // {
        //     if (property.boxedValue is not ScriptableEnum.Flags scriptableEnumFlags)
        //         return new Label("No FlagsScriptableEnum");
        //
        //     Type seType = scriptableEnumFlags.Type;
        //
        //     string label = property.displayName;
        //     var array = ScriptableEnum.GetAllOptions(seType);
        //     int totalCount = array.Length;
        //     var stringList = array.Select(x => x.name.ToString()).ToList();
        //
        //     var defaultIndexes = array.Where(scriptableEnumFlags.Values.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
        //     var defaultMask = MaskUtils.GetMaskFromIndices(defaultIndexes, totalCount);
        //
        //     MaskField field = new(label, stringList, defaultMask);
        //
        //     field.RegisterValueChangedCallback(e =>
        //     {
        //         int intValue = e.newValue;
        //
        //         switch (intValue)
        //         {
        //             case -1:
        //                 scriptableEnumFlags.Values = array.ToList();
        //                 break;
        //             case 0:
        //                 scriptableEnumFlags.Values = new List<ScriptableEnum>();
        //                 break;
        //             case >0:
        //                 var indexes = MaskUtils.GetIndicesFromMask(intValue, totalCount);
        //                 var chosenStrings = stringList.Where(x => indexes.Contains(stringList.IndexOf(x)));
        //                 var chosenOptions = array.Where(x => chosenStrings.Contains(x.name));
        //                 scriptableEnumFlags.Values = chosenOptions.ToList();
        //                 break;
        //         }
        //         
        //         property.boxedValue = scriptableEnumFlags;
        //         property.serializedObject.ApplyModifiedProperties();
        //     });
        //     
        //     field.TrackPropertyValue(property, callback =>
        //     {
        //         if (callback.boxedValue is not ScriptableEnum.Flags sef) 
        //             return;
        //         
        //         var indexes = array.Where(sef.Values.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
        //         int mask = MaskUtils.GetMaskFromIndices(indexes, totalCount);
        //         field.SetValueWithoutNotify(mask);
        //     });
        //     
        //     field.AddToClassList("unity-base-field__aligned");
        //     
        //     return field;
        // }
    }
}
