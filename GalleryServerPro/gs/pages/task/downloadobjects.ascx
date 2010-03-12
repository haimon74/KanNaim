<%@ Import Namespace="GalleryServerPro.Business" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="downloadobjects.ascx.cs"	Inherits="GalleryServerPro.Web.gs.pages.task.downloadobjects" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>

<script type="text/javascript">

	function ToggleSelectAll(masterCheckbox)
	{
		var checked = masterCheckbox.checked;
		var inputs = document.getElementsByTagName("input");
		for (var i = 0; i < inputs.length; i++)
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
	<p>
		<asp:Literal ID="l1" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Download_Objects_Select_Image_Size_Label_Text %>" />
		<asp:DropDownList ID="ddlImageSize" runat="server" />
	</p>
	<p>
		<input type="checkbox" id="chkSelectAll" onclick="javascript:ToggleSelectAll(this);" /><label
			for="chkSelectAll"><asp:Literal ID="l2" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Download_Objects_Check_Uncheck_All_Label_Text %>" /></label></p>
	<asp:Repeater ID="rptr" runat="server">
		<HeaderTemplate>
			<div class="gsp_floatcontainer">
		</HeaderTemplate>
		<ItemTemplate>
			<div class="<%# GetThumbnailCssClass(Container.DataItem.GetType()) %>">
				<p id="p1" runat="server" class="albumtitle" visible='<%# (Container.DataItem.GetType() == typeof(Album)) %>'>
					<asp:CheckBox ID="chkAlbum" runat="server" Text='<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType()) %>' />
				</p>
				<div class="op0" style="width: <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Width")) + 15).ToString() %>px;
					height: <%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Thumbnail.Height")) + 10).ToString() %>px;">
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
				<p id="p2" runat="server" visible='<%# (Container.DataItem.GetType() != typeof(Album)) %>'>
					<asp:CheckBox ID="chkMO" runat="server" Text='<%# GetGalleryObjectText(Eval("Title").ToString(), Container.DataItem.GetType()) %>' />
				</p>
				<asp:HiddenField ID="hdn" runat="server" Value="<%# GetId((IGalleryObject) Container.DataItem) %>" />
			</div>
		</ItemTemplate>
		<FooterTemplate>
			</div>
		</FooterTemplate>
	</asp:Repeater>
	<asp:PlaceHolder ID="phTaskFooter" runat="server" />
</div>
