using System;
using System.Linq;
using UnityEngine;

namespace AsmdefVisualizer.Editor
{
    [Serializable]
    public sealed class Asmdef
    {
        [SerializeField] public string name;
        [SerializeField] public string rootNamespace;
        [SerializeField] public string[] references;
        [SerializeField] public string[] includePlatforms;
        [SerializeField] public string[] excludePlatforms;
        [SerializeField] public bool allowUnsafeCode;
        [SerializeField] public bool overrideReferences;
        [SerializeField] public string[] precompiledReferences;
        [SerializeField] public bool autoReferenced;
        [SerializeField] public string[] defineConstraints;
        [SerializeField] public string versionDefines;
        [SerializeField] public bool noEngineReferences;

        public bool IsEditor()
        {
            return includePlatforms.Contains("Editor");
        }
    }
}
