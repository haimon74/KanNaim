using System;

public partial class UserControls_StyleManagementForm_InputImage : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private byte[] _fileData;
    public byte[] FileData
    {
        get { return InputAsyncFileUpload1.FileBytes; }
    }
    public string Width
    {
        get{ return InputNumberWidth.Text;}
    }
    public string Height
    {
        get { return InputNumberHeight.Text; }
    }
    public string HorAlign
    {
        get { return DropDownListBgImageHAlign.SelectedValue; }
    }
    public string VerAlign
    {
        get { return DropDownListBgImageVAlign.SelectedValue; }
    }
    public string Opacity
    {
        get { return InputOpacity1.Text; }
    }
}
