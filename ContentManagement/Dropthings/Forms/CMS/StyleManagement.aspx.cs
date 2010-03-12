using System;

public partial class CMS_StyleManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Form.Enctype = "multipart/form-data"; // for bug workaround though doesn't work...

        if (!IsPostBack)
        {
            ViewState["BackGroundImageEnabled"] = false;
        }
        EnableBackgroundImage((bool)ViewState["BackGroundImageEnabled"]);
    }
    protected void RadioButtonList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["BackGroundImageEnabled"] = (RadioButtonList2.SelectedIndex == 1);
        EnableBackgroundImage((bool)ViewState["BackGroundImageEnabled"]);
    }

    private void EnableBackgroundImage(bool enable)
    {
        //InputBackgroundImage1.Enable = enable;
    }
    protected void ButtonSave_Click(object sender, EventArgs e)
    {
        ButtonSave.Text = "clicked";
    }

    private enum FontStyleEnum
    {
        // font
        Font_Family,
        Font_Size,
        Font_Weight,
        Font_Style,
        Font_Color,
        Background_Color
    } ;

    private enum TextStyleEnum
    {
        // text
        Text_Align,
        Vertical_Align,
        Text_Indent,
        Direction,
        Letter_Spacing,
        Word_Spacing
    } ;

    private enum PositionStyleEnum
    {
        // positioning
        Padding,
        Margin,
        Width,
        Height,
        Border,
        Top,
        Bottom,
        Right,
        Left
    } ;

    private enum HyperlinkStyleEnum
    {
        // hyperlinks
        Link,
        Visited,
        Hover,
        Active
    } ;

    private enum ImageStyleEnum
    {
        // Image
        Background_Image_Url, 
        Text_Align,
        Vertical_Align,
        Width,
        Height,
        Top,
        Bottom,
        Right,
        Left,
        Opacity
    } ;

    protected void ButtonPreviewStyle_Click(object sender, EventArgs e)
    {
        string fontStyle = String.Format(
            "font-family: {0}; font-size: {1}; font-weight:{2}; font-style:{3}; color:#{4}; background-color:#{5};",
            //"font-family: {0}; font-size: {1}; font-weight:{2}; font-style:{3}; color:#{4}; background: {5};",
                InputFontFamily1.SelectedValue, 
                InputFontSize1.SelectedValue,
                (IsBoldFontCheckBox1.Checked) ? "bold" : "normal",
                (IsItalicFontCheckBox1.Checked) ? "italic" : "normal",
                InputFontColor1.Text ?? "000000",
                InputBackgroundColor1.Text ?? "FFFFFF" 
                //"rgba(00,00,00, 0.5)"
                );
        string textStyle = String.Format(
            "text-align:{0};vertical-align:{1};text-indent:{2};direction:{3};letter-spacing:{4};word-spacing:{5};",
            InputAlignment1.BgImageHAlignSelectedValue,
            InputAlignment1.BgImageVAlignSelectedValue,
            InputTextIdentationInPixels1.Text,
            InputTextDirection1.SelectedValue,
            InputLetterSpacing1.SelectedValue,
            InputWordSpacing1.SelectedValue
            );
        string positionStyle = String.Format(
            "padding:{0};margin:{1};width:{2};height:{3};border:{4};{5}:{6};{7}:{8};position:{9};",
            //"padding:{0};margin:{1};width:{2};height:{3};{4};{5}:{6};{7}:{8};position:{9};",
            (InputDivPadding.Text ?? "0") + "px",
            (InputDivMargin.Text ?? "0") + "px",
            (InputDivWidth.Text ?? "0") + "px",
            (InputDivHeight.Text ?? "0") + "px",
            //"border: 3px solid #0000ff",
            (InputBorderWidth1.SelectedValue ?? "0") + " " + InputBorderStyle1.SelectedValue + " #" + InputColorPickerBorder.Text,
            InputTopOrBottom1.SelectedValue,
            (InputNumber1.Text ?? "0") + "px",
            InputLeftOrRight1.SelectedValue,
            (InputNumber2.Text ?? "0") + "px",
            InputPositionType1.SelectedValue
            );
        
        string anchorStyle = "";
        if (RadioButtonHtmlObjectType.SelectedValue == "a")
        {
            anchorStyle = "a:Link{font-color:#" + InputColorPickerVisitedLink.Text + ";}" +
                          "a:Visited{font-color:#" + InputColorPicker1.Text + ";}" +
                          "a:Hover{font-color::#" + InputColorPicker2.Text + ";}" +
                          "a:Active:{font-color:#" + InputColorPicker3.Text + ";}";
        }

        int opacity;
        bool isParsed = int.TryParse(InputImage1.Opacity, out opacity);
        opacity = (isParsed) ? opacity : 0;

        string imageStyle = "";
        if (RadioButtonHtmlObjectType.SelectedValue == "img")
        {
            imageStyle = String.Format(
                "background-image:url('{0}'); text-align:{1};vertical-align:{2}; ;width:{3};height:{4};{5}:{6};{7}:{8}; opacity:{9}; filter:alpha(opacity={10});",
                "~/img/google.jpg", //TODO: give the image a name
                InputImage1.HorAlign,
                InputImage1.VerAlign,
                (InputImage1.Width ?? "0") + "px",
                (InputImage1.Height ?? "0") + "px",
                "top", "0px", "right", "0px",
                opacity/100,
                opacity
                );
        }
            //string roundedCornersStyle = "-moz-border-radius: 9em 6em ;-webkit-border-radius:  9em 6em ;";
            //"border-radius: 7.5em 5em ;box-width: border-box;width:13em;height:8em;";
        //if (Request.Browser.Browser.StartsWith("IE"))
            //roundedCornersStyle += "behavior: url(/css/border-radius.htc);";
            //"-moz-border-radius: 20px;-webkit-border-radius: 20px;-khtml-border-radius: 20px;border-radius: 20px;border-color:Blue; border-width:5px;border-style:solid; behavior: url(/css/border-radius.htc);";

        string style = fontStyle + textStyle + imageStyle + positionStyle;


        Panel1.Attributes["style"] = style + InputRoundedBorderStyle1.SelectedStyle;
    }
}
