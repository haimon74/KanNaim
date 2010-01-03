using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Kan_Naim_Main
{
    class GlobalValiables
    {
        static public TreeView _gCategoriesTreeView = new TreeView();

        //private const string ConStr = HaimDLL.Constants.MyLocalMsSqlKanNaimConnectionString;
        private const string ConStr = HaimDLL.Constants.My10InfoConnectionString;
        static public DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext(ConStr);
    }
}
