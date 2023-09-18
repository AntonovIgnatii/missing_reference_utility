using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

namespace MissingReferencesUtility
{
    public class AllScenesFinder : IMissingReferenceFinder
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
            
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).ToList();

            var result = new List<ResultContainer>();

            foreach (var scene in scenes)
            {
                Scene openScene;
                try
                {
                    openScene = EditorSceneManager.OpenScene(scene.path);
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.Message);
                    continue;
                }

                var findProblems =
                    SearchExecutor().FindMissingReferencesInScene
                    (
                        openScene,
                        onProgress,
                        onComplete,
                        0f,
                        1 / (float)scenes.Count
                    );
                
                var resultContainer = new ResultContainer(openScene.name, findProblems.missingComponents, findProblems.missingReferences);

                if (SearchExecutor().SearchIsCanceled) return result;

                result.Add(resultContainer);
            }

            onComplete.Invoke("Missing Reference Utility", $"Search in [All Scenes] completed successfully");
            
            return result;
        }
    }
}