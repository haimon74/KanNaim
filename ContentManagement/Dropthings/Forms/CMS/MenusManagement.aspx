<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/MasterPageAdmin.master" AutoEventWireup="true" CodeFile="MenusManagement.aspx.cs" Inherits="CMS_MenusManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <asp:Panel ID="Panel1" runat="server" Height="761px">
        <br />
        <br />
        <br />
        <br />
        <table style="width: 100%; height: 90px;">
            <tr>
                <td style="width: 195px">
                    <asp:Label ID="Label1" runat="server" Text="בחר תפריט לעריכה"></asp:Label>
                    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" 
                        DataSourceID="LinqDataSource1" DataTextField="MenuDescription" 
                        DataValueField="MenuId" 
                        onselectedindexchanged="DropDownList1_SelectedIndexChanged" Width="186px">
                    </asp:DropDownList>
                </td>
                <td style="width: 440px">
                    <asp:Label ID="Label11" runat="server" Text="טקסט להצגה כראש העץ"></asp:Label>
                    &nbsp;<asp:TextBox ID="TextBoxRootName" runat="server" ValidationGroup="vgNewMenu"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="TextBoxRootName" Display="Dynamic" 
                        ErrorMessage="חסר שם לשורש העץ" ValidationGroup="vgNewMenu">*</asp:RequiredFieldValidator>
                    <br />
                    &nbsp;<asp:Label ID="Label4" runat="server" Text="שם - תיאור תפריט" Width="110px"></asp:Label>
                    <br />
                    <asp:TextBox ID="TextBoxNewMenuName" runat="server" MaxLength="100" 
                        ValidationGroup="vgNewMenu" Width="259px"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="TextBoxNewMenuName" ErrorMessage="חסר שם לתפריט" 
                        ValidationGroup="vgNewMenu" Display="Dynamic">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:Button ID="ButtonAddNewMenu" runat="server" Text="צור תפריט חדש" 
                        Width="109px" onclick="ButtonAddNewMenu_Click" 
                        ValidationGroup="vgNewMenu" />
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                        ShowMessageBox="True" ShowSummary="False" ValidationGroup="vgNewMenu" 
                        DisplayMode="List" />
                </td>
                <td>
                    <asp:Button ID="ButtonDeleteMenu" runat="server" Text="מחק תפריט שלם" 
                        Width="97px" onclick="ButtonDeleteMenu_Click" />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 195px; height: 299px">
                    <br />
                    <asp:Label ID="Label2" runat="server" Text="סמן לעריכה"></asp:Label>
                    <br />
                    <asp:TreeView ID="TreeViewTest" runat="server" Height="187px" 
                        ShowCheckBoxes="All" ShowLines="True" Width="189px">
                    </asp:TreeView>
                </td>
                <td style="width: 440px; height: 299px">
                    <asp:Label ID="Label9" runat="server" Text="מיקום" Width="50px"></asp:Label>
                    <br />
                    <asp:TextBox ID="TextBoxPosition" runat="server" Enabled="False" Width="39px"></asp:TextBox>
                    &nbsp;<br />
                    <asp:Label ID="Label5" runat="server" Text="טקסט" Width="50px"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                        ControlToValidate="TextBoxDisplayText" Display="Dynamic" 
                        ErrorMessage="הזן טקסט להצגה" ValidationGroup="vgNewNode">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="TextBoxDisplayText" runat="server" MaxLength="30" 
                        Width="171px"></asp:TextBox>
                    &nbsp;
                    <br />
                    <asp:Label ID="Label6" runat="server" Text="בלון" Width="50px"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                        ControlToValidate="TextBoxToolTip" Display="Dynamic" 
                        ErrorMessage="הזן טקסט להצגה בבלון" ValidationGroup="vgNewNode">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="TextBoxToolTip" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                    &nbsp;<br />
                    <asp:Label ID="Label7" runat="server" Text="URL" Width="50px"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                        ControlToValidate="TextBoxUrl" Display="Dynamic" ErrorMessage="הזן קישורן" 
                        ValidationGroup="vgNewNode">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="TextBoxUrl" runat="server" MaxLength="200" Width="396px"></asp:TextBox>
                    <br />
                    <asp:Label ID="Label8" runat="server" Text="CSS" Width="50px"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                        ControlToValidate="TextBoxUrl" Display="Dynamic" ErrorMessage="הזן סטייל CSS" 
                        ValidationGroup="vgNewNode">*</asp:RequiredFieldValidator>
                    <br />
                    <asp:TextBox ID="TextBoxCss" runat="server" MaxLength="50" Width="169px"> </asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="ButtonAddNewNode" runat="server" Text="הוסף עלה למסומן" 
                        Width="132px" onclick="ButtonAddNewNode_Click" ValidationGroup="vgNewNode" />
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
                        DisplayMode="List" ShowMessageBox="True" ShowSummary="False" 
                        ValidationGroup="vgNewNode" />
                </td>
                <td style="height: 299px">
                    <asp:Button ID="ButtonDeleteChecked" runat="server" Text="מחק מסומנים" 
                        Width="97px" onclick="ButtonDeleteChecked_Click" />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 195px">
                    <asp:Label ID="Label10" runat="server" Text="סדר" Width="30px"></asp:Label>
                    &nbsp;&nbsp;<asp:Button ID="ButtonMoveBranch" runat="server" Text="++" 
                        Width="30px" onclick="ButtonMoveBranch_Click" />
                    &nbsp;<asp:Button ID="ButtonMoveUp" runat="server" Text="UP" Width="30px" 
                        onclick="ButtonMoveUp_Click" />
                    &nbsp;<asp:Button ID="ButtonMoveDown" runat="server" Text="DN" Width="30px" 
                        onclick="ButtonMoveDown_Click" />
                    &nbsp;<asp:Button ID="ButtonMoveLeaf" runat="server" Text="--" Width="30px" 
                        onclick="ButtonMoveLeaf_Click" />
                    &nbsp;</td>
                <td style="width: 440px">
                    <asp:LinqDataSource ID="LinqDataSource1" runat="server" 
                        ContextTypeName="DataAccess.App_Code.IndexItDataClassesDataContext" 
                        Select="new (MenuDescription, MenuId)" TableName="Table_LookupMenus">
                    </asp:LinqDataSource>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
    </asp:Panel>
                    
</asp:Content>

