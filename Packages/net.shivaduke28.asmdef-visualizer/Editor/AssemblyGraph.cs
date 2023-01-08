using System.Collections.Generic;
using UnityEditor.Compilation;

namespace AsmdefVisualizer.Editor
{
    public sealed class AssemblyGraph
    {
        readonly Dictionary<string, AssemblyNode> nodes = new();
        public IEnumerable<AssemblyNode> Nodes => nodes.Values;

        public AssemblyGraph(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                nodes[assembly.name] = new AssemblyNode(assembly);
            }
        }

        public bool TryGetNode(string name, out AssemblyNode node)
        {
            return nodes.TryGetValue(name, out node);
        }

        public void SetEditorAssembliesVisible(bool visible)
        {
            foreach (var node in nodes.Values)
            {
                var isEditor = (node.Assembly.flags & AssemblyFlags.EditorAssembly) == AssemblyFlags.EditorAssembly;
                if (!isEditor) continue;
                node.Visible = visible;
            }
        }
    }
}
