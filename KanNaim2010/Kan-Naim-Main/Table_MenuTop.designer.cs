using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_MenuTop")]
    public partial class Table_MenuTop
    {
		
        private int _ItemId;
		
        private string _DisplayText;
		
        private string _ImageUrl;
		
        private string _Url;
		
        private string _ToolTip;
		
        private int _LocationFromTheRight;
		
        private string _CssClass;
		
        private string _CssHover;
		
        public Table_MenuTop()
        {
        }
		
        [Column(Storage="_ItemId", DbType="Int NOT NULL")]
        public int ItemId
        {
            get
            {
                return this._ItemId;
            }
            set
            {
                if ((this._ItemId != value))
                {
                    this._ItemId = value;
                }
            }
        }
		
        [Column(Storage="_DisplayText", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string DisplayText
        {
            get
            {
                return this._DisplayText;
            }
            set
            {
                if ((this._DisplayText != value))
                {
                    this._DisplayText = value;
                }
            }
        }
		
        [Column(Storage="_ImageUrl", DbType="NVarChar(50)")]
        public string ImageUrl
        {
            get
            {
                return this._ImageUrl;
            }
            set
            {
                if ((this._ImageUrl != value))
                {
                    this._ImageUrl = value;
                }
            }
        }
		
        [Column(Storage="_Url", DbType="NVarChar(50)")]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                if ((this._Url != value))
                {
                    this._Url = value;
                }
            }
        }
		
        [Column(Storage="_ToolTip", DbType="NVarChar(50)")]
        public string ToolTip
        {
            get
            {
                return this._ToolTip;
            }
            set
            {
                if ((this._ToolTip != value))
                {
                    this._ToolTip = value;
                }
            }
        }
		
        [Column(Storage="_LocationFromTheRight", DbType="Int NOT NULL")]
        public int LocationFromTheRight
        {
            get
            {
                return this._LocationFromTheRight;
            }
            set
            {
                if ((this._LocationFromTheRight != value))
                {
                    this._LocationFromTheRight = value;
                }
            }
        }
		
        [Column(Storage="_CssClass", DbType="NVarChar(50)")]
        public string CssClass
        {
            get
            {
                return this._CssClass;
            }
            set
            {
                if ((this._CssClass != value))
                {
                    this._CssClass = value;
                }
            }
        }
		
        [Column(Storage="_CssHover", DbType="NVarChar(50)")]
        public string CssHover
        {
            get
            {
                return this._CssHover;
            }
            set
            {
                if ((this._CssHover != value))
                {
                    this._CssHover = value;
                }
            }
        }
    }
}