using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MissingReferencesUtility
{
    public class CurrentSceneFinder : IMissingReferenceFinder
    {
        public SearchExecutor SearchExecutor() => new ();
        
        public List<ResultContainer> Find
        (
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete
        )
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                return null;
            }
            
            var scene = SceneManager.GetActiveScene();
            
            var findProblems = SearchExecutor().FindMissingReferencesInScene(scene, onProgress, onComplete);
            
            var resultContainer = new ResultContainer("", findProblems.missingComponents, findProblems.missingReferences);
            
            onComplete.Invoke("Missing Reference Utility", $"Search in [Current Scene] completed successfully");
            return new List<ResultContainer> { resultContainer };
        }
    }
}