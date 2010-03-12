using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using UserControlsLibrary.WebControls;


public partial class Forms_UserControls_SlideShowControl : System.Web.UI.UserControl
{
    static private string _staticPath;
    static private string _staticContextKey;// = "../../images/";

    public int Interval{get; set;}

    public bool IsAutoPlay { get; set; }

    private string _slidesPath;
    private string _contextKey;// = "../../images/";

    public string SlidesPath
    {
        get { return _slidesPath; }
        set { _slidesPath = value; }
    }

    public string ContextKey
    {
        get { return _contextKey; }
        set { _contextKey = value; }
    }
    
    private int _rows = 3;
    public int Rows
    {
        get { return _rows; }
        set { _rows = value; }
    }
    
    private int _columns = 3;
    public int Columns
    {
        get { return _columns; }
        set { _columns = value; }
    }

    private int _tnWidth = 80;
    public int TnWidth
    {
        get { return _tnWidth; }
        set { _tnWidth = value; }
    }

    private int _tnHeight = 60;
    public int TnHeight
    {
        get { return _tnHeight; }
        set { _tnHeight = value; }
    }

    public static string StaticMapPath
    {
        get { return _staticPath; }
        set { _staticPath = value; }
    }

    public static string StaticContextKey
    {
        get { return _staticContextKey; }
        set { _staticContextKey = value; }
    }

    private const int Margin = 1;
    private const int Padding = 1;
    private const int Border = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        
        StaticContextKey = ContextKey;
        SlidesPath = Server.MapPath(ContextKey);
        StaticMapPath = SlidesPath;
        this.PanelSlidShow.Height = Rows * (TnHeight + 2*Margin + 2*Padding) + 2*Border;
        this.PanelSlidShow.Width = Columns * (TnWidth + 2*Margin + 2*Padding) + 2*Border;
           
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        for (int r = 1; r <= Rows; r++)
        {
            for (int c = 1; c <= Columns; c++)
            {
                string rcPos = "R" + r + "C" + c;
                Panel newPanel = new Panel
                {
                    ID = "PanelImage" + rcPos,
                    Width = TnWidth,
                    Height = TnHeight,
                    BorderWidth = Border,
                    BorderColor = Color.Blue,
                    BorderStyle = BorderStyle.Solid,
                };
                newPanel.Attributes["style"] = String.Format("padding:{0}px; float:right;", Margin);
                newPanel.Attributes["runat"] = "server";
                ImageButton image = new ImageButton()
                {
                    ID = "ImageThumb" + rcPos,
                    BackColor = Color.Yellow,
                    Width = TnWidth,
                    Height = TnHeight
                };
                image.Attributes["runat"] = "server";
                image.Attributes["style"] = String.Format("margin:{0}px;", Margin);
                newPanel.Controls.Add(image);
                
                LabelToSlideToolTip descriptionLabel = new LabelToSlideToolTip(ref image);
                descriptionLabel.ID = "DescriptionLabel4" + image.ID;
                descriptionLabel.Attributes["runat"] = "server";
                newPanel.Controls.Add(descriptionLabel);

                var slideShowExtender = new SlideShowExtender
                {
                    ID = "SlideShowImageExt" + rcPos,
                    AutoPlay = IsAutoPlay,
                    PlayInterval = (Interval > 500) ? Interval : 500,
                    TargetControlID = image.ID,
                    SlideShowServiceMethod = "GetSlides",
                    //SlideShowServicePath = "/ServiceSlideShow.asmx",
                    UseContextKey = true,
                    ContextKey = String.Format("{0}#{1}#{2}#{3}", r, Rows, c, Columns),
                    NextButtonID = "ButtonNext",
                    PlayButtonText = "Play",
                    StopButtonText = "Stop",
                    PreviousButtonID = "ButtonPrev",
                    PlayButtonID = "ButtonPlay",
                    Loop = true,
                    ImageTitleLabelID = "lblTitle",
                    ImageDescriptionLabelID = descriptionLabel.UniqueID//"lblDescription"
                };
            
                newPanel.Controls.Add(slideShowExtender);
                this.PanelSlidShow.Controls.Add(newPanel);
            }
        }
    }

    static private AjaxControlToolkit.Slide[] GetNoImagesFoundDirectory()
    {
        AjaxControlToolkit.Slide[] slides = new AjaxControlToolkit.Slide[1];

        // get image from web.config and verify it exists on file system
        string _noImagesFoundWebLocation = "../../img/NoImagesFound.jpg"; // System.Configuration.ConfigurationManager.AppSettings.Get("SlideServiceNoImagesFoundLocation");
        //if (!File.Exists(Server.MapPath(_noImagesFoundWebLocation)))
        //  throw new Exception("SlideService.asmx::GetNoImagesFoundDirectory - NoImagesFoundLocation found in web.config does not exist after server.mappath - " + Server.MapPath(_noImagesFoundWebLocation));

        // create slide from image
        slides[0] = new AjaxControlToolkit.Slide(_noImagesFoundWebLocation, "No Images Found: Please click on another directory", "");

        // return slide
        return (slides);
    }

    [System.Web.Services.WebMethod]
    public static AjaxControlToolkit.Slide[] GetSlides()
    {
        //ServiceSlideShow proxy = new ServiceSlideShow();
        //string contextKey = "/photos/";
        //string[] fileNames = proxy.GetAllSlides(contextKey);


        string[] fileNames = System.IO.Directory.GetFiles(StaticMapPath);

        if (fileNames.Length == 0)
            return GetNoImagesFoundDirectory();

        // create generic empty list of slides
        List<AjaxControlToolkit.Slide> list = new List<AjaxControlToolkit.Slide>();
        String justFileName;
        String displayedFileTitleOnSlider;
        String displayedFileDescriptionOnSlider;

        for (int i = 0; i < fileNames.Length; i++)
        {
            if (true)//(IsImage(Path.GetExtension(fileNames[i])))
            {
                // get complete filename
                justFileName = Path.GetFileName(fileNames[i]);

                // get title
                displayedFileTitleOnSlider = Path.GetFileNameWithoutExtension(fileNames[i]);

                // set description to empty
                displayedFileDescriptionOnSlider = String.Empty;

                // add file to list of slides
                list.Add(new AjaxControlToolkit.Slide(StaticContextKey + justFileName, displayedFileTitleOnSlider, displayedFileDescriptionOnSlider));
            }
        }
        return (list.ToArray());

        //return new AjaxControlToolkit.Slide[] { 
        //new AjaxControlToolkit.Slide("../../images/Blue hills.jpg", "Blue Hills", "Go Blue"),
        //new AjaxControlToolkit.Slide("../../images/Sunset.jpg", "Sunset", "Setting sun"),
        //new AjaxControlToolkit.Slide("../../images/Winter.jpg", "Winter", "Wintery..."),
        //new AjaxControlToolkit.Slide("../../images/Water lilies.jpg", "Water lillies", "Lillies in the water"),
        //new AjaxControlToolkit.Slide("../../images/VerticalPicture.jpg", "Sedona", "Portrait style picture")};
    }
    protected void ButtonTest_Click(object sender, EventArgs e)
    {
        string test = PanelSlidShow.Controls[0].Controls[0].ToString();
        
    }
    
}
