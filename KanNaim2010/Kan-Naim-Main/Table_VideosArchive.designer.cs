using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_VideosArchive")]
    public partial class Table_VideosArchive : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _Id;
		
        private int _CategoryId;
		
        private string _EmbedUrl;
		
        private string _Caption;
		
        private System.DateTime _Date;
		
        private string _CssClass;
		
        private string _AlternateText;
		
        private string _Description;
		
        private string _UrlLink;
		
        private System.Nullable<int> _LastTakId;
		
        private System.Nullable<int> _LastArticleId;
		
        private System.Nullable<int> _GalleryId;
		
        private EntityRef<Table_Article> _Table_Article;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        partial void OnEmbedUrlChanging(string value);
        partial void OnEmbedUrlChanged();
        partial void OnCaptionChanging(string value);
        partial void OnCaptionChanged();
        partial void OnDateChanging(System.DateTime value);
        partial void OnDateChanged();
        partial void OnCssClassChanging(string value);
        partial void OnCssClassChanged();
        partial void OnAlternateTextChanging(string value);
        partial void OnAlternateTextChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnUrlLinkChanging(string value);
        partial void OnUrlLinkChanged();
        partial void OnLastTakIdChanging(System.Nullable<int> value);
        partial void OnLastTakIdChanged();
        partial void OnLastArticleIdChanging(System.Nullable<int> value);
        partial void OnLastArticleIdChanged();
        partial void OnGalleryIdChanging(System.Nullable<int> value);
        partial void OnGalleryIdChanged();
        #endregion
		
        public Table_VideosArchive()
        {
            this._Table_Article = default(EntityRef<Table_Article>);
            OnCreated();
        }
		
        [Column(Storage="_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        public int Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if ((this._Id != value))
                {
                    if (this._Table_Article.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }
		
        [Column(Storage="_CategoryId", DbType="Int NOT NULL")]
        public int CategoryId
        {
            get
            {
                return this._CategoryId;
            }
            set
            {
                if ((this._CategoryId != value))
                {
                    this.OnCategoryIdChanging(value);
                    this.SendPropertyChanging();
                    this._CategoryId = value;
                    this.SendPropertyChanged("CategoryId");
                    this.OnCategoryIdChanged();
                }
            }
        }
		
        [Column(Storage="_EmbedUrl", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
        public string EmbedUrl
        {
            get
            {
                return this._EmbedUrl;
            }
            set
            {
                if ((this._EmbedUrl != value))
                {
                    this.OnEmbedUrlChanging(value);
                    this.SendPropertyChanging();
                    this._EmbedUrl = value;
                    this.SendPropertyChanged("EmbedUrl");
                    this.OnEmbedUrlChanged();
                }
            }
        }
		
        [Column(Storage="_Caption", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string Caption
        {
            get
            {
                return this._Caption;
            }
            set
            {
                if ((this._Caption != value))
                {
                    this.OnCaptionChanging(value);
                    this.SendPropertyChanging();
                    this._Caption = value;
                    this.SendPropertyChanged("Caption");
                    this.OnCaptionChanged();
                }
            }
        }
		
        [Column(Storage="_Date", DbType="Date NOT NULL")]
        public System.DateTime Date
        {
            get
            {
                return this._Date;
            }
            set
            {
                if ((this._Date != value))
                {
                    this.OnDateChanging(value);
                    this.SendPropertyChanging();
                    this._Date = value;
                    this.SendPropertyChanged("Date");
                    this.OnDateChanged();
                }
            }
        }
		
        [Column(Storage="_CssClass", DbType="NChar(20)")]
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
                    this.OnCssClassChanging(value);
                    this.SendPropertyChanging();
                    this._CssClass = value;
                    this.SendPropertyChanged("CssClass");
                    this.OnCssClassChanged();
                }
            }
        }
		
        [Column(Storage="_AlternateText", DbType="NVarChar(50)")]
        public string AlternateText
        {
            get
            {
                return this._AlternateText;
            }
            set
            {
                if ((this._AlternateText != value))
                {
                    this.OnAlternateTextChanging(value);
                    this.SendPropertyChanging();
                    this._AlternateText = value;
                    this.SendPropertyChanged("AlternateText");
                    this.OnAlternateTextChanged();
                }
            }
        }
		
        [Column(Storage="_Description", DbType="NVarChar(50)")]
        public string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if ((this._Description != value))
                {
                    this.OnDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._Description = value;
                    this.SendPropertyChanged("Description");
                    this.OnDescriptionChanged();
                }
            }
        }
		
        [Column(Storage="_UrlLink", DbType="NVarChar(50)")]
        public string UrlLink
        {
            get
            {
                return this._UrlLink;
            }
            set
            {
                if ((this._UrlLink != value))
                {
                    this.OnUrlLinkChanging(value);
                    this.SendPropertyChanging();
                    this._UrlLink = value;
                    this.SendPropertyChanged("UrlLink");
                    this.OnUrlLinkChanged();
                }
            }
        }
		
        [Column(Storage="_LastTakId", DbType="Int")]
        public System.Nullable<int> LastTakId
        {
            get
            {
                return this._LastTakId;
            }
            set
            {
                if ((this._LastTakId != value))
                {
                    this.OnLastTakIdChanging(value);
                    this.SendPropertyChanging();
                    this._LastTakId = value;
                    this.SendPropertyChanged("LastTakId");
                    this.OnLastTakIdChanged();
                }
            }
        }
		
        [Column(Storage="_LastArticleId", DbType="Int")]
        public System.Nullable<int> LastArticleId
        {
            get
            {
                return this._LastArticleId;
            }
            set
            {
                if ((this._LastArticleId != value))
                {
                    this.OnLastArticleIdChanging(value);
                    this.SendPropertyChanging();
                    this._LastArticleId = value;
                    this.SendPropertyChanged("LastArticleId");
                    this.OnLastArticleIdChanged();
                }
            }
        }
		
        [Column(Storage="_GalleryId", DbType="Int")]
        public System.Nullable<int> GalleryId
        {
            get
            {
                return this._GalleryId;
            }
            set
            {
                if ((this._GalleryId != value))
                {
                    this.OnGalleryIdChanging(value);
                    this.SendPropertyChanging();
                    this._GalleryId = value;
                    this.SendPropertyChanged("GalleryId");
                    this.OnGalleryIdChanged();
                }
            }
        }
		
        [Association(Name="Table_Article_Table_VideosArchive", Storage="_Table_Article", ThisKey="Id", OtherKey="EmbedObjId", IsForeignKey=true)]
        public Table_Article Table_Article
        {
            get
            {
                return this._Table_Article.Entity;
            }
            set
            {
                Table_Article previousValue = this._Table_Article.Entity;
                if (((previousValue != value) 
                     || (this._Table_Article.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_Article.Entity = null;
                        previousValue.Table_VideosArchives.Remove(this);
                    }
                    this._Table_Article.Entity = value;
                    if ((value != null))
                    {
                        value.Table_VideosArchives.Add(this);
                        this._Id = value.EmbedObjId;
                    }
                    else
                    {
                        this._Id = default(int);
                    }
                    this.SendPropertyChanged("Table_Article");
                }
            }
        }
		
        public event PropertyChangingEventHandler PropertyChanging;
		
        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }
		
        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}