using System.Linq;
using AsmdefVisualizer.Editor.GraphView;
using UnityEditor;
using UnityEditor.Compilation;
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
            var assemblies = CompilationPipeline.GetAssemblies();
            var assemblyGraph = new AssemblyGraph(assemblies);
            var graphView = new AssemblyGraphView(assemblyGraph);
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
                value = true
            };

            editorToggle.RegisterValueChangedCallback(x => assemblyGraph.SetEditorAssembliesVisible(x.newValue));
            scroll.Add(editorToggle);

            foreach (var assembly in assemblies.OrderBy(a => a.name))
            {
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
                scroll.Add(toggle);
            }
            rootVisualElement.Add(box);
        }
    }
}
