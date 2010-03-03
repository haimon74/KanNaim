<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InputAsyncFileUpload.ascx.cs" Inherits="UserControls_StyleManagementForm_InputAsyncFileUpload" %>


<script type="text/javascript">
    function fillCell(row, cellNumber, text) {
        var cell = row.insertCell(cellNumber);
        cell.innerHTML = text;
        cell.style.borderBottom = cell.style.borderRight = "solid 1px #aaaaff";
    }
    function addToClientTable(name, text) {
        var table = document.getElementById("<%= clientSide.ClientID %>");
        var row = table.insertRow(0);
        fillCell(row, 0, name);
        fillCell(row, 1, text);
    }
    
    function uploadError(sender, args) {
        addToClientTable(args.get_fileName(), "<span style='color:red;'>" + args.get_errorMessage() + "</span>");
    }
    function uploadComplete(sender, args) {
        var contentType = args.get_contentType();
        var text = args.get_length() + " bytes";
        if (contentType.length > 0) {
            text += ", '" + contentType + "'";
        }
        addToClientTable(args.get_fileName(), text);
    }
</script>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<div id="InputImage"  class="InputDiv">
    <div class="InputLabel">
        <asp:Label ID="Label1" runat="server" Text="בחר קובץ"></asp:Label>
    </div>
    <div class="InputLongHebrewContent">
        <cc1:AsyncFileUpload
            OnClientUploadError="uploadError" OnClientUploadComplete="uploadComplete" 
            runat="server" ID="AsyncFileUpload1" Width="400px" UploaderStyle="Modern" 
            UploadingBackColor="#CCFFFF" ThrobberID="myThrobber">
        </cc1:AsyncFileUpload>        
    </div>
    <div class="InputErrorMessage">
        <asp:Label runat="server" ID="myThrobber" style="display:none;" >            
            <img align="middle" alt="uploading..." src="uploading.gif" />
        </asp:Label>        
        <asp:Label runat="server" Text="&nbsp;" ID="uploadResult" />
        <table style="border-collapse: collapse; border: solid 0px #aaaaff;" runat="server" cellpadding="3" id="clientSide" />
    </div>
</div>
    <br />
