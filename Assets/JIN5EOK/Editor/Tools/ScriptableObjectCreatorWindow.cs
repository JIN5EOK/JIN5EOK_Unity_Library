using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Jin5eok.Editor
{
    public class ScriptableObjectCreatorWindow : EditorWindow
    {
        private bool _isShowAssembliesAll = false;
        private Assembly SelectedAssembly => 
            _selectedAssemblyIndex >= _allAssemblies.Length || _selectedAssemblyIndex < 0 
            ? null : _allAssemblies[_selectedAssemblyIndex];
        
        private int _selectedAssemblyIndex = 0;
        private Assembly[] _allAssemblies;
        private string[] _assemblyNames;
        
        private Type SelectedType => 
            _selectedTypeIndex >= _scriptableObjectTypes.Length || _selectedTypeIndex < 0 
            ? null : _scriptableObjectTypes[_selectedTypeIndex];
        
        private int _selectedTypeIndex = 0;
        private Type[] _scriptableObjectTypes;
        private string[] _typeNames;
        
        [MenuItem("JIN5EOK/ScriptableObject Creator")]
        public static void ShowWindow()
        {
            GetWindow<ScriptableObjectCreatorWindow>("ScriptableObject Creator");
        }
        
        private void OnEnable()
        {
            RefreshAssemblyList();
            RefreshScriptableObjectTypes();
        }

        private void RefreshAssemblyList()
        {
            _allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.FullName.StartsWith("System") && !assembly.FullName.StartsWith("Unity"))
                .OrderBy(assembly => assembly.GetName().Name)
                .ToArray();
            _assemblyNames = _allAssemblies.Select(assembly => assembly.GetName().Name).ToArray();
        }
        
        private void OnGUI()
        {
            // Select Assembly
            EditorGUILayout.LabelField("Select Assembly", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();

            bool beforeIsShowAssembliesAll = _isShowAssembliesAll;
            _isShowAssembliesAll = EditorGUILayout.Toggle("Show All Assemblies", _isShowAssembliesAll);
            
            int beforeSelectedAssemblyIndex = _selectedAssemblyIndex;
            _selectedAssemblyIndex = EditorGUILayout.Popup(_selectedAssemblyIndex, _assemblyNames);

            var isDirtyAssemblyInfos = beforeIsShowAssembliesAll != _isShowAssembliesAll ||
                                       beforeSelectedAssemblyIndex != _selectedAssemblyIndex;
            
            if (isDirtyAssemblyInfos == true)
            {
                RefreshScriptableObjectTypes();
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            // Select ScriptableObject
            EditorGUILayout.LabelField("Select ScriptableObject", EditorStyles.boldLabel);
            
            var isContainsScriptableObjects = _scriptableObjectTypes.Length == 0; 
            if (isContainsScriptableObjects == true)
            {
                EditorGUILayout.HelpBox("No ScriptableObjects in this Assembly", MessageType.Info);
            }
            else
            {
                _selectedTypeIndex = EditorGUILayout.Popup(_selectedTypeIndex, _typeNames);

                if (GUILayout.Button("Create ScriptableObject") == true)
                {
                    CreateAsset(SelectedType);
                }
            }
        }
        
        private void RefreshScriptableObjectTypes()
        {
            if (_isShowAssembliesAll == true)
            {
                _scriptableObjectTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => GetScriptableObjectTypeInAssembly(assembly))
                    .Distinct()
                    .OrderBy(t => t.Name)
                    .ToArray();
            }
            else
            {
                _scriptableObjectTypes = GetScriptableObjectTypeInAssembly(SelectedAssembly)
                    .OrderBy(t => t.Name)
                    .ToArray();
            }

            _typeNames = _scriptableObjectTypes.Select(t => t.Name).ToArray();
            _selectedTypeIndex = 0;
        }

        private Type[] GetScriptableObjectTypeInAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes()
                    .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract)
                    .ToArray();
            }
            catch
            {
                return Array.Empty<Type>();
            }
        }

        private void CreateAsset(Type type)
        {
            if (type == null)
            {
                return;
            }
            
            ScriptableObject asset = CreateInstance(type);
            string path = EditorUtility.SaveFilePanelInProject("Save Asset", $"New{type.Name}.asset", "asset", "Please enter a file name");
            
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
    }
}