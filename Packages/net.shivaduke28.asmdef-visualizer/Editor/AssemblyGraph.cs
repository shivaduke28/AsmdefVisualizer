using System.Collections.Generic;
using UnityEditor.Compilation;

namespace AsmdefVisualizer.Editor
{
    public sealed class AssemblyGraph
    {
        readonly Dictionary<string, AsmdefNode> nodes = new();
        public IEnumerable<AsmdefNode> Nodes => nodes.Values;

        public AssemblyGraph(IEnumerable<Asmdef> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                nodes[assembly.name] = new AsmdefNode(assembly);
            }
        }

        public bool TryGetNode(string name, out AsmdefNode node)
        {
            return nodes.TryGetValue(name, out node);
        }

        public void SetEditorAssembliesVisible(bool visible)
        {
            foreach (var node in nodes.Values)
            {
                if (!node.Asmdef.IsEditor()) continue;
                node.Visible = visible;
            }
        }
    }
}
