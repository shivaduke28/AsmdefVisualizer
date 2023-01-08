using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsmdefVisualizer.Editor
{
    public static class AssemblySorter
    {
        public static List<List<AssemblyNode>> Sort(IEnumerable<AssemblyNode> assemblies)
        {
            var result = new List<List<AssemblyNode>>();
            var nodes = new List<AssemblyNode>(assemblies);

            while (nodes.Count > 0)
            {
                var roots = nodes
                    .Where(node => node.Assembly.assemblyReferences.All(a => nodes.All(n => n.Assembly != a)))
                    .OrderBy(a => a.Assembly.name).ToList();
                if (roots.Count == 0)
                {
                    Debug.Log("root nodes are not found.");
                    break;
                }
                result.Add(roots);
                nodes.RemoveAll(x => roots.Contains(x));
            }

            return result;
        }
    }
}
