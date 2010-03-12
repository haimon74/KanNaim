<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPageAdmin.master" CodeFile="LoginRolesTest.aspx.cs" Inherits="LoginRolesTest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <asp:LoginView ID="LoginView1" runat="server" SkinID="0">
        <LoggedInTemplate>
            <br />
            שלום&nbsp;
            <asp:LoginName ID="LoginName1" runat="server" />
            <br />
            <asp:Button ID="Button3" runat="server" onclick="Button3_Click" 
                Text="אפשר תפריט ניהול" Width="175px" />
            <br />
            <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                Text="הוספת מנהל אתר חדש" />
            <br />
            <asp:Button ID="Button2" runat="server" onclick="Button2_Click" 
                Text="שינוי סיסמא" Width="178px" />
            <br />
            <asp:LoginStatus ID="LoginStatus1" runat="server" />
            <br />
        </LoggedInTemplate>
        <AnonymousTemplate>
            <asp:Login ID="Login1" runat="server" LoginButtonText="אישור" 
                PasswordLabelText="סיסמא" RememberMeText="שמור סיסמא" 
                TitleText="כניסת מנהל אתר בלבד" UserNameLabelText="שם משתמש" Width="354px">
            </asp:Login>
            <br />
            <asp:PasswordRecovery ID="PasswordRecovery1" runat="server" 
                AnswerLabelText="תשובה" QuestionLabelText="שאלה" 
                QuestionTitleText="זיהוי לפי שאלת זיהוי" SubmitButtonText="שלח" 
                SuccessText="הסיסמא נשלחה לאימייל שלך" 
                UserNameInstructionText="הזן שם משתמש לזיהוי" UserNameLabelText="שם משתמש" 
                UserNameTitleText="האם שכחת את הסיסמא ?" Width="351px">
            </asp:PasswordRecovery>
            <br />
        </AnonymousTemplate>
    </asp:LoginView>
        <br />
        <br />
        <br />
    <asp:ChangePassword ID="ChangePassword1" runat="server" BackColor="#F7F7DE" 
    BorderColor="#CCCC99" BorderStyle="Solid" BorderWidth="1px" 
    CancelButtonText="ביטול" ChangePasswordButtonText="שינוי סיסמא" 
    ConfirmNewPasswordLabelText="אישור סיסמא חדשה" ContinueButtonText="המשך" 
    Font-Names="Verdana" Font-Size="10pt" NewPasswordLabelText="סיסמא חדשה" 
    PasswordLabelText="סיסמא ישנה" UserNameLabelText="שם משתמש" Visible="False">
        <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
</asp:ChangePassword>
        <br />
        <br />
        <br />
<asp:CreateUserWizard ID="CreateUserWizard1" runat="server" 
                AnswerLabelText="תשובה לזיהוי" BackColor="#F7F7DE" BorderColor="#CCCC99" 
                BorderStyle="Solid" BorderWidth="1px" CancelButtonText="ביטול" 
                ConfirmPasswordLabelText="אישור סיסמא" ContinueButtonText="המשך" 
                CreateUserButtonText="צור משתמש" EmailLabelText="אימייל" 
                FinishCompleteButtonText="סיום" FinishPreviousButtonText="קודם" 
                Font-Names="Verdana" Font-Size="10pt" LoginCreatedUser="False" 
                PasswordLabelText="סיסמא" QuestionLabelText="שאלת זיהוי" 
                StartNextButtonText="הבא" StepNextButtonText="הבא" 
                StepPreviousButtonText="קודם" UserNameLabelText="שם משתמש" 
    Width="442px" Visible="False">
    <SideBarStyle BackColor="#7C6F57" BorderWidth="0px" Font-Size="0.9em" 
                    VerticalAlign="Top" />
    <SideBarButtonStyle BorderWidth="0px" Font-Names="Verdana" 
                    ForeColor="#FFFFFF" />
    <ContinueButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                    ForeColor="#284775" />
    <NavigationButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                    ForeColor="#284775" />
    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" 
                    HorizontalAlign="Center" />
    <CreateUserButtonStyle BackColor="#FFFBFF" BorderColor="#CCCCCC" 
                    BorderStyle="Solid" BorderWidth="1px" Font-Names="Verdana" 
                    ForeColor="#284775" />
    <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
    <StepStyle BorderWidth="0px" />
    <WizardSteps>
        <asp:CreateUserWizardStep runat="server">
        </asp:CreateUserWizardStep>
        <asp:CompleteWizardStep runat="server">
        </asp:CompleteWizardStep>
    </WizardSteps>
</asp:CreateUserWizard>
            
</asp:Content>