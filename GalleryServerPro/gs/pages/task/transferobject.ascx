<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="transferobject.ascx.cs" Inherits="GalleryServerPro.Web.gs.pages.task.transferobject" %>
<%@ Register Src="../../controls/albumtreeview.ascx" TagName="albumtreeview" TagPrefix="uc1" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<script type="text/javascript">

	function ToggleSelectAll(masterCheckbox)
	{
		var checked = masterCheckbox.checked;
		var inputs = document.getElementsByTagName("input");
		for (var i=0; i < inputs.length; i++)
		{
			if (inputs[i].type == 'checkbox')
			{
				inputs[i].checked = checked;
			}
		}
	}
</script>
<div class="gsp_content">
	<asp:PlaceHolder ID="phTaskHeader" runat="server" />
	<asp:PlaceHolder ID="phMsg" runat="server" />
	<asp:PlaceHolder ID="phCheckAllContainer" runat="server" />
	<asp:Repeater ID="rptr" runat="server">
		<HeaderTemplate>
			<div class="gsp_floatcontainer">
		</HeaderTemplate>
		<ItemTemplate>
			<div class="<%# GetThumbnailCssClass(Container.DataItem.GetType()) %>"">
				<%# GetAlbumText(Eval("Title").ToString(), Container.DataItem.GetType())%>
				<div class="op0" style="width: <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Width")) + 15).ToString() %>px;height: <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Height")) + 10).ToString() %>px;">
					<div class="op1">
						<div class="op2">
							<div class="sb">
								<div class="ib">
									<img src="<%# GetThumbnailUrl((IGalleryObject) Container.DataItem) %>" alt="<%# RemoveHtmlTags(Eval("Title").ToString()) %>"
										title="<%# RemoveHtmlTags(Eval("Title").ToString()) %>" style="width: <%# DataBinder.Eval(Container.DataItem, "Thumbnail.Width").ToString() %>px;
										height: <%# DataBinder.Eval(Container.DataItem, "Thumbnail.Height").ToString() %>px;" /></div>
							</div>
						</div>
					</div>
				</div>
				<p class="gsp_caption">
					<asp:CheckBox ID="chkbx" runat="server" Text='<%# GetTitle(Eval("Title").ToString()) %>' />
					<input id="Hidden1" runat="server" type="hidden" value='<%# GetId((IGalleryObject) Container.DataItem) %>' />
				</p>
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
	</asp:Repeater>
	<asp:PlaceHolder ID="phCustomValidatorContainer" runat="server" Visible="false" />
	<uc1:albumtreeview ID="tvUC" runat="server" Visible="false" RequireAlbumSelection="true" />
	<asp:PlaceHolder ID="phTaskFooter" runat="server" />
</div>