﻿using System;
using System.Linq;

namespace DbSql
{
    public class Filter
    {
        private static readonly DataClassesKanNaimDataContext Db = new DataClassesKanNaimDataContext();

        public static IQueryable<Table_OriginalPhotosArchive> GetOriginalPhotosByCategoryName(string catName)
        {
            int catId = Lookup.GetLookupCategoryIdFromName(catName);
            return GetOriginalPhotosByCategoryId(catId);
        }

        public static IQueryable<Table_OriginalPhotosArchive> GetOriginalPhotosByCategoryId(int catId)
        {
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

        public static Table_OriginalPhotosArchive GetOriginalPhotoFromId(int id)
        {
            var result = from c in Db.Table_OriginalPhotosArchives
                         where c.PhotoId == id
                         select c;
            try
            {
                return result.First();
            }
            catch
            {
                return null;
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
        public static IQueryable<Table_Taktzirim> GetAllTaktzirim()
        {
            return from c in Db.Table_Taktzirims
                   select c;
        }
        public static IQueryable<Table_Taktzirim> GetTaktzirimByTakTypeId(int id)
        {
            return from c in Db.Table_Taktzirims
                   where c.TakTypeId == id
                   select c;
        }
        public static IQueryable<Table_Taktzirim> GetTaktzirimByArticleId(int id)
        {
            return from c in Db.Table_Taktzirims
                   where c.ArticleId == id
                   select c;
        }
        public static IQueryable<Table_Taktzirim> GetTaktzirimByBroadcast()
        {
            return from c in Db.Table_Taktzirims
                   where c.ScheduleId > 0
                   orderby c.ScheduleId descending 
                   select c;
        }

        public static Table_VideosArchive GetVideoFromId(int id)
        {
            var result = from c in Db.Table_VideosArchives
                         where c.Id == id
                         select c;
            try
            {
                return result.First();
            }
            catch
            {
                return null;
            }
        }
        public static Table_LinksPageBottom GetPageBottomLinkByLinkId(int id)
        {
            var result =  from c in Db.Table_LinksPageBottoms
                           where c.LinkId == id
                           select c;
            try
            {
                return result.First();
            }
            catch
            {
                return null;
            }
        }
        public static Table_LinksPrefered GetPreferedLinkByLinkId(int id)
        {
            var result = from c in Db.Table_LinksPrefereds
                         where c.LinkId == id
                         select c;
            try
            {
                return result.First();
            }
            catch
            {
                return null;
            }
        }
        
    }
}
