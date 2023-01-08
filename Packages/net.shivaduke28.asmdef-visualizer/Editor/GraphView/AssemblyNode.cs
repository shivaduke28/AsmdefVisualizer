using UnityEditor.Compilation;
using UnityEditor.Experimental.GraphView;

namespace AsmdefVisualizer.Editor.GraphView
{
    public sealed class AssemblyNode : Node
    {
        public Port InputPort { get; }
        public Port OutputPort { get; }
        public Assembly Assembly { get; }

        public AssemblyNode(Assembly assembly)
        {
            Assembly = assembly;
            title = assembly.name;
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(Port));
            inputPort.portName = "Ref To";
            inputContainer.Add(inputPort);
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputPort.portName = "Ref By";
            outputContainer.Add(outputPort);

            InputPort = inputPort;
            OutputPort = outputPort;
        }

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                foreach (var edge in InputPort.connections)
                {
                    edge.visible = edge.output.node.visible & visible;
                }
                foreach (var edge in OutputPort.connections)
                {
                    edge.visible = edge.input.node.visible & visible;
                }
            }
        }
    }
}
