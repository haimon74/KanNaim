using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using UserControlsLibrary;

public partial class CMS_MenusManagement : System.Web.UI.Page
{
    protected void Page_PreInit(object sender, EventArgs e)
    {
        Page.Theme = "Theme2";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Session["ActiveCssFile"] = "Styles/secondary.css";
        ((MenusTreeContainer)Session["MenusTreeContainer"]).
            PopulateMenuToTreeView(1, TreeViewTest, true, false, false);
    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateMenuTreeView();
    }
    private void UpdateMenuTreeView()
    {
        TreeViewTest.Nodes.Clear();
        Application["IQ_Menus"] = DataAccess.Select.IqTableMenus();
        Application["MenusTreeContainer"] =
            new MenusTreeContainer((IQueryable<Table_Menus>)Application["IQ_Menus"]);
        Session["MenusTreeContainer"] =
            new MenusTreeContainer((IQueryable<Table_Menus>)Application["IQ_Menus"]);
        ((MenusTreeContainer)Session["MenusTreeContainer"]).
            PopulateMenuToTreeView(int.Parse(DropDownList1.SelectedValue), TreeViewTest, true, false, false);
        DropDownList1.DataBind();
        //TreeViewTest.DataBind();
    }
    protected void ButtonAddNewMenu_Click(object sender, EventArgs e)
    {
        Session["Table_LookupMenus"] = (IQueryable<Table_LookupMenus>) Application["Table_LookupMenus"];
        var menusList = (IQueryable<Table_LookupMenus>) Session["Table_LookupMenus"];

        Table_LookupMenus menu = DataAccess.Select.TableLookupMenuByName(TextBoxNewMenuName.Text);

        if (menu == null)
        {
            // add new menu
            Table_LookupMenus newLookup = new Table_LookupMenus
                                              {
                                                  MenuDescription = TextBoxNewMenuName.Text
                                              };
            newLookup = DataAccess.Insert.TableLookupMenu(newLookup);

            if (newLookup != null)
            {
                TextBoxNewMenuName.Text = "";

                Table_Menus newMenu = new Table_Menus
                                          {
                                              CssClassName = " ",
                                              DisplayText = TextBoxRootName.Text,
                                              MenuId = newLookup.MenuId,
                                              OnClickUrl = " ",
                                              ParentId = -1,
                                              ToolTip = TextBoxRootName.Text
                                          };
                newMenu = DataAccess.Insert.TableManus(newMenu);
                if (newMenu != null)
                {
                    TextBoxRootName.Text = "";
                    UpdateMenuTreeView();
                }
            }
        }
    }
    protected void ButtonDeleteMenu_Click(object sender, EventArgs e)
    {
        int menuId = int.Parse(DropDownList1.SelectedValue);
        DataAccess.Delete.TableLookupMenusByMenuId(menuId);
        DataAccess.Delete.TableMenusByMenuId(menuId);
        UpdateMenuTreeView();
    }
    private void RemoveParentAndChildNodes(TreeNode node)
    {
        foreach (TreeNode childNode in node.ChildNodes)
        {
            RemoveParentAndChildNodes(childNode);
        }
        DataAccess.Delete.TableMenusByItemId(int.Parse(node.Value));
    }
    protected void ButtonDeleteChecked_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            RemoveParentAndChildNodes(node);
        }
        UpdateMenuTreeView();
    }
    protected void ButtonAddNewNode_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            Table_Menus newNode = new Table_Menus
                                      {
                                          CssClassName = TextBoxCss.Text,
                                          DisplayText = TextBoxDisplayText.Text,
                                          MenuId = int.Parse(DropDownList1.SelectedValue),
                                          OnClickUrl = TextBoxUrl.Text,
                                          ParentId = int.Parse(node.Value),
                                          ToolTip = TextBoxToolTip.Text
                                      };
            DataAccess.Insert.TableManus(newNode);
        }
        UpdateMenuTreeView();
    }
    protected void ButtonMoveDown_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            int checkedIndex = node.Parent.ChildNodes.IndexOf(node);
            if (node.Parent.ChildNodes.Count > checkedIndex + 1)
            { // reorder nodes
                TreeNode checkedNode = node;
                Table_Menus checkedMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(checkedNode.Value));
                TreeNode nextNode = node.Parent.ChildNodes[checkedIndex + 1];
                Table_Menus nextMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(nextNode.Value));

                bool succeeded = ReorderTableMenusNodes(checkedMenuRecord, nextMenuRecord);
            }
        }
        UpdateMenuTreeView();
    }

    private bool ReorderTableMenusNodes(Table_Menus record1, Table_Menus record2)
    {
        Dictionary<string, object> args1 = new Dictionary<string, object>();
        Dictionary<string, object> args2 = new Dictionary<string, object>();

        args1["OnClickUrl"] = record1.OnClickUrl;
        args1["DisplayText"] = record1.DisplayText;
        args1["CssClassName"] = record1.CssClassName;
        args1["ToolTip"] = record1.ToolTip;

        args2["OnClickUrl"] = record2.OnClickUrl;
        args2["DisplayText"] = record2.DisplayText;
        args2["CssClassName"] = record2.CssClassName;
        args2["ToolTip"] = record2.ToolTip;

        bool returnValue = DataAccess.Update.TableMenus(record2.ItemId, args1);
        returnValue = returnValue && DataAccess.Update.TableMenus(record1.ItemId, args2);

        return returnValue;
    }
    protected void ButtonMoveUp_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            int checkedIndex = node.Parent.ChildNodes.IndexOf(node);
            if (node.Parent.ChildNodes.Count > 0)
            { // reorder nodes
                TreeNode checkedNode = node;
                Table_Menus checkedMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(checkedNode.Value));
                TreeNode beforeNode = node.Parent.ChildNodes[checkedIndex - 1];
                Table_Menus beforeMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(beforeNode.Value));

                bool succeeded = ReorderTableMenusNodes(beforeMenuRecord, checkedMenuRecord);
            }
        }
        UpdateMenuTreeView();
    }
    protected void ButtonMoveLeaf_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            int checkedIndex = node.Parent.ChildNodes.IndexOf(node);
            if (checkedIndex > 0)
            { // make node the leaf of its previous node
                TreeNode checkedNode = node;
                Table_Menus checkedMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(checkedNode.Value));
                TreeNode beforeNode = node.Parent.ChildNodes[checkedIndex - 1];
                //Table_Menus beforeMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(beforeNode.Value));

                Dictionary<string, object> args = new Dictionary<string, object>();
                args["ParentId"] = int.Parse(beforeNode.Value);
                bool succeeded = DataAccess.Update.TableMenus(checkedMenuRecord.ItemId, args);
            }
        }
        UpdateMenuTreeView();
    }
    protected void ButtonMoveBranch_Click(object sender, EventArgs e)
    {
        foreach (TreeNode node in TreeViewTest.CheckedNodes)
        {
            TreeNode grandParentNode = (node.Parent != null) ? node.Parent.Parent : node.Parent;
            int newParentId = (grandParentNode != null) ? int.Parse(grandParentNode.Value) : -1;
            if (newParentId > 0)
            { // set node's parent as grand parent
                TreeNode checkedNode = node;
                Table_Menus checkedMenuRecord = DataAccess.Select.TableMenuByItemId(int.Parse(checkedNode.Value));
                
                Dictionary<string, object> args = new Dictionary<string, object>();
                args["ParentId"] = newParentId;
                bool succeeded = DataAccess.Update.TableMenus(checkedMenuRecord.ItemId, args);
            }
        }
        UpdateMenuTreeView();
    }
}
