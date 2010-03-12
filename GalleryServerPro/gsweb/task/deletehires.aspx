<%@ Page Language="C#" MasterPageFile="~/master/taskMstr.master" AutoEventWireup="true"
	CodeBehind="deletehires.aspx.cs" Inherits="GalleryServerPro.Web.task.deletehires" %>

<%@ MasterType TypeName="GalleryServerPro.Web.Master.taskMstr" %>
<%@ Import Namespace="GalleryServerPro.Business.Interfaces" %>
<asp:Content ID="ct1" ContentPlaceHolderID="c3" runat="server">

	<script type="text/javascript">
	<!--
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
	--></script>

	<div class="addleftpadding1">
		<p class="textcol msgwarning" style="border-bottom: #369 1px solid;">
			<asp:Literal ID="litId" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Delete_HiRes_Warning %>" /></p>
		<h3>
			<% =GetPotentialSavings() %>
		</h3>
		<p class="textcol">
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:GalleryServerPro, Task_Delete_HiRes_Potential_Savings_Hdr %>" />
		</p>
	</div>
	<asp:PlaceHolder ID="phMsg" runat="server" />
	<p>
		<input type="checkbox" id="chkSelectAll" onclick="javascript:ToggleSelectAll(this);" /><label
			for="chkSelectAll"><asp:Literal runat="server" Text="<%$ Resources:GalleryServerPro, Task_Delete_HiRes_Check_Uncheck_All_Label_Text %>" /></label></p>
	<asp:Repeater ID="rptr" runat="server">
		<ItemTemplate>
			<div class="thmb">
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
				<p>
					<%# GetNoHiResImageText(DataBinder.Eval(Container.DataItem, "Optimized.Filename").ToString(), DataBinder.Eval(Container.DataItem, "Original.Filename").ToString()) %>
					<asp:CheckBox ID="chkbx" runat="server" Text='<%# GetTitle(Eval("Title").ToString()) %>'
						Visible='<%# DoesHiResImageExist(DataBinder.Eval(Container.DataItem, "Optimized.Filename").ToString(), DataBinder.Eval(Container.DataItem, "Original.Filename").ToString()) %>' />
					<br />
					<em>(<%# DataBinder.Eval(Container.DataItem, "Original.FileSizeKB") %>
						KB)</em>
					<input runat="server" type="hidden" value='<%# DataBinder.Eval(Container.DataItem, "ID").ToString() %>' />
				</p>
			</div>
		</ItemTemplate>
	</asp:Repeater>
	<hr style="clear: left; visibility: hidden;" />
</asp:Content>
