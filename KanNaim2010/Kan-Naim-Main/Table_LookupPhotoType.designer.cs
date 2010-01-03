using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupPhotoTypes")]
    public partial class Table_LookupPhotoType : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _PhotoTypeId;
		
        private string _TypeDescription;
		
        private int _Width;
		
        private int _Height;
		
        private int _MaxSize;
		
        private EntityRef<Table_PhotosArchive> _Table_PhotosArchive;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnPhotoTypeIdChanging(int value);
        partial void OnPhotoTypeIdChanged();
        partial void OnTypeDescriptionChanging(string value);
        partial void OnTypeDescriptionChanged();
        partial void OnWidthChanging(int value);
        partial void OnWidthChanged();
        partial void OnHeightChanging(int value);
        partial void OnHeightChanged();
        partial void OnMaxSizeChanging(int value);
        partial void OnMaxSizeChanged();
        #endregion
		
        public Table_LookupPhotoType()
        {
            this._Table_PhotosArchive = default(EntityRef<Table_PhotosArchive>);
            OnCreated();
        }
		
        [Column(Storage="_PhotoTypeId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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
                    if (this._Table_PhotosArchive.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnPhotoTypeIdChanging(value);
                    this.SendPropertyChanging();
                    this._PhotoTypeId = value;
                    this.SendPropertyChanged("PhotoTypeId");
                    this.OnPhotoTypeIdChanged();
                }
            }
        }
		
        [Column(Storage="_TypeDescription", DbType="NChar(20) NOT NULL", CanBeNull=false)]
        public string TypeDescription
        {
            get
            {
                return this._TypeDescription;
            }
            set
            {
                if ((this._TypeDescription != value))
                {
                    this.OnTypeDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._TypeDescription = value;
                    this.SendPropertyChanged("TypeDescription");
                    this.OnTypeDescriptionChanged();
                }
            }
        }
		
        [Column(Storage="_Width", DbType="Int NOT NULL")]
        public int Width
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
		
        [Column(Storage="_Height", DbType="Int NOT NULL")]
        public int Height
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
		
        [Column(Storage="_MaxSize", DbType="Int NOT NULL")]
        public int MaxSize
        {
            get
            {
                return this._MaxSize;
            }
            set
            {
                if ((this._MaxSize != value))
                {
                    this.OnMaxSizeChanging(value);
                    this.SendPropertyChanging();
                    this._MaxSize = value;
                    this.SendPropertyChanged("MaxSize");
                    this.OnMaxSizeChanged();
                }
            }
        }
		
        [Association(Name="Table_PhotosArchive_Table_LookupPhotoType", Storage="_Table_PhotosArchive", ThisKey="PhotoTypeId", OtherKey="PhotoTypeId", IsForeignKey=true)]
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
                        previousValue.Table_LookupPhotoTypes.Remove(this);
                    }
                    this._Table_PhotosArchive.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupPhotoTypes.Add(this);
                        this._PhotoTypeId = value.PhotoTypeId;
                    }
                    else
                    {
                        this._PhotoTypeId = default(int);
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
    }
}