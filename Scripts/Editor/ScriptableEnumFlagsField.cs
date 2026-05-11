using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Tauntastic.ScriptableEnums.Editor
{
    public class ScriptableEnumFlagsField : BaseField<string>
    {
        private const string _MASK_FIELD_NAME = "mask-field";
        
        private FieldInfo _fieldInfo;
        private SerializedProperty _property;
        private SerializedProperty _displayNameProperty;
        
        private MaskField _maskField;
        private Type _targetType;
        
        protected List<ScriptableEnum> list = new();
        protected int totalCount => list.Count;
        protected List<string> stringList = new();

        public ScriptableEnumFlagsField(SerializedProperty property, FieldInfo fieldInfo) : this(property.displayName)
        {
            BindPropertyAndFieldInfo(property, fieldInfo);
        }

        public ScriptableEnumFlagsField(string label = nameof(ScriptableEnumFlagsField)) : base(label, CreateVisualInputElement())
        {
            RegisterCallback<AttachToPanelEvent>(_ =>
            {
                Undo.undoRedoPerformed += RefreshOptions;
                EditorApplication.projectChanged += RefreshOptions;
            });

            RegisterCallback<DetachFromPanelEvent>(_ =>
            {
                Undo.undoRedoPerformed -= RefreshOptions;
                EditorApplication.projectChanged -= RefreshOptions;
            });
        }
        
        private static VisualElement CreateVisualInputElement()
        {
            VisualElement root = new()
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    maxHeight = 21
                }
            };

            MaskField maskField = new("")
            {
                name = _MASK_FIELD_NAME,
                style =
                {
                    flexGrow = 1f,
                    marginLeft = 0,
                    marginRight = 0,
                }
            };
            
            root.Add(maskField);

            return root;
        }
        
        private void BindPropertyAndFieldInfo(SerializedProperty property, FieldInfo fieldInfo)
        {
            _property = property;
            _fieldInfo = fieldInfo;

            if (_fieldInfo == null)
                throw new ArgumentException("Cannot find field info for property: " + property.propertyPath);

            _targetType = _fieldInfo.FieldType;
            
            if (_targetType.IsGenericType)
                _targetType = _targetType.GetGenericArguments()[0];

            if (_targetType == null)
                throw new ArgumentException("ScriptableEnumField can only be used with ScriptableObject properties.");

            _maskField = this.Q<MaskField>(_MASK_FIELD_NAME);
            
            if (property.boxedValue is not ScriptableEnum.Flags scriptableEnumFlags) return;

            RefreshOptions();
            
            var indexes = list.Where(scriptableEnumFlags.Values.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
            int mask = MaskUtils.GetMaskFromIndices(indexes, totalCount);
            
            _maskField.choices = stringList;
            _maskField.SetValueWithoutNotify(mask);

            _maskField.RegisterValueChangedCallback(e =>
            {
                mask = e.newValue;

                switch (mask)
                {
                    case -1:
                        scriptableEnumFlags.Values = list.ToList();
                        break;
                    case 0:
                        scriptableEnumFlags.Values = new List<ScriptableEnum>();
                        break;
                    case >0:
                        indexes = MaskUtils.GetIndicesFromMask(mask, totalCount);
                        var chosenStrings = stringList.Where(x => indexes.Contains(stringList.IndexOf(x)));
                        var chosenOptions = list.Where(x => chosenStrings.Contains(x.name));
                        scriptableEnumFlags.Values = chosenOptions.ToList();
                        break;
                }
                
                property.boxedValue = scriptableEnumFlags;
                property.serializedObject.ApplyModifiedProperties();
            });
            
            _maskField.TrackPropertyValue(property, callback =>
            {
                if (callback.boxedValue is not ScriptableEnum.Flags sef) return;
                
                indexes = list.Where(sef.Values.Contains).Select(x => stringList.IndexOf(x.name)).ToList();
                mask = MaskUtils.GetMaskFromIndices(indexes, totalCount);
                _maskField.SetValueWithoutNotify(mask);
            });
        }

        private void RefreshOptions()
        {
            if (_property.boxedValue is not ScriptableEnum.Flags scriptableEnumFlags) return;
            
            Type seType = scriptableEnumFlags.Type;
            list = ScriptableEnum.GetAll(seType).Where(x => x != null).ToList();
            stringList = list.Select(x => x.name.ToString()).ToList();
        }
    }
}