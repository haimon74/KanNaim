using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class Select
    {
        private static readonly IndexItDataClassesDataContext _db = Linq2Sql.Db;

        public static IQueryable<Table_Menus> IqTableMenuByMenuId(int menuId)
        {
            return from c in _db.Table_Menus
                   where c.MenuId == menuId
                   select c;
        }

        public static IQueryable<Table_Menus> IqTableMenuByItemId(int itemId)
        {
            return from c in _db.Table_Menus
                   where c.ItemId == itemId
                   select c;
        }

        public static Table_Menus TableMenuByItemId(int itemId)
        {
            return IqTableMenuByItemId(itemId).SingleOrDefault();
        }

        public static Table_Menus TableMenuByRootName(string rootName)
        {
            return (from c in _db.Table_Menus
                   where c.DisplayText == rootName && c.ParentId == -1
                   select c).SingleOrDefault();
        }

        public static IQueryable<Table_LookupMenus> AllTableLookupMenu()
        {
            return from c in _db.Table_LookupMenus
                   select c;
        }

        public static Table_LookupMenus TableLookupMenuById(int id)
        {
            return (IqTableLookupMenuById(id)).SingleOrDefault();
        }
        public static IQueryable<Table_LookupMenus> IqTableLookupMenuById(int id)
        {
            return from c in _db.Table_LookupMenus
                   where c.MenuId == id
                   select c;
        }
        public static Table_LookupMenus TableLookupMenuByName(string name)
        {
            return (from c in _db.Table_LookupMenus
                    where c.MenuDescription == name
                    select c).SingleOrDefault();
        }

        public static IQueryable<Table_Menus> IqTableMenus()
        {
             return from c in _db.Table_Menus
                    select c;
        }
    }
}
