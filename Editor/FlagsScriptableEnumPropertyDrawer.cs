using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Tauntastic.ScriptableEnums.Editor
{
    [CustomPropertyDrawer(typeof(FlagsScriptableEnum), true)]
    public class FlagsScriptableEnumPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            if (property.boxedValue is not FlagsScriptableEnum flagsScriptableEnum)
                return new Label("No FlagsScriptableEnum");

            Type seType = flagsScriptableEnum.SEType;

            string label = property.displayName;
            var array = ScriptableEnum.GetAllOptions(seType);
            int totalCount = array.Length;
            var stringList = array.Select(x => x.name.ToString()).ToList();

            var defaultIndexes = array.Where(flagsScriptableEnum.ValuesSE.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
            var defaultMask = GetMaskFromIndices(defaultIndexes, totalCount);

            MaskField field = new(label, stringList, defaultMask);

            field.RegisterValueChangedCallback(e =>
            {
                int intValue = e.newValue;

                switch (intValue)
                {
                    case -1:
                        flagsScriptableEnum.ValuesSE = array.ToList();
                        break;
                    case 0:
                        flagsScriptableEnum.ValuesSE = new List<ScriptableEnum>();
                        break;
                    case >0:
                        var indexes = GetIndicesFromMask(intValue, totalCount);
                        var chosenStrings = stringList.Where(x => indexes.Contains(stringList.IndexOf(x)));
                        var chosenOptions = array.Where(x => chosenStrings.Contains(x.name));
                        flagsScriptableEnum.ValuesSE = chosenOptions.ToList();
                        break;
                }
                
                property.boxedValue = flagsScriptableEnum;
                property.serializedObject.ApplyModifiedProperties();
            });
            
            field.TrackPropertyValue(property, callback =>
            {
                if (callback.boxedValue is not FlagsScriptableEnum fse) 
                    return;
                var indexes = array.Where(flagsScriptableEnum.ValuesSE.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
                int mask = GetMaskFromIndices(indexes, totalCount);
                field.SetValueWithoutNotify(mask);
            });
            field.AddToClassList("unity-base-field__aligned");
            
            return field;
        }

        /// <summary>
        /// Converts a bitmask integer from a MaskField into a list of indices.
        /// </summary>
        /// <param name="mask">The integer bitmask. A value of -1 represents "Everything".</param>
        /// <param name="totalOptionCount">The total number of options available in the mask field.</param>
        /// <returns>A list of zero-based indices for each selected option.</returns>
        public List<int> GetIndicesFromMask(int mask, int totalOptionCount)
        {
            var indices = new List<int>();

            // Handle the "Everything" case where the mask is -1 (all bits set)
            if (mask == -1)
            {
                for (int i = 0; i < totalOptionCount; i++)
                {
                    indices.Add(i);
                }
                return indices;
            }

            // Iterate through each possible option to see if its corresponding bit is set
            for (int i = 0; i < totalOptionCount; i++)
            {
                // Check if the bit at position 'i' is set in the mask
                if ((mask & (1 << i)) != 0)
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        public int GetMaskFromIndices(List<int> indices, int totalOptionCount)
        {
            int mask = 0;
            
            for (int i = 0; i < totalOptionCount; i++)
            {
                if (indices.Contains(i))
                {
                    mask |= 1 << i;
                }
            }
            
            return mask;
        }
    }
}
