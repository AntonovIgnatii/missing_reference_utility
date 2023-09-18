using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace MissingReferencesUtility
{
    public class MissingReferencesTreeElements
    {
        private TreeViewItem m_CoreViewItem;
        
        private TreeViewItem m_CurrentSceneViewItem;
        private TreeViewItem m_AllSceneViewItem;
        private TreeViewItem m_AssetsViewItem;

        private int m_LastID = 100;
        
        public TreeViewItem CoreViewItem()
        {
            if (m_CoreViewItem == null)
            {
                BuildDefaultViewItems();
            }

            return m_CoreViewItem;
        }
        
        public void BuildDefaultViewItems()
        {
            m_CoreViewItem           = new TreeViewItem   { id = (int)FindType.None, depth = -1, displayName = "Analyze Rules" };
            
            m_CurrentSceneViewItem   = new TreeViewItem   { id = (int)FindType.CurrentScene, depth = 0, displayName = "Current Scene Rule" };
            m_AllSceneViewItem       = new TreeViewItem   { id = (int)FindType.AllScenes, depth = 0, displayName = "All Scene Rule" };
            m_AssetsViewItem         = new TreeViewItem   { id = (int)FindType.Assets, depth = 0, displayName = "All Project Assets Rule" };
            
            m_CoreViewItem.AddChild(m_CurrentSceneViewItem);
            m_CurrentSceneViewItem.AddChild(EmptyViewItem());
            m_CoreViewItem.AddChild(m_AllSceneViewItem);
            m_AllSceneViewItem.AddChild(EmptyViewItem());
            m_CoreViewItem.AddChild(m_AssetsViewItem);
            m_AssetsViewItem.AddChild(EmptyViewItem());
            
            m_LastID = 100;
        }

        private void Repaint()
        {
            m_CoreViewItem           = new TreeViewItem   { id = (int)FindType.None, depth = -1, displayName = "Analyze Rules" };
            
            m_CoreViewItem.AddChild(m_CurrentSceneViewItem);
            m_CoreViewItem.AddChild(m_AllSceneViewItem);
            m_CoreViewItem.AddChild(m_AssetsViewItem);
        }
        public void AddCustomTreeItems(int selectedID, List<ResultContainer> resultContainers)
        {
            if (ValidateResults(resultContainers) == false) return;
            
            var selectedEnum = (FindType)selectedID;
            TreeViewItem targetTreeViewItem = null;
            
            switch (selectedEnum)
            {
                case FindType.CurrentScene:
                    m_CurrentSceneViewItem   = new TreeViewItem   { id = (int)FindType.CurrentScene, depth = 0, displayName = "Current Scene Rule" };
                    m_CoreViewItem.AddChild(m_CurrentSceneViewItem);
                    targetTreeViewItem = m_CurrentSceneViewItem;
                    break;
                case FindType.AllScenes:
                    m_AllSceneViewItem       = new TreeViewItem   { id = (int)FindType.AllScenes, depth = 0, displayName = "All Scene Rule" };
                    m_CoreViewItem.AddChild(m_AllSceneViewItem);
                    targetTreeViewItem = m_AllSceneViewItem;
                    break;
                case FindType.Assets:
                    m_AssetsViewItem         = new TreeViewItem   { id = (int)FindType.Assets, depth = 0, displayName = "All Project Assets Rule" };
                    m_CoreViewItem.AddChild(m_AssetsViewItem);
                    targetTreeViewItem = m_AssetsViewItem;
                    break;
                case FindType.None:
                default:
                    return;
            }

            Repaint();
            
            var totalCount = 0;
            foreach (var container in resultContainers)
            {
                totalCount += container.MissingComponents.Count;
                totalCount += container.MissingReferences.Count;
                
                if (!string.IsNullOrEmpty(container.ContainerName))
                {
                    var contextTreeViewItem = new TreeViewItem() { id = ++m_LastID, depth = 1, displayName = $"{container.ContainerName} ({totalCount})" };
                    
                    targetTreeViewItem.AddChild(contextTreeViewItem);
                    
                    EnqueueResults(contextTreeViewItem, "Missing References", 2, container.MissingReferences.OrderBy(x => x).ToList());
                    EnqueueResults(contextTreeViewItem, "Missing Components", 2, container.MissingComponents.OrderBy(x => x).ToList());
                    
                    continue;
                }

                EnqueueResults(targetTreeViewItem, "Missing References", 1, container.MissingReferences.OrderBy(x => x).ToList());
                EnqueueResults(targetTreeViewItem,"Missing Components", 1, container.MissingComponents.OrderBy(x => x).ToList());
            }

            targetTreeViewItem.displayName = $"{targetTreeViewItem.displayName} ({totalCount})";
        }

        private void EnqueueResults(TreeViewItem parent, string context, int depth, List<string> paths)
        {
            var contextTreeViewItem = new TreeViewItem() { id = ++m_LastID, depth = depth, displayName = $"{context} ({paths.Count})" };

            parent.AddChild(contextTreeViewItem);
            
            foreach (var path in paths)
            {
                var pathTreeViewItem = new TreeViewItem() { id = ++m_LastID, depth = depth + 1, displayName = $"{path}" };
                
                contextTreeViewItem.AddChild(pathTreeViewItem);
            }
        }

        private bool ValidateResults(List<ResultContainer> resultContainers)
        {
            if (resultContainers == null || resultContainers.Count < 1)
            {
                return false;
            }

            var result = false;

            foreach (var container in resultContainers)
            {
                if (container.MissingComponents is { Count: > 0 }) result = true;
                if (container.MissingReferences is { Count: > 0 }) result = true;
            }
            
            return result;
        }

        private TreeViewItem EmptyViewItem() => new TreeViewItem() { id = -1, depth = 1, displayName = "No issues found" };
    }
}