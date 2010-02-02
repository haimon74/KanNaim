using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_Broadcast")]
    public partial class Table_Broadcast : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private System.DateTime _StartDateTime;
		
        private bool _isRecursive;
		
        private bool _isDaily;
		
        private bool _isWeekly;
		
        private bool _isMonthly;
		
        private bool _isYearly;
		
        private System.DateTime _EndDateTime;
		
        private int _TakId;
		
        private int _TakTypeId;
		
        private int _CategoryId;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnStartDateTimeChanging(System.DateTime value);
        partial void OnStartDateTimeChanged();
        partial void OnisRecursiveChanging(bool value);
        partial void OnisRecursiveChanged();
        partial void OnisDailyChanging(bool value);
        partial void OnisDailyChanged();
        partial void OnisWeeklyChanging(bool value);
        partial void OnisWeeklyChanged();
        partial void OnisMonthlyChanging(bool value);
        partial void OnisMonthlyChanged();
        partial void OnisYearlyChanging(bool value);
        partial void OnisYearlyChanged();
        partial void OnEndDateTimeChanging(System.DateTime value);
        partial void OnEndDateTimeChanged();
        partial void OnTakIdChanging(int value);
        partial void OnTakIdChanged();
        partial void OnTakTypeIdChanging(int value);
        partial void OnTakTypeIdChanged();
        partial void OnCategoryIdChanging(int value);
        partial void OnCategoryIdChanged();
        #endregion
		
        public Table_Broadcast()
        {
            OnCreated();
        }
		
        [Column(Storage="_StartDateTime", DbType="DateTime NOT NULL")]
        public System.DateTime StartDateTime
        {
            get
            {
                return this._StartDateTime;
            }
            set
            {
                if ((this._StartDateTime != value))
                {
                    this.OnStartDateTimeChanging(value);
                    this.SendPropertyChanging();
                    this._StartDateTime = value;
                    this.SendPropertyChanged("StartDateTime");
                    this.OnStartDateTimeChanged();
                }
            }
        }
		
        [Column(Storage="_isRecursive", DbType="Bit NOT NULL")]
        public bool isRecursive
        {
            get
            {
                return this._isRecursive;
            }
            set
            {
                if ((this._isRecursive != value))
                {
                    this.OnisRecursiveChanging(value);
                    this.SendPropertyChanging();
                    this._isRecursive = value;
                    this.SendPropertyChanged("isRecursive");
                    this.OnisRecursiveChanged();
                }
            }
        }
		
        [Column(Storage="_isDaily", DbType="Bit NOT NULL")]
        public bool isDaily
        {
            get
            {
                return this._isDaily;
            }
            set
            {
                if ((this._isDaily != value))
                {
                    this.OnisDailyChanging(value);
                    this.SendPropertyChanging();
                    this._isDaily = value;
                    this.SendPropertyChanged("isDaily");
                    this.OnisDailyChanged();
                }
            }
        }
		
        [Column(Storage="_isWeekly", DbType="Bit NOT NULL")]
        public bool isWeekly
        {
            get
            {
                return this._isWeekly;
            }
            set
            {
                if ((this._isWeekly != value))
                {
                    this.OnisWeeklyChanging(value);
                    this.SendPropertyChanging();
                    this._isWeekly = value;
                    this.SendPropertyChanged("isWeekly");
                    this.OnisWeeklyChanged();
                }
            }
        }
		
        [Column(Storage="_isMonthly", DbType="Bit NOT NULL")]
        public bool isMonthly
        {
            get
            {
                return this._isMonthly;
            }
            set
            {
                if ((this._isMonthly != value))
                {
                    this.OnisMonthlyChanging(value);
                    this.SendPropertyChanging();
                    this._isMonthly = value;
                    this.SendPropertyChanged("isMonthly");
                    this.OnisMonthlyChanged();
                }
            }
        }
		
        [Column(Storage="_isYearly", DbType="Bit NOT NULL")]
        public bool isYearly
        {
            get
            {
                return this._isYearly;
            }
            set
            {
                if ((this._isYearly != value))
                {
                    this.OnisYearlyChanging(value);
                    this.SendPropertyChanging();
                    this._isYearly = value;
                    this.SendPropertyChanged("isYearly");
                    this.OnisYearlyChanged();
                }
            }
        }
		
        [Column(Storage="_EndDateTime", DbType="DateTime NOT NULL")]
        public System.DateTime EndDateTime
        {
            get
            {
                return this._EndDateTime;
            }
            set
            {
                if ((this._EndDateTime != value))
                {
                    this.OnEndDateTimeChanging(value);
                    this.SendPropertyChanging();
                    this._EndDateTime = value;
                    this.SendPropertyChanged("EndDateTime");
                    this.OnEndDateTimeChanged();
                }
            }
        }
		
        [Column(Storage="_TakId", DbType="Int NOT NULL", IsPrimaryKey=true)]
        public int TakId
        {
            get
            {
                return this._TakId;
            }
            set
            {
                if ((this._TakId != value))
                {
                    this.OnTakIdChanging(value);
                    this.SendPropertyChanging();
                    this._TakId = value;
                    this.SendPropertyChanged("TakId");
                    this.OnTakIdChanged();
                }
            }
        }
		
        [Column(Storage="_TakTypeId", DbType="Int NOT NULL")]
        public int TakTypeId
        {
            get
            {
                return this._TakTypeId;
            }
            set
            {
                if ((this._TakTypeId != value))
                {
                    this.OnTakTypeIdChanging(value);
                    this.SendPropertyChanging();
                    this._TakTypeId = value;
                    this.SendPropertyChanged("TakTypeId");
                    this.OnTakTypeIdChanged();
                }
            }
        }
		
        [Column(Storage="_CategoryId", DbType="Int NOT NULL", IsPrimaryKey=true)]
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