
using System;
using System.Web.UI.HtmlControls;
using System.Linq;
using System.Xml.Linq;
using Dropthings.Widget.Widgets.Flickr;
using Dropthings.Widget.Framework;
using Dropthings.Web.Framework;


public partial class FlickrWidget : System.Web.UI.UserControl, IWidget
{

    private const string FLICKR_API_KEY = "cd56d368d1522e3320567754b1ac20ed";
    private const string SECRET = "1b498d5cbaaa4343"; 
    private const string FLICKER_API_SIG = "&api_sig=4613daaa66dbb79a4c54746d4664cb31";
    private const string AUTH_TOKEN = "72157623572705640-c1d4c9adf63aaaa3";
    private const string MOST_RECENT ="http://www.flickr.com/services/rest/?method=flickr.photos.getRecent&api_key="+FLICKR_API_KEY;
    private const string INTERESTING = "http://www.flickr.com/services/rest/?method=flickr.interestingness.getList&api_key="+FLICKR_API_KEY;
    private const string ENTER_TAG ="http://www.flickr.com/services/rest/?method=flickr.photos.search&api_key="+FLICKR_API_KEY+"&tags=";
    //private const string FIND_BY_USERNAME = "http://www.flickr.com/services/rest/?method=flickr.people.findByUsername&api_key="+FLICKR_API_KEY+"&username=";
    private const string FIND_BY_SETNAME = "http://www.flickr.com/services/rest/?method=flickr.photosets.getPhotos&api_key=" + FLICKR_API_KEY + "&user_id=48139038@N07" + "&photoset_id=";
    //private const string FIND_BY_EMAIL = "http://www.flickr.com/services/rest/?method=flickr.people.findByEmail&api_key="+FLICKR_API_KEY+"&find_email=";
    private const string FIND_BY_EMAIL_TAG = "http://www.flickr.com/services/rest/?method=flickr.photos.search&api_key=" + FLICKR_API_KEY + "&user_id=48139038@N07" + "&text=";
    private const string PHOTOS_FROM_FLICKR_USER = "http://www.flickr.com/services/rest/?method=flickr.people.getPublicPhotos&api_key=" + FLICKR_API_KEY + "&user_id=";

    private int _columns = 3;
    private int _rows = 3;
    private string _rootElement = "photos";

    private IWidgetHost _Host;

    private int PageIndex
    {
        get 
        { 
            return (int)(ViewState[this.ClientID + "_PageIndex"] ?? 0);
        }
        set { ViewState[this.ClientID + "_PageIndex"] = value; }
    }

    private XElement _State;

    private XElement State
    {
        get
        {
            if( _State == null )
            {
                string stateXml = this._Host.GetState();
                if (string.IsNullOrEmpty(stateXml))
                {
                    //stateXml = "<state><type>MostPopular</type><tag /></state>";
                    _State = new XElement("state",
                        new XElement("type", "MostPopular"),
                        new XElement("tag", ""), 
                        new XElement("SetName", ""),
                        new XElement("EmailTag", ""),
                        new XElement("FlickerId", ""),
                        new XElement("Rows", ""),
                        new XElement("Columns", ""));
                }
                else
                {
                    _State = XElement.Parse(stateXml);
                }
            }
            return _State;
        }
    }

    public enum PhotoTypeEnum
    {
        MostRecent = 0,
        MostPopular = 1,
        Tag = 2,
        SetName = 3,
        EmailTag = 4,
        FlickerId = 5
    }

    public PhotoTypeEnum TypeOfPhoto
    {
        get { return (PhotoTypeEnum)Enum.Parse( typeof( PhotoTypeEnum ), State.Element("type").Value ); }
        set { State.Element("type").Value = value.ToString(); }
    }
    public string PhotoTag
    {
        get { return State.Element("tag").Value; }
        set { State.Element("tag").Value = value; }
    }

    public string PhotoSetName // gallery name
    {
        get { return State.Element("SetName").Value; }
        set { State.Element("SetName").Value = value; }
    }

    public string PhotoEmailTag
    {
        get { return State.Element("EmailTag").Value; }
        set { State.Element("EmailTag").Value = value; }
    }

    public string PhotoFlickerId
    {
        get { return State.Element("FlickerId").Value; }
        set { State.Element("FlickerId").Value = value; }
    }

    public int Columns
    {
        get { return _columns; }
        set { _columns = value; }
    }

    public int Rows
    {
        get { return _rows; }
        set { _rows = value; }
    }

