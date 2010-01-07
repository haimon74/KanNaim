using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupIndexes")]
    public partial class Table_LookupIndex : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _Id;
		
        private string _IndexName;
		
        private int _GroupNameId;
		
        private EntityRef<Table_Index> _Table_Index;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnIndexNameChanging(string value);
        partial void OnIndexNameChanged();
        partial void OnGroupNameIdChanging(int value);
        partial void OnGroupNameIdChanged();
        #endregion
		
        public Table_LookupIndex()
        {
            this._Table_Index = default(EntityRef<Table_Index>);
            OnCreated();
        }
		
        [Column(Storage="_Id", DbType="Int NOT NULL", IsPrimaryKey=true)]
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
                    if (this._Table_Index.HasLoadedOrAssignedValue)
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
		
        [Column(Storage="_IndexName", DbType="NChar(30) NOT NULL", CanBeNull=false)]
        public string IndexName
        {
            get
            {
                return this._IndexName;
            }
            set
            {
                if ((this._IndexName != value))
                {
                    this.OnIndexNameChanging(value);
                    this.SendPropertyChanging();
                    this._IndexName = value;
                    this.SendPropertyChanged("IndexName");
                    this.OnIndexNameChanged();
                }
            }
        }
		
        [Column(Storage="_GroupNameId", DbType="Int NOT NULL")]
        public int GroupNameId
        {
            get
            {
                return this._GroupNameId;
            }
            set
            {
                if ((this._GroupNameId != value))
                {
                    this.OnGroupNameIdChanging(value);
                    this.SendPropertyChanging();
                    this._GroupNameId = value;
                    this.SendPropertyChanged("GroupNameId");
                    this.OnGroupNameIdChanged();
                }
            }
        }
		
        [Association(Name="Table_Index_Table_LookupIndex", Storage="_Table_Index", ThisKey="Id", OtherKey="CategoryId", IsForeignKey=true)]
        public Table_Index Table_Index
        {
            get
            {
                return this._Table_Index.Entity;
            }
            set
            {
                Table_Index previousValue = this._Table_Index.Entity;
                if (((previousValue != value) 
                     || (this._Table_Index.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_Index.Entity = null;
                        previousValue.Table_LookupIndexes.Remove(this);
                    }
                    this._Table_Index.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupIndexes.Add(this);
                        this._Id = value.CategoryId;
                    }
                    else
                    {
                        this._Id = default(int);
                    }
                    this.SendPropertyChanged("Table_Index");
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