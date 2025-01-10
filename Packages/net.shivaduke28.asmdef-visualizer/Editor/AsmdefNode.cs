using System;

namespace AsmdefVisualizer.Editor
{
    public sealed class AsmdefNode
    {
        public Asmdef Asmdef { get; }

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

        public AsmdefNode(Asmdef asmdef)
        {
            Asmdef = asmdef;
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
