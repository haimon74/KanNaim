using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LookupReporters")]
    public partial class Table_LookupReporter
    {
		
        private int _UserId;
		
        private string _PublishNameShort;
		
        private string _PublishNameLong;
		
        private string _FirstName;
		
        private string _LastName;
		
        private string _MobilePhone;
		
        private string _OtherPhone;
		
        private string _Fax;
		
        private string _Address;
		
        private string _Email;
		
        public Table_LookupReporter()
        {
        }
		
        [Column(Storage="_UserId", DbType="Int NOT NULL")]
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
                    this._UserId = value;
                }
            }
        }
		
        [Column(Storage="_PublishNameShort", DbType="NVarChar(15) NOT NULL", CanBeNull=false)]
        public string PublishNameShort
        {
            get
            {
                return this._PublishNameShort;
            }
            set
            {
                if ((this._PublishNameShort != value))
                {
                    this._PublishNameShort = value;
                }
            }
        }
		
        [Column(Storage="_PublishNameLong", DbType="NVarChar(30)")]
        public string PublishNameLong
        {
            get
            {
                return this._PublishNameLong;
            }
            set
            {
                if ((this._PublishNameLong != value))
                {
                    this._PublishNameLong = value;
                }
            }
        }
		
        [Column(Storage="_FirstName", DbType="NVarChar(15)")]
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
                    this._FirstName = value;
                }
            }
        }
		
        [Column(Storage="_LastName", DbType="NVarChar(15)")]
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
                    this._LastName = value;
                }
            }
        }
		
        [Column(Storage="_MobilePhone", DbType="NChar(15)")]
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
                    this._MobilePhone = value;
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
                    this._OtherPhone = value;
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
                    this._Fax = value;
                }
            }
        }
		
        [Column(Storage="_Address", DbType="NVarChar(50)")]
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
                    this._Address = value;
                }
            }
        }
		
        [Column(Storage="_Email", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
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
                    this._Email = value;
                }
            }
        }
    }
}