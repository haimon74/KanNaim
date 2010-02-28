using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_StyleManagementForm_InputNumber : System.Web.UI.UserControl
{
    private string _labelText = "";
    private string _rangeMin = int.MinValue.ToString();
    private string _rangeMax = int.MaxValue.ToString();
    private bool _isNeverClicked = true;

    public string LabelText
    {
        get { return _labelText; }
        set { _labelText = value; }
    }

    public string RangeMin
    {
        get { return _rangeMin; }
        set { _rangeMin = value; }
    }

    public string RangeMax
    {
        get { return _rangeMax; }
        set { _rangeMax = value; }
    }

    public bool IsNeverClicked
    {
        get { return _isNeverClicked; }
        set { _isNeverClicked = value; }
    }

    //public string LabelText
    //{
    //    get { return (string)ViewState["LabelText"]; }
    //    set { ViewState["LabelText"] = value; }
    //}

    //public string RangeMin
    //{
    //    get { return (string)ViewState["RangeMin"]; }
    //    set { ViewState["RangeMin"] = value; }
    //}

    //public string RangeMax
    //{
    //    get { return (string)ViewState["RangeMax"]; }
    //    set { ViewState["RangeMax"] = value; }
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Label1.Text = LabelText;
            TextBox1.ToolTip = String.Format("integer range: {0} ~ {1}", RangeMin, RangeMax);
            RangeValidator1.ToolTip = String.Format("integer range: {0} ~ {1}", RangeMin, RangeMax);
            RangeValidator1.ErrorMessage = String.Format("input value is not in range: {0} ~ {1}", RangeMin, RangeMax);
            RangeValidator1.MinimumValue = RangeMin;
            RangeValidator1.MaximumValue = RangeMax;
        }
    }
}
