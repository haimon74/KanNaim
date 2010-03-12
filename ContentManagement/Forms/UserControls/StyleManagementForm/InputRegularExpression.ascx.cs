using System;
using System.Web.UI.WebControls;

public partial class UserControls_StyleManagementForm_InputRegularExpression : System.Web.UI.UserControl
{
    private string _labelText = "string";
    private string _watermarkCssClass = "watermark";
    private string _watermarkText = "string field";
    private string _errorMsg = "ValidationExpressionError";
    private string _regularExpression = "";
    private string _validationGroup = "";
    private int _textMaxLength = 50;
    private TextBoxMode _textBoxMode = TextBoxMode.SingleLine;

    private LanguageEnum _language = LanguageEnum.Hebrew;

    public enum LanguageEnum
    {
        Hebrew,
        English
    } ;

    public string LabelText
    {
        get { return _labelText; }
        set { _labelText = value; }
    }

    public string WatermarkCssClass
    {
        get { return _watermarkCssClass; }
        set { _watermarkCssClass = value; }
    }

    public string WatermarkText
    {
        get { return _watermarkText; }
        set { _watermarkText = value; }
    }

    public string ErrorMsg
    {
        get { return _errorMsg; }
        set { _errorMsg = value; }
    }

    public string ValidationGroup
    {
        get { return _validationGroup; }
        set { _validationGroup = value; }
    }

    public string RegularExpression
    {
        get { return _regularExpression; }
        set { _regularExpression = value; }
    }

    public LanguageEnum Language
    {
        get { return _language; }
        set { _language = value; }
    }

    public int TextMaxLength
    {
        get { return _textMaxLength; }
        set { _textMaxLength = value; }
    }

    public TextBoxMode Mode
    {
        get { return _textBoxMode; }
        set { _textBoxMode = value; }
    }

    public string Text
    {
        get { return TextBox1.Text; }
        set { TextBox1.Text = value; }
    }

    public string Value
    {
        get { return TextBox1.Text; }
        set { TextBox1.Text = value; }
    }

    public TextBox Textbox
    {
        get { return TextBox1; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = LabelText;
        TextBox1.CssClass = String.Format("{0}Text",Language);
        TextBox1.MaxLength = TextMaxLength;
        TextBox1.TextMode = Mode;
        TextBoxWatermarkExtender1.WatermarkCssClass = WatermarkCssClass;
        TextBoxWatermarkExtender1.WatermarkText = WatermarkText;
        RegularExpressionValidator1.ErrorMessage = ErrorMsg;
        RegularExpressionValidator1.ValidationGroup = ValidationGroup;
        RegularExpressionValidator1.ValidationExpression = RegularExpression;
    }
}
