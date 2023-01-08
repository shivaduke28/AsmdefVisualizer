using AsmdefVisualizer.Editor.GraphView;
using UnityEditor;
using UnityEditor.Compilation;

namespace AsmdefVisualizer.Editor
{
    public sealed class AssemblyGraphEditorWindow : EditorWindow
    {
        [MenuItem("Tools/AsmdefVisualizer/Open Assembly Graph")]
        public static void Open()
        {
            GetWindow<AssemblyGraphEditorWindow>("Assembly Graph");
        }

        void OnEnable()
        {
            var graphView = new AssemblyGraphView();
            var assemblies = CompilationPipeline.GetAssemblies();
            var sorted = AssemblySorter.Sort(assemblies);
            graphView.AddAssemblies(sorted);

            rootVisualElement.Add(graphView);
        }
    }
}
