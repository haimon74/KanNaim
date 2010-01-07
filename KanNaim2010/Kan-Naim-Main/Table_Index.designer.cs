using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_Indexes")]
    public partial class Table_Index : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _Id;
		
        private string _BusinessName;
		
        private string _HomePageUrl;
		
        private string _MobilePhone;
		
        private string _OtherPhone;
		
        private string _Fax;
		
        private string _Email;
		
        private string _Description;
		
        private int _IndexType;
		
        private int _CategoryId;
		
        private EntitySet<Table_LookupIndex> _Table_LookupIndexes;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnIdChanging(int value);
        partial void OnIdChanged();
        partial void OnBusinessNameChanging(string value);
        partial void OnBusinessNameChanged();
        partial void OnHomePageUrlChanging(string value);
        partial void OnHomePageUrlChanged();
        partial void OnMobilePhoneChanging(string value);
        partial void OnMobilePhoneChanged();
        partial void OnOtherPhoneChanging(string value);
        partial void OnOtherPhoneChanged();
        partial void OnFaxChanging(string value);
        partial void OnFaxChanged();
        partial void OnEmailChanging(string value);
        partial void OnEmailChanged();
        partial void OnDescriptionChanging(string value);
        partial void OnDescriptionChanged();
        partial void OnIndexTypeChanging(int value);
        partial void OnIndexTypeChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        #endregion
		
        public Table_Index()
        {
            this._Table_LookupIndexes = new EntitySet<Table_LookupIndex>(new Action<Table_LookupIndex>(this.attach_Table_LookupIndexes), new Action<Table_LookupIndex>(this.detach_Table_LookupIndexes));
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
                    this.OnIdChanging(value);
                    this.SendPropertyChanging();
                    this._Id = value;
                    this.SendPropertyChanged("Id");
                    this.OnIdChanged();
                }
            }
        }
		
        [Column(Storage="_BusinessName", DbType="NChar(50) NOT NULL", CanBeNull=false)]
        public string BusinessName
        {
            get
            {
                return this._BusinessName;
            }
            set
            {
                if ((this._BusinessName != value))
                {
                    this.OnBusinessNameChanging(value);
                    this.SendPropertyChanging();
                    this._BusinessName = value;
                    this.SendPropertyChanged("BusinessName");
                    this.OnBusinessNameChanged();
                }
            }
        }
		
        [Column(Storage="_HomePageUrl", DbType="NChar(200) NOT NULL", CanBeNull=false)]
        public string HomePageUrl
        {
            get
            {
                return this._HomePageUrl;
            }
            set
            {
                if ((this._HomePageUrl != value))
                {
                    this.OnHomePageUrlChanging(value);
                    this.SendPropertyChanging();
                    this._HomePageUrl = value;
                    this.SendPropertyChanged("HomePageUrl");
                    this.OnHomePageUrlChanged();
                }
            }
        }
		
        [Column(Storage="_MobilePhone", DbType="NChar(15) NOT NULL", CanBeNull=false)]
        public string MobilePhone
        {
            get
            {
                return this._MobilePhone;
            }
            set
            {
                if ((this._MobilePhone != value))
                {
                    this.OnMobilePhoneChanging(value);
                    this.SendPropertyChanging();
                    this._MobilePhone = value;
                    this.SendPropertyChanged("MobilePhone");
                    this.OnMobilePhoneChanged();
                }
            }
        }
		
        [Column(Storage="_OtherPhone", DbType="NChar(15)")]
        public string OtherPhone
        {
            get
            {
                return this._OtherPhone;
            }
            set
            {
                if ((this._OtherPhone != value))
                {
                    this.OnOtherPhoneChanging(value);
                    this.SendPropertyChanging();
                    this._OtherPhone = value;
                    this.SendPropertyChanged("OtherPhone");
                    this.OnOtherPhoneChanged();
                }
            }
        }
		
        [Column(Storage="_Fax", DbType="NChar(15)")]
        public string Fax
        {
            get
            {
                return this._Fax;
            }
            set
            {
                if ((this._Fax != value))
                {
                    this.OnFaxChanging(value);
                    this.SendPropertyChanging();
                    this._Fax = value;
                    this.SendPropertyChanged("Fax");
                    this.OnFaxChanged();
                }
            }
        }
		
        [Column(Storage="_Email", DbType="NChar(30) NOT NULL", CanBeNull=false)]
        public string Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                if ((this._Email != value))
                {
                    this.OnEmailChanging(value);
                    this.SendPropertyChanging();
                    this._Email = value;
                    this.SendPropertyChanged("Email");
                    this.OnEmailChanged();
                }
            }
        }
		
        [Column(Storage="_Description", DbType="NChar(500)")]
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
		
        [Column(Storage="_IndexType", DbType="Int NOT NULL")]
        public int IndexType
        {
            get
            {
                return this._IndexType;
            }
            set
            {
                if ((this._IndexType != value))
                {
                    this.OnIndexTypeChanging(value);
                    this.SendPropertyChanging();
                    this._IndexType = value;
                    this.SendPropertyChanged("IndexType");
                    this.OnIndexTypeChanged();
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
		
        [Association(Name="Table_Index_Table_LookupIndex", Storage="_Table_LookupIndexes", ThisKey="CategoryId", OtherKey="Id")]
        public EntitySet<Table_LookupIndex> Table_LookupIndexes
        {
            get
            {
                return this._Table_LookupIndexes;
            }
            set
            {
                this._Table_LookupIndexes.Assign(value);
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
		
        private void attach_Table_LookupIndexes(Table_LookupIndex entity)
        {
            this.SendPropertyChanging();
            entity.Table_Index = this;
        }
		
        private void detach_Table_LookupIndexes(Table_LookupIndex entity)
        {
            this.SendPropertyChanging();
            entity.Table_Index = null;
        }
    }
}