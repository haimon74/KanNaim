<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SlideShowSandbox.aspx.cs" Inherits="Forms_CMS_SlideShowSandbox" %>
<%@ Import Namespace="System.Collections.Generic"%>
<%@ Import Namespace="System.IO"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../UserControls/SlideShowControl.ascx" tagname="SlideShowControl" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="ServiceSlideShow" %>
<%@ Import Namespace="System.Web.Services" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script runat="Server" type="text/C#">
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
    public static AjaxControlToolkit.Slide[] GetAllSlides()
    {
        //ServiceSlideShow proxy = new ServiceSlideShow();
        //string contextKey = "/photos/";
        //string[] fileNames = proxy.GetAllSlides(contextKey);


        string[] fileNames = System.IO.Directory.GetFiles(SlidesPath);

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
                list.Add(new AjaxControlToolkit.Slide(ContextKey + justFileName, displayedFileTitleOnSlider, displayedFileDescriptionOnSlider));
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

    private static AjaxControlToolkit.Slide[] _allPhotos;
        
    [WebMethod]
    [System.Web.Script.Services.ScriptMethod]
    static public AjaxControlToolkit.Slide[] GetSlides(string contextKey)
    {
        _allPhotos = GetAllSlides();
        string[] spliters = {"#"};
        string[] keys = contextKey.Split(spliters, StringSplitOptions.RemoveEmptyEntries);
        int rowNum = int.Parse(keys[0]);
        int maxRow = int.Parse(keys[1]);
        int colNum = int.Parse(keys[2]);
        int maxCol = int.Parse(keys[3]);
        
        int tnPerPanel = maxCol * maxRow;
        int offset = (rowNum - 1) * maxCol + colNum - 1;
        AjaxControlToolkit.Slide[] returnPhotos = new AjaxControlToolkit.Slide[_allPhotos.Length / tnPerPanel + 1];

        int j = 0;
        for (int i = offset; i < _allPhotos.Length; i += tnPerPanel, j++)
        {
            returnPhotos[j] = _allPhotos[i];
        }

        return returnPhotos;
    }
    </script>    
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <p>Here is the Slide Show Control:</p><br />
        <uc1:SlideShowControl ID="SlideShowControl1" runat="server" Rows="4" Columns="3"/>

    </div>
    </form>
</body>
</html>
