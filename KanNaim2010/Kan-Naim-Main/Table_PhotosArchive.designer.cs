using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_PhotosArchive")]
    public partial class Table_PhotosArchive : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _Id;
		
        private string _ImageUrl;
		
        private int _PhotoTypeId;
		
        private System.DateTime _Date;
		
        private string _CssClass;
		
        private string _UrlLink;
		
        private System.Nullable<int> _LastTakId;
		
        private System.Nullable<int> _LastArticleId;
		
        private System.Nullable<int> _GalleryId;
		
        private System.Nullable<int> _Width;
		
        private System.Nullable<int> _Height;
		
        private int _OriginalPhotoId;
		
        private EntitySet<Table_LookupPhotoType> _Table_LookupPhotoTypes;
		
        private EntitySet<Table_OriginalPhotosArchive> _Table_OriginalPhotosArchives;
		
        private EntityRef<Table_Article> _Table_Article;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnImageUrlChanging(string value);
        partial void OnImageUrlChanged();
        partial void OnPhotoTypeIdChanging(int value);
        partial void OnPhotoTypeIdChanged();
        partial void OnDateChanging(System.DateTime value);
        partial void OnDateChanged();
        partial void OnCssClassChanging(string value);
        partial void OnCssClassChanged();
        partial void OnUrlLinkChanging(string value);
        partial void OnUrlLinkChanged();
        partial void OnLastTakIdChanging(System.Nullable<int> value);
        partial void OnLastTakIdChanged();
        partial void OnLastArticleIdChanging(System.Nullable<int> value);
        partial void OnLastArticleIdChanged();
        partial void OnGalleryIdChanging(System.Nullable<int> value);
        partial void OnGalleryIdChanged();
        partial void OnWidthChanging(System.Nullable<int> value);
        partial void OnWidthChanged();
        partial void OnHeightChanging(System.Nullable<int> value);
        partial void OnHeightChanged();
        partial void OnOriginalPhotoIdChanging(int value);
        partial void OnOriginalPhotoIdChanged();
        #endregion
		
        public Table_PhotosArchive()
        {
            this._Table_LookupPhotoTypes = new EntitySet<Table_LookupPhotoType>(new Action<Table_LookupPhotoType>(this.attach_Table_LookupPhotoTypes), new Action<Table_LookupPhotoType>(this.detach_Table_LookupPhotoTypes));
            this._Table_OriginalPhotosArchives = new EntitySet<Table_OriginalPhotosArchive>(new Action<Table_OriginalPhotosArchive>(this.attach_Table_OriginalPhotosArchives), new Action<Table_OriginalPhotosArchive>(this.detach_Table_OriginalPhotosArchives));
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
		
        [Column(Storage="_ImageUrl", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
                    this.OnImageUrlChanging(value);
                    this.SendPropertyChanging();
                    this._ImageUrl = value;
                    this.SendPropertyChanged("ImageUrl");
                    this.OnImageUrlChanged();
                }
            }
        }
		
        [Column(Storage="_PhotoTypeId", DbType="Int NOT NULL")]
        public int PhotoTypeId
        {
            get
            {
                return this._PhotoTypeId;
            }
            set
            {
                if ((this._PhotoTypeId != value))
                {
                    this.OnPhotoTypeIdChanging(value);
                    this.SendPropertyChanging();
                    this._PhotoTypeId = value;
                    this.SendPropertyChanged("PhotoTypeId");
                    this.OnPhotoTypeIdChanged();
                }
            }
        }
		
        [Column(Storage="_Date", DbType="DateTime NOT NULL")]
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
		
        [Column(Storage="_Width", DbType="Int")]
        public System.Nullable<int> Width
        {
            get
            {
                return this._Width;
            }
            set
            {
                if ((this._Width != value))
                {
                    this.OnWidthChanging(value);
                    this.SendPropertyChanging();
                    this._Width = value;
                    this.SendPropertyChanged("Width");
                    this.OnWidthChanged();
                }
            }
        }
		
        [Column(Storage="_Height", DbType="Int")]
        public System.Nullable<int> Height
        {
            get
            {
                return this._Height;
            }
            set
            {
                if ((this._Height != value))
                {
                    this.OnHeightChanging(value);
                    this.SendPropertyChanging();
                    this._Height = value;
                    this.SendPropertyChanged("Height");
                    this.OnHeightChanged();
                }
            }
        }
		
        [Column(Storage="_OriginalPhotoId", DbType="Int NOT NULL")]
        public int OriginalPhotoId
        {
            get
            {
                return this._OriginalPhotoId;
            }
            set
            {
                if ((this._OriginalPhotoId != value))
                {
                    this.OnOriginalPhotoIdChanging(value);
                    this.SendPropertyChanging();
                    this._OriginalPhotoId = value;
                    this.SendPropertyChanged("OriginalPhotoId");
                    this.OnOriginalPhotoIdChanged();
                }
            }
        }
		
        [Association(Name="Table_PhotosArchive_Table_LookupPhotoType", Storage="_Table_LookupPhotoTypes", ThisKey="PhotoTypeId", OtherKey="PhotoTypeId")]
        public EntitySet<Table_LookupPhotoType> Table_LookupPhotoTypes
        {
            get
            {
                return this._Table_LookupPhotoTypes;
            }
            set
            {
                this._Table_LookupPhotoTypes.Assign(value);
            }
        }
		
        [Association(Name="Table_PhotosArchive_Table_OriginalPhotosArchive", Storage="_Table_OriginalPhotosArchives", ThisKey="OriginalPhotoId", OtherKey="PhotoId")]
        public EntitySet<Table_OriginalPhotosArchive> Table_OriginalPhotosArchives
        {
            get
            {
                return this._Table_OriginalPhotosArchives;
            }
            set
            {
                this._Table_OriginalPhotosArchives.Assign(value);
            }
        }
		
        [Association(Name="Table_Article_Table_PhotosArchive", Storage="_Table_Article", ThisKey="Id", OtherKey="ImageId", IsForeignKey=true)]
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
                        previousValue.Table_PhotosArchives.Remove(this);
                    }
                    this._Table_Article.Entity = value;
                    if ((value != null))
                    {
                        value.Table_PhotosArchives.Add(this);
                        this._Id = value.ImageId;
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
		
        private void attach_Table_LookupPhotoTypes(Table_LookupPhotoType entity)
        {
            this.SendPropertyChanging();
            entity.Table_PhotosArchive = this;
        }
		
        private void detach_Table_LookupPhotoTypes(Table_LookupPhotoType entity)
        {
            this.SendPropertyChanging();
            entity.Table_PhotosArchive = null;
        }
		
        private void attach_Table_OriginalPhotosArchives(Table_OriginalPhotosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_PhotosArchive = this;
        }
		
        private void detach_Table_OriginalPhotosArchives(Table_OriginalPhotosArchive entity)
        {
            this.SendPropertyChanging();
            entity.Table_PhotosArchive = null;
        }
    }
}