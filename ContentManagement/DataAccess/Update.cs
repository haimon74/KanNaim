using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess
{
    public class Update
    {
        private static readonly IndexItDataClassesDataContext _db = Linq2Sql.Db;

        public static bool TableMenus(int key, Dictionary<string,object> args)
        {
            try
            {
                Table_Menus rec = Select.TableMenuByItemId(key);
                rec.ParentId = (int) (args.ContainsKey("ParentId") ? args["ParentId"] : rec.ParentId);
                rec.OnClickUrl = (string)(args.ContainsKey("OnClickUrl") ? args["OnClickUrl"] : rec.OnClickUrl);
                rec.MenuId = (int) (args.ContainsKey("MenuId") ? args["MenuId"] : rec.MenuId);
                rec.DisplayText = (string) (args.ContainsKey("DisplayText") ? args["DisplayText"] : rec.DisplayText);
                rec.CssClassName = (string)(args.ContainsKey("CssClassName") ? args["CssClassName"] : rec.CssClassName);
                rec.ToolTip = (string) (args.ContainsKey("ToolTip") ? args["ToolTip"] : rec.ToolTip);
                _db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
