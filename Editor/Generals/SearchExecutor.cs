using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace MissingReferencesUtility
{
    public class SearchExecutor
    {
        public bool SearchIsCanceled { get; private set; }
        
        public (List<string> missingReferences, List<string> missingComponents) FindMissingReferences
        (
            string context,
            string[] paths,
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete,
            float initialProgress = 0f,
            float progressWeight = 1f
        )
        {
            var queue = new Queue<ProgressContainer>();
            foreach (var path in paths)
            {
                var rootObject = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
                if (rootObject == null || !rootObject) continue;

                queue.Enqueue(new ProgressContainer(progressWeight / (float)paths.Length, rootObject, path));
            }

            var result = FindMissingReferences
            (
                context,
                queue,
                onProgress,
                onComplete,
                true,
                initialProgress
            );
            return result;
        }

        public (List<string> missingReferences, List<string> missingComponents) FindMissingReferencesInScene
        (
            Scene scene,
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete,
            float initialProgress = 0f,
            float progressWeight = 1f
        )
        {
            var rootObjects = scene.GetRootGameObjects();

            var queue = new Queue<ProgressContainer>();
            foreach (var rootObject in rootObjects)
            {
                queue.Enqueue(new ProgressContainer(progressWeight / rootObjects.Length, rootObject, rootObject.transform.GetPath()));
            }

            var result = FindMissingReferences
            (
                scene.path,
                queue,
                onProgress,
                onComplete,
                false,
                initialProgress
            );
            return result;
        }

        private (List<string> missingReferences, List<string> missingComponents) FindMissingReferences
        (
            string context,
            Queue<ProgressContainer> queue,
            Func<string, string, float, bool> onProgress,
            Action<string, string> onComplete,
            bool isAssetObject,
            float currentProgress = 0f
        )
        {
            var missingReferences = new List<string>();
            var missingComponents = new List<string>();

            while (queue.Any())
            {
                var data = queue.Dequeue();
                var components = data.GameObject.GetComponents<Component>();

                float progressEachComponent = (data.Progress) / (components.Length + data.GameObject.transform.childCount);

                foreach (var component in components)
                {
                    currentProgress += progressEachComponent;
                    if (onProgress($"Searching missing references in {context}", data.GameObject.name, currentProgress))
                    {
                        SearchIsCanceled = true;
                        onComplete.Invoke("Missing Reference Utility", $"Search in [{context}] was canceled");
                        return (missingReferences, missingComponents);
                    }
                    
                    if (!component)
                    {
                        missingComponents.Add($"Path: {data.Path}");
                        continue;
                    }

                    using var so = new SerializedObject(component);
                    using var sp = so.GetIterator();
                    
                    while (sp.NextVisible(true))
                    {
                        if (sp.propertyType != SerializedPropertyType.ObjectReference) continue;
                        if (sp.objectReferenceValue == null
                            && sp.objectReferenceInstanceIDValue != 0)
                        {
                            missingReferences.Add($"Path: {data.Path} | Component: {component.name} | Reference: {sp.name}");
                        }
                    }
                }

                foreach (Transform child in data.GameObject.transform)
                {
                    if (child.gameObject == data.GameObject) continue;
                    var path = isAssetObject
                        ? AssetDatabase.GetAssetPath(child.gameObject)
                        : child.GetPath();
                    
                    queue.Enqueue(new ProgressContainer(progressEachComponent, child.gameObject, path));
                }
            }
            
            return (missingReferences, missingComponents);
        }
    }
}