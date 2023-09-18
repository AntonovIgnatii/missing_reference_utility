using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace MissingReferencesUtility
{
    public class MissingReferencesUtilityWindow : EditorWindow
    {
        [SerializeField] private TreeViewState m_TreeViewState = new ();
        
        private MissingReferencesTreeView m_MissingReferenceTreeView;
        private MissingReferenceExecutor m_MissingReferenceExecutor;
        
        [MenuItem("Tools/Find Missing Reference/Analyze Window")]
        private static void ShowWindow()
        {
            var window = GetWindow<MissingReferencesUtilityWindow> ();
            window.titleContent = new GUIContent ("Missing Reference Find Utility");
            window.Show ();
        }

        private void OnEnable()
        {
            m_TreeViewState ??= new TreeViewState();
            m_MissingReferenceExecutor ??= new MissingReferenceExecutor();

            m_MissingReferenceTreeView = new MissingReferencesTreeView(m_TreeViewState);
        }

        private void OnGUI()
        {
            DrawToolBar();
            DrawTreeView();
        }

        private void DrawToolBar()
        {
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Analyze Selected Rules"))
                {
                    var selection = m_MissingReferenceTreeView.GetSelection();

                    foreach (var id in selection)
                    {
                        var searchResult = m_MissingReferenceExecutor.AnalyzeSelectedRule((FindType)id);
                        m_MissingReferenceTreeView.AddCustomTreeItems(id, searchResult);
                    }
                }

                if (GUILayout.Button("Clear"))
                {
                    m_MissingReferenceTreeView.Clear();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTreeView()
        {
            m_MissingReferenceTreeView.OnGUI(new Rect(0, 30, position.width, position.height));
        }
    }
}