using System;
using UnityEditor.Compilation;

namespace AsmdefVisualizer.Editor
{
    public sealed class AssemblyNode
    {
        public Assembly Assembly { get; }

        bool visible;

        public bool Visible
        {
            get => visible;
            set
            {
                visible = value;
                onVisibleChange?.Invoke(visible);
            }
        }

        delegate void OnVisibleChange(bool visible);

        OnVisibleChange onVisibleChange;

        public AssemblyNode(Assembly assembly)
        {
            Assembly = assembly;
            Visible = true;
        }

        public void AddHandler(Action<bool> handler)
        {
            onVisibleChange += handler.Invoke;
        }

        public void RemoveHandler(Action<bool> handler)
        {
            onVisibleChange -= handler.Invoke;
        }
    }
}
