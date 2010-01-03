using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupRoles")]
    public partial class Table_LookupRole : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _RoleId;
		
        private string _RoleDescription;
		
        private EntityRef<Table_User> _Table_User;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnRoleIdChanging(int value);
        partial void OnRoleIdChanged();
        partial void OnRoleDescriptionChanging(string value);
        partial void OnRoleDescriptionChanged();
        #endregion
		
        public Table_LookupRole()
        {
            this._Table_User = default(EntityRef<Table_User>);
            OnCreated();
        }
		
        [Column(Storage="_RoleId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
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
                    if (this._Table_User.HasLoadedOrAssignedValue)
                    {
                        throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
                    }
                    this.OnRoleIdChanging(value);
                    this.SendPropertyChanging();
                    this._RoleId = value;
                    this.SendPropertyChanged("RoleId");
                    this.OnRoleIdChanged();
                }
            }
        }
		
        [Column(Storage="_RoleDescription", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string RoleDescription
        {
            get
            {
                return this._RoleDescription;
            }
            set
            {
                if ((this._RoleDescription != value))
                {
                    this.OnRoleDescriptionChanging(value);
                    this.SendPropertyChanging();
                    this._RoleDescription = value;
                    this.SendPropertyChanged("RoleDescription");
                    this.OnRoleDescriptionChanged();
                }
            }
        }
		
        [Association(Name="Table_User_Table_LookupRole", Storage="_Table_User", ThisKey="RoleId", OtherKey="RoleId", IsForeignKey=true)]
        public Table_User Table_User
        {
            get
            {
                return this._Table_User.Entity;
            }
            set
            {
                Table_User previousValue = this._Table_User.Entity;
                if (((previousValue != value) 
                     || (this._Table_User.HasLoadedOrAssignedValue == false)))
                {
                    this.SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        this._Table_User.Entity = null;
                        previousValue.Table_LookupRoles.Remove(this);
                    }
                    this._Table_User.Entity = value;
                    if ((value != null))
                    {
                        value.Table_LookupRoles.Add(this);
                        this._RoleId = value.RoleId;
                    }
                    else
                    {
                        this._RoleId = default(int);
                    }
                    this.SendPropertyChanged("Table_User");
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