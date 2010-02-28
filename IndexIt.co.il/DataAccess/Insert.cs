using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class Insert
    {
        private static readonly IndexItDataClassesDataContext _db = Linq2Sql.Db;

        public static Table_LookupMenus TableLookupMenu(Table_LookupMenus menu)
        {
            _db.Table_LookupMenus.InsertOnSubmit(menu);
            _db.SubmitChanges();
            return Select.TableLookupMenuByName(menu.MenuDescription);
        }

        public static Table_Menus TableManus(Table_Menus node)
        {
            _db.Table_Menus.InsertOnSubmit(node);
            _db.SubmitChanges();
            return Select.TableMenuByRootName(node.DisplayText);
        }
    }
}
