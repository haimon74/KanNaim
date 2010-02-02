using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    [Table(Name="dbo.Table_Taktzirim")]
    public partial class Table_Taktzirim
    {
        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _TakId;
		
        private int _ArticleId;
		
        private string _TakTitle;
		
        private string _TakContent;
		
        private int _TakTypeId;
		
        private int _PhotoId;
		
        private int _EmbedObjId;
		
        private int _ScheduleId;

        private string _Categories;
		
        public Table_Taktzirim()
        {
        }
		
        [Column(Storage="_TakId", DbType="Int NOT NULL")]
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
                    this._TakId = value;
                }
            }
        }
		
        [Column(Storage="_ArticleId", DbType="Int NOT NULL")]
        public int ArticleId
        {
            get
            {
                return this._ArticleId;
            }
            set
            {
                if ((this._ArticleId != value))
                {
                    this._ArticleId = value;
                }
            }
        }
		
        [Column(Storage="_TakTitle", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
        public string TakTitle
        {
            get
            {
                return this._TakTitle;
            }
            set
            {
                if ((this._TakTitle != value))
                {
                    this._TakTitle = value;
                }
            }
        }
		
        [Column(Storage="_TakContent", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
        public string TakContent
        {
            get
            {
                return this._TakContent;
            }
            set
            {
                if ((this._TakContent != value))
                {
                    this._TakContent = value;
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
                    this._TakTypeId = value;
                }
            }
        }
		
        [Column(Storage="_PhotoId", DbType="Int NOT NULL")]
        public int PhotoId
        {
            get
            {
                return this._PhotoId;
            }
            set
            {
                if ((this._PhotoId != value))
                {
                    this._PhotoId = value;
                }
            }
        }
		
        [Column(Storage="_EmbedObjId", DbType="Int NOT NULL")]
        public int EmbedObjId
        {
            get
            {
                return this._EmbedObjId;
            }
            set
            {
                if ((this._EmbedObjId != value))
                {
                    this._EmbedObjId = value;
                }
            }
        }
		
        [Column(Storage="_ScheduleId", DbType="Int NOT NULL")]
        public int ScheduleId
        {
            get
            {
                return this._ScheduleId;
            }
            set
            {
                if ((this._ScheduleId != value))
                {
                    this._ScheduleId = value;
                }
            }
        }

        [Column(Storage = "_Categories", DbType = "NVarChar(200)")]
        public string Categories
        {
            get
            {
                return this._Categories;
            }
            set
            {
                if ((this._Categories != value))
                {
                    this.OnCategoriesChanging(value);
                    this.SendPropertyChanging();
                    this._Categories = value;
                    this.SendPropertyChanged("Categories");
                    this.OnCategoriesChanged();
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
        partial void OnCategoriesChanging(string value);
        partial void OnCategoriesChanged();
    }
}