using System;
using System.IO;
using System.Linq;
using System.Text;

namespace UserControlsLibrary
{
    public class XmlSiteMapGenerator
    {
        private static int _rootId;
        private static IQueryable<Table_Menus> _menuItems;
        private static int _parent;
        private static string _rootPath;
        private static string _siblingPath;
        private static string _siblingFileName;
        private static int _menuId;
        static public void GenerateSiteMapFiles(
            int menuId, string rootPath, string siblingPath, string siblingFileName)
        {
            _menuId = menuId;
            _rootPath = rootPath;
            _siblingPath = siblingPath;
            _siblingFileName = siblingFileName;
            _menuItems = DataAccess.Select.IqTableMenuByMenuId(menuId);

            _rootId = (from c in _menuItems
                       where c.ParentId == -1
                       select c.ItemId).SingleOrDefault();

            GenerateSiteMapLevelFile(_rootId);
        }

        static protected bool GenerateSiteMapLevelFile(int parentId)
        {
            string nodesContent = "";
            IQueryable<Table_Menus> _levelMenuItems = from c in _menuItems
                                                      where c.ParentId == parentId
                                                      select c;
            if (_levelMenuItems.Count() == 0)
                return true;

            foreach (var item in _levelMenuItems)
            {
                bool isLeaf = GenerateSiteMapLevelFile(item.ItemId);

                if (isLeaf)
                {
                    nodesContent += String.Format("\t\t<siteMapNode  title=\"{0}\" description=\"{1}\" url=\"{2}\"/> \n",
                        item.DisplayText.Trim(), item.ToolTip.Trim(), item.OnClickUrl.Trim());
                }
                else
                {
                    nodesContent += String.Format("\t\t<siteMapNode siteMapFile=\"{0}{1}.sitemap\" /> \n", _siblingFileName, item.ItemId);
                }
            }

            string title = (from c in _menuItems
                            where c.ItemId == parentId
                            select c.DisplayText).SingleOrDefault();
            string sitemapContent = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" +
                                    "<siteMap xmlns=\"http://schemas.microsoft.com/AspNet/SiteMap-File-1.0\" > \n " +
                                    String.Format("\t<siteMapNode title=\"{0}\"> \n", title.Trim()) +
                                     nodesContent +
                                     "\t</siteMapNode> \n</siteMap> \n";

            string path;
            if (parentId == _rootId)
                path = _rootPath;
            else
                path = _siblingPath + String.Format("{0}.sitemap", parentId);

            if (File.Exists(path))
                File.Delete(path);
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.WriteThrough);
            byte[] buffer = new UTF8Encoding(true).GetBytes(sitemapContent);
            fs.Write(buffer, 0, buffer.Length);
            fs.Close();

            return false;
        }
    }
}
