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
            var graphView = new AssemblyGraphView();
            var assemblies = CompilationPipeline.GetAssemblies();
            // var assemblies = CompilationPipeline.GetAssemblies().Where(a => (a.flags & AssemblyFlags.EditorAssembly) != AssemblyFlags.EditorAssembly);
            var sorted = AssemblySorter.Sort(assemblies);
            graphView.AddAssemblies(sorted);

            rootVisualElement.Add(graphView);
            var box = new Box
            {
                style = { width = 500, height = 400 }
            };
            var scroll = new ScrollView();
            scroll.StretchToParentSize();
            box.Add(scroll);
            foreach (var assembly in assemblies.OrderBy(a => a.name))
            {
                var toggle = new Toggle(assembly.name)
                {
                    value = true
                };
                toggle.RegisterValueChangedCallback(x => graphView.SetVisible(assembly.name, x.newValue));
                scroll.Add(toggle);
            }
            rootVisualElement.Add(box);
        }
    }
}
