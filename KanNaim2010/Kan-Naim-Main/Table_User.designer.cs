using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_Users")]
    public partial class Table_User : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _UserId;
		
        private string _UserName;
		
        private string _FirstName;
		
        private string _LastName;
		
        private int _RoleId;
		
        private string _Password;
		
        private string _Address;
		
        private string _PhoneNumber;
		
        private EntitySet<Table_LookupRole> _Table_LookupRoles;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnUserIdChanging(int value);
        partial void OnUserIdChanged();
        partial void OnUserNameChanging(string value);
        partial void OnUserNameChanged();
        partial void OnFirstNameChanging(string value);
        partial void OnFirstNameChanged();
        partial void OnLastNameChanging(string value);
        partial void OnLastNameChanged();
        partial void OnRoleIdChanging(int value);
        partial void OnRoleIdChanged();
        partial void OnPasswordChanging(string value);
        partial void OnPasswordChanged();
        partial void OnAddressChanging(string value);
        partial void OnAddressChanged();
        partial void OnPhoneNumberChanging(string value);
        partial void OnPhoneNumberChanged();
        #endregion
		
        public Table_User()
        {
            this._Table_LookupRoles = new EntitySet<Table_LookupRole>(new Action<Table_LookupRole>(this.attach_Table_LookupRoles), new Action<Table_LookupRole>(this.detach_Table_LookupRoles));
            OnCreated();
        }
		
        [Column(Storage="_UserId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        public int UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                if ((this._UserId != value))
                {
                    this.OnUserIdChanging(value);
                    this.SendPropertyChanging();
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");
                    this.OnUserIdChanged();
                }
            }
        }
		
        [Column(Storage="_UserName", DbType="NChar(20) NOT NULL", CanBeNull=false)]
        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                if ((this._UserName != value))
                {
                    this.OnUserNameChanging(value);
                    this.SendPropertyChanging();
                    this._UserName = value;
                    this.SendPropertyChanged("UserName");
                    this.OnUserNameChanged();
                }
            }
        }
		
        [Column(Storage="_FirstName", DbType="NChar(20) NOT NULL", CanBeNull=false)]
        public string FirstName
        {
            get
            {
                return this._FirstName;
            }
            set
            {
                if ((this._FirstName != value))
                {
                    this.OnFirstNameChanging(value);
                    this.SendPropertyChanging();
                    this._FirstName = value;
                    this.SendPropertyChanged("FirstName");
                    this.OnFirstNameChanged();
                }
            }
        }
		
        [Column(Storage="_LastName", DbType="NChar(20) NOT NULL", CanBeNull=false)]
        public string LastName
        {
            get
            {
                return this._LastName;
            }
            set
            {
                if ((this._LastName != value))
                {
                    this.OnLastNameChanging(value);
                    this.SendPropertyChanging();
                    this._LastName = value;
                    this.SendPropertyChanged("LastName");
                    this.OnLastNameChanged();
                }
            }
        }
		
        [Column(Storage="_RoleId", DbType="Int NOT NULL")]
        public int RoleId
        {
            get
            {
                return this._RoleId;
            }
            set
            {
                if ((this._RoleId != value))
                {
                    this.OnRoleIdChanging(value);
                    this.SendPropertyChanging();
                    this._RoleId = value;
                    this.SendPropertyChanged("RoleId");
                    this.OnRoleIdChanged();
                }
            }
        }
		
        [Column(Storage="_Password", DbType="NChar(20) NOT NULL", CanBeNull=false)]
        public string Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if ((this._Password != value))
                {
                    this.OnPasswordChanging(value);
                    this.SendPropertyChanging();
                    this._Password = value;
                    this.SendPropertyChanged("Password");
                    this.OnPasswordChanged();
                }
            }
        }
		
        [Column(Storage="_Address", DbType="NChar(30)")]
        public string Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                if ((this._Address != value))
                {
                    this.OnAddressChanging(value);
                    this.SendPropertyChanging();
                    this._Address = value;
                    this.SendPropertyChanged("Address");
                    this.OnAddressChanged();
                }
            }
        }
		
        [Column(Storage="_PhoneNumber", DbType="NChar(15)")]
        public string PhoneNumber
        {
            get
            {
                return this._PhoneNumber;
            }
            set
            {
                if ((this._PhoneNumber != value))
                {
                    this.OnPhoneNumberChanging(value);
                    this.SendPropertyChanging();
                    this._PhoneNumber = value;
                    this.SendPropertyChanged("PhoneNumber");
                    this.OnPhoneNumberChanged();
                }
            }
        }
		
        [Association(Name="Table_User_Table_LookupRole", Storage="_Table_LookupRoles", ThisKey="RoleId", OtherKey="RoleId")]
        public EntitySet<Table_LookupRole> Table_LookupRoles
        {
            get
            {
                return this._Table_LookupRoles;
            }
            set
            {
                this._Table_LookupRoles.Assign(value);
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
		
        private void attach_Table_LookupRoles(Table_LookupRole entity)
        {
            this.SendPropertyChanging();
            entity.Table_User = this;
        }
		
        private void detach_Table_LookupRoles(Table_LookupRole entity)
        {
            this.SendPropertyChanging();
            entity.Table_User = null;
        }
    }
}