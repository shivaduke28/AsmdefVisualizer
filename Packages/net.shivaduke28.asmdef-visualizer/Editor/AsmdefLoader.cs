using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace AsmdefVisualizer.Editor
{
    public static class AsmdefLoader
    {
        [MenuItem("AsmdefVisualizer/Load")]
        public static void Load()
        {
            var assemblies = CompilationPipeline.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                Debug.Log($"[{assembly.rootNamespace}] {assembly.name}\n{string.Join(",\n", assembly.assemblyReferences.Select(x => x.name))}");
            }
        }
    }
}
