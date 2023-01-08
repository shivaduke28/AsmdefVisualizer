using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AsmdefVisualizer.Editor.GraphView
{
    public sealed class AssemblyGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        readonly Dictionary<string, AssemblyNode> nodeMap = new();

        // https://technote.qualiarts.jp/article/10#graphview%E3%81%AB%E3%82%88%E3%82%8B%E5%AE%9F%E8%A3%85%E3%81%AE%E6%89%8B%E5%BC%95%E3%81%8D
        public AssemblyGraphView()
        {
            this.StretchToParentSize();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            Insert(0, new GridBackground());
        }

        public void AddAssemblies(List<List<Assembly>> sortedAssemblies)
        {
            for (var i = 0; i < sortedAssemblies.Count; i++)
            {
                var assemblies = sortedAssemblies[i];
                for (var j = 0; j < assemblies.Count; j++)
                {
                    var assembly = assemblies[j];
                    var node = new AssemblyNode(assembly);
                    node.SetPosition(new Rect(new Vector2(400f * i, 100f * j), new Vector2(0, 0)));
                    AddElement(node);
                    nodeMap.Add(node.Assembly.name, node);
                }
            }

            foreach (var node in nodeMap.Values)
            {
                foreach (var reference in node.Assembly.assemblyReferences)
                {
                    var target = nodeMap[reference.name];
                    var edge = node.InputPort.ConnectTo(target.OutputPort);
                    AddElement(edge);
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }

        public void SetVisible(string name, bool visible)
        {
            if (nodeMap.TryGetValue(name, out var node))
            {
                node.Visible = visible;
            }
        }
    }
}
