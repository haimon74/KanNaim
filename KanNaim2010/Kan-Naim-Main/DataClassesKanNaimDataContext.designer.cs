using System.Data.Linq.Mapping;

namespace Kan_Naim_Main
{
    

    [System.Data.Linq.Mapping.DatabaseAttribute(Name="Kan-Naim")]
    public partial class DataClassesKanNaimDataContext : System.Data.Linq.DataContext
    {
		
        private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
        #region Extensibility Method Definitions
        partial void OnCreated();
        partial void InsertTable_LookupArticleStatus(Table_LookupArticleStatus instance);
        partial void UpdateTable_LookupArticleStatus(Table_LookupArticleStatus instance);
        partial void DeleteTable_LookupArticleStatus(Table_LookupArticleStatus instance);
        partial void InsertTable_LookupCategory(Table_LookupCategory instance);
        partial void UpdateTable_LookupCategory(Table_LookupCategory instance);
        partial void DeleteTable_LookupCategory(Table_LookupCategory instance);
        partial void InsertTable_LookupPhotoType(Table_LookupPhotoType instance);
        partial void UpdateTable_LookupPhotoType(Table_LookupPhotoType instance);
        partial void DeleteTable_LookupPhotoType(Table_LookupPhotoType instance);
        partial void InsertTable_VideosArchive(Table_VideosArchive instance);
        partial void UpdateTable_VideosArchive(Table_VideosArchive instance);
        partial void DeleteTable_VideosArchive(Table_VideosArchive instance);
        partial void InsertTable_PhotosArchive(Table_PhotosArchive instance);
        partial void UpdateTable_PhotosArchive(Table_PhotosArchive instance);
        partial void DeleteTable_PhotosArchive(Table_PhotosArchive instance);
        partial void InsertTable_Article(Table_Article instance);
        partial void UpdateTable_Article(Table_Article instance);
        partial void DeleteTable_Article(Table_Article instance);
        partial void InsertTable_OriginalPhotosArchive(Table_OriginalPhotosArchive instance);
        partial void UpdateTable_OriginalPhotosArchive(Table_OriginalPhotosArchive instance);
        partial void DeleteTable_OriginalPhotosArchive(Table_OriginalPhotosArchive instance);
        partial void InsertTable_LookupRole(Table_LookupRole instance);
        partial void UpdateTable_LookupRole(Table_LookupRole instance);
        partial void DeleteTable_LookupRole(Table_LookupRole instance);
        partial void InsertTable_User(Table_User instance);
        partial void UpdateTable_User(Table_User instance);
        partial void DeleteTable_User(Table_User instance);
        #endregion
		
        public DataClassesKanNaimDataContext() : 
            base(global::Kan_Naim_Main.Properties.Settings.Default._10infoConnectionString, mappingSource)
        {
            OnCreated();
        }
		
        public DataClassesKanNaimDataContext(string connection) : 
            base(connection, mappingSource)
        {
            OnCreated();
        }
		
        public DataClassesKanNaimDataContext(System.Data.IDbConnection connection) : 
            base(connection, mappingSource)
        {
            OnCreated();
        }
		
        public DataClassesKanNaimDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
            base(connection, mappingSource)
        {
            OnCreated();
        }
		
        public DataClassesKanNaimDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
            base(connection, mappingSource)
        {
            OnCreated();
        }
		
        public System.Data.Linq.Table<Table_LookupArticleStatus> Table_LookupArticleStatus
        {
            get
            {
                return this.GetTable<Table_LookupArticleStatus>();
            }
        }
		
        public System.Data.Linq.Table<Table_LookupCategory> Table_LookupCategories
        {
            get
            {
                return this.GetTable<Table_LookupCategory>();
            }
        }
		
        public System.Data.Linq.Table<Table_LookupPhotoType> Table_LookupPhotoTypes
        {
            get
            {
                return this.GetTable<Table_LookupPhotoType>();
            }
        }
		
        public System.Data.Linq.Table<Table_VideosArchive> Table_VideosArchives
        {
            get
            {
                return this.GetTable<Table_VideosArchive>();
            }
        }
		
        public System.Data.Linq.Table<Table_PhotosArchive> Table_PhotosArchives
        {
            get
            {
                return this.GetTable<Table_PhotosArchive>();
            }
        }
		
        public System.Data.Linq.Table<Table_Article> Table_Articles
        {
            get
            {
                return this.GetTable<Table_Article>();
            }
        }
		
        public System.Data.Linq.Table<Table_Taktzirim> Table_Taktzirims
        {
            get
            {
                return this.GetTable<Table_Taktzirim>();
            }
        }
		
        public System.Data.Linq.Table<Table_LookupReporter> Table_LookupReporters
        {
            get
            {
                return this.GetTable<Table_LookupReporter>();
            }
        }
		
        public System.Data.Linq.Table<Table_OriginalPhotosArchive> Table_OriginalPhotosArchives
        {
            get
            {
                return this.GetTable<Table_OriginalPhotosArchive>();
            }
        }
		
        public System.Data.Linq.Table<Table_LookupRole> Table_LookupRoles
        {
            get
            {
                return this.GetTable<Table_LookupRole>();
            }
        }
		
        public System.Data.Linq.Table<Table_User> Table_Users
        {
            get
            {
                return this.GetTable<Table_User>();
            }
        }

        public System.Data.Linq.Table<Table_LinksPrefered> Table_LinksPrefereds
		{
			get
			{
				return this.GetTable<Table_LinksPrefered>();
			}
		}
		
		public System.Data.Linq.Table<Table_LinksPageBottom> Table_LinksPageBottoms
		{
			get
			{
				return this.GetTable<Table_LinksPageBottom>();
			}
		}
		
		public System.Data.Linq.Table<Table_MenuRightSide> Table_MenuRightSides
		{
			get
			{
				return this.GetTable<Table_MenuRightSide>();
			}
		}
		
		public System.Data.Linq.Table<Table_MenuTop> Table_MenuTops
		{
			get
			{
				return this.GetTable<Table_MenuTop>();
			}
		}
        public System.Data.Linq.Table<Table_LookupIndex> Table_LookupIndexes
        {
            get
            {
                return this.GetTable<Table_LookupIndex>();
            }
        }

        public System.Data.Linq.Table<Table_Index> Table_Indexes
        {
            get
            {
                return this.GetTable<Table_Index>();
            }
        }
	}
}