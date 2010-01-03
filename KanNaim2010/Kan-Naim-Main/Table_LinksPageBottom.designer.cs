using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LinksPageBottom")]
    public partial class Table_LinksPageBottom : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _LinkId;
		
        private string _DispleyText;
		
        private bool _FlagVisible;
		
        private string _AltText;
		
        private string _Url;
		
        private System.Nullable<int> _BottomUrlCatId;
		
        private System.Nullable<int> _OrderPlace;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnLinkIdChanging(int value);
        partial void OnLinkIdChanged();
        partial void OnDispleyTextChanging(string value);
        partial void OnDispleyTextChanged();
        partial void OnFlagVisibleChanging(bool value);
        partial void OnFlagVisibleChanged();
        partial void OnAltTextChanging(string value);
        partial void OnAltTextChanged();
        partial void OnUrlChanging(string value);
        partial void OnUrlChanged();
        partial void OnBottomUrlCatIdChanging(System.Nullable<int> value);
        partial void OnBottomUrlCatIdChanged();
        partial void OnOrderPlaceChanging(System.Nullable<int> value);
        partial void OnOrderPlaceChanged();
        #endregion
		
        public Table_LinksPageBottom()
        {
            OnCreated();
        }
		
        [Column(Storage="_LinkId", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
        public int LinkId
        {
            get
            {
                return this._LinkId;
            }
            set
            {
                if ((this._LinkId != value))
                {
                    this.OnLinkIdChanging(value);
                    this.SendPropertyChanging();
                    this._LinkId = value;
                    this.SendPropertyChanged("LinkId");
                    this.OnLinkIdChanged();
                }
            }
        }
		
        [Column(Storage="_DispleyText", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string DispleyText
        {
            get
            {
                return this._DispleyText;
            }
            set
            {
                if ((this._DispleyText != value))
                {
                    this.OnDispleyTextChanging(value);
                    this.SendPropertyChanging();
                    this._DispleyText = value;
                    this.SendPropertyChanged("DispleyText");
                    this.OnDispleyTextChanged();
                }
            }
        }
		
        [Column(Storage="_FlagVisible", DbType="Bit NOT NULL")]
        public bool FlagVisible
        {
            get
            {
                return this._FlagVisible;
            }
            set
            {
                if ((this._FlagVisible != value))
                {
                    this.OnFlagVisibleChanging(value);
                    this.SendPropertyChanging();
                    this._FlagVisible = value;
                    this.SendPropertyChanged("FlagVisible");
                    this.OnFlagVisibleChanged();
                }
            }
        }
		
        [Column(Storage="_AltText", DbType="NVarChar(100)")]
        public string AltText
        {
            get
            {
                return this._AltText;
            }
            set
            {
                if ((this._AltText != value))
                {
                    this.OnAltTextChanging(value);
                    this.SendPropertyChanging();
                    this._AltText = value;
                    this.SendPropertyChanged("AltText");
                    this.OnAltTextChanged();
                }
            }
        }
		
        [Column(Storage="_Url", DbType="NChar(100)")]
        public string Url
        {
            get
            {
                return this._Url;
            }
            set
            {
                if ((this._Url != value))
                {
                    this.OnUrlChanging(value);
                    this.SendPropertyChanging();
                    this._Url = value;
                    this.SendPropertyChanged("Url");
                    this.OnUrlChanged();
                }
            }
        }
		
        [Column(Storage="_BottomUrlCatId", DbType="Int")]
        public System.Nullable<int> BottomUrlCatId
        {
            get
            {
                return this._BottomUrlCatId;
            }
            set
            {
                if ((this._BottomUrlCatId != value))
                {
                    this.OnBottomUrlCatIdChanging(value);
                    this.SendPropertyChanging();
                    this._BottomUrlCatId = value;
                    this.SendPropertyChanged("BottomUrlCatId");
                    this.OnBottomUrlCatIdChanged();
                }
            }
        }
		
        [Column(Storage="_OrderPlace", DbType="Int")]
        public System.Nullable<int> OrderPlace
        {
            get
            {
                return this._OrderPlace;
            }
            set
            {
                if ((this._OrderPlace != value))
                {
                    this.OnOrderPlaceChanging(value);
                    this.SendPropertyChanging();
                    this._OrderPlace = value;
                    this.SendPropertyChanged("OrderPlace");
                    this.OnOrderPlaceChanged();
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