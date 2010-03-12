using System;

public partial class UserControls_StyleManagementForm_InputAsyncFileUpload : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void UploadComplete(object sender, EventArgs e)
    {
        FileBytes = AsyncFileUpload1.FileBytes;
    }

    private byte[] _fileBytes;

    public byte[] FileBytes
    {
        get { return _fileBytes;}
        set { _fileBytes = value;}
    }
}
