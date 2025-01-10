using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsmdefVisualizer.Editor
{
    public static class AssemblySorter
    {
        public static List<List<AsmdefNode>> Sort(IEnumerable<AsmdefNode> assemblies)
        {
            var result = new List<List<AsmdefNode>>();
            var nodes = new List<AsmdefNode>(assemblies);

            while (nodes.Count > 0)
            {
                var roots = nodes
                    .Where(node => node.Asmdef.references.All(a => nodes.All(n => n.Asmdef.name != a)))
                    .OrderBy(a => a.Asmdef.name).ToList();
                if (roots.Count == 0)
                {
                    Debug.Log("root nodes are not found.");
                    break;
                }
                result.Add(roots);
                nodes.RemoveAll(x => roots.Contains(x));
            }

            foreach (var layer in result)
            {
                layer.Sort((a, b) =>
                {
                    var aCenter = GetBaryCenter(a, result);
                    var bCenter = GetBaryCenter(b, result);
                    return aCenter.CompareTo(bCenter);
                });
            }

            return result;


            float GetBaryCenter(AsmdefNode node, List<List<AsmdefNode>> layers)
            {
                var references = node.Asmdef.references;
                if (references.Length == 0)
                {
                    return 0;
                }

                var sum = 0f;
                foreach (var reference in references)
                {
                    foreach (var layer in layers)
                    {
                        var index = layer.FindIndex(x => x.Asmdef.name == reference);
                        if (index != -1)
                        {
                            sum += index;
                            break;
                        }
                    }
                }

                return sum / references.Length;
            }
        }
    }
}
