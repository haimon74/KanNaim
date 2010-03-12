using System;
using System.Collections.Generic;
using System.Globalization;
using ComponentArt.Web.UI;
using GalleryServerPro.Business;
using GalleryServerPro.Business.Interfaces;
using GalleryServerPro.ErrorHandler.CustomExceptions;

namespace GalleryServerPro.Web.Controls
{
	public partial class albumtreeview : GalleryUserControl
	{
		#region Private Fields

		private IAlbum _albumToSelect;
		private IIntegerCollection _albumIdsToSelect = new IntegerCollection();
		private SecurityActions _requiredSecurityPermissions;
		private string _securityPermissionParm;

		#endregion

		#region Protected Events

		protected void Page_Load(object sender, EventArgs e)
		{
			RegisterJavascript();

			ConfigureControlsEveryTime();
		}

		protected void cv_OnServerValidate(object sender, System.Web.UI.WebControls.ServerValidateEventArgs args)
		{
			int albumId;
			args.IsValid = ((tv.SelectedNode != null) && Int32.TryParse(tv.SelectedNode.Value, out albumId) && (albumId > int.MinValue));
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets a reference to the ComponentArt.Web.UI.TreeView control within this user control.
		/// </summary>
		public TreeView TreeView
		{
			get
			{
				return tv;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether more than one checkbox can be selected at a time in the treeview.
		/// This property should be set before calling DataBind().
		/// </summary>
		public bool AllowMultiSelect
		{
			get
			{
				object viewStateValue = ViewState["AllowMultiSelect"];
				bool allowMultiSelect;
				if ((viewStateValue != null) && (Boolean.TryParse(viewStateValue.ToString(), out allowMultiSelect)))
					return allowMultiSelect;
				else
					return false;
			}
			set
			{
				ViewState["AllowMultiSelect"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the user is required to select an album from the treeview.
		/// Default is false.
		/// </summary>
		public bool RequireAlbumSelection
		{
			get
			{
				return cv.Enabled;
			}
			set
			{
				cv.Enabled = value;
			}
		}

		/// <summary>
		/// Gets or sets the security permission the logged on user must have in order for an album to be displayed in the
		/// treeview. It may be a single value or some combination of valid enumeration values.
		/// </summary>
		public SecurityActions RequiredSecurityPermissions
		{
			get
			{
				return this._requiredSecurityPermissions;
			}
			set
			{
				if (!SecurityActionEnumHelper.IsValidSecurityAction(value))
					throw new ArgumentException("Invalid SecurityActions enumeration value.");

				this._requiredSecurityPermissions = value;
			}
		}

		/// <summary>
		/// Gets a string representing the RequiredSecurityPermission property that can be used as a querystring parameter.
		/// Ex: "&secaction=3"
		/// </summary>
		private string SecurityPermissionQueryStringParm
		{
			get
			{
				if (String.IsNullOrEmpty(this._securityPermissionParm))
				{
					if (SecurityActionEnumHelper.IsValidSecurityAction(this.RequiredSecurityPermissions))
					{
						this._securityPermissionParm = String.Format(CultureInfo.CurrentCulture, "&secaction={0}", (int)this.RequiredSecurityPermissions);
					}
				}

				return this._securityPermissionParm;
			}
		}

		/// <summary>
		/// Gets a list of the checked treeview nodes in the treeview.
		/// </summary>
		public TreeViewNode[] CheckedNodes
		{
			get
			{
				return tv.CheckedNodes;
			}
		}

		/// <summary>
		/// Gets a collection of the "highest" checked nodes.
		/// </summary>
		public IIntegerCollection TopLevelCheckedAlbumIds
		{
			get
			{
				TreeViewNode[] checkedNodes = tv.CheckedNodes;
				IIntegerCollection checkedNodeIds = new IntegerCollection();

				foreach (TreeViewNode node in checkedNodes)
				{
					if (IsTopLevelCheckedNode(node))
					{
						checkedNodeIds.Add(Convert.ToInt32(node.Value, CultureInfo.InvariantCulture));
					}
				}

				return checkedNodeIds;
			}
		}

		/// <summary>
		/// Gets a reference to the collection of album IDs whose associated checkboxes should be checked.
		/// Add the desired album IDs to this collection and then call DataBind(). This user control 
		/// guarantees that ALL albums in this collection are rendered and made visible during the DataBind() method.
		/// The collection is cleared after the databind is finished. Use the property TopLevelCheckedAlbumIds
		/// to retrieve the list of top level checked albums after the user has interacted with the control.
		/// </summary>
		public IIntegerCollection AlbumIdsToSelect
		{
			get
			{
				if (this._albumIdsToSelect == null)
				{
					this._albumIdsToSelect = new IntegerCollection();
				}

				return this._albumIdsToSelect;
			}
		}

		///// <summary>
		///// Gets a collection of the album IDs corresponding to checked nodes in the treeview.
		///// </summary>
		//public IIntegerCollection CheckedAlbumIds
		//{
		//  get
		//  {
		//    TreeViewNode[] checkedNodes = tv.CheckedNodes;
		//    IIntegerCollection checkedNodeIds = new IntegerCollection();

		//    foreach (TreeViewNode node in checkedNodes)
		//    {
		//      checkedNodeIds.Add(Convert.ToInt32(node.Value));
		//    }

		//    return checkedNodeIds;
		//  }
		//}
		
		/// <summary>
		/// Gets or sets the selected node in the treeview. Only one node can be selected at a time.
		/// </summary>
		public TreeViewNode SelectedNode
		{
			get
			{
				return tv.SelectedNode;
			}
			set
			{
				tv.SelectedNode = value;
			}
		}

		/// <summary>
		/// Gets or sets the with of the treeview control.
		/// </summary>
		public System.Web.UI.WebControls.Unit Width
		{
			get
			{
				return tv.Width;
			}
			set
			{
				tv.Width = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the treeview control.
		/// </summary>
		public System.Web.UI.WebControls.Unit Height
		{
			get
			{
				return tv.Height;
			}
			set
			{
				tv.Height = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of a javascript function to invoke when a treenode is selected.
		/// Returns String.Empty if it has not been assigned.
		/// </summary>
		public string ClientOnTreeNodeSelectJavascriptFunctionName
		{
			get
			{
				object viewStateValue = ViewState["OnNodeSelect"];

				return (viewStateValue != null ? viewStateValue.ToString() : String.Empty);
			}
			set
			{
				ViewState["OnNodeSelect"] = value;
			}
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// Render the treeview to two levels - the root album and its direct children. If the AlbumIdsToCheck property
		/// has items in its collection, make sure every album in the collection is rendered, no matter how deep in the album heirarchy 
		/// they may be. If the albumToSelect parameter is specified, then make sure this album is rendered and 
		/// selected/checked, no matter how deep in the hierarchy it may be.
		/// </summary>
		public void BindTreeView()
		{
			BindTreeView(null);
		}

		/// <summary>
		/// Render the treeview to two levels - the root album and its direct children. If the AlbumIdsToCheck property
		/// has items in its collection, make sure every album in the collection is rendered, no matter how deep in the album heirarchy 
		/// they may be. If the albumToSelect parameter is specified, then make sure this album is rendered and 
		/// selected/checked, no matter how deep in the hierarchy it may be.
		/// </summary>
		/// <param name="albumToSelect">An album to be selected, checked, and made visible. The treeview is automatically expanded as
		/// needed to ensure this album is visible.</param>
		public void BindTreeView(IAlbum albumToSelect)
		{
			this._albumToSelect = albumToSelect;

			DataBindTreeView();

			this._albumToSelect = null;

			this.AlbumIdsToSelect.Clear();
		}

		#endregion

		#region Private Methods

		private void ConfigureControlsEveryTime()
		{
			tv.LineImagesFolderUrl = String.Concat(Util.GalleryRoot, "/images/componentart/treeview/lines");

			cv.Text = String.Format(CultureInfo.CurrentCulture, "<span class='gsp_bold'>{0} </span>{1}", Resources.GalleryServerPro.Task_Transfer_Objects_Cannot_Transfer_No_Destination_Album_Selected_Hdr, Resources.GalleryServerPro.Task_Transfer_Objects_Cannot_Transfer_No_Destination_Album_Selected_Dtl);

			if (this.AllowMultiSelect)
			{
				tv.ClientEvents.NodeExpand = new ClientEvent("tv_onNodeExpand");
				//tv.ClientEvents.NodeCheckChange = new ClientEvent("tv_onNodeCheckChange");
			}
		}

		/// <summary>
		/// Render the treeview with the first two levels of albums that are viewable to the logged on user.
		/// </summary>
		private void DataBindTreeView()
		{
			#region Validation

			//if (!this.AllowMultiSelect && this.AlbumIdsToCheck.Count > 1)
			//{
			//  throw new InvalidOperationException("The property AllowMultiSelect must be false when multiple album IDs have been assigned to the property AlbumIdsToCheck.");
			//}

			if (!this.AllowMultiSelect && this.AlbumIdsToSelect.Count > 1)
			{
				throw new InvalidOperationException("The property AllowMultiSelect must be false when multiple album IDs have been assigned to the property TopLevelAlbumIdsToCheck.");
			}

			if (!SecurityActionEnumHelper.IsValidSecurityAction(this.RequiredSecurityPermissions))
			{
				throw new InvalidOperationException("The property GalleryServerPro.Web.Controls.albumtreeview.RequiredSecurityPermissions must be assigned before the TreeView can be rendered.");
			}
			
			#endregion

			// Add root node.
			IAlbum rootAlbum = Factory.LoadRootAlbum(this.RequiredSecurityPermissions, this.GalleryPage.GetGalleryServerRolesForUser());

			TreeViewNode rootNode = new TreeViewNode();
			rootNode.Text = Util.RemoveHtmlTags(rootAlbum.Title);
			rootNode.Value = rootAlbum.Id.ToString(CultureInfo.InvariantCulture);
			rootNode.ID = rootAlbum.Id.ToString(CultureInfo.InvariantCulture);
			rootNode.Expanded = true;

			bool isAlbumSelectable = !rootAlbum.IsVirtualAlbum;
			rootNode.ShowCheckBox = isAlbumSelectable;
			rootNode.Selectable = isAlbumSelectable;
			if (!isAlbumSelectable) rootNode.HoverCssClass = String.Empty;

			// Select and check this node if needed.
			if (isAlbumSelectable && (this._albumToSelect != null) && (rootAlbum.Id == _albumToSelect.Id))
			{
				tv.SelectedNode = rootNode;
				rootNode.Checked = true;
			}

			// Check this node if needed.
			if (this._albumIdsToSelect.Contains(rootAlbum.Id))
			{
				rootNode.Checked = true;
			}

			tv.Nodes.Clear();
			tv.Nodes.Add(rootNode);

			// Add the first level of albums below the root album.
			BindAlbumToTreeview(rootAlbum.GetChildGalleryObjects(GalleryObjectType.Album), rootNode, false);

			if ((this._albumToSelect != null) && (tv.SelectedNode == null))
			{
				// We have an album we are supposed to select, but we haven't encountered it in the first two levels,
				// so expand the treeview as needed to include this album.
					BindSpecificAlbumToTreeview(this._albumToSelect);
			}

			// Make sure all specified albums are visible and checked.
			foreach (int albumId in this._albumIdsToSelect)
			{
				BindSpecificAlbumToTreeview(Factory.LoadAlbumInstance(albumId, false));
			}
		}

		/// <summary>
		/// Bind the specified album to the treeview. This method assumes the treeview has at least the root node already
		/// built. The specified album can be at any level in the hierarchy. Nodes between the album and the existing top node
		/// are automatically created so that the full node path to the album is shown.
		/// </summary>
		/// <param name="album">An album to be added to the treeview.</param>
		private void BindSpecificAlbumToTreeview(IAlbum album)
		{
			if (tv.FindNodeById(album.Id.ToString(CultureInfo.InvariantCulture)) == null)
			{
				// Get a stack of albums that go from the current album to the top level album.
				// Once the stack is built we'll then add these albums to the treeview so that the full heirarchy
				// to the current album is shown.
				TreeViewNode existingParentNode;
				Stack<IAlbum> albumParents = GetAlbumsBetweenTopLevelNodeAndAlbum(tv, album, out existingParentNode);

				if (existingParentNode == null)
					return;

				BindSpecificAlbumToTreeview(existingParentNode, albumParents);
			}
		}

		/// <summary>
		/// Retrieve a list of albums that are in the heirarchical path between the specified album and a node in the treeview.
		/// The node that is discovered as the ancestor of the album is assigned to the existingParentNode parameter.
		/// </summary>
		/// <param name="treeview">The treeview with at least one node added to it. At least one node must be an ancestor of the 
		/// specified album.</param>
		/// <param name="album">An album. This method navigates the ancestors of this album until it finds a matching node in the treeview.</param>
		/// <param name="existingParentNode">The existing node in the treeview that is an ancestor of the specified album is assigned to
		/// this parameter.</param>
		/// <returns>Returns a list of albums where the first album (the one returned by calling Pop) is a child of the album 
		/// represented by the existingParentNode treeview node, and each subsequent album is a child of the previous album.
		/// The final album is the same album specified in the album parameter.</returns>
		private static Stack<IAlbum> GetAlbumsBetweenTopLevelNodeAndAlbum(ComponentArt.Web.UI.TreeView treeview, IAlbum album, out TreeViewNode existingParentNode)
		{
			if (treeview.Nodes.Count == 0)
				throw new ArgumentException("The treeview must have at least one top-level node before calling the function GetAlbumsBetweenTopLevelNodeAndAlbum().");
			
			Stack<IAlbum> albumParents = new Stack<IAlbum>();
			albumParents.Push(album);

			IAlbum parentAlbum = (IAlbum) album.Parent;

			albumParents.Push(parentAlbum);

			// Navigate up from the specified album until we find an album that exists in the treeview. Remember,
			// the treeview has been built with the root node and the first level of albums, so eventually we
			// should find an album. If not, just return without showing the current album.
			while ((existingParentNode = treeview.FindNodeById(parentAlbum.Id.ToString(CultureInfo.InvariantCulture))) == null)
			{
				parentAlbum = parentAlbum.Parent as IAlbum;

				if (parentAlbum == null)
					break;

				albumParents.Push(parentAlbum);
			}

			// Since we found a node in the treeview we don't need to add the most recent item in the stack. Pop it off.
			albumParents.Pop();

			return albumParents;
		}

		/// <summary>
		/// Bind the heirarchical list of albums to the specified treeview node.
		/// </summary>
		/// <param name="existingParentNode">The treeview node to add the first album in the stack to.</param>
		/// <param name="albumParents">A list of albums where the first album should be a child of the specified treeview
		/// node, and each subsequent album is a child of the previous album.</param>
		private void BindSpecificAlbumToTreeview(TreeViewNode existingParentNode, Stack<IAlbum> albumParents)
		{
			// Assumption: The first album in the stack is a child of the existingParentNode node.
			existingParentNode.Expanded = true;

			// For each album in the heirarchy of albums to the current album, add the album and all its siblings to the 
			// treeview.
			foreach (IAlbum album in albumParents)
			{
				if (existingParentNode.Nodes.Count == 0)
				{
					// Add all the album's siblings to the treeview.
					IGalleryObjectCollection childAlbums = Factory.LoadAlbumInstance(Convert.ToInt32(existingParentNode.ID, CultureInfo.InvariantCulture), true).GetChildGalleryObjects(GalleryObjectType.Album, true);
					BindAlbumToTreeview(childAlbums, existingParentNode, false);
				}

				// Now find the album in the siblings we just added that matches the current album in the stack.
				// Set that album as the new parent and expand it.
				TreeViewNode nodeInAlbumHeirarchy = null;
				foreach (TreeViewNode node in existingParentNode.Nodes)
				{
					if (node.ID == album.Id.ToString(CultureInfo.InvariantCulture))
					{
						nodeInAlbumHeirarchy = node;
						nodeInAlbumHeirarchy.Expanded = true;
						break;
					}
				}

				if (nodeInAlbumHeirarchy == null)
					throw new InvalidOperationException(String.Format(CultureInfo.CurrentCulture, "Album ID {0} is not a child of the treeview node representing album ID {1}.", album.Id, Convert.ToInt32(existingParentNode.Value, CultureInfo.InvariantCulture)));

				existingParentNode = nodeInAlbumHeirarchy;
			}
			existingParentNode.Expanded = false;
		}

		/// <summary>
		/// Add the collection of albums to the specified treeview node.
		/// </summary>
		/// <param name="albums">The collection of albums to add the the treeview node.</param>
		/// <param name="parentNode">The treeview node that will receive child nodes representing the specified albums.</param>
		/// <param name="expandNode">Specifies whether the nodes should be expanded.</param>
		private void BindAlbumToTreeview(IGalleryObjectCollection albums, TreeViewNode parentNode, bool expandNode)
		{
			string handlerPath = String.Concat(Util.GalleryRoot, "/handler/gettreeviewxml.ashx");

			foreach (IAlbum album in albums)
			{
				TreeViewNode node = new TreeViewNode();
				node.Text = Util.RemoveHtmlTags(album.Title);
				node.Value = album.Id.ToString(CultureInfo.InvariantCulture);
				node.ID = album.Id.ToString(CultureInfo.InvariantCulture);
				node.Expanded = expandNode;

				node.ShowCheckBox = true;
				node.Selectable = true;

				if (album.GetChildGalleryObjects(GalleryObjectType.Album).Count > 0)
				{
					node.ContentCallbackUrl = String.Format(CultureInfo.CurrentCulture, "{0}?aid={1}{2}", handlerPath, album.Id.ToString(CultureInfo.InvariantCulture), this.SecurityPermissionQueryStringParm);
				}

				// Select and check this node if needed.
				if ((this._albumToSelect != null) && (album.Id == this._albumToSelect.Id))
				{
					tv.SelectedNode = node;
					node.Checked = true;
					node.Expanded = true;
					// Expand the child of the selected album.
					BindAlbumToTreeview(album.GetChildGalleryObjects(GalleryObjectType.Album), node, false);
				}
				
				// Check this node if needed.
				if (this._albumIdsToSelect.Contains(album.Id))
				{
					node.Checked = true;
				}

				parentNode.Nodes.Add(node);
			}
		}

		/// <summary>
		/// Determines whether the specified node is the "highest" checked node, or whether it has any ancestor nodes that are checked.
		/// </summary>
		/// <param name="albumNode">A treeview node for which to determine if any of its parents are checked.</param>
		/// <returns>Returns true if none of this node's ancestors is checked; otherwise returns false.</returns>
		private static bool IsTopLevelCheckedNode(TreeViewNode albumNode)
		{
			if (!albumNode.Checked)
				throw new WebException("Only checked treeview nodes should be passed to the IsTopLevelCheckedNode() method. Instead, the specified node was not checked.");

			TreeViewNode node = albumNode;
			while (node.ParentNode != null)
			{
				node = node.ParentNode;
				if (node.Checked)
					return false;
			}

			return true;
		}

		private void RegisterJavascript()
		{
			string script;
			if (this.AllowMultiSelect)
			{
				script = String.Format(CultureInfo.InvariantCulture, @"
var addedIds = new Array();
var removedIds = new Array();

function tv_onNodeSelect(sender, eventArgs)
{{
	// Manage the checking and unchecking when the node text is clicked.
	// When a node is checked, all its children should be checked. When a node
	// is unchecked, all its parents should be unchecked.
	var selectedNode = eventArgs.get_node(); 
	if (selectedNode == null) return;

	if (selectedNode.get_checked() == true) // User is unchecking node
	{{
		uncheckAll(selectedNode, true);
	}}
	else
	{{
		checkAll(selectedNode);
	}}
	{0}
	sender.render();
}}

function checkAll(parentNode)
{{
	parentNode.set_checked(true);
	var nodes = parentNode.get_nodes();
	for (var i = 0; i < nodes.get_length(); i++)
	{{
		checkAll(nodes.getNode(i));
	}}
}}

function uncheckAll(node, navigateUp)
{{
	if (node == null)
		return;

	node.set_checked(false);
	if (navigateUp)
	{{
		uncheckAll(node.get_parentNode(), navigateUp);
	}}
	else
	{{
		// Drill down, unchecking along the way
		var nodes = node.get_nodes();
		for (var i = 0; i < nodes.get_length(); i++)
		{{
			uncheckAll(nodes.getNode(i), navigateUp);
		}}
	}}
}}
		
function tv_onNodeBeforeCheckChange(sender, eventArgs)
{{
	// Enforce rules: When a node is checked, all its children should be checked. When a node
	// is unchecked, all its parents should be unchecked.
	var checkedNode = eventArgs.get_node(); 
	if (checkedNode == null) return;

	if (checkedNode.get_checked()) // Checked property gives 'before' state
	{{
		// Since the 'before' state is checked, user is trying to uncheck the node.
		uncheckAll(checkedNode, true);
	}}
	else
		checkAll(checkedNode); // User is checking node - always allow.
		
	//The sender parameter is the treeview - tell it to render so that the updates are displayed.
	sender.render();
}}

function tv_onNodeExpand(sender, eventArgs)
{{
	var node = eventArgs.get_node();
	if (node.get_checked())
	{{
		checkAll(node);
		sender.render();
	}}
}}

",
	 GetTreeNodeOnSelectJavascriptFunction()
);
			}
			else
			{
				script = String.Format(CultureInfo.InvariantCulture, @"
function tv_onNodeSelect(sender, e)
{{
	// Manage the checking and unchecking when the node text is clicked.
	var selectedNode = e.get_node(); 
	if (selectedNode == null) return;

	sender.unCheckAll();
	selectedNode.set_checked(true); 
	{0}
	sender.render();
}}
		
function tv_onNodeBeforeCheckChange(sender, e)
{{
	var node = e.get_node(); 
	if (node == null) return;

	sender.unCheckAll(); // user is checking this node
	if (!node.get_checked()) // Checked property gives 'before' state
	{{
		node.set_checked(true);
		sender.selectNodeById(node.get_id());
	}}
		
	sender.render();
}}",
	 GetTreeNodeOnSelectJavascriptFunction()
	 );
			}

			System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "tvFunctions", script, true);

		}

		private string GetTreeNodeOnSelectJavascriptFunction()
		{
			if (String.IsNullOrEmpty(this.ClientOnTreeNodeSelectJavascriptFunctionName))
				return string.Empty;

			return String.Concat(this.ClientOnTreeNodeSelectJavascriptFunctionName, "(sender, e);");
		}

		#endregion
	}
}