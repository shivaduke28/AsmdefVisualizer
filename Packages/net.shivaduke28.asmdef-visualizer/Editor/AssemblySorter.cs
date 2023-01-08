using System.Collections.Generic;
using System.Linq;
using UnityEditor.Compilation;
using UnityEngine;

namespace AsmdefVisualizer.Editor
{
    public static class AssemblySorter
    {
        public static List<List<Assembly>> Sort(IEnumerable<Assembly> assemblies)
        {
            var result = new List<List<Assembly>>();
            var nodes = new List<Assembly>(assemblies);

            while (nodes.Count > 0)
            {
                var roots = nodes.Where(assembly => assembly.assemblyReferences.All(a => !nodes.Contains(a))).OrderBy(a => a.name).ToList();
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
