using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AsmdefVisualizer.Editor.GraphView
{
    public sealed class AssemblyGraphView : UnityEditor.Experimental.GraphView.GraphView
    {
        readonly AssemblyGraph assemblyGraph;
        readonly Dictionary<string, AssemblyNodeView> nodeMap = new();

        // https://technote.qualiarts.jp/article/10#graphview%E3%81%AB%E3%82%88%E3%82%8B%E5%AE%9F%E8%A3%85%E3%81%AE%E6%89%8B%E5%BC%95%E3%81%8D
        public AssemblyGraphView(AssemblyGraph assemblyGraph)
        {
            this.assemblyGraph = assemblyGraph;
            this.StretchToParentSize();
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            Insert(0, new GridBackground());
        }

        public void InitializeNodes()
        {
            var nodeViews = nodeMap.Values.ToArray();
            foreach (var nodeView in nodeViews)
            {
                var inputs = nodeView.InputPort.connections.ToArray();
                foreach (var input in inputs)
                {
                    RemoveElement(input);
                }
                var outputs = nodeView.OutputPort.connections.ToArray();
                foreach (var output in outputs)
                {
                    RemoveElement(output);
                }
                if (assemblyGraph.TryGetNode(nodeView.Asmdef.name, out var node))
                {
                    node.RemoveHandler(nodeView.SetVisibility);
                }
                RemoveElement(nodeView);
            }
            nodeMap.Clear();

            var visibleNodes = assemblyGraph.Nodes.Where(node => node.Visible);
            var sortedNodesList = AssemblySorter.Sort(visibleNodes);
            for (var i = 0; i < sortedNodesList.Count; i++)
            {
                var sortedNodes = sortedNodesList[i];
                for (var j = 0; j < sortedNodes.Count; j++)
                {
                    var node = sortedNodes[j];
                    var nodeView = new AssemblyNodeView(node.Asmdef);
                    // todo: NodeViewでAddHandlerする
                    node.AddHandler(nodeView.SetVisibility);
                    nodeView.SetPosition(new Rect(new Vector2(400f * i, 100f * j), new Vector2(0, 0)));
                    AddElement(nodeView);
                    nodeMap.Add(nodeView.Asmdef.name, nodeView);
                }
            }

            foreach (var node in nodeMap.Values)
            {
                foreach (var reference in node.Asmdef.references)
                {
                    if (nodeMap.TryGetValue(reference, out var target))
                    {
                        var edge = node.InputPort.ConnectTo(target.OutputPort);
                        AddElement(edge);
                    }
                }
            }
        }

        public override List<Port> GetCompatiblePorts(Port startAnchor, NodeAdapter nodeAdapter)
        {
            return ports.ToList();
        }
    }
}
