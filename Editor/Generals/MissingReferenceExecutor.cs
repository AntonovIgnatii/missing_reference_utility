using System.Collections.Generic;
using UnityEditor;

namespace MissingReferencesUtility
{
    public class MissingReferenceExecutor
    {
        private CurrentSceneFinder m_CurrentSceneFinder;
        private AllScenesFinder m_AllSceneFinder;
        private AssetsFinder m_AssetsFinder;

        private readonly string _progressBarTitle = "Missing Reference Utility";
        private readonly string _progressBarInfo = "Preparing search";

        public MissingReferenceExecutor()
        {
            m_CurrentSceneFinder = new CurrentSceneFinder();
            m_AllSceneFinder = new AllScenesFinder();
            m_AssetsFinder = new AssetsFinder();
        }

        public List<ResultContainer> AnalyzeSelectedRule(FindType findType)
        {
            DisplayProgressBar(_progressBarTitle, _progressBarInfo, 0f);

            switch (findType)
            {
                case FindType.CurrentScene: return m_CurrentSceneFinder.Find(DisplayCanceledProgressBar, DisplayDialog);
                case FindType.AllScenes: return m_AllSceneFinder.Find(DisplayCanceledProgressBar, DisplayDialog);
                case FindType.Assets: return m_AssetsFinder.Find(DisplayCanceledProgressBar, DisplayDialog);
                case FindType.None:
                default:
                    EditorUtility.ClearProgressBar();
                    return null;
            }
        }

        private void DisplayProgressBar(string title, string info, float progress)
            => EditorUtility.DisplayProgressBar($"{title}", $"{info}", progress);

        private bool DisplayCanceledProgressBar(string title, string info, float progress)
            => EditorUtility.DisplayCancelableProgressBar($"{title}", $"{info}", progress);

        private void DisplayDialog(string title, string message)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog($"{title}", $"{message}", "Ok");
        }
    }
}