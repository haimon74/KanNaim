using System;
using System.Linq;

namespace Kan_Naim_Main.DataAccess
{
    class Filter
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public static IQueryable<Table_OriginalPhotosArchive> GetOriginalPhotosByCategoryName(string catName)
        {
            int catId = Lookup.GetLookupCategoryIdFromName(catName);

            return from c in Db.Table_OriginalPhotosArchives
                   where c.CategoryId == catId
                   select c;
        }

        public static IQueryable<Table_PhotosArchive> GetPhotosArchiveByOriginalPhotoId(int originalPhotoId)
        {
            return from c in Db.Table_PhotosArchives
                   where c.OriginalPhotoId == originalPhotoId
                   select c;
        }

        public static int GetOriginalPhotoIdByPhotoName(string photoName)
        {
            try
            {
                return (from c in Db.Table_OriginalPhotosArchives
                        where c.Name == photoName
                        select c).Single().PhotoId;
            }
            catch(Exception exception)
            {
                return -1;
            }
        }

        public static IQueryable<Table_User> GetUserByUserNameOrPhone(string str)
        {
            return (from c in Db.Table_Users
                    where (c.PhoneNumber == str || c.UserName == str)
                    select c);
        }
        public static int GetUserIdFromUserNameOrPhone(string str)
        {
            var result = GetUserByUserNameOrPhone(str);
            return result.First().UserId;
        }
        public static string GetUserRoleIdFromUserNameOrPhone(string str)
        {
            var result = GetUserByUserNameOrPhone(str);
            int roleId = result.First().RoleId;
            try
            {
                return (from c in Db.Table_LookupRoles
                        where roleId == c.RoleId
                        select c.RoleDescription).First();
            }
            catch
            {
                return "";
            }
        }

        
    }
}
