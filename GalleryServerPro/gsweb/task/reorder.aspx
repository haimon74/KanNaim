<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true"
	CodeBehind="reorder.aspx.cs" Inherits="GalleryServerPro.Web.task.reorder" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">

	<script type="text/javascript">
function SwapSnaps(sender, eventArgs) 
{
	// User has dragged a thumbnail and is hovering over another thumbnail. The snap control automatically inserts
	// the dragged thumbnail into the destination, but we need to pull out the existing thumbnail and and shift
	// all thumbnails between the source and destination by one.
	var destPanel; // Panel user is hovering over
	var swapToPanel; // Panel to receive thumbnail from left or right sibling
	var swapFromPanel; // Panel with two thumbnails where we need to extract one and assign it to its left or right sibling
	var n; var j;
	if (eventArgs.get_elementId() == null || eventArgs.get_index() < 0) return; // Ignore
     
	var destPanelId = parseInt(eventArgs.get_elementId().substring(2));

	for(curPanelId = 0; curPanelId < _snapDockIds.length; curPanelId++)
	{
		var curPanel = document.getElementById("sd"+curPanelId);
		if((curPanel.childNodes.length == 0) || ((curPanel.childNodes.length == 1) && (curPanel.childNodes[0].nodeType == 3)))
		{
			//if (curPanelId-destPanelId == 1) return;
			console.log('dest index: ' + destPanelId);
			console.log('source index: ' + curPanelId);
			console.log('dest ID: ' + _goIds[destPanelId]);
			console.log('source ID: ' + _goIds[curPanelId]);
			
			if (curPanelId > destPanelId)
			{
				Array.insert(_goIds, destPanelId, _goIds[curPanelId]);
				Array.removeAt(_goIds, curPanelId + 1);
				console.log(_goIds);
				// User is moving a thumbnail closer to the beginning
				for (j = curPanelId; j > destPanelId; j--)
				{
					swapToPanel = document.getElementById("sd"+j);
					swapFromPanel = document.getElementById("sd"+(j-1));
					// Grab the last node that is an element and move it to the next panel
					for(n = swapFromPanel.lastChild; n != null; n = n.previousSibling)
					{
						if (n.nodeType == 1) { swapToPanel.appendChild(n); break; }
					}
				}
			}
			else
			{
				// User is moving a thumbnail closer to the end
				console.log('before: ' + _goIds);
				Array.insert(_goIds, destPanelId + 1, _goIds[curPanelId]);
				Array.removeAt(_goIds, curPanelId);
				console.log('after: ' + _goIds);
				for (j = curPanelId; j < destPanelId; j++)
				{
					swapToPanel = document.getElementById("sd"+j);
					swapFromPanel = document.getElementById("sd"+(j+1));
					// Grab the last node that is an element and move it to the previous panel
					for(n = swapFromPanel.lastChild; n != null; n = n.previousSibling)
					{
						if (n.nodeType == 1) { swapToPanel.appendChild(n); break; }
					}
				}
			}
			break;
		} 
	}
}
	</script>

	<asp:Repeater ID="rprDock" runat="server">
		<ItemTemplate>
			<div id="<%# GetSnapDockId() %>" class="snapDock">
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<asp:Repeater ID="rprSnap" runat="server" OnItemDataBound="rprSnap_ItemDataBound">
		<ItemTemplate>
			<CA:Snap ID="sp" DockingContainers="<%# SnapDockIds %>" CurrentDockingContainer="<%# GetSnapDockingContainerId() %>"
				CurrentDockingIndex="0" DraggingStyle="Original" DockingStyle="SolidOutline" MustBeDocked="True"
				IsCollapsed="false" runat="server">
				<ClientEvents>
					<SnapDock EventHandler="SwapSnaps" />
				</ClientEvents>
				<Content>
					<div class="<%# GetThumbnailCssClass(((RepeaterItem)Container.Parent).DataItem.GetType()) %>"
						onmousedown="<%# Container.ClientID %>.startDragging(event);">
						<%# GetAlbumText(((IGalleryObject)((RepeaterItem)Container.Parent).DataItem).Title, ((RepeaterItem)Container.Parent).DataItem.GetType())%>
						<div class="op0" style="width: <%# (Convert.ToInt32(DataBinder.Eval(((RepeaterItem)Container.Parent).DataItem, "Thumbnail.Width")) + 15).ToString() %>px;
							height: <%# (Convert.ToInt32(DataBinder.Eval(((RepeaterItem)Container.Parent).DataItem, "Thumbnail.Height")) + 10).ToString() %>px;">
							<div class="op1">
								<div class="op2">
									<div class="sb">
										<div class="ib">
											<%# GetThumbnailImageTag((IGalleryObject)((RepeaterItem)Container.Parent).DataItem) %>
										</div>
									</div>
								</div>
							</div>
						</div>
						<%# GetMediaObjectText(Eval("Title").ToString(), ((RepeaterItem)Container.Parent).DataItem.GetType())%>
					</div>
				</Content>
			</CA:Snap>
			<input runat="server" type="hidden" value='<%# GetGalleryObjectId((IGalleryObject) Container.DataItem) %>' />
		</ItemTemplate>
	</asp:Repeater>
	<hr style="clear: left; visibility: hidden;" />
</asp:Content>
