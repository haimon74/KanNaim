using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class Delete
    {
        private static readonly IndexItDataClassesDataContext _db = Linq2Sql.Db;

        private static bool TableMenus(IEnumerable<Table_Menus> enumerable)
        {
            try
            {
                _db.Table_Menus.DeleteAllOnSubmit(enumerable);
                _db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TableMenusByMenuId(int menuId)
        {
            IQueryable<Table_Menus> menus =  Select.IqTableMenuByMenuId(menuId);

            IEnumerable<Table_Menus> enumerable = menus.AsEnumerable();

            return TableMenus(enumerable);
        }
        public static bool TableMenusByItemId(int itemId)
        {
            IQueryable<Table_Menus> menus = Select.IqTableMenuByItemId(itemId);

            IEnumerable<Table_Menus> enumerable = menus.AsEnumerable();

            return TableMenus(enumerable);
        }

        private static bool TableLookupMenus(IEnumerable<Table_LookupMenus> enumerable)
        {
            try
            {
                _db.Table_LookupMenus.DeleteAllOnSubmit(enumerable);
                _db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TableLookupMenusByMenuId(int menuId)
        {
            IQueryable<Table_LookupMenus> menus = Select.IqTableLookupMenuById(menuId);

            IEnumerable<Table_LookupMenus> enumerable = menus.AsEnumerable();

            return TableLookupMenus(enumerable);
        }
    }
}
