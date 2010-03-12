<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true"
	CodeBehind="assignthumbnail.aspx.cs" Inherits="GalleryServerPro.Web.task.assignthumbnail" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">
	<asp:Repeater ID="rptr" runat="server">
		<ItemTemplate>
			<div class="thmb">
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
				<p>
					<asp:RadioButton ID="rb" runat="server" GroupName="thmb" Text='<%# GetTitle(Eval("Title").ToString()) %>'
						Checked='<%# IsAlbumThumbnail(Convert.ToInt32(Eval("ID"))) %>' onclick="SetUniqueRadioButton('rptr.*thmb', this);" /><input
							runat="server" type="hidden" value='<%# Eval("ID") %>' />
				</p>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<hr style="clear: left; visibility: hidden;" />
</asp:Content>
