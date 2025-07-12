using System;
using System.Collections.Generic;
using Jin5eok.Helper;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
namespace Jin5eok.Patterns.Component
{
    [CustomPropertyDrawer(typeof(SerializableTypeElementWrapper))]
    public class SerializableComponentDrawer<T> : PropertyDrawer
    {
        protected string HeaderField { get; set; } = typeof(T).Name;
        
        private ReorderableList _reorderableList;
        private SerializedProperty _serializedComponents;

        private string _componentFieldName = "Components";

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_reorderableList == null)
            {
                Initialize(position,property,label);
            }
            property.serializedObject.Update();
            _reorderableList?.DoLayoutList();
            property.serializedObject.ApplyModifiedProperties();
        }
        
        protected virtual void Initialize(Rect position, SerializedProperty property, GUIContent label)
        {
            var group = property.managedReferenceValue as SerializableTypeElementWrapper;
            _serializedComponents = property.FindPropertyRelative(_componentFieldName);
            _reorderableList = new ReorderableList(property.serializedObject, _serializedComponents, true, true, true, true);
            _reorderableList.drawHeaderCallback += OnDrawHaeaderCallback;
            _reorderableList.onAddDropdownCallback = OnAddDropDownCallback;
            _reorderableList.drawElementCallback += DrawElementCallback;
            _reorderableList.elementHeightCallback += ElementHeightCallback;
        }
        
        protected virtual void OnDrawHaeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, HeaderField);
        }
        
        protected virtual void OnAddDropDownCallback(Rect buttonrect, ReorderableList list)
        {
            var menu = new GenericMenu();
            var subclassTypes = ReflectionHelper.GetSubclasses<T>();
            
            for (int i = 0; i < list.serializedProperty.arraySize; i++)
            {
                var managedReference = list.serializedProperty.GetArrayElementAtIndex(i).managedReferenceValue;
                if (managedReference is SerializableTypeElement serializable)
                {
                    subclassTypes.Remove(serializable.GetType());
                    menu.AddDisabledItem(new GUIContent(serializable.GetType().ToString()), true);
                }
            }
            
            foreach (var subclassType in subclassTypes)
            {
                menu.AddItem(new GUIContent(subclassType.ToString()), false, () =>
                {
                    var created = Activator.CreateInstance(subclassType);
                    if (created is T component)
                    {
                        AddItem(component);
                    }
                });
            }
            menu.DropDown(buttonrect);
        }

        protected virtual void AddItem(T component)
        {
            _serializedComponents.arraySize++;
            _serializedComponents.serializedObject.ApplyModifiedProperties();
            var serialized = _serializedComponents.GetArrayElementAtIndex(_serializedComponents.arraySize - 1);
            serialized.managedReferenceValue = component;
            serialized.serializedObject.ApplyModifiedProperties();
        }
        
        protected virtual void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var target = _serializedComponents.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), target, new GUIContent(target.type.Replace("managedReference","")), true);
        }

        protected virtual float ElementHeightCallback(int index)
        {
            SerializedProperty element = _serializedComponents.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element, element.isExpanded) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}