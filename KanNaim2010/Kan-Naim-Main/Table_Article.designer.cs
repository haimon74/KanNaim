using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_Articles")]
    public partial class Table_Article : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _ArticleId;
		
        private int _CategoryId;
		
        private bool _IsPublished;
		
        private int _StatusId;
		
        private string _Title;
		
        private string _SubTitle;
		
        private string _ArticleContent;
		
        private System.DateTime _UpdateDate;
		
        private System.DateTime _CreateDate;
		
        private int _CreatedBy;
		
        private int _UpdatedBy;
		
        private bool _FlagShowDateTime;
		
        private bool _FlagActiveRSS;
		
        private bool _FlagActiveMivzak;
		
        private bool _FlagTak3ColPicTxt;
		
        private bool _FlagTak3ColPic;
		
        private bool _FlagTak3ColTxt;
		
        private bool _FlagTak2ColPicTxt;
		
        private bool _FlagTak2ColPic;
		
        private bool _FlagTak2ColTxt;
		
        private bool _FlagTak1ColPicTxt;
		
        private bool _FlagTak1ColPic;
		
        private bool _FlagTak1ColTxt;
		
        private bool _FlagTakSmallPicTxt;
		
        private bool _FlagTakSmallPic;
		
        private bool _FlagTakSmallTxt;
		
        private bool _FlagTakLineFeeds;
		
        private int _CountViews;
		
        private string _Summery;
		
        private System.Nullable<int> _CountComments;
		
        private int _ImageId;
		
        private int _EmbedObjId;
		
        private string _MetaTags;
		
        private string _KeysLookup;
		
        private string _KeysAssociated;
		
        private EntitySet<Table_LookupArticleStatus> _Table_LookupArticleStatus;
		
        private EntitySet<Table_LookupCategory> _Table_LookupCategories;
		
        private EntitySet<Table_LookupCategory> _Table_LookupCategories1;
		
        private EntitySet<Table_VideosArchive> _Table_VideosArchives;
		
        private EntitySet<Table_PhotosArchive> _Table_PhotosArchives;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnArticleIdChanging(int value);
        partial void OnArticleIdChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        partial void OnIsPublishedChanging(bool value);
        partial void OnIsPublishedChanged();
        partial void OnStatusIdChanging(int value);
        partial void OnStatusIdChanged();
        partial void OnTitleChanging(string value);
        partial void OnTitleChanged();
        partial void OnSubTitleChanging(string value);
        partial void OnSubTitleChanged();
        partial void OnArticleContentChanging(string value);
        partial void OnArticleContentChanged();
        partial void OnUpdateDateChanging(System.DateTime value);
        partial void OnUpdateDateChanged();
        partial void OnCreateDateChanging(System.DateTime value);
        partial void OnCreateDateChanged();
        partial void OnCreatedByChanging(int value);
        partial void OnCreatedByChanged();
        partial void OnUpdatedByChanging(int value);
        partial void OnUpdatedByChanged();
        partial void OnFlagShowDateTimeChanging(bool value);
        partial void OnFlagShowDateTimeChanged();
        partial void OnFlagActiveRSSChanging(bool value);
        partial void OnFlagActiveRSSChanged();
        partial void OnFlagActiveMivzakChanging(bool value);
        partial void OnFlagActiveMivzakChanged();
        partial void OnFlagTak3ColPicTxtChanging(bool value);
        partial void OnFlagTak3ColPicTxtChanged();
        partial void OnFlagTak3ColPicChanging(bool value);
        partial void OnFlagTak3ColPicChanged();
        partial void OnFlagTak3ColTxtChanging(bool value);
        partial void OnFlagTak3ColTxtChanged();
        partial void OnFlagTak2ColPicTxtChanging(bool value);
        partial void OnFlagTak2ColPicTxtChanged();
        partial void OnFlagTak2ColPicChanging(bool value);
        partial void OnFlagTak2ColPicChanged();
        partial void OnFlagTak2ColTxtChanging(bool value);
        partial void OnFlagTak2ColTxtChanged();
        partial void OnFlagTak1ColPicTxtChanging(bool value);
        partial void OnFlagTak1ColPicTxtChanged();
        partial void OnFlagTak1ColPicChanging(bool value);
        partial void OnFlagTak1ColPicChanged();
        partial void OnFlagTak1ColTxtChanging(bool value);
        partial void OnFlagTak1ColTxtChanged();
        partial void OnFlagTakSmallPicTxtChanging(bool value);
        partial void OnFlagTakSmallPicTxtChanged();
        partial void OnFlagTakSmallPicChanging(bool value);
        partial void OnFlagTakSmallPicChanged();
        partial void OnFlagTakSmallTxtChanging(bool value);
        partial void OnFlagTakSmallTxtChanged();
        partial void OnFlagTakLineFeedsChanging(bool value);
        partial void OnFlagTakLineFeedsChanged();
        partial void OnCountViewsChanging(int value);
        partial void OnCountViewsChanged();
        partial void OnSummeryChanging(string value);
        partial void OnSummeryChanged();
        partial void OnCountCommentsChanging(System.Nullable<int> value);
        partial void OnCountCommentsChanged();
        partial void OnImageIdChanging(int value);
        partial void OnImageIdChanged();
        partial void OnEmbedObjIdChanging(int value);
        partial void OnEmbedObjIdChanged();
        partial void OnMetaTagsChanging(string value);
        partial void OnMetaTagsChanged();
        partial void OnKeysLookupChanging(string value);
        partial void OnKeysLookupChanged();
        partial void OnKeysAssociatedChanging(string value);
        partial void OnKeysAssociatedChanged();
        #endregion
		
        public Table_Article()
        {
            this._Table_LookupArticleStatus = new EntitySet<Table_LookupArticleStatus>(new Action<Table_LookupArticleStatus>(this.attach_Table_LookupArticleStatus), new Action<Table_LookupArticleStatus>(this.detach_Table_LookupArticleStatus));
            this._Table_LookupCategories = new EntitySet<Table_LookupCategory>(new Action<Table_LookupCategory>(this.attach_Table_LookupCategories), new Action<Table_LookupCategory>(this.detach_Table_LookupCategories));
            this._Table_LookupCategories1 = new EntitySet<Table_LookupCategory>(new Action<Table_LookupCategory>(this.attach_Table_LookupCategories1), new Action<Table_LookupCategory>(this.detach_Table_LookupCategories1));
            this._Table_VideosArchives = new EntitySet<Table_VideosArchive>(new Action<Table_VideosArchive>(this.attach_Table_VideosArchives), new Action<Table_VideosArchive>(this.detach_Table_VideosArchives));
            this._Table_PhotosArchives = new EntitySet<Table_PhotosArchive>(new Action<Table_PhotosArchive>(this.attach_Table_PhotosArchives), new Action<Table_PhotosArchive>(this.detach_Table_PhotosArchives));
            OnCreated();
        }
		
        [Column(Storage="_ArticleId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        public int ArticleId
        {
            get
            {
                return this._ArticleId;
            }
            set
            {
                if ((this._ArticleId != value))
                {
                    this.OnArticleIdChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleId = value;
                    this.SendPropertyChanged("ArticleId");
                    this.OnArticleIdChanged();
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
		
        [Column(Storage="_IsPublished", DbType="Bit NOT NULL")]
        public bool IsPublished
        {
            get
            {
                return this._IsPublished;
            }
            set
            {
                if ((this._IsPublished != value))
                {
                    this.OnIsPublishedChanging(value);
                    this.SendPropertyChanging();
                    this._IsPublished = value;
                    this.SendPropertyChanged("IsPublished");
                    this.OnIsPublishedChanged();
                }
            }
        }
		
        [Column(Storage="_StatusId", DbType="Int NOT NULL")]
        public int StatusId
        {
            get
            {
                return this._StatusId;
            }
            set
            {
                if ((this._StatusId != value))
                {
                    this.OnStatusIdChanging(value);
                    this.SendPropertyChanging();
                    this._StatusId = value;
                    this.SendPropertyChanged("StatusId");
                    this.OnStatusIdChanged();
                }
            }
        }
		
        [Column(Storage="_Title", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.OnTitleChanging(value);
                    this.SendPropertyChanging();
                    this._Title = value;
                    this.SendPropertyChanged("Title");
                    this.OnTitleChanged();
                }
            }
        }
		
        [Column(Storage="_SubTitle", DbType="NVarChar(250) NOT NULL", CanBeNull=false)]
        public string SubTitle
        {
            get
            {
                return this._SubTitle;
            }
            set
            {
                if ((this._SubTitle != value))
                {
                    this.OnSubTitleChanging(value);
                    this.SendPropertyChanging();
                    this._SubTitle = value;
                    this.SendPropertyChanged("SubTitle");
                    this.OnSubTitleChanged();
                }
            }
        }
		
        [Column(Storage="_ArticleContent", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
        public string ArticleContent
        {
            get
            {
                return this._ArticleContent;
            }
            set
            {
                if ((this._ArticleContent != value))
                {
                    this.OnArticleContentChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleContent = value;
                    this.SendPropertyChanged("ArticleContent");
                    this.OnArticleContentChanged();
                }
            }
        }
		
        [Column(Storage="_UpdateDate", DbType="DateTime NOT NULL")]
        public System.DateTime UpdateDate
        {
            get
            {
                return this._UpdateDate;
            }
            set
            {
                if ((this._UpdateDate != value))
                {
                    this.OnUpdateDateChanging(value);
                    this.SendPropertyChanging();
                    this._UpdateDate = value;
                    this.SendPropertyChanged("UpdateDate");
                    this.OnUpdateDateChanged();
                }
            }
        }
		
        [Column(Storage="_CreateDate", DbType="DateTime NOT NULL")]
        public System.DateTime CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                if ((this._CreateDate != value))
                {
                    this.OnCreateDateChanging(value);
                    this.SendPropertyChanging();
                    this._CreateDate = value;
                    this.SendPropertyChanged("CreateDate");
                    this.OnCreateDateChanged();
                }
            }
        }
		
        [Column(Storage="_CreatedBy", DbType="Int NOT NULL")]
        public int CreatedBy
        {
            get
            {
                return this._CreatedBy;
            }
            set
            {
                if ((this._CreatedBy != value))
                {
                    this.OnCreatedByChanging(value);
                    this.SendPropertyChanging();
                    this._CreatedBy = value;
                    this.SendPropertyChanged("CreatedBy");
                    this.OnCreatedByChanged();
                }
            }
        }
		
        [Column(Storage="_UpdatedBy", DbType="Int NOT NULL")]
        public int UpdatedBy
        {
            get
            {
                return this._UpdatedBy;
            }
            set
            {
                if ((this._UpdatedBy != value))
                {
                    this.OnUpdatedByChanging(value);
                    this.SendPropertyChanging();
                    this._UpdatedBy = value;
                    this.SendPropertyChanged("UpdatedBy");
                    this.OnUpdatedByChanged();
                }
            }
        }
		
        [Column(Storage="_FlagShowDateTime", DbType="Bit NOT NULL")]
        public bool FlagShowDateTime
        {
            get
            {
                return this._FlagShowDateTime;
            }
            set
            {
                if ((this._FlagShowDateTime != value))
                {
                    this.OnFlagShowDateTimeChanging(value);
                    this.SendPropertyChanging();
                    this._FlagShowDateTime = value;
                    this.SendPropertyChanged("FlagShowDateTime");
                    this.OnFlagShowDateTimeChanged();
                }
            }
        }
		
        [Column(Storage="_FlagActiveRSS", DbType="Bit NOT NULL")]
        public bool FlagActiveRSS
        {
            get
            {
                return this._FlagActiveRSS;
            }
            set
            {
                if ((this._FlagActiveRSS != value))
                {
                    this.OnFlagActiveRSSChanging(value);
                    this.SendPropertyChanging();
                    this._FlagActiveRSS = value;
                    this.SendPropertyChanged("FlagActiveRSS");
                    this.OnFlagActiveRSSChanged();
                }
            }
        }
		
        [Column(Storage="_FlagActiveMivzak", DbType="Bit NOT NULL")]
        public bool FlagActiveMivzak
        {
            get
            {
                return this._FlagActiveMivzak;
            }
            set
            {
                if ((this._FlagActiveMivzak != value))
                {
                    this.OnFlagActiveMivzakChanging(value);
                    this.SendPropertyChanging();
                    this._FlagActiveMivzak = value;
                    this.SendPropertyChanged("FlagActiveMivzak");
                    this.OnFlagActiveMivzakChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak3ColPicTxt", DbType="Bit NOT NULL")]
        public bool FlagTak3ColPicTxt
        {
            get
            {
                return this._FlagTak3ColPicTxt;
            }
            set
            {
                if ((this._FlagTak3ColPicTxt != value))
                {
                    this.OnFlagTak3ColPicTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak3ColPicTxt = value;
                    this.SendPropertyChanged("FlagTak3ColPicTxt");
                    this.OnFlagTak3ColPicTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak3ColPic", DbType="Bit NOT NULL")]
        public bool FlagTak3ColPic
        {
            get
            {
                return this._FlagTak3ColPic;
            }
            set
            {
                if ((this._FlagTak3ColPic != value))
                {
                    this.OnFlagTak3ColPicChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak3ColPic = value;
                    this.SendPropertyChanged("FlagTak3ColPic");
                    this.OnFlagTak3ColPicChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak3ColTxt", DbType="Bit NOT NULL")]
        public bool FlagTak3ColTxt
        {
            get
            {
                return this._FlagTak3ColTxt;
            }
            set
            {
                if ((this._FlagTak3ColTxt != value))
                {
                    this.OnFlagTak3ColTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak3ColTxt = value;
                    this.SendPropertyChanged("FlagTak3ColTxt");
                    this.OnFlagTak3ColTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak2ColPicTxt", DbType="Bit NOT NULL")]
        public bool FlagTak2ColPicTxt
        {
            get
            {
                return this._FlagTak2ColPicTxt;
            }
            set
            {
                if ((this._FlagTak2ColPicTxt != value))
                {
                    this.OnFlagTak2ColPicTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak2ColPicTxt = value;
                    this.SendPropertyChanged("FlagTak2ColPicTxt");
                    this.OnFlagTak2ColPicTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak2ColPic", DbType="Bit NOT NULL")]
        public bool FlagTak2ColPic
        {
            get
            {
                return this._FlagTak2ColPic;
            }
            set
            {
                if ((this._FlagTak2ColPic != value))
                {
                    this.OnFlagTak2ColPicChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak2ColPic = value;
                    this.SendPropertyChanged("FlagTak2ColPic");
                    this.OnFlagTak2ColPicChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak2ColTxt", DbType="Bit NOT NULL")]
        public bool FlagTak2ColTxt
        {
            get
            {
                return this._FlagTak2ColTxt;
            }
            set
            {
                if ((this._FlagTak2ColTxt != value))
                {
                    this.OnFlagTak2ColTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak2ColTxt = value;
                    this.SendPropertyChanged("FlagTak2ColTxt");
                    this.OnFlagTak2ColTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak1ColPicTxt", DbType="Bit NOT NULL")]
        public bool FlagTak1ColPicTxt
        {
            get
            {
                return this._FlagTak1ColPicTxt;
            }
            set
            {
                if ((this._FlagTak1ColPicTxt != value))
                {
                    this.OnFlagTak1ColPicTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak1ColPicTxt = value;
                    this.SendPropertyChanged("FlagTak1ColPicTxt");
                    this.OnFlagTak1ColPicTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak1ColPic", DbType="Bit NOT NULL")]
        public bool FlagTak1ColPic
        {
            get
            {
                return this._FlagTak1ColPic;
            }
            set
            {
                if ((this._FlagTak1ColPic != value))
                {
                    this.OnFlagTak1ColPicChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak1ColPic = value;
                    this.SendPropertyChanged("FlagTak1ColPic");
                    this.OnFlagTak1ColPicChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTak1ColTxt", DbType="Bit NOT NULL")]
        public bool FlagTak1ColTxt
        {
            get
            {
                return this._FlagTak1ColTxt;
            }
            set
            {
                if ((this._FlagTak1ColTxt != value))
                {
                    this.OnFlagTak1ColTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTak1ColTxt = value;
                    this.SendPropertyChanged("FlagTak1ColTxt");
                    this.OnFlagTak1ColTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTakSmallPicTxt", DbType="Bit NOT NULL")]
        public bool FlagTakSmallPicTxt
        {
            get
            {
                return this._FlagTakSmallPicTxt;
            }
            set
            {
                if ((this._FlagTakSmallPicTxt != value))
                {
                    this.OnFlagTakSmallPicTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTakSmallPicTxt = value;
                    this.SendPropertyChanged("FlagTakSmallPicTxt");
                    this.OnFlagTakSmallPicTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTakSmallPic", DbType="Bit NOT NULL")]
        public bool FlagTakSmallPic
        {
            get
            {
                return this._FlagTakSmallPic;
            }
            set
            {
                if ((this._FlagTakSmallPic != value))
                {
                    this.OnFlagTakSmallPicChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTakSmallPic = value;
                    this.SendPropertyChanged("FlagTakSmallPic");
                    this.OnFlagTakSmallPicChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTakSmallTxt", DbType="Bit NOT NULL")]
        public bool FlagTakSmallTxt
        {
            get
            {
                return this._FlagTakSmallTxt;
            }
            set
            {
                if ((this._FlagTakSmallTxt != value))
                {
                    this.OnFlagTakSmallTxtChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTakSmallTxt = value;
                    this.SendPropertyChanged("FlagTakSmallTxt");
                    this.OnFlagTakSmallTxtChanged();
                }
            }
        }
		
        [Column(Storage="_FlagTakLineFeeds", DbType="Bit NOT NULL")]
        public bool FlagTakLineFeeds
        {
            get
            {
                return this._FlagTakLineFeeds;
            }
            set
            {
                if ((this._FlagTakLineFeeds != value))
                {
                    this.OnFlagTakLineFeedsChanging(value);
                    this.SendPropertyChanging();
                    this._FlagTakLineFeeds = value;
                    this.SendPropertyChanged("FlagTakLineFeeds");
                    this.OnFlagTakLineFeedsChanged();
                }
            }
        }
		
        [Column(Storage="_CountViews", DbType="Int NOT NULL")]
        public int CountViews
        {
            get
            {
                return this._CountViews;
            }
            set
            {
                if ((this._CountViews != value))
                {
                    this.OnCountViewsChanging(value);
                    this.SendPropertyChanging();
                    this._CountViews = value;
                    this.SendPropertyChanged("CountViews");
                    this.OnCountViewsChanged();
                }
            }
        }
		
        [Column(Storage="_Summery", DbType="NText", UpdateCheck=UpdateCheck.Never)]
        public string Summery
        {
            get
            {
                return this._Summery;
            }
            set
            {
                if ((this._Summery != value))
                {
                    this.OnSummeryChanging(value);
                    this.SendPropertyChanging();
                    this._Summery = value;
                    this.SendPropertyChanged("Summery");
                    this.OnSummeryChanged();
                }
            }
        }
		
        [Column(Storage="_CountComments", DbType="Int")]
        public System.Nullable<int> CountComments
        {
            get
            {
                return this._CountComments;
            }
            set
            {
                if ((this._CountComments != value))
                {
                    this.OnCountCommentsChanging(value);
                    this.SendPropertyChanging();
                    this._CountComments = value;
                    this.SendPropertyChanged("CountComments");
                    this.OnCountCommentsChanged();
                }
            }
        }
		
        [Column(Storage="_ImageId", DbType="Int")]
        public int ImageId
        {
            get
            {
                return this._ImageId;
            }
            set
            {
                if ((this._ImageId != value))
                {
                    this.OnImageIdChanging(value);
                    this.SendPropertyChanging();
                    this._ImageId = value;
                    this.SendPropertyChanged("ImageId");
                    this.OnImageIdChanged();
                }
            }
        }
		
        [Column(Storage="_EmbedObjId", DbType="Int")]
        public int EmbedObjId
        {
            get
            {
                return this._EmbedObjId;
            }
            set
            {
                if ((this._EmbedObjId != value))
                {
                    this.OnEmbedObjIdChanging(value);
                    this.SendPropertyChanging();
                    this._EmbedObjId = value;
                    this.SendPropertyChanged("EmbedObjId");
                    this.OnEmbedObjIdChanged();
                }
            }
        }
		
        [Column(Storage="_MetaTags", DbType="NVarChar(200)")]
        public string MetaTags
        {
            get
            {
                return this._MetaTags;
            }
            set
            {
                if ((this._MetaTags != value))
                {
                    this.OnMetaTagsChanging(value);
                    this.SendPropertyChanging();
                    this._MetaTags = value;
                    this.SendPropertyChanged("MetaTags");
                    this.OnMetaTagsChanged();
                }
            }
        }
		
        [Column(Storage="_KeysLookup", DbType="NVarChar(90)")]
        public string KeysLookup
        {
            get
            {
                return this._KeysLookup;
            }
            set
            {
                if ((this._KeysLookup != value))
                {
                    this.OnKeysLookupChanging(value);
                    this.SendPropertyChanging();
                    this._KeysLookup = value;
                    this.SendPropertyChanged("KeysLookup");
                    this.OnKeysLookupChanged();
                }
            }
        }
		
        [Column(Storage="_KeysAssociated", DbType="NVarChar(90)")]
        public string KeysAssociated
        {
            get
            {
                return this._KeysAssociated;
            }
            set
            {
                if ((this._KeysAssociated != value))
                {
                    this.OnKeysAssociatedChanging(value);
                    this.SendPropertyChanging();
                    this._KeysAssociated = value;
                    this.SendPropertyChanged("KeysAssociated");
                    this.OnKeysAssociatedChanged();
                }
            }
        }
		
        [Association(Name="Table_Article_Table_LookupArticleStatus", Storage="_Table_LookupArticleStatus", ThisKey="StatusId", OtherKey="StatusId")]
        public EntitySet<Table_LookupArticleStatus> Table_LookupArticleStatus
        {
            get
            {
                return this._Table_LookupArticleStatus;
            }
            set
            {
                this._Table_LookupArticleStatus.Assign(value);
            }
        }
		
        [Association(Name="Table_Article_Table_LookupCategory", Storage="_Table_LookupCategories", ThisKey="CategoryId", OtherKey="CatId")]
        public EntitySet<Table_LookupCategory> Table_LookupCategories
        {
            get
            {
                return this._Table_LookupCategories;
            }
            set
            {
                this._Table_LookupCategories.Assign(value);
            }
        }
		
        [Association(Name="Table_Article_Table_LookupCategory1", Storage="_Table_LookupCategories1", ThisKey="CategoryId", OtherKey="CatId")]
        public EntitySet<Table_LookupCategory> Table_LookupCategories1
        {
            get
            {
                return this._Table_LookupCategories1;
            }
            set
            {
                this._Table_LookupCategories1.Assign(value);
            }
        }
		
        [Association(Name="Table_Article_Table_VideosArchive", Storage="_Table_VideosArchives", ThisKey="EmbedObjId", OtherKey="Id")]
        public EntitySet<Table_VideosArchive> Table_VideosArchives
        {
            get
            {
                return this._Table_VideosArchives;
            }
            set
            {
                this._Table_VideosArchives.Assign(value);
            }
        }
		
        [Association(Name="Table_Article_Table_PhotosArchive", Storage="_Table_PhotosArchives", ThisKey="ImageId", OtherKey="Id")]
        public EntitySet<Table_PhotosArchive> Table_PhotosArchives
        {
            get
            {
                return this._Table_PhotosArchives;
            }
            set
            {
                this._Table_PhotosArchives.Assign(value);
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
		
        private void attach_Table_LookupArticleStatus(Table_LookupArticleStatus entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = this;
        }
		
        private void detach_Table_LookupArticleStatus(Table_LookupArticleStatus entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = null;
        }
		
        private void attach_Table_LookupCategories(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = this;
        }
		
        private void detach_Table_LookupCategories(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = null;
        }
		
        private void attach_Table_LookupCategories1(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article1 = this;
        }
		
        private void detach_Table_LookupCategories1(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article1 = null;
        }
		
        private void attach_Table_VideosArchives(Table_VideosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = this;
        }
		
        private void detach_Table_VideosArchives(Table_VideosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = null;
        }
		
        private void attach_Table_PhotosArchives(Table_PhotosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = this;
        }
		
        private void detach_Table_PhotosArchives(Table_PhotosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_Article = null;
        }
    }
}