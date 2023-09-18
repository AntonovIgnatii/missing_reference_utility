using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace MissingReferencesUtility
{
    public class MissingReferencesTreeView : TreeView
    {
        private MissingReferencesTreeElements m_CoreViewItem;
        public MissingReferencesTreeView(TreeViewState state) : base(state)
        {
            showAlternatingRowBackgrounds = true;
            showBorder = true;
            
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            m_CoreViewItem ??= new MissingReferencesTreeElements();

            return m_CoreViewItem.CoreViewItem();
        }

        public void AddCustomTreeItems(int selectedID, List<ResultContainer> result)
        {
            m_CoreViewItem.AddCustomTreeItems(selectedID, result);

            Reload();
        }
        
        public void Clear()
        {
            m_CoreViewItem.BuildDefaultViewItems();

            SetupDepthsFromParentsAndChildren(m_CoreViewItem.CoreViewItem());
            
            Reload();
        }
    }
}