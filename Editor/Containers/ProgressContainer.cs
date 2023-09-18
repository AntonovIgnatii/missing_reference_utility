using UnityEngine;

namespace MissingReferencesUtility
{
    public struct ProgressContainer
    {
        public float Progress;
        public GameObject GameObject;
        public string Path;

        public ProgressContainer(float progress, GameObject gameObject, string path)
        {
            Progress = progress;
            GameObject = gameObject;
            Path = path;
        }
    }
}