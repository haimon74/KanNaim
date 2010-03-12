using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace UserControlsLibrary
{
    public class MenusTreeContainer
    {
        private TreeView _root;

        public MenusTreeContainer(IQueryable<Table_Menus> menus)
        {   
            _root = new TreeView();
            var menuRoots = from c in menus
                            where c.ParentId == -1
                            select c;

            foreach (var root in menuRoots)
            {
                var newNode = new TreeNode(root.DisplayText, root.ItemId.ToString(), 
                    root.CssClassName, root.OnClickUrl, "");
                newNode.ToolTip = root.MenuId.ToString();
                newNode.SelectAction = TreeNodeSelectAction.SelectExpand;
                _root.Nodes.Add(newNode);
                PopulateSubLevelNodes(ref menus, newNode);
            }
        }

        private static void PopulateSubLevelNodes(ref IQueryable<Table_Menus> menus, TreeNode parent)
        {
            var childNodes = from c in menus
                             where c.ParentId.ToString() == parent.Value
                             select c;

            foreach (var node in childNodes)
            {
                var newNode = new TreeNode(node.DisplayText, node.ItemId.ToString(), 
                    node.CssClassName, node.OnClickUrl, node.OnClickUrl);
                newNode.SelectAction = TreeNodeSelectAction.SelectExpand;
                parent.ChildNodes.Add(newNode);
                PopulateSubLevelNodes(ref menus, newNode);
            }
        }

        public TreeView GetMenuAsTreeView(int menuId)
        {
            string menuIdStr = menuId.ToString();
            var returnValue = new TreeView();
            
            var root = new TreeNode();
            foreach (TreeNode node in _root.Nodes)
            {
                if (node.ToolTip == menuIdStr)
                    root = node;
            }
            returnValue.Nodes.Add(root);
            return returnValue;
        }
        private TreeNode GetRootByMenuId(int menuId)
        {
            string menuIdStr = menuId.ToString();
            
            var root = new TreeNode();
            TreeNode[] roots = new TreeNode[_root.Nodes.Count];
            
            _root.Nodes.CopyTo(roots, 0);

            foreach (TreeNode node in roots)
            {
                if (node.ToolTip == menuIdStr)
                    root = node;
            }
            return root;
        }
        private void ShowAllCheckBoxes(bool show, TreeNode treeNode)
        {
            foreach (TreeNode node in treeNode.ChildNodes)
            {
                node.ShowCheckBox = show;
                ShowAllCheckBoxes(show, node);
            }
        }
        public TreeNode Clone(TreeNode treeNode)
        {
            TreeNode returnNode = new TreeNode
                                      {
                                          Checked = treeNode.Checked,
                                          NavigateUrl = treeNode.NavigateUrl,
                                          Text = treeNode.Text,
                                          ToolTip = treeNode.ToolTip,
                                          Expanded = treeNode.Expanded,
                                          ImageUrl = treeNode.ImageUrl,
                                          ImageToolTip = treeNode.ImageToolTip,
                                          PopulateOnDemand = treeNode.PopulateOnDemand,
                                          SelectAction = treeNode.SelectAction,
                                          Selected = treeNode.Selected,
                                          Value = treeNode.Value,
                                          ShowCheckBox = treeNode.ShowCheckBox,
                                          Target = treeNode.Target
                                      };
            foreach (TreeNode node in treeNode.ChildNodes)
            {
                TreeNode clonedNode = Clone(node);
                returnNode.ChildNodes.Add(clonedNode);
            }
            return returnNode;
        }

        public void PopulateMenuToTreeView(int menuId, TreeView treeView)
        {
            //TreeNode newRoot = Clone(GetRootByMenuId(menuId));
            treeView.Nodes.Add(GetRootByMenuId(menuId));
        }
        public void PopulateMenuToTreeView(int menuId, TreeView treeView,
            bool showCheckBoxes, bool expandAll, bool collapseAll)
        {
            treeView.Nodes.Add(GetRootByMenuId(menuId));
            treeView.Nodes[0].Expand();


            foreach (TreeNode node in treeView.Nodes)
            {
                node.ShowCheckBox = showCheckBoxes;
                ShowAllCheckBoxes(showCheckBoxes, node);
            }
            if (expandAll)
                treeView.Nodes[0].ExpandAll();
            if (collapseAll)
                treeView.Nodes[0].CollapseAll();
        }
    }
}