    public string RootElement
    {
        get { return _rootElement; }
        set { _rootElement = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CustomEmailTagTextBox.Text = Context.Profile.UserName;
        //CustomUserNameTextBox.Text = "48139038@N07";

        if (!this._Host.IsFirstLoad || ProxyAsync.IsUrlInCache(Cache, this.GetPhotoUrl()))
            this.LoadPhotoView(this, e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!this._Host.IsFirstLoad)
            this.ShowPictures(this.PageIndex);
    }

    protected void LoadPhotoView(object sender, EventArgs e)
    {
        this.ShowPictures(this.PageIndex);

        this.FlickrWidgetMultiview.ActiveViewIndex = 1;
        this.FlickrWidgetTimer.Enabled = false;
    }

    private void LoadState()
    {   
        mostInterestingRadioButton.Checked = (this.TypeOfPhoto == PhotoTypeEnum.MostPopular);
        mostRecentRadioButton.Checked = (this.TypeOfPhoto == PhotoTypeEnum.MostRecent);
        
        bool isTag = (this.TypeOfPhoto == PhotoTypeEnum.Tag);
        customTagRadioButton.Checked = isTag;
        ShowTagButton.Enabled = isTag;
        CustomTagTextBox.Text = (isTag)? this.PhotoTag : "" ;

        bool isUserName = (this.TypeOfPhoto == PhotoTypeEnum.SetName);
        customSetNameRadioButton.Checked = isUserName;
        ShowUserNameButton.Enabled = isUserName;
        CustomSetNameTextBox.Text = (isUserName) ? this.PhotoSetName : "";

        bool isEmail = (this.TypeOfPhoto == PhotoTypeEnum.EmailTag);
        customEmailTagRadioButton.Checked = isEmail;
        ShowEmailButton.Enabled = isEmail;
        CustomEmailTagTextBox.Text = (isEmail) ? this.PhotoEmailTag : "";

        bool isUserId = (this.TypeOfPhoto == PhotoTypeEnum.FlickerId);
        customFlickerIdRadioButton.Checked = isUserId;
        ShowUserIdButton.Enabled = isUserId;
        CustomFlickerIdTextBox.Text = (isUserId) ? this.PhotoFlickerId : "";

        DropDownListRows.SelectedIndex = Rows - 1;
        DropDownListColumns.SelectedIndex = Columns - 1;
    }

    void IWidget.Init(IWidgetHost host)
    {
        this._Host = host;
    }

    void IWidget.ShowSettings()
    {
        settingsPanel.Visible = true;

        this.LoadState();
        
    }
    void IWidget.HideSettings()
    {
        settingsPanel.Visible = false;
    }
    void IWidget.Minimized()
    {
    }
    void IWidget.Maximized()
    {
    }
    void IWidget.Closed()
    {
    }
    protected void photoTypeRadio_CheckedChanged(object sender, EventArgs e)
    {
        this.SaveState();
        
        if (mostRecentRadioButton.Checked || mostInterestingRadioButton.Checked)
            this.ShowPictures(this.PageIndex);
    }

    private void SaveState()
    {
        bool isTag = customTagRadioButton.Checked;
        bool isUserName = customSetNameRadioButton.Checked;
        bool isEmail = customEmailTagRadioButton.Checked;
        bool isUserId = customFlickerIdRadioButton.Checked;
        
        ShowTagButton.Enabled = isTag;
        ShowUserNameButton.Enabled = isUserName;
        ShowEmailButton.Enabled = isEmail;
        ShowUserIdButton.Enabled = isUserId;
        
        Rows = DropDownListRows.SelectedIndex + 1;
        Columns = DropDownListColumns.SelectedIndex + 1; 
        
        if (mostRecentRadioButton.Checked)
            this.TypeOfPhoto = PhotoTypeEnum.MostRecent;
        else if( mostInterestingRadioButton.Checked )
            this.TypeOfPhoto = PhotoTypeEnum.MostPopular;
        else if(isTag)
        {
            this.TypeOfPhoto = PhotoTypeEnum.Tag;
            this.PhotoTag = this.CustomTagTextBox.Text;
        }
        else if (isUserName)
        {
            this.TypeOfPhoto = PhotoTypeEnum.SetName;
            this.PhotoSetName = this.CustomSetNameTextBox.Text;
        }
        else if (isEmail)
        {
            this.TypeOfPhoto = PhotoTypeEnum.EmailTag;
            this.PhotoEmailTag = this.CustomEmailTagTextBox.Text;
        }
        else if (isUserId)
        {
            this.TypeOfPhoto = PhotoTypeEnum.FlickerId;
            this.PhotoFlickerId = this.CustomFlickerIdTextBox.Text;
        }
        Rows = DropDownListRows.SelectedIndex + 1;
        Columns = DropDownListColumns.SelectedIndex + 1;

        this._Host.SaveState(this.State.Xml());
        this.PageIndex = 0;
        this._State = null;
    }

    private string GetPhotoUrl()
    {
        string url = MOST_RECENT;

        if (this.TypeOfPhoto == PhotoTypeEnum.Tag)
            url = ENTER_TAG + this.PhotoTag;
        if (this.TypeOfPhoto == PhotoTypeEnum.SetName)
            url = FIND_BY_SETNAME + this.PhotoSetName;
        if (this.TypeOfPhoto == PhotoTypeEnum.EmailTag)
            url = FIND_BY_EMAIL_TAG + this.PhotoEmailTag;
        if (this.TypeOfPhoto == PhotoTypeEnum.FlickerId)
            url = PHOTOS_FROM_FLICKR_USER + this.PhotoFlickerId;
        else if (this.TypeOfPhoto == PhotoTypeEnum.MostPopular)
            url = INTERESTING;
        else if (this.TypeOfPhoto == PhotoTypeEnum.MostRecent)
            url = MOST_RECENT;


        return url;
    }

    private string LoadPictures()
    {
        string cachedXml = new ProxyAsync().GetXml(GetPhotoUrl(), 5); // was 10
        return cachedXml;
    }

    private void ShowPictures(int pageIndex)
    {
        var xml = this.LoadPictures();
        if( string.IsNullOrEmpty(xml) || xml.Contains("fail") ) return;
        var xroot = XElement.Parse(xml);
        
        RootElement = "photos";
        if (xml.Contains("photoset"))
            RootElement = "photoset";

        var photos = (from photo in xroot.Element(RootElement).Elements("photo")
                    select new PhotoInfo
                    { 
                        Id = (string)photo.Attribute("id"),
                        Owner = (string)photo.Attribute("owner"),
                        Title = (string)photo.Attribute("title"),
                        Secret = (string)photo.Attribute("secret"),
                        Server = (string)photo.Attribute("server"),
                        Farm = (string)photo.Attribute("Farm"),
                        /*IsPublic = (bool)photo.Attribute("ispublic"),
                        IsFriend = (bool)photo.Attribute("isfriend"),
                        IsFamily = (bool)photo.Attribute("isfamily")*/
                    }).Skip(pageIndex*Columns*Rows).Take(Columns*Rows);
        
        HtmlTable table = new HtmlTable();
        table.Align = "center";
        var row = 0;
        var col = 0;
        var count = 0;
        foreach( var photo in photos )
        {
            if( col == 0 )
                table.Rows.Add( new HtmlTableRow() );

            var cell = new HtmlTableCell();


            var div = new HtmlGenericControl("div");
            div.Attributes.Add("class", "preview");

            var img = new HtmlImage();
            img.Src = photo.PhotoUrl(true);
            //img.Width = img.Height = 75;
            img.Border = 0;
            img.Attributes.Add("class", "preview");
            //img.Attributes.Add("onmouseover", "Zoom.larger(this, 150, 150)");
            //img.Attributes.Add("onmouseout", "Zoom.smaller(this, 150, 150)");

            var link = new HtmlGenericControl("a");
            link.Attributes["href"] = photo.PhotoPageUrl;      
            link.Attributes["target"] = "_blank";
            link.Attributes["title"] = photo.Title;
            
            link.Controls.Add(img);
            div.Controls.Add(link);                       
            cell.Controls.Add(div);

            table.Rows[row].Cells.Add(cell);

            col ++;
            if( col == Columns )
            {
                col = 0; row ++;
            }

            count ++;
        }

        photoPanel.Controls.Clear();
        photoPanel.Controls.Add(table);

        if( pageIndex == 0 )
        {
            this.ShowPrevious.Visible = false;
            this.ShowNext.Visible = true;
        }
        else
        {
            this.ShowPrevious.Visible = true;
        }
        if( count < Columns*Rows )
        {
            this.ShowNext.Visible = false;
        }

    }
    protected void ShowPrevious_Click(object sender, EventArgs e)
    {
        this.PageIndex --;        
        this.ShowPictures(this.PageIndex);
    }
    protected void ShowNext_Click(object sender, EventArgs e)
    {
        this.PageIndex ++;
        this.ShowPictures(this.PageIndex);
    }

    protected void ShowTagButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoTag = this.CustomTagTextBox.Text;
        this.SaveState();
    }

    protected void DropDownListRows_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Rows = DropDownListRows.SelectedIndex + 1;
        this.SaveState();
    }

    protected void DropDownListColumns_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Columns = DropDownListColumns.SelectedIndex + 1;
        this.SaveState();
    }

    protected void ShowUserNameButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoSetName = this.CustomSetNameTextBox.Text;
        this.SaveState();
    }

    protected void ShowEmailButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoEmailTag = this.CustomEmailTagTextBox.Text;
        this.SaveState();
    }

    protected void ShowUserIdButton_Clicked(object sender, EventArgs e)
    {
        this.PhotoFlickerId = this.CustomFlickerIdTextBox.Text;
        this.SaveState();
    }
}
