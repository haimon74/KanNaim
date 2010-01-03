using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupCategories")]
    public partial class Table_LookupCategory : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _CatId;
		
        private int _ParentCatId;
		
        private string _CatHebrewName;
		
        private string _CatEnglishName;
		
        private string _Tags;
		
        private string _RelatedCatIds;
		
        private System.Nullable<int> _PhotoId;
		
        private System.Nullable<int> _RssId;
		
        private string _MetaTags;
		
        private string _Description;
		
        private EntityRef<Table_Article> _Table_Article;
		
        private EntityRef<Table_Article> _Table_Article1;
		
        private EntityRef<Table_OriginalPhotosArchive> _Table_OriginalPhotosArchive;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnCatIdChanging(int value);
        partial void OnCatIdChanged();
        partial void OnParentCatIdChanging(int value);
        partial void OnParentCatIdChanged();
        partial void OnCatHebrewNameChanging(string value);
        partial void OnCatHebrewNameChanged();
        partial void OnCatEnglishNameChanging(string value);
        partial void OnCatEnglishNameChanged();
        partial void OnTagsChanging(string value);
        partial void OnTagsChanged();
        partial void OnRelatedCatIdsChanging(string value);
        partial void OnRelatedCatIdsChanged();
        partial void OnPhotoIdChanging(System.Nullable<int> value);
        partial void OnPhotoIdChanged();
        partial void OnRssIdChanging(System.Nullable<int> value);
        partial void OnRssIdChanged();
        partial void OnMetaTagsChanging(string value);
        partial void OnMetaTagsChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        #endregion
		
        public Table_LookupCategory()
        {
            this._Table_Article = default(EntityRef<Table_Article>);
            this._Table_Article1 = default(EntityRef<Table_Article>);
            this._Table_OriginalPhotosArchive = default(EntityRef<Table_OriginalPhotosArchive>);
            OnCreated();
        }
		
        [Column(Storage="_CatId", DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        public int CatId
        {
            get
            {
                return this._CatId;
            }
            set
            {
                if ((this._CatId != value))
                {
                    if (this._Table_Article.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnCatIdChanging(value);
                    this.SendPropertyChanging();
                    this._CatId = value;
                    this.SendPropertyChanged("CatId");
                    this.OnCatIdChanged();
                }
            }
        }
		
        [Column(Storage="_ParentCatId", DbType="Int NOT NULL")]
        public int ParentCatId
        {
            get
            {
                return this._ParentCatId;
            }
            set
            {
                if ((this._ParentCatId != value))
                {
                    this.OnParentCatIdChanging(value);
                    this.SendPropertyChanging();
                    this._ParentCatId = value;
                    this.SendPropertyChanged("ParentCatId");
                    this.OnParentCatIdChanged();
                }
            }
        }
		
        [Column(Storage="_CatHebrewName", DbType="NChar(30) NOT NULL", CanBeNull=false)]
        public string CatHebrewName
        {
            get
            {
                return this._CatHebrewName;
            }
            set
            {
                if ((this._CatHebrewName != value))
                {
                    this.OnCatHebrewNameChanging(value);
                    this.SendPropertyChanging();
                    this._CatHebrewName = value;
                    this.SendPropertyChanged("CatHebrewName");
                    this.OnCatHebrewNameChanged();
                }
            }
        }
		
        [Column(Storage="_CatEnglishName", DbType="NChar(30) NOT NULL", CanBeNull=false)]
        public string CatEnglishName
        {
            get
            {
                return this._CatEnglishName;
            }
            set
            {
                if ((this._CatEnglishName != value))
                {
                    this.OnCatEnglishNameChanging(value);
                    this.SendPropertyChanging();
                    this._CatEnglishName = value;
                    this.SendPropertyChanged("CatEnglishName");
                    this.OnCatEnglishNameChanged();
                }
            }
        }
		
        [Column(Storage="_Tags", DbType="NVarChar(100)")]
        public string Tags
        {
            get
            {
                return this._Tags;
            }
            set
            {
                if ((this._Tags != value))
                {
                    this.OnTagsChanging(value);
                    this.SendPropertyChanging();
                    this._Tags = value;
                    this.SendPropertyChanged("Tags");
                    this.OnTagsChanged();
                }
            }
        }
		
        [Column(Storage="_RelatedCatIds", DbType="NVarChar(30)")]
        public string RelatedCatIds
        {
            get
            {
                return this._RelatedCatIds;
            }
            set
            {
                if ((this._RelatedCatIds != value))
                {
                    this.OnRelatedCatIdsChanging(value);
                    this.SendPropertyChanging();
                    this._RelatedCatIds = value;
                    this.SendPropertyChanged("RelatedCatIds");
                    this.OnRelatedCatIdsChanged();
                }
            }
        }
		
        [Column(Storage="_PhotoId", DbType="Int")]
        public System.Nullable<int> PhotoId
        {
            get
            {
                return this._PhotoId;
            }
            set
            {
                if ((this._PhotoId != value))
                {
                    this.OnPhotoIdChanging(value);
                    this.SendPropertyChanging();
                    this._PhotoId = value;
                    this.SendPropertyChanged("PhotoId");
                    this.OnPhotoIdChanged();
                }
            }
        }
		
        [Column(Storage="_RssId", DbType="Int")]
        public System.Nullable<int> RssId
        {
            get
            {
                return this._RssId;
            }
            set
            {
                if ((this._RssId != value))
                {
                    this.OnRssIdChanging(value);
                    this.SendPropertyChanging();
                    this._RssId = value;
                    this.SendPropertyChanged("RssId");
                    this.OnRssIdChanged();
                }
            }
        }
		
        [Column(Storage="_MetaTags", DbType="NVarChar(250)")]
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
		
        [Column(Storage="_Description", DbType="NVarChar(250)")]
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
		
        [Association(Name="Table_Article_Table_LookupCategory", Storage="_Table_Article", ThisKey="CatId", OtherKey="CategoryId", IsForeignKey=true)]
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
                        previousValue.Table_LookupCategories.Remove(this);
                    }
                    this._Table_Article.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupCategories.Add(this);
                        this._CatId = value.CategoryId;
                    }
                    else
                    {
                        this._CatId = default(int);
                    }
                    this.SendPropertyChanged("Table_Article");
                }
            }
        }
		
        [Association(Name="Table_Article_Table_LookupCategory1", Storage="_Table_Article1", ThisKey="CatId", OtherKey="CategoryId", IsForeignKey=true)]
        public Table_Article Table_Article1
        {
            get
            {
                return this._Table_Article1.Entity;
            }
            set
            {
                Table_Article previousValue = this._Table_Article1.Entity;
                if (((previousValue != value) 
                     || (this._Table_Article1.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_Article1.Entity = null;
                        previousValue.Table_LookupCategories1.Remove(this);
                    }
                    this._Table_Article1.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupCategories1.Add(this);
                        this._CatId = value.CategoryId;
                    }
                    else
                    {
                        this._CatId = default(int);
                    }
                    this.SendPropertyChanged("Table_Article1");
                }
            }
        }
		
        [Association(Name="Table_OriginalPhotosArchive_Table_LookupCategory", Storage="_Table_OriginalPhotosArchive", ThisKey="CatId", OtherKey="CategoryId", IsForeignKey=true)]
        public Table_OriginalPhotosArchive Table_OriginalPhotosArchive
        {
            get
            {
                return this._Table_OriginalPhotosArchive.Entity;
            }
            set
            {
                Table_OriginalPhotosArchive previousValue = this._Table_OriginalPhotosArchive.Entity;
                if (((previousValue != value) 
                     || (this._Table_OriginalPhotosArchive.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_OriginalPhotosArchive.Entity = null;
                        previousValue.Table_LookupCategories.Remove(this);
                    }
                    this._Table_OriginalPhotosArchive.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupCategories.Add(this);
                        this._CatId = value.CategoryId;
                    }
                    else
                    {
                        this._CatId = default(int);
                    }
                    this.SendPropertyChanged("Table_OriginalPhotosArchive");
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