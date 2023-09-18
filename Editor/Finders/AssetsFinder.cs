using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace MissingReferencesUtility
{
    public class AssetsFinder : IMissingReferenceFinder
    {
        public SearchExecutor SearchExecutor() => new ();

        public List<ResultContainer> Find
        (
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete
        )
        {
            var allAssetPaths = AssetDatabase.GetAllAssetPaths();
            var allObjects = allAssetPaths
                .Where(IsProjectAsset)
                .ToArray();

            var findProblems = SearchExecutor().FindMissingReferences("Project", allObjects, onProgress, onComplete);

            var resultContainer = new ResultContainer("", findProblems.missingComponents, findProblems.missingReferences);
            
            onComplete.Invoke("Missing Reference Utility", $"Search in [Project Assets] completed successfully");
            return new List<ResultContainer> { resultContainer };
        }
        
        private bool IsProjectAsset(string path)
        {
#if UNITY_EDITOR_OSX
            return !path.StartsWith("/");
#else
            return path.Substring(1, 2) != ":/";
#endif
        }
    }
}