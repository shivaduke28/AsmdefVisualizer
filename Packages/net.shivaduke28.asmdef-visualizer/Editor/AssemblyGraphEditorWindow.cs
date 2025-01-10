using System;
using System.Collections.Generic;
using System.Linq;
using AsmdefVisualizer.Editor.GraphView;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace AsmdefVisualizer.Editor
{
    public sealed class AssemblyGraphEditorWindow : EditorWindow
    {
        [MenuItem("Tools/AsmdefVisualizer/Open Assembly Graph")]
        public static void Open()
        {
            GetWindow<AssemblyGraphEditorWindow>("Assembly Graph");
        }

        bool visible;

        void OnEnable()
        {
            var asmdefSet = GetAsmdefs();
            var assemblyGraph = new AssemblyGraph(asmdefSet.Values.Select(x => x.asmdef));
            var graphView = new AssemblyGraphView(assemblyGraph);

            // hide editor assemblies by default
            assemblyGraph.SetEditorAssembliesVisible(false);

            graphView.InitializeNodes();

            rootVisualElement.Add(graphView);
            var box = new Box
            {
                style = { width = 500, height = 400 }
            };
            var scroll = new ScrollView();
            scroll.StretchToParentSize();
            box.Add(scroll);

            var resetButton = new Button
            {
                text = "Reset"
            };
            resetButton.clicked += graphView.InitializeNodes;
            scroll.Add(resetButton);

            var editorToggle = new Toggle("Editor Assemblies")
            {
                value = false
            };

            editorToggle.RegisterValueChangedCallback(x => assemblyGraph.SetEditorAssembliesVisible(x.newValue));
            scroll.Add(editorToggle);

            var nodes = assemblyGraph.Nodes.OrderBy(node => node.Asmdef.name).ToArray();

            foreach (var node in nodes)
            {
                var assembly = node.Asmdef;
                var toggle = new Toggle(assembly.name)
                {
                    value = true
                };
                toggle.RegisterValueChangedCallback(x =>
                {
                    if (assemblyGraph.TryGetNode(assembly.name, out var node))
                    {
                        node.Visible = x.newValue;
                    }
                });
                node.AddHandler(x => toggle.value = x);
                scroll.Add(toggle);
            }

            rootVisualElement.Add(box);
        }

        static Dictionary<string, (Asmdef asmdef, string path)> GetAsmdefs()
        {
            var guids = AssetDatabase.FindAssets("t:AssemblyDefinitionAsset", new[] { "Assets", "Packages" });
            var assemblyDefinitions = new Dictionary<string, (Asmdef, string)>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<AssemblyDefinitionAsset>(path);
                var asmdef = JsonUtility.FromJson<Asmdef>(asset.text);
                if (asmdef != null)
                {
                    // nullの場合があるのでから配列で初期化する
                    asmdef.references ??= Array.Empty<string>();
                    asmdef.includePlatforms ??= Array.Empty<string>();
                    assemblyDefinitions[asmdef.name] = (asmdef, path);
                }
            }

            return assemblyDefinitions;
        }
    }
}
