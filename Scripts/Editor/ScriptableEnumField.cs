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
    public class ScriptableEnumField : BaseField<string>
    {
        private const string _POPUP_FIELD_NAME = "popup-field";
        private const string _PING_BUTTON_NAME = "ping-button";
        private const string _OPEN_PROPERTY_EDITOR_BUTTON_NAME  = "open-property-editor-button";

        private FieldInfo _fieldInfo;
        private SerializedProperty _property;
        private SerializedProperty _displayNameProperty;

        private PopupField<string> _popupField;
        private Type _targetType;

        private int _cacheVersion = -1;

        public ScriptableEnumField(SerializedProperty property, FieldInfo fieldInfo)
            : this(property.displayName)
        {
            BindPropertyAndFieldInfo(property, fieldInfo);
        }

        public ScriptableEnumField(string label = nameof(ScriptableEnumField)) : base(label, CreateVisualInputElement())
        {
            RegisterCallback<AttachToPanelEvent>(_ =>
            {
                Undo.undoRedoPerformed += RefreshOptions;
                ScriptableEnumCache.Invalidated += RefreshOptions;
                RefreshOptions();
            });

            RegisterCallback<DetachFromPanelEvent>(_ =>
            {
                Undo.undoRedoPerformed -= RefreshOptions;
                ScriptableEnumCache.Invalidated -= RefreshOptions;
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

            PopupField<string> popupField = new("")
            {
                name = _POPUP_FIELD_NAME,
                style =
                {
                    flexGrow = 1f,
                    marginLeft = 0,
                    marginRight = 0,
                }
            };

            Button pingButton = new()
            {
                name = _PING_BUTTON_NAME,
                text = "💡",
                style =
                {
                    paddingTop = 0,
                    paddingBottom = 0,
                    marginLeft = 1,
                    marginRight = 0,
                }
            };
            
            Button openPropertyEditorButton = new()
            {
                name = _OPEN_PROPERTY_EDITOR_BUTTON_NAME,
                text = "👉",
                style =
                {
                    marginLeft = 1,
                    marginRight = 0,
                    paddingTop = 0,
                    paddingBottom = 5,
                    paddingLeft = 4,
                    paddingRight = 4,
                }
            };

            root.Add(popupField);
            root.Add(pingButton);
            root.Add(openPropertyEditorButton);
            
            return root;
        }

        private void BindPropertyAndFieldInfo(SerializedProperty property, FieldInfo fieldInfo)
        {
            _property = property;
            _fieldInfo = fieldInfo;

            if (_fieldInfo == null)
                throw new ArgumentException("Cannot find field info for property: " + property.propertyPath);

            _targetType = _fieldInfo.FieldType;

            if (_targetType == null)
                throw new ArgumentException("ScriptableEnumField can only be used with ScriptableObject properties.");
            
            if (_targetType.IsGenericType)
                _targetType = _targetType.GetGenericArguments()[0];

            _popupField = this.Q<PopupField<string>>(_POPUP_FIELD_NAME);
            Button pingButton = this.Q<Button>(_PING_BUTTON_NAME);
            Button openPropertyEditorButton = this.Q<Button>(_OPEN_PROPERTY_EDITOR_BUTTON_NAME);

            if (_property.IsPropertyInUnityObject())
                AddToClassList("unity-base-field__aligned");
            else
                labelElement.style.minWidth = 0;
            
            RegisterCallbackOnce<GeometryChangedEvent>(_ =>
            {
                if (string.IsNullOrEmpty(label))
                    _popupField.style.marginLeft = 0;
            });

            _popupField.TrackPropertyValue(property, p =>
            {
                Object obj = p.objectReferenceValue;
                bool exists = obj != null;
                pingButton.style.display = exists ? DisplayStyle.Flex : DisplayStyle.None;
                pingButton.SetEnabled(exists);
                openPropertyEditorButton.style.display = exists ? DisplayStyle.Flex : DisplayStyle.None;
                openPropertyEditorButton.SetEnabled(exists);
                _popupField.SetValueWithoutNotify(GetCurrentDisplayName(obj));
            });
            
            _popupField.TrackSerializedObjectValue(property.serializedObject, _ =>
            {
                _popupField.SetValueWithoutNotify(GetCurrentDisplayName(property.objectReferenceValue));
            });

            _popupField.RegisterValueChangedCallback(evt => { OnSelectionChanged(property, evt.newValue); });

            bool exists = property.objectReferenceValue != null;

            pingButton.clickable = new Clickable(() =>
            {
                if (property.objectReferenceValue != null)
                    EditorGUIUtility.PingObject(property.objectReferenceValue);
            });

            pingButton.style.display = exists ? DisplayStyle.Flex : DisplayStyle.None;
            pingButton.SetEnabled(exists);

            openPropertyEditorButton.clickable = new Clickable(() =>
            {
                EditorUtility.OpenPropertyEditor(property.objectReferenceValue);
            });
            
            openPropertyEditorButton.style.display = exists ? DisplayStyle.Flex : DisplayStyle.None;
            openPropertyEditorButton.SetEnabled(exists);

            RefreshOptions();
        }

        private void RefreshOptions()
        {
            if (_popupField == null || _property == null) return;

            ScriptableEnumCache.Entry entry = ScriptableEnumCache.Get(_targetType);

            if (_cacheVersion != ScriptableEnumCache.Version)
            {
                _cacheVersion = ScriptableEnumCache.Version;
                _popupField.choices = entry.Choices;
                _popupField.SetEnabled(entry.Choices.Count > 1);
            }

            _popupField.SetValueWithoutNotify(entry.GetDisplayName(_property.objectReferenceValue as ScriptableObject));
        }

        private string GetCurrentDisplayName(Object obj)
        {
            return ScriptableEnumCache.Get(_targetType).GetDisplayName(obj as ScriptableObject);
        }

        private void OnSelectionChanged(SerializedProperty property, string newValue)
        {
            if (ScriptableEnumCache.Get(_targetType).NameToAsset.TryGetValue(newValue, out ScriptableObject asset))
                property.objectReferenceValue = asset;
            else if (newValue == ScriptableEnumCache.NullChoice)
                property.objectReferenceValue = null;
            else
                Debug.LogError("Error in selection change.");

            property.serializedObject.ApplyModifiedProperties();
        }
    }
}