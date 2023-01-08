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
            inputContainer.Add(inputPort);
            var outputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(Port));
            outputContainer.Add(outputPort);

            InputPort = inputPort;
            OutputPort = outputPort;
        }
    }
}
