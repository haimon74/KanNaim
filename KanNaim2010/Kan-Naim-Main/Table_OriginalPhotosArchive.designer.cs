using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_OriginalPhotosArchive")]
    public partial class Table_OriginalPhotosArchive : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _PhotoId;
		
        private System.Data.Linq.Binary _ImageData;
		
        private int _CategoryId;
		
        private string _Caption;
		
        private System.DateTime _Date;
		
        private string _Name;
		
        private string _AlternateText;
		
        private string _Description;
		
        private System.Nullable<int> _Width;
		
        private System.Nullable<int> _Height;
		
        private EntitySet<Table_LookupCategory> _Table_LookupCategories;
		
        private EntityRef<Table_PhotosArchive> _Table_PhotosArchive;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnPhotoIdChanging(int value);
        partial void OnPhotoIdChanged();
        partial void OnImageDataChanging(System.Data.Linq.Binary value);
        partial void OnImageDataChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        partial void OnCaptionChanging(string value);
        partial void OnCaptionChanged();
        partial void OnDateChanging(System.DateTime value);
        partial void OnDateChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnAlternateTextChanging(string value);
        partial void OnAlternateTextChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnWidthChanging(System.Nullable<int> value);
        partial void OnWidthChanged();
        partial void OnHeightChanging(System.Nullable<int> value);
        partial void OnHeightChanged();
        #endregion
		
        public Table_OriginalPhotosArchive()
        {
            this._Table_LookupCategories = new EntitySet<Table_LookupCategory>(new Action<Table_LookupCategory>(this.attach_Table_LookupCategories), new Action<Table_LookupCategory>(this.detach_Table_LookupCategories));
            this._Table_PhotosArchive = default(EntityRef<Table_PhotosArchive>);
            OnCreated();
        }

        [Column(Storage = "_PhotoId", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int PhotoId
        {
            get
            {
                return this._PhotoId;
            }
            set
            {
                if ((this._PhotoId != value))
                {
                    if (this._Table_PhotosArchive.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnPhotoIdChanging(value);
                    this.SendPropertyChanging();
                    this._PhotoId = value;
                    this.SendPropertyChanged("PhotoId");
                    this.OnPhotoIdChanged();
                }
            }
        }
		
        [Column(Storage="_ImageData", DbType="Image NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
        public System.Data.Linq.Binary ImageData
        {
            get
            {
                return this._ImageData;
            }
            set
            {
                if ((this._ImageData != value))
                {
                    this.OnImageDataChanging(value);
                    this.SendPropertyChanging();
                    this._ImageData = value;
                    this.SendPropertyChanged("ImageData");
                    this.OnImageDataChanged();
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
		
        [Column(Storage="_Name", DbType="NVarChar(30) NOT NULL", CanBeNull=false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
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
		
        [Column(Storage="_Description", DbType="NVarChar(150)")]
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
		
        [Association(Name="Table_OriginalPhotosArchive_Table_LookupCategory", Storage="_Table_LookupCategories", ThisKey="CategoryId", OtherKey="CatId")]
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
		
        [Association(Name="Table_PhotosArchive_Table_OriginalPhotosArchive", Storage="_Table_PhotosArchive", ThisKey="PhotoId", OtherKey="OriginalPhotoId", IsForeignKey=true)]
        public Table_PhotosArchive Table_PhotosArchive
        {
            get
            {
                return this._Table_PhotosArchive.Entity;
            }
            set
            {
                Table_PhotosArchive previousValue = this._Table_PhotosArchive.Entity;
                if (((previousValue != value) 
                     || (this._Table_PhotosArchive.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_PhotosArchive.Entity = null;
                        previousValue.Table_OriginalPhotosArchives.Remove(this);
                    }
                    this._Table_PhotosArchive.Entity = value;
                    if ((value != null))
                    {
                        value.Table_OriginalPhotosArchives.Add(this);
                        this._PhotoId = value.OriginalPhotoId;
                    }
                    else
                    {
                        this._PhotoId = default(int);
                    }
                    this.SendPropertyChanged("Table_PhotosArchive");
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
		
        private void attach_Table_LookupCategories(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_OriginalPhotosArchive = this;
        }
		
        private void detach_Table_LookupCategories(Table_LookupCategory entity)
        {
            this.SendPropertyChanging();
            entity.Table_OriginalPhotosArchive = null;
        }
    }
}