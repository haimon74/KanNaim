using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_LinksPrefered")]
    public partial class Table_LinksPrefered : INotifyPropertyChanging, INotifyPropertyChanged
    {
		
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
        private int _LinkId;
		
        private System.Nullable<int> _PhotoId;
		
        private System.Nullable<int> _ArticleId;
		
        private string _Url;
		
        private System.Nullable<int> _OrderPlace;
		
        private string _AltText;
		
        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnLinkIdChanging(int value);
        partial void OnLinkIdChanged();
        partial void OnPhotoIdChanging(System.Nullable<int> value);
        partial void OnPhotoIdChanged();
        partial void OnArticleIdChanging(System.Nullable<int> value);
        partial void OnArticleIdChanged();
        partial void OnUrlChanging(string value);
        partial void OnUrlChanged();
        partial void OnOrderPlaceChanging(System.Nullable<int> value);
        partial void OnOrderPlaceChanged();
        partial void OnAltTextChanging(string value);
        partial void OnAltTextChanged();
        #endregion
		
        public Table_LinksPrefered()
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
		
        [Column(Storage="_PhotoId", DbType="Int")]
        public System.Nullable<int> PhotoId
        {
            get
            {
                return this._PhotoId;
            }
            set
            {
                if ((this._PhotoId != value))
                {
                    this.OnPhotoIdChanging(value);
                    this.SendPropertyChanging();
                    this._PhotoId = value;
                    this.SendPropertyChanged("PhotoId");
                    this.OnPhotoIdChanged();
                }
            }
        }
		
        [Column(Storage="_ArticleId", DbType="Int")]
        public System.Nullable<int> ArticleId
        {
            get
            {
                return this._ArticleId;
            }
            set
            {
                if ((this._ArticleId != value))
                {
                    this.OnArticleIdChanging(value);
                    this.SendPropertyChanging();
                    this._ArticleId = value;
                    this.SendPropertyChanged("ArticleId");
                    this.OnArticleIdChanged();
                }
            }
        }
		
        [Column(Storage="_Url", DbType="NChar(150)")]
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
		
        [Column(Storage="_AltText", DbType="NChar(100) NOT NULL", CanBeNull=false)]
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