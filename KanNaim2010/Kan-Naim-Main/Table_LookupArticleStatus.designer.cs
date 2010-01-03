using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupArticleStatus")]
    public partial class Table_LookupArticleStatus : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _StatusId;
		
        private string _StatusDescription;
		
        private EntityRef<Table_Article> _Table_Article;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnStatusIdChanging(int value);
        partial void OnStatusIdChanged();
        partial void OnStatusDescriptionChanging(string value);
        partial void OnStatusDescriptionChanged();
        #endregion
		
        public Table_LookupArticleStatus()
        {
            this._Table_Article = default(EntityRef<Table_Article>);
            OnCreated();
        }
		
        [Column(Storage="_StatusId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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
                    if (this._Table_Article.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnStatusIdChanging(value);
                    this.SendPropertyChanging();
                    this._StatusId = value;
                    this.SendPropertyChanged("StatusId");
                    this.OnStatusIdChanged();
                }
            }
        }
		
        [Column(Storage="_StatusDescription", DbType="NVarChar(20) NOT NULL", CanBeNull=false)]
        public string StatusDescription
        {
            get
            {
                return this._StatusDescription;
            }
            set
            {
                if ((this._StatusDescription != value))
                {
                    this.OnStatusDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._StatusDescription = value;
                    this.SendPropertyChanged("StatusDescription");
                    this.OnStatusDescriptionChanged();
                }
            }
        }
		
        [Association(Name="Table_Article_Table_LookupArticleStatus", Storage="_Table_Article", ThisKey="StatusId", OtherKey="StatusId", IsForeignKey=true)]
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
                        previousValue.Table_LookupArticleStatus.Remove(this);
                    }
                    this._Table_Article.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupArticleStatus.Add(this);
                        this._StatusId = value.StatusId;
                    }
                    else
                    {
                        this._StatusId = default(int);
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